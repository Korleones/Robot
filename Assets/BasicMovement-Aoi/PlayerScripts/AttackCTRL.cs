using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCTRL : MonoBehaviour
{

    public int damage;
    Animator Anim; 
    Transform RegionTrans;
    BoxCollider2D RegionColl;
    Rigidbody2D Rig;

    public bool canAttack = true;
    public float AttackDuration;
    [Header("状态")]
    public bool IsOnGround ;
    public bool IsClimbing;
    public bool IsDashing;
    [Header("collider位置")]
    public Vector3 upvec;
    public Vector3 upqua;
    public Vector3 downvec;
    public Vector3 downqua;
    [Header("击中效果")]
    public float UpEffection;
    public float DownEffection;
    public float LeftEffection;
    public float RightEffection;

    







    private void Awake()
    {
        Anim = GetComponent<Animator>();
        RegionTrans = GameObject.FindGameObjectWithTag("PlayerAttackRegion").GetComponent<Transform>();
        RegionColl=GameObject.FindGameObjectWithTag("PlayerAttackRegion").GetComponent<BoxCollider2D>();
        RegionColl.enabled = false;
        Rig = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
            
    }
    
    void Update()
    {
        IsOnGround = GetComponent<PlayerCTRL>().IsOnGround;
        IsClimbing = GetComponent<PlayerCTRL>().IsClimbing;
        IsDashing = GetComponent<PlayerCTRL>().IsDashing;
        
        if (canAttack)
        {
            Attack();
        }
        if(IsClimbing)
        {
            Anim.SetBool("Attack", false);
            canAttack = true;
        }
    }

    public void Attack()
    {
        if (Input.GetButtonDown("Attack") && !Anim.GetBool("Attack"))  
        {//按键且不在冲刺
            #region 在地上
            if (IsOnGround) {
                
                if (Input.GetAxisRaw("Vertical") > 0) {
                    TransToUp();
                    Anim.SetBool("Attack", true);   
                }
                else 
                {
                    Anim.SetBool("Attack", true);       //设置动画播放
                }
                
            }
            #endregion
            #region 在空中 
            else if (!IsOnGround&&!IsClimbing) 
            {

                if (Input.GetAxisRaw("Vertical") > 0)
                {
                    TransToUp();
                    Anim.SetBool("Attack", true);
                }
                else if (Input.GetAxisRaw("Vertical") < 0 && !IsClimbing)
                {
                    TransToDown();
                    Anim.SetBool ("Attack", true);
                }
                else if(!IsClimbing) 
                { 
                    Anim.SetBool("Attack", true);
                }
                    
            }
            #endregion


        }

    }
    
    #region 翻转Region
    public void TransToUp() {
        RegionTrans.transform.localPosition = upvec;
        RegionTrans.transform.localEulerAngles = upqua;

    }
    
    public void TransToDown()
    {
        RegionTrans.transform.localPosition = downvec;
        RegionTrans.transform.localEulerAngles = downqua;

    }
    public void Transback() {
        RegionTrans.transform.localPosition = new Vector3(0, 0, 0);
        RegionTrans.transform.localEulerAngles = new Vector3(0, 0, 0);

    }
    #endregion
    
    public void AttacktoIdle()
    {
        Anim.SetBool("Attack", false);
    }    
    #region 开始&结束攻击
    public void StartAttack() {
        canAttack = false;
        RegionColl.enabled = true;
        RegionTrans.position = new Vector3(RegionTrans.position.x, RegionTrans.position.y +     0.01f, RegionTrans.position.z);
    }
    public void EndAttack() {
        RegionColl.enabled= false;
        Transback();
        canAttack = true;
        Anim.SetBool("Attack", false);
    }
    public int count;
    #endregion

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
    //    {
    //        Debug.Log("这是什么呆瓜bug啊？");
    //        DamageCTRL.TakeDamage(collision.gameObject);
    //        GetComponent<PlayerCTRL>().WasDashed = false;
    //        if (Input.GetAxisRaw("Vertical") < 0)
    //        {
    //            Rig.velocity = new Vector2(Rig.velocity.x, DownEffection);
    //        }
    //        else if (Input.GetAxisRaw("Vertical") > 0)
    //        {
    //            Rig.velocity = new Vector2(Rig.velocity.x, Rig.velocity.y + UpEffection);
    //        }
    //    }
    //    if (collision.gameObject.CompareTag("ReversibleThorn"))
    //    {
    //        collision.gameObject.GetComponent<ReversiblThorn>().Refresh();
    //    }
    //    if (collision.gameObject.CompareTag("Tear"))
    //    {
    //        GetComponent<PlayerCTRL>().WasDashed = false;
    //        Destroy(collision.gameObject);
    //    }
    //}




}
