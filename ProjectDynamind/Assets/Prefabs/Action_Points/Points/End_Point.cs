using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[RequireComponent(typeof(BoxCollider))]
public class End_Point : MonoBehaviour
{
    private BoxCollider boxCollider;
    public string nextScene;
    
    public string trigger_Message;

    // Start is called before the first frame update
    void Start()
    {
        //set BoxCollider to is Triggerd so Objects don't Collide
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;

        //if the scene is empty throw an except because then nothing can be loaded
        if (nextScene == "")
            throw new NullReferenceException($"You need to set a Scene in {gameObject.name}. Scene was null");
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            string adress = "";
            Transform currentObject = gameObject.transform;

            //Print out the complete adress of Objects (parents of child)
            while (currentObject != null) 
            {
                adress = $"{currentObject.name}/{adress}";
                currentObject = currentObject.transform.parent;
            } 

            Debug.Log($"Sent by \"{adress}\": load:{nextScene}");


            //Load the scene thats name is the String in nextScene 
            //Important!!! has to be added under File->Build Settings...->Scenes in Build
            SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
