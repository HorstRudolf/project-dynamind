using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Death_Zone : MonoBehaviour
{
    private BoxCollider BoxCollider;
    private Respawn SpawnPoint;


    // Start is called before the first frame update
    void Start()
    {
        BoxCollider = GetComponent<BoxCollider>();
        BoxCollider.isTrigger = true;

        SpawnPoint = GameObject.Find("Spawn_Point").GetComponent<Respawn>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            string adress = "";
            Transform currentObject = gameObject.transform;

            while (currentObject != null)
            {
                adress = $"{currentObject.name}/{adress}";
                currentObject = currentObject.transform.parent;
            }

            Debug.Log($"Sent by \"{adress}\": Death at: {gameObject.name}");

            //TODO: swap with trigger.
            SpawnPoint.isDead = true;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
