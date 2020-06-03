using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public string PlayerName = "Player";
    public bool isDead = false, isResetting = false;
    GameObject player;

    //TODO: See if somehow I can use the data-type Check_Point (would give a reference to the current check Point too)
    public Vector3 currentCheckpointPosition;
    

    // Start is called before the first frame update
    void Start()
    {
        //Find the player Object 
       // player = GameObject.Find(PlayerName);
        player = GameObject.FindWithTag("Player");

        //set the current Checkpoint position to the position of the SpawnPoint
        currentCheckpointPosition = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: See if I can use Triggers or events
        if (isResetting)
        {
            //set players position and the checkpoints position to the spawnpoints position
            player.transform.position = currentCheckpointPosition = gameObject.transform.position;
            isResetting = false;
        }
        else if (isDead)
        {
            //set the players position the last checkpoints position
            player.transform.position = currentCheckpointPosition;
            isDead = false;
        }
    }
}
