using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;


// Only Runs when Unity is in Editor Mode
[ExecuteInEditMode]
public class Render_Boxes : MonoBehaviour
{
   
    GameObject[] gameObjects;
    public Color SpawnColor, CheckpointColor, EndpointColor, StandardColor, FrameColor;



    //Draws A cube arround every Triggerzone
    void OnDrawGizmos()
    { 

        Transform[] Transforms = GetComponentsInChildren<Transform>();

        foreach (Transform child in Transforms)
        {
            if (child != null)
            {

                switch (child.name)
                {
                    case string a when a.Contains("Spawn_Point"):
                        Gizmos.color = SpawnColor;
                        break;
                    case string a when a.Contains("Check_Point"):
                        Gizmos.color = CheckpointColor;
                        break;
                    case string a when a.Contains("End_Point"):
                        Gizmos.color = EndpointColor;
                        break;
                    default:
                        Gizmos.color = StandardColor;
                        break;
                }

                //states before its drawn what dimensions the cube has
                Gizmos.matrix = child.localToWorldMatrix;

                //Draws the cube that is specified in Gizmos.matrix
                Gizmos.DrawCube(Vector3.zero, Vector3.one);
                Gizmos.color = FrameColor;
                Gizmos.DrawWireCube(Vector3.zero, Vector3.one);

            }
        }
    
    }
}
