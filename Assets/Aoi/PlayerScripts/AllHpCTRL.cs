using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllHpCTRL : MonoBehaviour
{
    public int MaxHp,MaxMp,CurHp, CurMp;
    public bool IsDead = false;



    Animator Anim;
    // Start is called before the first frame update
    void Start()
    {
        CurHp = MaxHp;
        CurMp = MaxMp;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        
    }

    public void TakeDamage(int damage)//受伤

    {
        CurHp -= damage;
        //Debug.Log(CurHp);
        if (Death()) return;
    }


    public bool Death() {//死了嘛？
        if (CurHp <= 0)
        {
           // Anim.SetTrigger("Death");//其实这个可以改成bool比较好
            IsDead=true;
            Destroy(gameObject);
            return true;

        }
        IsDead= false;
        return false;
    }



}
