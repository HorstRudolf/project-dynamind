using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    // Controll speed of mouse
    public float mouseSensitivity = 100f;

    public Transform playerBody;

    float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // That you stay on screen with the mouse
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // Process user input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;

        // That the view cannot be rotated 360 degrees over the Y-axis
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
