using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class ZoneTrigger : MonoBehaviour
{
    BoxCollider boxCollider;
    Rigidbody rigidbody;
    Vector3 originalPos;


    public enum RespawnBehavior
    {
        Alive,
        Inanimate,
    }

    public RespawnBehavior respawnBehavior;
   

    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;
        rigidbody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider col)
    {
        //Effects both living stuff and objects
        if (col.CompareTag("DestructionZone") && (respawnBehavior == RespawnBehavior.Alive || respawnBehavior == RespawnBehavior.Inanimate))
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            transform.rotation = Quaternion.Euler( Vector3.zero);
            transform.position = originalPos;

        }
        //only Affects living stuff (e.g. Laser kills stuff but doesn't destroy stuff)
        else if ((col.CompareTag("DeathZone") && respawnBehavior == RespawnBehavior.Alive))
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            transform.rotation = Quaternion.Euler(Vector3.zero);
            transform.position = originalPos;
        }
    }
}
