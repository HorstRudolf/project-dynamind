﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.Remoting.Activation;
using Unity.Collections;
using UnityEngine;
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
    public float gravity = -9.81f;
    public float jumpHeigth = 3f;

    public static Vector3 velocity;
    bool isGrounded;

    // states to check if an object specific script is ready to be called
    static bool pickedUpItem = false;
    static bool reloadReady = true;
    static bool rangBell = false;
    static bool bridgeUp = false;

    bool standingUp = true;
    bool falling = false;
    float fallTimer = 0;


    PickUp puItem;

    // create possible objects to modify player speed
    enum GroundType { Floor = 1, SmallAngle = 2, MediumAngle = 3, BigAngle = 4 }
    public enum ObjectType { None = 1, Light = 2, Medium = 3, Heavy = 4, Untagged = 1 }
    public double movementSpeedModifier = 1;

    public enum Status { LadderClimbing, Walking, Exhausted, Sprinting }

    public static Status currentStatus = Status.Walking;

    Transform playerTransform;

    Vector3 position;

    // Update is called once per frame
    void Update()
    {

        CheckStatus();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ladder")
        {
            SetPlayerPositionToLaderPosition(other);

            currentStatus = Status.LadderClimbing;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ladder")
        {
            SetPlayerPositionToDefault(other);

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
            controller.Move(move * speed * Time.deltaTime);
        }
        else if (Input.GetKey("s"))
        {
            float z = Input.GetAxis("Vertical");
            Vector3 move = transform.up * z;

            // Move player
            controller.Move(move * speed * Time.deltaTime);

            if (playerTransform.gameObject.transform.position.y == position.y)
            {
                currentStatus = Status.Walking;
            }
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            controller.Move(player.transform.forward * -jumpHeigth * 2 * Time.deltaTime);
        }
        position = new Vector3(0, playerTransform.gameObject.transform.position.y, 0);
    }

    void CheckStatus()
    {
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

        Vector3 move = transform.right * x + transform.forward * z;

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

        if (Input.GetKey("c") && !falling && standingUp)
        {
            FallDown();
            falling = true;

        }
        else if (Input.GetKey("v") && !falling && !standingUp)
        {
            StandUp();
            standingUp = true;
        }
        if (falling)
        {
            fallTimer++;
            FallDown();
            if (fallTimer > 90)
            {
                falling = false;
                fallTimer = 0;
                standingUp = false;
            }
        }
        if (standingUp)
        {
            UpdateMovementspeed();
            if (Input.GetKey("e"))
            {
                TriggerScript();


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

            Vector3 move = transform.right * x + transform.forward * z;

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

        double distance = Math.Sqrt(Math.Abs(velocity.x * velocity.x + velocity.z * velocity.z)); //calculate distance from Object to Player with pythagoras
        if (tag == "Dynamite")
        {
            distance = (distance < 0.1f) ? 0.1f : (distance > 4f) ? 100f : distance;
        }
        else if (tag == "Bazooka")
        {
            distance = (distance < 0.1f) ? 0.1f : (distance > 4f) ? 100f : distance;    // keep distance between 0.1 and 4 to avoid flying 
                                                                                        // unrealistically far when distance is too high
            distance /= 2;
        }


        velocity.x += (float)(5 * explosionDir.x / distance);
        velocity.z += (float)(5 * explosionDir.z / distance);

        float yScaling = (float)(1 / Math.Abs(distance * 0.2));
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
    public void UpdateMovementspeed()
    {
        double moveSpeedModOj = 1;
        double moveSpeedModGt = 1;

        if (hand.transform.childCount > 0) // check to see if player is currently holding an item
        {
            Transform objInHand = hand.transform.GetChild(0);
            ObjectType oj = (ObjectType)System.Enum.Parse(typeof(ObjectType), objInHand.tag);
            if ((int)oj <= 3)            // if item is 'medium' or lighter we can move while holding it
            {
                moveSpeedModOj = (double)1 / (1 + 0.5 * ((int)oj - 1));
            }
            else // else we can't move
            {
                moveSpeedModOj = 0;
            }
        }

        Collider[] col = Physics.OverlapSphere(player.transform.position, 2f);
        foreach (Collider c in col)
        {

            if (c.tag.Contains("Angle")) // check for floor that player stands on
            {
                GroundType gt = (GroundType)System.Enum.Parse(typeof(GroundType), c.tag);   // get 'angle' of floor
                moveSpeedModGt = (double)1 / (1 + 0.75 * ((int)gt - 1));            // and adjust movespeed based on that                  
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

    public static void UpdateReload()
    {
        reloadReady = true;
    }
    public static void HandsAreFree()
    {
        pickedUpItem = false;
    }
}

