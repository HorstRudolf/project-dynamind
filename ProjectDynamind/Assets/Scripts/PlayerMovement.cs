using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.Remoting.Activation;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.SocialPlatforms;
using UnityEngine.Windows.Speech;

public class PlayerMovement : MonoBehaviour
{
    public GameObject player;
    public CharacterController controller;
    public GameObject hand;
    public Camera cam;

    public float speed = 12f;
    public float ladderSpeed = 6f;
    public float gravity = -9.81f;
    public float jumpHeigth = 3f;
    float moveSpeedModInitial = 0.60f;

    public static Vector3 velocity;
    bool isGrounded;

    // states to check if an object specific script is ready to be called
    static bool pickedUpItem = false;
    static bool reloadReady = true;
    static bool rangBell = false;
    static bool bridgeUp = false;

    bool standingUp = true;
    bool falling = false;
    static bool explosionFall = false;
    float fallTimer = 0;

    public Text lifeUI;
    int life = 100;
    bool takenDamageRecently = false;
    float timeSinceTakenDamage = 0;
    float timeSinceRegenLife = 0;
    float timeSinceStandingOnDamageGround = 0;

    bool fallStarted = false;
    float timeSinceFall = 0;
    PickUp puItem;

    // create possible objects to modify player speed
   public enum GroundType { Floor = 1, SmallAngle = 2, MediumAngle = 3, BigAngle = 4 }
    public enum ObjectType { None = 1, Light = 2, Medium = 3, Heavy = 4, Untagged = 1 }
    public double movementSpeedModifier = 1;

    public enum Status { LadderClimbing, Walking, Exhausted, Sprinting }

    public enum CarryStatus { Okay, ToHeavy}

    public static Status currentStatus = Status.Walking;

    public static CarryStatus currentCarryStatus = CarryStatus.Okay;

    Transform playerTransform;

    Vector3 position;


    public static GroundType groundTag = GroundType.Floor;

    // Update is called once per frame
    void Update()
    {
        
        CheckStatus();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ladder")
        {
           // SetPlayerPositionToLaderPosition(other);

            currentStatus = Status.LadderClimbing;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ladder")
        {
            //SetPlayerPositionToDefault(other);

            currentStatus = Status.Walking;
        }
    }

    void SetPlayerPositionToLaderPosition(Collider col)
    {
        player.transform.position = new Vector3(col.gameObject.transform.position.x, player.transform.position.y, player.transform.position.z);

        Vector3 ladderRotation = new Vector3(col.gameObject.transform.rotation.x, col.gameObject.transform.rotation.y, col.gameObject.transform.rotation.z);

        player.transform.rotation = Quaternion.Euler(ladderRotation);

    }

    void SetPlayerPositionToDefault(Collider col)
    {
        Vector3 playerRotation = new Vector3(0f, player.transform.rotation.y, 0f);

        player.transform.rotation = Quaternion.Euler(playerRotation);

    }

    public void LadderMovement()
    {
        playerTransform = player.transform;

        if (Input.GetKey("w"))
        {
            float z = Input.GetAxis("Vertical");
            Vector3 move = transform.up * z;

            // Move player
            controller.Move(move * ladderSpeed * Time.deltaTime);
        }
        else if (Input.GetKey("s"))
        {
            float z = Input.GetAxis("Vertical");
            Vector3 move = transform.up * z;

            // Move player
            controller.Move(move * ladderSpeed * Time.deltaTime);

            if (playerTransform.gameObject.transform.position.y == position.y)
            {
                currentStatus = Status.Walking;
            }
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            controller.Move(player.transform.forward * -jumpHeigth *2 * Time.deltaTime);
        }
        position = new Vector3(0, playerTransform.gameObject.transform.position.y, 0);
    }

    void CheckStatus()
    {
        LifeUpdate();
        if (currentStatus == Status.Walking || currentStatus == Status.Sprinting)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {                
                SprintingMovement();
            }
            else
            {
                PlayerMove();
            }
            
        }
        else if (currentStatus == Status.LadderClimbing)
        {
            LadderMovement();
        }
        else if (currentStatus == Status.Exhausted)
        {
            ExhaustedMove();
        }
    }

    public void SprintingMovement()
    {
        currentStatus = Status.Sprinting;

        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            velocity = new Vector3(0, 0, 0);
        }

        // Process user input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = moveSpeedModInitial * (transform.right * x + transform.forward * z);

        // Move player
        controller.Move(move * speed * 1.7f * Time.deltaTime * (float)movementSpeedModifier);


        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeigth * -2f * gravity);
        }

        // fall speed
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void ExhaustedMove()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            velocity = new Vector3(0, 0, 0);
        }

        // Process user input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        // Move player
        controller.Move(move * speed / 2 * Time.deltaTime * (float)movementSpeedModifier);


        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeigth * -2f * gravity);
        }

        // fall speed
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void PlayerMove()
    {
        

        currentStatus = Status.Walking;
        FallDamage();
        //if (Input.GetKey("c") && !falling && standingUp)
        //{
        //    FallDown();
        //    falling = true;

        //}
        //else if (Input.GetKey("v") && !falling && !standingUp)
        //{
        //    StandUp();
        //    standingUp = true;
        //}
        //if (falling)
        //{
        //    fallTimer++;
        //    FallDown();
        //    if (fallTimer > 90)
        //    {
        //        falling = false;
        //        fallTimer = 0;
        //        standingUp = false;
        //    }
        //}
        if (standingUp)
        {
            UpdateMovementspeed();
            if (Input.GetKey("e"))
            {
                TriggerScript();
            }
            else if (Input.GetMouseButton(0))
            {
                EquipItem();
            }

            isGrounded = controller.isGrounded;

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
                velocity = new Vector3(0, 0, 0);
            }

            // Process user input
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = moveSpeedModInitial*(transform.right * x + transform.forward * z);

            // Move player
            controller.Move(move * speed * Time.deltaTime * (float)movementSpeedModifier);


            if (Input.GetKey(KeyCode.Space) && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeigth * -2f * gravity);
            }

            // fall speed
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }

    public static void ExplosionForce(Vector3 explosionDir, String tag)
    {
        velocity = explosionDir;
        explosionFall = true;
        double distance = Math.Sqrt(Math.Abs(velocity.x * velocity.x + velocity.z * velocity.z)); //calculate distance from Object to Player with pythagoras

        distance = (distance < 0.1f) ? 0.1f : (distance > 4f) ? 100f : distance;


        velocity.x += (float)(5 * explosionDir.x / distance);
        velocity.z += (float)(5 * explosionDir.z / distance);

        float yScaling = (float)(1 / Math.Abs(distance * 0.2));
        if (tag == "Bazooka")
        {
            distance /= 2;
            velocity.y += (yScaling > 20f) ? 20f : (yScaling < 0.5f) ? 0f : yScaling;
        }
        else 
            velocity.y += (yScaling > 10f) ? 10f : (yScaling < 0.5f) ? 0f : yScaling;




    }

    public void TriggerScript()
    {

        Ray posit = cam.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(posit, out hit, 5f))
        {

            // get object that player is currenlty aiming at
            if (hit.collider.GetComponent<Rigidbody>() != null)
            {

                // and apply its script
                Rigidbody col = hit.collider.GetComponent<Rigidbody>();
                if (col.GetComponent<PickUp>() != null && !pickedUpItem)
                {
                    puItem = col.GetComponent<PickUp>();
                    puItem.PickUpItem();
                    pickedUpItem = true;
                }
                else if (col.GetComponent<AmmoBox>() != null && reloadReady)
                {
                    AmmoBox oc = col.GetComponent<AmmoBox>();
                    oc.Reload();
                    reloadReady = false;
                }
                else if (col.GetComponent<Bell>() != null && !rangBell)
                {
                    Bell bell = col.GetComponent<Bell>();
                    bell.RingBell();
                    rangBell = true;
                }
                else if (col.GetComponent<BridgeCranck>() != null && !bridgeUp)
                {
                    BridgeCranck bc = col.GetComponent<BridgeCranck>();
                    bc.CranckIt();
                    bridgeUp = true;
                }
                else if (col.GetComponent<BazookaMechanics>() != null)
                //else if (hand.transform.childCount == 0 && col.GetComponent<BazookaMechanics>() != null)
                {
                    BazookaMechanics bm = col.GetComponent<BazookaMechanics>();
                    DynamiteMechanics dm = player.GetComponent<DynamiteMechanics>();
                    dm.DisableRestrictions();
                    player.GetComponent<DynamiteMechanics>().enabled = false;
                    bm.PickUp();
                }
                else if (col.name.Contains("Dynamite"))
                {

                    if (hand.transform.childCount > 0)
                    {
                        hand.transform.DetachChildren();
                    }
                    //restrict gravity and position object into players hands
                    col.transform.GetComponent<CapsuleCollider>().enabled = false;
                    col.transform.SetParent(GameObject.Find("RightHand").transform);
                    col.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    col.transform.GetComponent<Rigidbody>().useGravity = false;
                    col.transform.position = hand.transform.position;
                    col.transform.localEulerAngles = new Vector3(-90, 0, 0);
                    player.GetComponent<DynamiteMechanics>().enabled = true;

                    Destroy(GameObject.Find("DynamiteObject"));

                }
                else if (col.GetComponent<GrenadeLauncher>() != null)
                {
                    GrenadeLauncher gl = col.GetComponent<GrenadeLauncher>();
                    DynamiteMechanics dm = player.GetComponent<DynamiteMechanics>();
                    dm.DisableRestrictions();
                    player.GetComponent<DynamiteMechanics>().enabled = false;
                    gl.PickUp();
                }
            }
        }

    }
    public void EquipItem()
    {
        Ray posit = cam.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(posit, out hit, 5f))
        {
            if (hit.collider.GetComponent<DynamitCrateInteraction>() != null)
            {
                DynamitCrateInteraction dynCrate = hit.collider.GetComponent<DynamitCrateInteraction>();
                dynCrate.EquipDynamite();
            }
        }
    }
    public void UpdateMovementspeed()
    {
        double moveSpeedModOj = 1;
        double moveSpeedModGt = 1;

        if (hand.transform.childCount > 0) // check to see if player is currently holding an item
        {
            Rigidbody rb = hand.transform.GetChild(0).GetComponent<Rigidbody>();
            float mass = rb.mass;
            if (mass <= 3)            // if item is 'medium' or lighter we can move while holding it
            {
                moveSpeedModOj = 1 / (1 + 0.5 * (mass - 1));
            }
            else // else we can't move
            {
                moveSpeedModOj = 0;
            }

            if (mass >= 3)
            {
                currentCarryStatus = CarryStatus.ToHeavy;
            }
            else
            {
                currentCarryStatus = CarryStatus.Okay;
            }

        }
        else
        {
            currentCarryStatus = CarryStatus.Okay;
        }

        Collider[] col = Physics.OverlapSphere(player.transform.position, 0.1f);
        foreach (Collider c in col)
        {

            if (c.tag.Contains("Angle")) // check for floor that player stands on
            {
                GroundType gt = (GroundType)System.Enum.Parse(typeof(GroundType), c.tag);   // get 'angle' of floor
                moveSpeedModGt = (double)1 / (1 + 0.75 * ((int)gt - 1));            // and adjust movespeed based on that   
                groundTag = gt;
            }
            else if (c.tag.Contains("Place_Holder")) // Change later for ground damage types
            {
                timeSinceStandingOnDamageGround += Time.deltaTime;
                if (timeSinceStandingOnDamageGround > 0.5)
                {
                    TakeDamage(1);
                    timeSinceStandingOnDamageGround = 0;
                }
                
            }
            else if(c.tag.Contains("Floor"))
            {

                groundTag = GroundType.Floor;
            }
        }
        movementSpeedModifier = (double)moveSpeedModGt * moveSpeedModOj;

    }

    public void FallDown()
    {
        controller.transform.Rotate(new Vector3(0.5f, 0.5f, 1));
    }

    public void StandUp()
    {
        controller.transform.eulerAngles = new Vector3(0, 0, 0);
    }
    public void TakeDamage(int dmg)
    {
        life -= dmg;
        takenDamageRecently = true;
        timeSinceRegenLife = 0;
        timeSinceTakenDamage = 0;
    }
    public static void UpdateReload()
    {
        reloadReady = true;
    }
    public static void HandsAreFree()
    {
        pickedUpItem = false;
    }
    public void Respawn()
    {
        life = 100;
        takenDamageRecently = false;
        timeSinceTakenDamage = 0;
        timeSinceRegenLife = 0;

        //set position to starting point of the map
        Vector3 spawnpoint =(Vector3) GameObject.Find("Spawn_Point").transform.position;

        CharacterController playerCC = player.GetComponentInParent<CharacterController>();
        playerCC.enabled = false;
        playerCC.transform.position = spawnpoint;
        playerCC.enabled = true;

    }

    public void LifeUpdate()
    {
        lifeUI.text = life + "";
        if (life <= 0) //add "respawn" functionality (go to last checkpoint I guess?)
        {
            Respawn();

        }
        if (takenDamageRecently)
        {
            timeSinceTakenDamage += Time.deltaTime;
            timeSinceRegenLife += Time.deltaTime;
            if (timeSinceRegenLife >= 0.5 & timeSinceTakenDamage >= 5)
            {
                life++;
                timeSinceRegenLife = 0;
            }
            if (life >= 100)
            {
                takenDamageRecently = false;
                timeSinceRegenLife = 0;
                timeSinceTakenDamage = 0;
            }
        }
    }
    public void FallDamage()
    {
        bool floorFound = false;

        Collider[] grounded = Physics.OverlapSphere(player.transform.position, 1f);     
        foreach(Collider col in grounded)
        {
            
            if (col.gameObject.name != "PlayerModel" && !col.gameObject.name.Contains("Wall")) // when a colliding object is neither the player itself
            {                                                                                  // nor a wall (aka player is flying)
                floorFound = true;                                                             // we stop the FallDamage(); timer
                break;
            }
        }
        if (!floorFound)
        {
            fallStarted = true;
        }
        else if (floorFound && fallStarted)
        {
            fallStarted = false;
            if (timeSinceFall > 0.6 && !explosionFall) // check to see if fall etiher crossed a min height or was caused by an explosion
            {
                double force = (timeSinceFall-0.6) * 9.81; // if thats not the case we take damage
                Debug.Log(force);
                TakeDamage((int)(0.2*force*force));
            }
            timeSinceFall = 0;   // reset fall properties
            explosionFall = false;
        }
        if (fallStarted)
        {
            timeSinceFall += Time.deltaTime;
        }   
    }
}

