using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    public float PlateSpeed = 1.3f;
    public UnityEvent onUnpress, onPress;

    bool pressed = false;
    int colCount = 0;
    Vector3 oldPos;

    void Start()
    {
        oldPos = gameObject.transform.position;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
            colCount++;        
        else if (col.CompareTag("Heavy"))
            colCount++;
     
        if (colCount > 0 && !pressed)
        {
            onPress.Invoke();
            pressed = true;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
            colCount--;

        else if (col.CompareTag("Heavy"))
            colCount--;

        if (colCount <= 0 && pressed)
        {
            onUnpress.Invoke();
            pressed = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pressed)
        {
            if (gameObject.transform.position.y > ( oldPos.y -gameObject.transform.lossyScale.y/2))
                transform.position += new Vector3(0, -PlateSpeed, 0) * gameObject.transform.lossyScale.y * Time.deltaTime;
        }
        else
        {
            if(gameObject.transform.position.y < oldPos.y)
                transform.position += new Vector3(0, PlateSpeed, 0) * gameObject.transform.lossyScale.y * Time.deltaTime;
        }
    }
}
