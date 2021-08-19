using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DamageCTRL
{
    public static void TakeDamage(GameObject enemy)
    {
        if(enemy.CompareTag("Cryer"))
        {
            enemy.GetComponent<Cryer.CryerFSM>().parameter.getHit = true;
        }
        else if(enemy.CompareTag("LaserShooter"))
        {
            enemy.GetComponent<LaserShooter.LSFSM>().parameter.getHit = true;
        }
    }
}
