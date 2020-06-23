using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
    public Animation anim;

    // Start is called before the first frame update
    void Start()
    {
        anim.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!anim.isPlaying)
        {
            anim.Rewind();
            anim.Play();
        }
        
    }
}
