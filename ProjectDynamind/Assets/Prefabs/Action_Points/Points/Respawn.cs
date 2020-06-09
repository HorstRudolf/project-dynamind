using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public string PlayerName = "Player";
    public bool isDead = false, isResetting = false;
    CharacterController player;

    //TODO: See if somehow I can use the data-type Check_Point (would give a reference to the current check Point too)
    public Vector3 currentCheckpointPosition;
    

    // Start is called before the first frame update
    void Start()
    {
        //Find the player Object 
        // player = GameObject.Find(PlayerName);
        GameObject current = GameObject.FindWithTag("Player");
        player = current.GetComponentInParent<CharacterController>();

        //set the current Checkpoint position to the position of the SpawnPoint
        currentCheckpointPosition = gameObject.transform.position;
    }

    public void Resets()
    {
        player.enabled = false;
        player.transform.position = currentCheckpointPosition = gameObject.transform.position;
        player.enabled = true;
        isResetting = false;
    }

    public void Kill()
    {
        player.enabled = false;
        player.transform.position = currentCheckpointPosition;
        player.enabled = true;
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: see if keycodes could be swapped with custome keycodes to better remap them (e.g. Tastaturzuweiseung)
        if (Input.GetKeyDown(KeyCode.F1))
            Kill();
        else if (Input.GetKeyDown(KeyCode.F2))
            Resets();
    }
}
