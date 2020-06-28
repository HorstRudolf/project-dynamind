using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UIElements;

public class BazookaMechanics : MonoBehaviour
{
    public GameObject bazooka;
    public GameObject cam;
    public Transform hand;
    public GameObject rocket;
    public Transform rHand;
    public Transform schaft;

    bool reloadStarted = false;
    float timer = 4;
    int counter = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (bazooka.transform.parent != null) //only allow functions when we hold the bazooka in players hands
        {
            if (Input.GetMouseButton(1) && hand.transform.childCount > 0)
            {
                Shoot();
                reloadStarted = true;
                timer = 0;
            }
            if (hand.transform.childCount == 0 && timer >= 3)
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
            bazooka.GetComponent<CapsuleCollider>().enabled = true;
        }


    }
    void Shoot()
    {
        Rigidbody rocket = hand.transform.GetChild(0).GetComponent<Rigidbody>();
        hand.transform.DetachChildren();
        rocket.AddForce(1000f * cam.transform.forward);
        rocket.GetComponent<CapsuleCollider>().enabled = true;

    }
    void Reload()
    {
        GameObject rock = Instantiate(rocket, hand);
        rock.GetComponent<CapsuleCollider>().enabled = false;

    }
    public void PickUp()
    {

        if (rHand.transform.childCount == 0)
        {
            // create bazooka and apply restriction/position/rotation
            GameObject bazookaInstance = Instantiate(bazooka);
            bazookaInstance.GetComponent<CapsuleCollider>().enabled = false;
            bazookaInstance.GetComponent<Rigidbody>().useGravity = false;
            bazookaInstance.transform.SetParent(rHand);
            bazookaInstance.transform.position = rHand.position;
            var rotationV = rHand.transform.rotation.eulerAngles;
            rotationV.z = 90;
            bazookaInstance.transform.rotation = Quaternion.Euler(rotationV);

            Destroy(bazooka);
            counter++;
        }
        else
        {
            if (!rHand.transform.GetChild(0).name.Contains("Bazooka"))
            {
                // drop item already in hand (as long as it's not a Bazooka)
                rHand.transform.GetChild(0).GetComponent<Rigidbody>().useGravity = true;
                rHand.transform.GetChild(0).GetComponent<CapsuleCollider>().enabled = true;
                rHand.transform.DetachChildren();
                // and create bazooka and apply restriction/position/rotation
                GameObject bazookaInstance = Instantiate(bazooka);
                bazookaInstance.GetComponent<CapsuleCollider>().enabled = false;
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
