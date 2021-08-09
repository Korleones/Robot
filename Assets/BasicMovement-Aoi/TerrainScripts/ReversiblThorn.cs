using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ReversiblThorn : Thorn
{
    public float turnDuration;
    public float turnOffsetDuration;        //平台翻转前的缓冲时间
    bool turnTimerOpen = false;
    float turnTimer = 0f;
    int turnDirection = 1;
    float turnOffset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            TurnOver();
        }
    }
    private void FixedUpdate()
    {
        if (turnTimerOpen)
        {
            turnTimer += Time.deltaTime;
            if (turnTimer > turnOffsetDuration)
            {
                transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + 180 * turnDirection / turnDuration * Time.fixedDeltaTime);
                if (turnDirection > 0 && (180 - transform.eulerAngles.z) < 2)
                {
                    transform.eulerAngles = new Vector3(0, 0, 180);
                    turnDirection *= -1;
                    turnTimerOpen = false;
                    turnTimer = 0;
                }
                else if (turnDirection < 0 && transform.eulerAngles.z < 2)
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    turnDirection *= -1;
                    turnTimerOpen = false;
                    turnTimer = 0;
                }
            }
        }
    }
    public void TurnOver()
    {
        if(!turnTimerOpen)
        {
            turnTimerOpen = true;
        }
    }
}
