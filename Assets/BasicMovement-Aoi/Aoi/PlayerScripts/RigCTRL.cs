using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigCTRL : MonoBehaviour
{
    public float gravityAccelerate;
    public bool gravitySwitch;
    Vector3 velocity = Vector3.zero;

    private void FixedUpdate()
    {
        GravityEffect();
    }

    private void GravityEffect()
    {
        if(gravitySwitch)
        {
            //velocity += gravityAccelerate * Time.fixedDeltaTime;
            //float offset = Time.fixedDeltaTime * Time.fixedDeltaTime * gravityAccelerate + velocity * Time.fixedDeltaTime;
            //transform.position = new Vector3(transform.position.x, transform.position.y - offset, transform.position.z);
        }
    }
    //public void SetVelocity(Vector3 )
    //{
    //    velocity = 0;
    //}
}
