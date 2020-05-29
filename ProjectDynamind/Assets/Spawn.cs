using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public Vector3 position, size;
    public BoxCollider BoxCollider;


    // Start is called before the first frame update
    void Start()
    {
        this.BoxCollider.size = this.size;
        this.BoxCollider.center = this.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
