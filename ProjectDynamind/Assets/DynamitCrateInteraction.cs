using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamitCrateInteraction : MonoBehaviour
{
    public float reachForAction = 4.0f;
    public string tagOfObject = "Player";
    GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag(tagOfObject);
    }

    // Update is called once per frame
    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && Vector3.Distance(player.transform.position, gameObject.transform.position) < reachForAction)
        {
            //TODO: Dynamit counter ehöhen
        }
    }
}
