using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Check_Point : MonoBehaviour
{
    BoxCollider boxCollider;
    Respawn spawn_Point;

    // Start is called before the first frame update
    void Start()
    {
        //set BoxCollider to is Triggerd so Objects don't Collide
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;

        //fetch the spawnpoint object so it can be referenced Later
        spawn_Point = GameObject.Find("Spawn_Point").GetComponent<Respawn>();
    }


    private void OnTriggerEnter(Collider col)
    {
        //if the object entered the collider and Triggered this event is tagged as player 
        if (col.tag == "Player")
        {
            string adress = "";
            Transform currentObject = gameObject.transform;

            //Print out the complete Adress of Objects (parents of child)
            while (currentObject != null)
            {
                adress = $"{currentObject.name}/{adress}";
                currentObject = currentObject.transform.parent;
            }

            Debug.Log($"Sent by \"{adress}\": Checkpoint at: {gameObject.name}");

            /*IDEA: maybe make sure player always respawns on the bottom of the BoxCollider
             * (TransformYposOfBoxCollider - TransformHeightOfBoxColider/2 + TransformHeightOfPlayer/2)
            */
            /*
             * IDEA: Save coordinates of player as he enters the checkpoint instead of coordinates 
             * (could lead to problems with clipping through walls and more importantly floors 
             * -> you would be able to walk through them and fall down)
            */
            //sets the new spawnpoint to the checkpoints position
            spawn_Point.currentCheckpointPosition = gameObject.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
