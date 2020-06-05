using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class ZoneTrigger : MonoBehaviour
{
    BoxCollider boxCollider;
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
    }

    private void OnTriggerEnter(Collider col)
    {
        //Effects both living stuff and objects
        if (col.CompareTag("DestructionZone") && (respawnBehavior == RespawnBehavior.Alive || respawnBehavior == RespawnBehavior.Inanimate))
            transform.position = originalPos;
        //only Affects living stuff (e.g. Laser kills stuff but doesn't destroy stuff)
        else if ((col.CompareTag("DeathZone") && respawnBehavior == RespawnBehavior.Alive))
            transform.position = originalPos;
    }
}
