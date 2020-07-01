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

    public void EquipDynamite()
    {
        player.GetComponent<DynamiteMechanics>().enabled = true;
    }
}
