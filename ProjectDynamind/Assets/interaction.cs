using System;
using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;

public class interaction : MonoBehaviour
{
    float progress = 0f;
    public float reachForAction = 2.0f;
    public int turnInDegrees = 360;
    public GameObject bridge, player;
    public string tagOfObject = "Player";
    Vector3 rotationPoint;




    // Start is called before the first frame update
    void Start()
    {
        rotationPoint = transform.position + Vector3.down * transform.localScale.y/2;
        player = GameObject.FindGameObjectWithTag(tagOfObject);
    }

    private void OnMouseOver()
    {
        if (Input.GetKey(KeyCode.E) && Vector3.Distance(player.transform.position, gameObject.transform.position) < reachForAction)
        {
            progress = Mathf.Clamp01( progress+ 0.001f);
            transform.rotation = Quaternion.Euler(new Vector3(0,0,90) + new Vector3(turnInDegrees, 0, 0) * progress);
            bridge.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0) * progress);
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        

    }
}
