using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ReversiblThorn : Thorn
{
    public float turnDuration;
    public float turnOffsetDuration;        //平台翻转前的缓冲时间
    public float resetOffsetDuration;        //平台翻转前的缓冲时间
    bool turnTimerOpen = false;
    float turnTimer = 0f;
    bool resetTimerOpen = false;
    float resetTimer = 0f;
    int turnDirection = 1;
    public float bufferTime;
    int status = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void FixedUpdate()
    {
        if (turnTimerOpen)
        {
            status = 2;
            turnTimer += Time.deltaTime;
            if (turnTimer > turnOffsetDuration)
            {
                transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + 180 * turnDirection / turnDuration * Time.fixedDeltaTime);
                if (turnDirection > 0 && (180 - transform.eulerAngles.z) < 2)
                {
                    status = 1;
                    transform.eulerAngles = new Vector3(0, 0, 180);
                    turnDirection *= -1;
                    turnTimerOpen = false;
                    turnTimer = 0;
                }
            }
        }
        if(resetTimerOpen)
        {
            status = 2;
            resetTimer += Time.deltaTime;
            if (resetTimer > resetOffsetDuration)
            {
                transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + 180 * turnDirection / turnDuration * Time.fixedDeltaTime);
                if (turnDirection < 0 && transform.eulerAngles.z < 2)
                {
                    status = 0;
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    turnDirection *= -1;
                    resetTimerOpen = false;
                    resetTimer = 0;
                }
            }
        }
    }
    public void TurnOver()
    {
        if (status == 0)
             turnTimerOpen = true;
    }
    public void Refresh()
    {
        if(status == 1)
            resetTimerOpen = true;
    }
}
