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
    [SerializeField]
    private LayerMask LadderMask;
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeigth = 3f;

    public static Vector3 velocity;
    bool isGrounded;

    public enum Status { LadderClimbing, Walking }

    public Status currentStatus = Status.Walking;

    // Update is called once per frame
    void Update()
    {
        CheckStatus();
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

    void CheckForLadder()
    {
        if (Physics.OverlapBox(controller.gameObject.transform.position, transform.localScale / 2, Quaternion.identity, LadderMask) != null)
            currentStatus = Status.LadderClimbing;
    }

    public void LadderMovement()
    {
        Collider[] hitColliders = Physics.OverlapBox(controller.gameObject.transform.position, transform.localScale / 2, Quaternion.identity, LadderMask);
        GameObject Ladder = hitColliders[0].gameObject;
        GameObject PlayerObject = controller.gameObject;


        if (Ladder.transform.lossyScale.y > PlayerObject.transform.position.y)
        {
            // Process user input
            float z = Input.GetAxis("Vertical");

            Vector3 move = new Vector3(0, z);

            // Move player
            controller.Move(move * speed * Time.deltaTime);

        }


        currentStatus = Status.Walking;
    }

    //public void CheckForLadder()
    //{
    //    GameObject PlayerObject = controller.gameObject;

    //    Collider[] hitColliders = Physics.OverlapBox(PlayerObject.transform.position, transform.localScale / 2, Quaternion.identity, LadderMask);

    //    if (hitColliders.Length > 0 && currentStatus == Status.Walking)
    //    {
    //        Collider hitCollider = hitColliders[0];
    //        currentStatus = Status.LadderClimbing;


    //        LadderMovement(hitCollider);

    //    }
    //   else
    //    {
    //        PlayerMove();
    //    }

    //    hitColliders = null;


    //}

    public void PlayerMove()
    {
        CheckForLadder();

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
