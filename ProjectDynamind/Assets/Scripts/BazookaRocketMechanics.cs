using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazookaRocketMechanics : MonoBehaviour
{
    public GameObject rocket;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (rocket.transform.parent != null&&rocket.transform.parent.parent.parent == null)
        {
            Destroy(rocket);
        }        
    }
    private void OnCollisionEnter(Collision collision)
    {
        Collider[] colliders = Physics.OverlapSphere(rocket.transform.position, 10f);
        foreach (Collider col in colliders)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if (rb != null) // Add explosionforce to all nearby rigidbody object
            {
                if (rb.mass < 4) // Mass of 'very-heavy' object, change if needed
                {
                    rb.AddExplosionForce(1500f, rocket.transform.position, 10f, 0.1f);
                }
                
            }
            else if (col.tag == "Player") // or player
            {
                Vector3 explDir = col.transform.position - rocket.transform.position;
                PlayerMovement.ExplosionForce(explDir, "Bazooka");
                col.GetComponent<PlayerMovement>().TakeDamage(5);
            }
        }
        Destroy(rocket);

    }
}
