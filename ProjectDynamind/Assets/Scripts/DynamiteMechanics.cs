using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DynamiteMechanics : MonoBehaviour
{
    public Transform spawnPos;

    public GameObject dynSpawnee;
    public GameObject dynObj;
    //public GameObject explosionEffect;
    public GameObject cam;
    public CharacterController playerObj;

    public float countdown = 0;
    public float throwForce = 400f;
    public bool thrown = false;

    Vector3 vel;

    void Update()
    {
       

        if (GameObject.Find("DynamiteObject(Clone)") == null && countdown <= 0) // create one object when none exist
        {
            dynObj = Instantiate(dynSpawnee, spawnPos);
            Rigidbody rb = dynObj.GetComponent<Rigidbody>();
            dynObj.GetComponent<CapsuleCollider>().enabled = false;
        }
        else if (Input.GetMouseButtonDown(1) && GameObject.Find("DynamiteObject(Clone)") != null && !thrown) // when obj exists and we press 'throw'-button it throws
        {
            ThrowDynamite();
        }


        countdown -= Time.deltaTime; // change countdown to facilitate automatic explosion
        if ((countdown <= 0 || Input.GetKey("e")) && thrown) // or explosion when 'explosion'-key is pressed
        {
            Explode();
        }
    }
    void ThrowDynamite()
    {
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
            Debug.Log(col);
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(750f, dynObj.transform.position, 10f, 0.2f, ForceMode.Acceleration);
            }
            else if (col is CharacterController)
            {
                Vector3 explDir = (col.transform.position - dynObj.transform.position);
                PlayerMovement.ExplosionForce(explDir);
            }
        }
        Destroy(dynObj);
        thrown = false;
        countdown = 2;
    }
}
