using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Rendering;

public class MouseLook : MonoBehaviour
{
    // Controll speed of mouse
    public float mouseSensitivity = 100f;

    public Transform playerBody;

    float xRotation = 0f;
    float yRotation = 0f;

    PlayerMovement.Status currentStatus = PlayerMovement.currentStatus;

    // Start is called before the first frame update
    void Start()
    {
        // That you stay on screen with the mouse
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {
        currentStatus = PlayerMovement.currentStatus;

        CheckStatus();


    }

    void LadderMouseLook()
    {
        // Process user input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        yRotation += mouseX;

        // That the view cannot be rotated 360 degrees over the Y-axis
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        yRotation = Mathf.Clamp(yRotation, -60f, 60f);


        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    void NormalMouseLook()
    {

        // Process user input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        yRotation += mouseX;

        // That the view cannot be rotated 360 degrees over the Y-axis
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);


        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    void CheckStatus()
    {
        if (currentStatus == PlayerMovement.Status.Walking)
        {
            NormalMouseLook();
        }
        else if (currentStatus == PlayerMovement.Status.LadderClimbing)
        {
            LadderMouseLook();
        }
    }
}
