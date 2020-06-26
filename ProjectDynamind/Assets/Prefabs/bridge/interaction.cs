using System;
using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class interaction : MonoBehaviour
{
    public float reachForAction = 4.0f;
    public string triggerName;
    public GameObject bridge, lever;
    GameObject player;
    Animator animator;
    public string tagOfObject = "Player";




    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag(tagOfObject);
        animator = bridge.GetComponent<Animator>();
    }

   

    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.E) && Vector3.Distance(player.transform.position, gameObject.transform.position) < reachForAction)
        {
            lever.transform.rotation = Quaternion.Euler(lever.transform.rotation.eulerAngles.x, lever.transform.rotation.eulerAngles.y + 180, lever.transform.rotation.eulerAngles.z);

            //TODO: trigger für den animator
            animator.SetTrigger(triggerName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        

    }
}
