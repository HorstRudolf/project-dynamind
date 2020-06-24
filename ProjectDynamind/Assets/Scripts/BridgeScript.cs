using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeScript : MonoBehaviour
{
    public GameObject bridgeObject;
    Animator bridge;

    void Start()
    {
        bridge = bridgeObject.GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Debug.Log("Bridge Triggered");
            bridge.SetTrigger("BridgeTrig");
        }
    }
}
