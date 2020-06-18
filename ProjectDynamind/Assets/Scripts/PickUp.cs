using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Transform destination;
    public GameObject cam;
    bool  pickedUp = false;
    float force = 500;

    public void PickUpItem()
    {
        Rigidbody rbItem = this.GetComponent<Rigidbody>();
        if (GameObject.Find("DynamiteObject(Clone)") != null) // if player is currently holding dyn
        {
            Destroy(GameObject.Find("DynamiteObject(Clone)")); // then we destroy that dyn object
            DynamiteMechanics.PickedUpAnItem();                // and signal our player model that his hands are full
        }

        // make the object we wanna pick up a child of our hand  
        // with needed restrictions(gravitiy off, rotation/position frozen)
        rbItem.useGravity = false;                                  
        rbItem.transform.position = destination.transform.position;
        rbItem.transform.parent = GameObject.Find("MiddleHand").transform; 
        pickedUp = true;                                              
        rbItem.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        DynamiteMechanics.PickedUpAnItem();
    }

    public void ThrowItem(float countdown)
    {
        Rigidbody rbItem = this.GetComponent<Rigidbody>();
        // calculate throw force based on time the key was pressed with a max of 2000f
        float throwForce = (force * countdown > 2000) ? 2000 : force * countdown; 

        rbItem.transform.parent = null; // set the object we wanna throw as an child of our world
        rbItem.constraints = RigidbodyConstraints.None; // and remove its constraints that were introduced in the PickUp method
        rbItem.useGravity = true;

        rbItem.AddForce(cam.transform.forward * throwForce); // throw item
        pickedUp = false;                           // And tell our player that his hands are 'free' again
        DynamiteMechanics.ThrewDownItem();

        
    }
}
