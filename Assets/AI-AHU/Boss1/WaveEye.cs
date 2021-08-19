using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEye : MonoBehaviour
{
    public float moveSpeed;
    public float attackspeed;
    public Animator animator;
    public Rigidbody2D rb;
    private Vector3 position,p1,p2;
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        position = transform.position;
    }

    
    void Update()
    {
        
    }
    public void WaveAttack(float attackPositionx, float attackPositiony)
    {
        p1 = new Vector3(position[0], attackPositiony, 0);
        p2 = new Vector3(attackPositionx, attackPositiony, 0);
        ReactMove(attackPositionx, attackPositiony);
        AttackBehaviour(attackPositionx, attackPositiony);
        RestoreMove();
    }
    void ReactMove(float attackPositionx, float attackPositiony)
    {
        rb.velocity = moveSpeed*(p1 - position);
        animator.Play("React");
        while (Vector3.Distance(transform.position, p1) > 0.1f) { }//存疑
    }
    void AttackBehaviour(float attackPositionx, float attackPositiony)
    {
        rb.velocity = moveSpeed * (p2 - transform.position);
        animator.Play("Attack");
        while (Vector3.Distance(transform.position, p2) > 0.1f) { }//存疑
    }
    void RestoreMove()
    {
        rb.velocity = moveSpeed * (position - transform.position);
        while (Vector3.Distance(transform.position, position) > 0.1f) { }//存疑
    }
    void OnCollisionEnter(Collision other)
    {
        if(string.Equals(other.gameObject.tag, "Player")){
            //击中
        }
    }

}
