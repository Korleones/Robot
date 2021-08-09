using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : Platform
{
    public float resetTime;
    float Timer = 0f;
    bool TimerOpen = false;
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
            if(Timer > resetTime)
            {
                TimerOpen = false;
                Timer = 0;
                GetComponent<SpriteRenderer>().enabled = true;
                GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }
    public void Break()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        TimerOpen = true;
    }
}
