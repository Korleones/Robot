using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : Platform
{
    public float resetTime;
    public float bufferTime;
    float Timer = 0f;
    bool TimerOpen = false;
    bool isBroken = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if(TimerOpen)
        {
            Timer += Time.fixedDeltaTime;
            if (!isBroken && Timer > bufferTime)
            {
                GetComponent<MeshRenderer>().enabled = false;
                GetComponent<BoxCollider2D>().enabled = false;
                isBroken = true;
                Timer = 0;
            }
            if (isBroken && Timer > resetTime)
            {
                isBroken = false;
                TimerOpen = false;
                Timer = 0;
                GetComponent<MeshRenderer>().enabled = true;
                GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }
    public void Break()
    {
        TimerOpen = true;
    }
}
