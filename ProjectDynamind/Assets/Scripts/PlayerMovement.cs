using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class PlayerMovement : MonoBehaviour
{
    public GameObject player;
    public CharacterController controller;
    public static GameObject cam;
    public Transform dest;
    public GameObject hand;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeigth = 3f;

    public static Vector3 velocity;
    bool isGrounded;
    bool pickedUpItem = false;
    float throwCountdown = 0;
    bool throwCountdownStarted = false;
    PickUp rbItem;

    enum GroundType {Floor = 1, SmallAngle = 2, MediumAngle = 3, BigAngle = 4, Untagged = 1, Light = 1, Medium = 1, Heavy = 1, DeathZone = 1 }
    enum ObjectType {None = 1, Light = 2, Medium = 3, Heavy = 4, Untagged = 1 }
    public double movementSpeedModifier = 1;


    // Update is called once per frame
    void Update()
    {
        UpdateMovementspeed();
        // pickup item
        if (Input.GetKey("q"))
        {
            GetItem();
        }

        // calculate length of item throw button and throw it based on the time the button was held
        if (pickedUpItem && Input.GetMouseButtonDown(1))
        {
            throwCountdownStarted = true;
        }
        if (pickedUpItem && Input.GetMouseButtonUp(1))
        {
            rbItem.ThrowItem(throwCountdown);
            throwCountdown = 0;
            throwCountdownStarted = false;
        }
   
        if (throwCountdownStarted)
        {
            throwCountdown += Time.deltaTime;
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

    public static void ExplosionForce(Vector3 explosionDir)
    {
        velocity = explosionDir; 

        double distance = Math.Sqrt(Math.Abs(velocity.x * velocity.x + velocity.z * velocity.z)); //calculate distance from Object to Player with pythagoras

        distance = (distance < 0.1f) ? 0.1f : (distance > 4f) ? 100f : distance;    // keep distance between 0.1 and 4 to avoid flying unrealistically far
                                                                                    // getting impacted when distance is too high
        velocity.x += (float)(5 * explosionDir.x / distance);
        velocity.z += (float)(5 * explosionDir.z / distance);

        float yScaling = (float)(1 / Math.Abs(distance * 0.2)); 
        velocity.y += (yScaling > 5f) ? 5f : (yScaling < 0.5f) ? 0f : yScaling;
        


    }

    public void GetItem()
    {
        Vector3 destin = dest.transform.position; //'Hand' that holds the item

        Collider[] colliders = Physics.OverlapSphere((destin), 2f); // check to see if theres an item nearby

        foreach (Collider c in colliders)
        {
            Rigidbody rb = c.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rbItem = rb.GetComponent<PickUp>();
                rbItem.PickUpItem();        
                pickedUpItem = true;
                //rbItem = pu;
                break;
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
            if ((int)oj <= 3)            // if item is 'medium' heavy or lighter we can move while holding it
            {
                moveSpeedModOj = (double)1 /(1+0.5*((int)oj-1));
            }
            else // else we can't move
            {
                moveSpeedModOj = 0;
            }            
        }

        Collider[] col = Physics.OverlapSphere(player.transform.position, 2f);
        foreach(Collider c in col)
        {
           
            if (c.tag != "Player") // check for objects that collide, that are not our player
            {
                GroundType gt = (GroundType)System.Enum.Parse(typeof(GroundType), c.tag); // get 'weight' of item
                moveSpeedModGt = (double)1/(1 + 0.75*((int)gt-1));                        // and adjust movespeed based on that
            }   
         }
        movementSpeedModifier = (double)moveSpeedModGt * moveSpeedModOj;

    }
}
