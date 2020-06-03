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
    public GameObject camera;

    public float countdown = 3;
    public float throwForce = 400f;
    public bool thrown = false;

    void Update()
    {
        if (GameObject.Find("DynamiteObject(Clone)") == null) // create one object when none exist
        {
            dynObj = Instantiate(dynSpawnee, spawnPos);
            Rigidbody rb = dynObj.GetComponent<Rigidbody>();
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

        Rigidbody rb = dynObj.GetComponent<Rigidbody>();
        rb.AddForce(camera.transform.forward * throwForce); // throw item forward (from camera angle)
        rb.useGravity = true;

        countdown = 3; // start countdown for automatic explosion
        thrown = true; // allows 'explosion'-key to realize that object is out of characters hand
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 50f); // get all objects in radius
        //Instantiate(explosionEffect, dynObj.transform.position, dynObj.transform.rotation); // explosion effect
        foreach (Collider col in colliders)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(2000f, dynObj.transform.position, 10f, 0.2f, ForceMode.Acceleration);
            }

        }
        Destroy(dynObj);
        thrown = false;
    }
}
