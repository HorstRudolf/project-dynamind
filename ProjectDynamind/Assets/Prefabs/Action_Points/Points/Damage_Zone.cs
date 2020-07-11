using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Damage_Zone : MonoBehaviour
{
    // Start is called before the first frame update
    BoxCollider BoxCollider;
    public bool triggered = false;
    float lastDamage;
    PlayerMovement playerMovement;

    public int damageAmount = 1;
    public float pause = 1;


    void Start()
    {
        BoxCollider = GetComponent<BoxCollider>();
        BoxCollider.isTrigger = true;
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();


        lastDamage = Time.time;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player") && Time.time - lastDamage >= pause)
        {
            string adress = "";

            Transform currentObject = gameObject.transform;

            while (currentObject != null)
            {
                adress = $"{currentObject.name}/{adress}";
                currentObject = currentObject.transform.parent;
            }

            Debug.Log($"Sent by \"{adress}\": Damage Zone Entered: {gameObject.name}");
        }
    }


    private void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player") && Time.time - lastDamage >= pause)
        {
            lastDamage = Time.time;
            playerMovement.TakeDamage(damageAmount);
        }
    }
}
