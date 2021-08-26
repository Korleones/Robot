using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AllHpCTRL : MonoBehaviour
{
    public int MaxHp, MaxMp, CurHp, CurMp;
    public bool IsDead = false;
    public float restartOffsetTime;

    bool restartTimer = false;
    float restartTimeCount = 0f;

    public Animator Anim;
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
        if (restartTimer)
        {
            restartTimeCount += Time.fixedDeltaTime;
            if (restartTimeCount > restartOffsetTime)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    public void TakeDamage(int damage)//受伤

    {
        CurHp -= damage;
        //Debug.Log(CurHp);
        if (Death()) return;
    }


    public bool Death()
    {//死了嘛？
        if (CurHp <= 0)
        {
            // Anim.SetTrigger("Death");//其实这个可以改成bool比较好
            IsDead = true;
            PlayerCTRL.Player.isDeath = true;
            PlayerCTRL.Player.MoveDisabled();
            PlayerCTRL.Player.canFall = false;
            PlayerCTRL.Player.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Anim.SetBool("IsDeath", true);
            restartTimer = true;
            return true;

        }
        IsDead = false;
        return false;
    }
}

