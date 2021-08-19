using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPCTRL : MonoBehaviour
{
    public int hp;
    public int mp;
    private int m_hp;
    private int m_mp;
    // Start is called before the first frame update
    void Start()
    {
        m_hp = hp;
        m_mp = mp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        
    }

}
