using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DynamiteMechanics : MonoBehaviour
{
    //TODO: Adjust explosion/throw forces, radius etc.
    public Transform spawnPos; // change to hand in which dyn is supposed to be held
    public GameObject spawnee; // change to dyn object
    public GameObject obj; // change to dyn object
    public float countdown = 3;
    public float throwForce = 400f; 
    public bool thrown = false;

    void Update()
    {
        if (GameObject.Find("Dynamite(Clone)") == null ) // create one object when none exist
        
        {
             obj =Instantiate(spawnee, spawnPos.position, spawnPos.rotation);
        }
        if (Input.GetMouseButtonDown(1) && GameObject.Find("Dynamite(Clone)") != null) // when obj exists and we press 'throw'-button it throws
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
        GameObject dynamite = obj;
        Rigidbody rb = dynamite.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * throwForce); // throw item forward (from camera angle)
        rb.useGravity = true;
        countdown = 3; // start countdown for automatic explosion
        thrown = true; // allows 'explosion'-key to realize that object is out of characters hand
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 50f); // get all objects in radius

        foreach (Collider col in colliders)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(2000f, obj.transform.position, 10f, 0.2f, ForceMode.Acceleration);
            }

        }
        Destroy(obj);
        thrown = false;
    }
}
