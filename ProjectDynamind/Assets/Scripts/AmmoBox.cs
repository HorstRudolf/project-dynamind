using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    double timer = 0;
    bool ammoLeft = true;

    // Update is called once per frame
    void Update()
    {
        // Allow player to reload only once every 15 seconds
        if (!ammoLeft)
        {
            timer -= Time.deltaTime;
        }
        if (timer <= 0)
        {
            ammoLeft = true;
            PlayerMovement.UpdateReload();
        }
        
    }

    public void Reload()
    {
        if (ammoLeft)
        {
            DynamiteMechanics.AddAmmo();
            timer = 15;
            ammoLeft = false;
        }      
    }
}
