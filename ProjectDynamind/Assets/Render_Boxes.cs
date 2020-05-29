using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


// Only Runs when Unity is in Editor Mode
[ExecuteInEditMode]
public class Render_Boxes : MonoBehaviour
{
   
    GameObject[] gameObjects;



    //Draws A cube arround every Triggerzone
    void OnDrawGizmos()
    {
        int childCount = gameObject.transform.childCount;
        Transform[] Transforms = GetComponentsInChildren<Transform>();

        foreach (Transform child in Transforms)
        {
            if (child != null)
            {

                switch (child.name)
                {
                    case string a when a.Contains("Spawn_Point"):
                        Gizmos.color = new Color(0, 1, 0, 0.5f);
                        break;
                    case string a when a.Contains("Check_Point"):
                        Gizmos.color = new Color(1, 1, 0, 0.5f);
                        break;
                    case string a when a.Contains("End_Point"):
                        Gizmos.color = new Color(1, 0, 0, 0.5f);
                        break;
                    default:
                        Gizmos.color = new Color(1, 1, 1, 0.5f);
                        break;

                }
                
                Gizmos.DrawCube(child.position, child.lossyScale);
            }
        }
    
    }
}
