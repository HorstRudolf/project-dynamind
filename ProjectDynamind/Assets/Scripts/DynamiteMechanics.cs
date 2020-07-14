using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class DynamiteMechanics : MonoBehaviour
{
    public Transform rHand;

    public GameObject dynSpawnee;
    public GameObject dynObj;
    //public GameObject explosionEffect;
    public GameObject cam;
    public CharacterController playerObj;
    public Text ui;

    public static bool pickedUpGameObject;
    public float countdown = 0;
    public float throwForce = 400f;
    public bool thrown = false;
    public bool infiniteAmmo = false;
    static int ammo = 6;

    void Update()
    {

        if (infiniteAmmo)
        {
            ui.text = "infinite";
        }
        else
        {
            ui.text = ammo + "";
        }
        
        if (GameObject.Find("DynamiteObject(Clone)") == null && countdown <= 0 && !pickedUpGameObject && (ammo > 0 ||infiniteAmmo)) // create one object when none exist
        {
            dynSpawnee.GetComponent<Rigidbody>().useGravity = false;
            dynObj = Instantiate(dynSpawnee, rHand);
            dynObj.GetComponent<CapsuleCollider>().enabled = false;
            dynObj.GetComponent<Rigidbody>().useGravity = false;


        }
        else if (Input.GetMouseButtonDown(1) && GameObject.Find("DynamiteObject(Clone)") != null && !thrown) // when obj exists and we press 'throw'-button it throws
        {
            ThrowDynamite();
            ammo--;
        }


        countdown -= Time.deltaTime; // change countdown to facilitate automatic explosion
        if ((countdown <= 0 || Input.GetKey("e")) && thrown) // or explosion when 'explosion'-key is pressed
        {
            Explode();
        }
    }
    void ThrowDynamite()
    {
        dynObj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        dynObj.transform.SetParent(null); //unchain dyn from player movement
        dynObj.GetComponent<CapsuleCollider>().enabled = true;

        Rigidbody rb = dynObj.GetComponent<Rigidbody>();
        rb.AddForce(cam.transform.forward * throwForce); // throw item forward (from camera angle)
        rb.useGravity = true;

        countdown = 3; // start countdown for automatic explosion
        thrown = true; // allows 'explosion'-key to realize that object is out of characters hand
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(dynObj.transform.position, 10f); // get all objects in radius
        //Instantiate(explosionEffect, dynObj.transform.position, dynObj.transform.rotation); // explosion effect

        foreach (Collider col in colliders)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if (rb != null) // add force if it's a rigidbody
            {
                rb.AddExplosionForce(750f, dynObj.transform.position, 10f, 0.1f, ForceMode.Acceleration);
            }
            else if (col is CharacterController) // seperate ExplosionForce for character due to cc restrictions
            {
                Vector3 explDir = (col.transform.position - dynObj.transform.position);
                PlayerMovement.ExplosionForce(explDir, "Dynamite");
            }
        }
        dynObj.GetComponent<Rigidbody>().useGravity = false;
        Destroy(dynObj);
        thrown = false;
        countdown = 2;
    }
    public void PickMeUp()
    {
        if (rHand.transform.childCount == 0)
        {
            GameObject dynInstance = Instantiate(dynObj);
            dynInstance.GetComponent<CapsuleCollider>().enabled = false;
            dynInstance.GetComponent<Rigidbody>().useGravity = false;
            dynInstance.transform.SetParent(rHand);
            dynInstance.transform.position = rHand.position;
            dynInstance.transform.rotation = rHand.rotation;
            Destroy(dynObj);

        }
        else
        {
            if (!rHand.transform.GetChild(0).name.Contains("DynamiteObject"))
            {
                rHand.transform.GetChild(0).GetComponent<Rigidbody>().useGravity = true;
                rHand.transform.GetChild(0).GetComponent<CapsuleCollider>().enabled = true;
                rHand.transform.DetachChildren();
                GameObject dynInstance = Instantiate(dynObj);
                dynInstance.GetComponent<CapsuleCollider>().enabled = false;
                dynInstance.GetComponent<Rigidbody>().useGravity = false;
                dynInstance.transform.SetParent(rHand);
                dynInstance.transform.position = rHand.position;
                dynInstance.transform.rotation = rHand.rotation;
                Destroy(dynObj);

            }

        }
    }
    public static void PickedUpAnItem()
    {
        pickedUpGameObject = true;
    }

    public static void ThrewDownItem()
    {
        pickedUpGameObject = false;
        PlayerMovement.HandsAreFree();
    }
    public static void AddAmmo()
    {
        ammo += 3;
    }
    public void DisableRestrictions()
    {
        if (ammo > 0)
        {
            dynObj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            dynObj.GetComponent<Rigidbody>().useGravity = true;
            dynObj.GetComponent<CapsuleCollider>().enabled = true;
        }

        ui.text = "";
    }
}
