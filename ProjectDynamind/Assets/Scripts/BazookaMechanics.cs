using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UIElements;

public class BazookaMechanics : MonoBehaviour
{
    public GameObject bazooka;
    public GameObject cam;
    public Transform shaft;
    public GameObject rocket;
    public Transform rHand;

    bool reloadStarted = false;
    float timer = 4;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (bazooka.transform.parent != null) //only allow functions when we hold the bazooka in players hands
        {
            if (Input.GetMouseButton(1) && shaft.transform.childCount > 0)
            {
                Shoot();
                reloadStarted = true;
                timer = 0;
            }
            if (shaft.transform.childCount == 0 && timer >= 3)
            {
                Reload();
            }
            if (reloadStarted)
            {
                timer += Time.deltaTime;
            }
        }
        else
        {
            bazooka.GetComponent<Rigidbody>().useGravity = true;
            bazooka.GetComponent<Collider>().enabled = true;
        }


    }
    void Shoot()
    {
        Rigidbody rocket = shaft.transform.GetChild(0).GetComponent<Rigidbody>();
        shaft.transform.DetachChildren();

        rocket.AddForce(1000f * cam.transform.forward);
        rocket.GetComponent<Collider>().enabled = true;

    }
    void Reload()
    {
        GameObject rock = Instantiate(rocket, shaft);
        rock.GetComponent<Collider>().enabled = false;

    }
    public void PickUp()
    {

        if (rHand.transform.childCount == 0)
        {
            // create bazooka and apply restriction/position/rotation
            GameObject bazookaInstance = Instantiate(bazooka);
            bazookaInstance.GetComponent<Collider>().enabled = false;
            bazookaInstance.GetComponent<Rigidbody>().useGravity = false;
            bazookaInstance.transform.SetParent(rHand);
            bazookaInstance.transform.position = rHand.position;
            var rotationV = rHand.transform.rotation.eulerAngles;
            rotationV.z = 90;
            bazookaInstance.transform.rotation = Quaternion.Euler(rotationV);

            Destroy(bazooka);
        }
        else
        {
            if (!rHand.transform.GetChild(0).name.Contains("Bazooka"))
            {
                // drop item already in hand (as long as it's not a Bazooka)
                rHand.transform.GetChild(0).GetComponent<Rigidbody>().useGravity = true;
                rHand.transform.GetChild(0).GetComponent<Collider>().enabled = true;
                rHand.transform.DetachChildren();
                // and create bazooka and apply restriction/position/rotation
                GameObject bazookaInstance = Instantiate(bazooka);
                bazookaInstance.GetComponent<Collider>().enabled = false;
                bazookaInstance.GetComponent<Rigidbody>().useGravity = false;
                bazookaInstance.transform.SetParent(rHand);
                bazookaInstance.transform.position = rHand.position;
                var rotationV = rHand.transform.rotation.eulerAngles;
                rotationV.z = 90;
                bazookaInstance.transform.rotation = Quaternion.Euler(rotationV);

                Destroy(bazooka);

            }
        }


    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name == "Floor")
        {
            bazooka.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ 
                                                          | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY 
                                                          | RigidbodyConstraints.FreezeRotationZ;
        }
        
    }
}
