using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class GrenadeLauncher : MonoBehaviour
{
    public GameObject grenade;
    public GameObject grenadeLauncher;
    public GameObject cam;

    public Transform rHand;
    public Transform bulletPoint;

    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(grenadeLauncher.transform.parent != null) // only allow function when player holds the Grenade Launcher
        {
            timer -= Time.deltaTime;
            if (!GameObject.Find("Grenade(Clone)") && bulletPoint.transform.childCount == 0 && grenadeLauncher.transform.parent != null && timer <= 0)
            {
                Reload();
            }
            if (Input.GetMouseButton(1) && bulletPoint.transform.childCount > 0)
            {
                Shoot();
                timer = 7;
            }
        }
        else // disable the restrictions needed to steadily hold the weapon in case the weapon isn't in players hands anymoer
        {
            grenadeLauncher.GetComponent<Rigidbody>().useGravity = true;
            grenadeLauncher.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            grenadeLauncher.GetComponent<CapsuleCollider>().enabled = true;  
        }
   
    }
    void Shoot()
    {
        Rigidbody grenadeRb = bulletPoint.transform.GetChild(0).GetComponent<Rigidbody>();
        grenadeRb.useGravity = true;
        bulletPoint.transform.DetachChildren();
        grenadeRb.AddForce(700f * cam.transform.forward);
        grenadeRb.GetComponent<SphereCollider>().enabled = true;
    }

    void Reload()
    {
        GameObject grenadeInstance = Instantiate(grenade, bulletPoint);
    }

    public void PickUp()
    {
        if (rHand.transform.childCount == 0)
        {
            // create grenade and apply restriction/position/rotation
            GameObject grenadeInstance = Instantiate(grenadeLauncher);
            grenadeInstance.GetComponent<CapsuleCollider>().enabled = false;
            grenadeInstance.GetComponent<Rigidbody>().useGravity = false;
            grenadeInstance.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            grenadeInstance.transform.SetParent(rHand);
            grenadeInstance.transform.position = rHand.position;
            var rotationV = rHand.transform.rotation.eulerAngles;

            grenadeInstance.transform.rotation = Quaternion.Euler(rotationV);

            Destroy(grenadeLauncher);

        }
        else
        {
            if (!rHand.transform.GetChild(0).name.Contains("Grenade") )
            {
                // drop item already in hand (as long as it's not a GrenadeLauncher)
                rHand.transform.GetChild(0).GetComponent<Rigidbody>().useGravity = true;
                rHand.transform.GetChild(0).GetComponent<CapsuleCollider>().enabled = true;
                rHand.transform.DetachChildren();
                // and create grenade and apply restriction/position/rotation
                GameObject grenadeInstance = Instantiate(grenadeLauncher);
                grenadeInstance.GetComponent<CapsuleCollider>().enabled = false;
                grenadeInstance.transform.SetParent(rHand);
                grenadeInstance.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                grenadeInstance.GetComponent<Rigidbody>().useGravity = false;
               
                grenadeInstance.transform.position = rHand.position;
                var rotationV = rHand.transform.rotation.eulerAngles;
                rotationV.z = 90;
                grenadeInstance.transform.rotation = Quaternion.Euler(rotationV);

                Destroy(grenadeLauncher);

            }
        }
    }
}
