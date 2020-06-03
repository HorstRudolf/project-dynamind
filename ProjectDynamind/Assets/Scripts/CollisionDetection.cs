using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject dynamiteObject;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Floor")
        {
            Debug.Log("hit the bottom");
            Rigidbody rb = dynamiteObject.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezePosition;
        }
    }
}
