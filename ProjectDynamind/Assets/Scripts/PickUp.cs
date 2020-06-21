using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Transform dest;
    public GameObject cam;

    bool timerThrowStarted = false;
    bool pickedUpItem;

    float timer = 5;
    float timerThrowForce = 0;
    float force = 500;

    int type = 1;

    public void Update()
    {
        // Drop item if either the timer wears off (replaces endurance for now) or 'f' is pressed
        if (timer <= 0 || (Input.GetKey("f") && (GameObject.Find("RightHand")).transform.childCount != 0))
        {
            DropItem();
        }
        // start time to calculate the force the item is supposed to be thrown ´with
        if (pickedUpItem && Input.GetMouseButtonDown(1))
        {
            timerThrowStarted = true;
        }
        else if (pickedUpItem && Input.GetMouseButtonUp(1))
        {
            ThrowItem();
            pickedUpItem = false;
            timerThrowStarted = false;
        }
        if (timerThrowStarted)
        {
            timerThrowForce += Time.deltaTime;
        }

        // timer to drop item after endurance has worn off
        if (pickedUpItem)
        {
            timer -= Time.deltaTime;
        }

    }
    public void PickUpItem()
    {

        Rigidbody rb = this.GetComponent<Rigidbody>();
        // if rb != null, does it change anything?

        // destroy dynamite object if it's in players hand
        if (GameObject.Find("DynamiteObject(Clone)") != null)
        {
            Destroy(GameObject.Find("DynamiteObject(Clone)"));
            DynamiteMechanics.PickedUpAnItem();
        }
        // get 'heaviness' of item to calculate how long player can hold it in his hands
        PlayerMovement.ObjectType obj = (PlayerMovement.ObjectType)System.Enum.Parse(typeof(PlayerMovement.ObjectType), rb.tag);
        timer = (float)(30 / (int)obj);

        pickedUpItem = true;
        // Add restrictions to prevent item moving unnecessarily
        rb.useGravity = false;
        rb.transform.position = dest.transform.position;
        rb.GetComponent<Collider>().enabled = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        // And place it in players hand
        rb.transform.parent = GameObject.Find("RightHand").transform;



    }
    public void DropItem()
    {
        // can only drop item if there is an item in players hand
        GameObject rightHandObject = GameObject.Find("RightHand");
        if (rightHandObject.transform.childCount > 0)
        {
            // get child and check if that it's not the dynamite ( or any weapon for later update
            Transform child = rightHandObject.transform.GetChild(0);
            if (child.name != "DynamiteObject(Clone)")
            {
                // remove all constraints
                Rigidbody rbChild = child.GetComponent<Rigidbody>();
                rbChild.constraints = RigidbodyConstraints.None;
                rbChild.useGravity = true;
                rbChild.GetComponent<Collider>().enabled = true;
                // and 'throw' the object without any force (other than gravity)
                rightHandObject.transform.DetachChildren();
                //and signalise the DynamiteObject that it can be picked up again
                DynamiteMechanics.ThrewDownItem();
            }
            

        }
        

    }
    public void ThrowItem()
    {
        Rigidbody rbItem = this.GetComponent<Rigidbody>();
        // calculate throw force based on time the key was pressed with a max of 2000f
        float throwForce = (force * timerThrowForce > 2000) ? 2000 : force * timerThrowForce;

        rbItem.transform.parent = null; // set the object we wanna throw as an child of our world ( free from any player movement)
        rbItem.constraints = RigidbodyConstraints.None; // and remove its constraints that were introduced in the PickUp method
        rbItem.useGravity = true;
        rbItem.GetComponent<Collider>().enabled = true;

        rbItem.AddForce(cam.transform.forward * throwForce); // throw item

        //and signalise the DynamiteObject that it can be picked up again
        DynamiteMechanics.ThrewDownItem();
        timerThrowForce = 0;
    }
}
