using LaserShooter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public GameObject laseShooter;
    private Parameter parameter;
    private bool hit;
    
    // Start is called before the first frame update
    void Start()
    {
        parameter = laseShooter.GetComponent<LSFSM>().parameter;
    }

    // Update is called once per frame
    void Update()
    {
        if (parameter.shooting && hit)
        {
            //让玩家受伤
        }
        if (!parameter.shooting && parameter.hasShot)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            hit = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            hit = false;
        }
    }
}
