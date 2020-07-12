using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DynamitCrateInteraction : MonoBehaviour
{
    public float reachForAction = 4.0f;
    public string tagOfObject = "Player";
    GameObject player;
    float reloadTimer = 0;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag(tagOfObject);
    }
    private void Update()
    {
        reloadTimer -= Time.deltaTime;
    }

    public void EquipDynamite()
    {
        if (player.GetComponent<DynamiteMechanics>().enabled == false)
        {
            player.GetComponent<DynamiteMechanics>().enabled = true;
            reloadTimer = 15;
        }

        else
        {
            if (reloadTimer<= 0)
            {
                DynamiteMechanics.AddAmmo();
                reloadTimer = 15;
            }
        }
        
        
    }
}
