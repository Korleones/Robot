using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public Vector3 initialPosition;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = initialPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
