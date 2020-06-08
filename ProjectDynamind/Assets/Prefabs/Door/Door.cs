using Boo.Lang.Environments;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEditor.UIElements;
using UnityEngine;

public class Door : MonoBehaviour
{
    float journeyLenght, startTime;
    public float speed = 1;
    public Vector3 endPos, startPos;
    public AnimationCurve speeds;

    public float distanceCovered;

    bool open = false;
    

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        endPos = transform.position + new Vector3(0, - transform.lossyScale.y, 0);
        journeyLenght = Vector3.Distance(transform.position, endPos);
        
    }

    public void Open()
    {
        open = true;
    }

    public void Close()
    {
        open = false;
    }
        
        
    // Update is called once per frame
    void Update()
    {
        distanceCovered = Vector3.Distance(transform.position, endPos) / journeyLenght ;

        if (open && transform.position.y > endPos.y)
            transform.position += new Vector3(0, -speeds.Evaluate(distanceCovered), 0) * Time.deltaTime; 
        else if (!open && transform.position.y < startPos.y)
            transform.position += new Vector3(0, speeds.Evaluate(distanceCovered), 0) * Time.deltaTime;
    }
}
