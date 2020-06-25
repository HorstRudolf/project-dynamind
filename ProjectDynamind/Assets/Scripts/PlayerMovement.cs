using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows.Speech;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeigth = 3f;


    public static Vector3 velocity;
    bool isGrounded;

    public enum Status { LadderClimbing, Walking }

    [SerializeField]
    public Status currentStatus = Status.Walking;

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
            //Collider col = other;
            //Debug.Log("x: " + col.gameObject.transform.rotation.x +" y: " + col.gameObject.transform.rotation.y + " x: " + col.gameObject.transform.rotation.z);
            //playerTransform.rotation = Quaternion.Euler(col.gameObject.transform.rotation.x, col.gameObject.transform.rotation.y, col.gameObject.transform.rotation.z);
            currentStatus = Status.LadderClimbing;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ladder")
        {
            //Collider col = other;
            //Debug.Log("x: " + col.gameObject.transform.rotation.x + " y: " + col.gameObject.transform.rotation.y + " x: " + col.gameObject.transform.rotation.z);
            //playerTransform.rotation = Quaternion.Euler(0, col.gameObject.transform.rotation.y, 0);
            currentStatus = Status.Walking;
        }
    }


    void CheckStatus()
    {
        if (currentStatus == Status.Walking)
        {
            PlayerMove();
        }
        else if (currentStatus == Status.LadderClimbing)
        {
            LadderMovement();
        }
    }

    public void LadderMovement()
    {
        playerTransform = controller.gameObject.transform;

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

            velocity.y = Mathf.Sqrt(jumpHeigth * -2f * gravity);
            velocity.z = -3;
            velocity.x = -3;
            // fall speed
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
        position = new Vector3(0, playerTransform.gameObject.transform.position.y, 0);
    }


    public void PlayerMove()
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
        controller.Move(move * speed * Time.deltaTime);


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

        double distance = Math.Sqrt(Math.Abs(velocity.x * velocity.x + velocity.z * velocity.z)); //caculate distance from Object to Player with pythagoras

        distance = (distance < 0.1f) ? 0.1f : (distance > 4f) ? 100f : distance;    // keep distance between 0.1 and 4 to avoid flying unrealistically far
                                                                                    // getting impacted when distance is too high
        velocity.x += (float)(5 * explosionDir.x / distance);
        velocity.z += (float)(5 * explosionDir.z / distance);

        float yScaling = (float)(1 / Math.Abs(distance * 0.2));
        velocity.y += (yScaling > 5f) ? 5f : (yScaling < 0.5f) ? 0f : yScaling;



    }

}
