using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GrenadeMechanics : MonoBehaviour
{
    public GameObject grenade;
    Vector3 lastPos;
    float speed;
    public Text text;

    float timer = 3;
    float ditch = 1;


    //Start is called before the first frame update
    void Start()
    {
        text = GameObject.Find("ExplosionTimerGrenade").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Explodetimer: " + timer + " seconds";
        // either decrease the explosiontimer so grenade will explode, or add to it when it's in players hand based on scroll wheel movement
        if (grenade.transform.parent == null)
        {
            timer -= Time.deltaTime;
            
        }
        else
        {
            timer += Input.GetAxis("Mouse ScrollWheel");
            if (timer > 6)
            {
                timer = 6;
            }
            else if (timer < 1.5)
            {
                timer = 1.5f;
            }
        }

        if (timer <= 0)
        {
            Explode();
            Destroy(grenade);
        }
        if (grenade.transform.parent != null && grenade.transform.parent.parent.parent == null) // if third parent(grenadeLauncher obj is null
        {                                                                                       // then player doesn't hold it anymore, hence we 
            Destroy(grenade);                                                                   // destroy the grenade (prevents bugs)
            text.text = "";                                                                     // and remove the explosiontimer
        }
        
        
        lastPos = grenade.transform.position;                                                   // necessery to calculate speed at collision
    }

    private void OnCollisionEnter(Collision collision)
    {      
        speed = (grenade.transform.position - lastPos).magnitude / Time.deltaTime;

        if (ditch > 3)                                                                          // only allow grenade to jump 3 times
        {
            grenade.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            grenade.GetComponent<Rigidbody>().AddExplosionForce(speed * (20f/ditch), grenade.transform.position, 1f, 0.1f); // make grenade "jump"
        }
        ditch++;
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(grenade.transform.position, 10f); // get all objects in radius
        //Instantiate(explosionEffect, dynObj.transform.position, dynObj.transform.rotation); // explosion effect

        foreach (Collider col in colliders)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if (rb != null) // add force if it's a rigidbody
            {
                rb.AddExplosionForce(750f, grenade.transform.position, 10f, 0.1f, ForceMode.Acceleration);
            }
            else if (col is CharacterController) // seperate ExplosionForce for character due to cc restrictions
            {
                Vector3 explDir = (col.transform.position - grenade.transform.position);
                PlayerMovement.ExplosionForce(explDir, "Dynamite");
            }
        }
        grenade.GetComponent<Rigidbody>().useGravity = false;
        Destroy(grenade);


    }
}
