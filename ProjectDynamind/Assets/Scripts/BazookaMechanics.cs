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
        if (bazooka.transform.parent != null)
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
            GameObject bazookaInstance = Instantiate(bazooka);
            bazookaInstance.GetComponent<Rigidbody>().useGravity = false;
            bazookaInstance.transform.SetParent(rHand);
            bazookaInstance.transform.position = rHand.position;
            var rotationV = rHand.transform.rotation.eulerAngles;
            rotationV.z = 90;
            bazookaInstance.transform.rotation = Quaternion.Euler(rotationV);
            bazookaInstance.GetComponent<CapsuleCollider>().enabled = false;
            Destroy(bazooka);
            counter++;
        }
        else
        {
            if (!rHand.transform.GetChild(0).name.Contains("Bazooka"))
            {
                rHand.transform.GetChild(0).GetComponent<Rigidbody>().useGravity = true;
                rHand.transform.GetChild(0).GetComponent<CapsuleCollider>().enabled = true;
                rHand.transform.DetachChildren();
                GameObject bazookaInstance = Instantiate(bazooka);
                bazookaInstance.GetComponent<Rigidbody>().useGravity = false;
                bazookaInstance.transform.SetParent(rHand);
                bazookaInstance.transform.position = rHand.position;
                var rotationV = rHand.transform.rotation.eulerAngles;
                rotationV.z = 90;
                bazookaInstance.transform.rotation = Quaternion.Euler(rotationV);
                bazookaInstance.GetComponent<CapsuleCollider>().enabled = false;
                Destroy(bazooka);
                counter++;
            }
        }


    }
}
