using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCTRL : MonoBehaviour
{
    public bool canGetDamage = true;
    public bool isDamaging = false;
    public float GDMaxForce;
    public float GDDuration;
    public float GDForce;
    public float GDWaitTime;
    public float IneffectiveTime;
    Vector2 GDDir;

    [Header("移动")]
    public float WalkSpeed;
    public float AccelerateTime;
    public float DecelerateTime;
    bool CanMove = true;
    [Header("跳跃")]
    public float JumpingSpeed;
    public float FallMultiplier;
    public float LowJumpMultiplier;
    public float advancedTime;
    public bool CanJump = true;
    public float gravityScale;
    [Header("触地判定")]
    public Vector2 PointOffset;
    public Vector2 GroundSize;
    public LayerMask GroundLayerMask;
    bool GravityModifier = true;
    [Header("冲刺")]
    public float DashForce;
    public float DashMaxForce;
    public float DashDuration;
    public float DashWaitTime;
    public bool IsDashing = false;
    bool WasDashed;
    Vector2 DashingDir;
    [Header("爬墙")]
    public Vector2 LeftWallPointOffset;
    public Vector2 RightWallPointOffset;
    public LayerMask WallLayerMask;
    public Vector2 WallSize;
    public float WallWaitTime;
    public float WallVelocity;
    public float WallAccelerateTime;
    public float holdingTimeOffset;
    [Header("爬墙跳")]
    Vector2 ClimbingJumpDir;
    public float ClimbingJumpForce;
    public float ClimbingJumpMaxForce;
    public float ClimbingJumpDuration;
    public float ClimbingJumpWaitTime;
    public Vector2 ClimbingJumpSpeed;
    public float CJOffsetTime;
    [Header("测试，请勿修改或调用")]
    int FaceToRight = 1;
    Rigidbody2D Rig;
    float velocityX;
    public bool IsOnGround;
    public bool IsJumping;
    public bool IsClimbing;
    bool IsBesideRightWall;
    bool IsBesideLeftWall;
    public int IsWallJumping = 0;
    bool ClimbingAccelerateTimerOpen = false;
    float ClimbingAccelerateTimer = 0f;
    bool ClimbingJumpTimerOpen = false;
    float ClimbingJumpTimer = 0f;
    bool JumpingTimerOpen = false;  //跳跃手感优化计时器
    float JumpingTimer = 0f;
    public int LatestCJStatus = 0;
    public bool isCJumped = false;   //用于记录上一刻在右墙还是左墙滑落
    bool canCJump = true;
    Animator m_animator;


    private void Awake()
    {
        Rig = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
    }
    private void Start()
    {
        Rig.gravityScale = gravityScale;
    }
    
    private void Update()
    {
        #region 初始化
        if (Input.GetAxisRaw("Horizontal") > 0 && !IsClimbing)
        {
            FaceToRight = 1;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (Input.GetAxisRaw("Horizontal") < 0 && !IsClimbing)
        {
            FaceToRight = -1;
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        IsOnGround = OnGround();     //判断当前角色是否碰到地面
        IsBesideRightWall = BesideRightWall();    //判断当前角色是否碰到右边的墙
        IsBesideLeftWall = BesideLeftWall();    //判断当前角色是否碰到左边的墙
        if (!IsOnGround && (IsBesideRightWall || IsBesideLeftWall)) 
        {
            m_animator.SetBool("Climbing", true);
            IsClimbing = true;
            if(IsBesideLeftWall)
            {
                LatestCJStatus = -1;
            }
            else if(IsBesideRightWall)
            {
                LatestCJStatus = 1;
            }
        }
        else
        {
            m_animator.SetBool("Climbing", false);
            ClimbingAccelerateTimerOpen = false;
            ClimbingAccelerateTimer = 0f;
            IsClimbing = false;
        }
        #endregion

        #region 爬墙细节优化：下落加速
        if (ClimbingAccelerateTimerOpen)
        {
            ClimbingAccelerateTimer += Time.deltaTime;
        }
        #endregion

        #region 跳跃手感优化
        if (JumpingTimerOpen)
        {
            JumpingTimer += Time.deltaTime;
            if(JumpingTimer > advancedTime)
            {
                if(IsOnGround)
                {
                    Rig.velocity = new Vector2(Rig.velocity.x / 10f, JumpingSpeed);
                }
                JumpingTimer = 0f;
                JumpingTimerOpen = false;
            }

        }
        #endregion
        
        #region 爬墙
        if (IsClimbing && Input.GetAxis("Vertical") >= 0)
        {
            if (Rig.velocity.y < 0)
            {
                ClimbingAccelerateTimerOpen = true;
                if(ClimbingAccelerateTimer >= 0.5f)
                {
                    Rig.velocity = new Vector2(Rig.velocity.x, -WallVelocity * ClimbingAccelerateTimer * ClimbingAccelerateTimer * 4f);
                }
                else 
                    Rig.velocity = new Vector2(Rig.velocity.x, -WallVelocity);
            }
        }
        #endregion

        #region 爬墙手感优化
        if (IsClimbing)
        {
            if ((IsBesideLeftWall && Input.GetKeyDown(KeyCode.D)) || (IsBesideRightWall && Input.GetKeyDown(KeyCode.A)))
            {
                ClimbingJumpTimerOpen = true;
                CanMove = false;
            }
            else if(Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
            {
                ClimbingJumpTimerOpen = false;
                ClimbingJumpTimer = 0;
                CanMove = true;
            }
        }
        if (ClimbingJumpTimerOpen) 
        {
            ClimbingJumpTimer += Time.deltaTime;
            if (ClimbingJumpTimer > CJOffsetTime)
            {
                ClimbingJumpTimerOpen = false;
                ClimbingJumpTimer = 0;
                CanMove = true;
            }
        }
        #endregion

        #region 跳跃
        if (CanJump)
        {
            if (Input.GetButtonDown("Jump") && !IsJumping && IsOnGround)
            {
                m_animator.SetBool("Jump", true);
                Rig.velocity = new Vector2(Rig.velocity.x / 10f, JumpingSpeed);
                IsJumping = true;
            }
            else if (Input.GetButtonDown("Jump") && Rig.velocity.y < 0 && !IsOnGround && !IsClimbing)
            {
                JumpingTimerOpen = true;
            }
            if (IsOnGround && Input.GetAxis("Jump") == 0 || Rig.velocity.y < 0)
            {
                m_animator.SetBool("Jump", false);
                IsJumping = false;
            }
        }
        //小跳
        if (CanMove && Input.GetButtonUp("Jump") && Rig.velocity.y > 0)
        {
            Rig.velocity = new Vector2(Rig.velocity.x, 0);
        }
        #endregion

        #region 爬墙跳
        if (Input.GetButtonDown("Jump") && IsClimbing)
        {
            if (canCJump && !isCJumped && IsBesideLeftWall)
            {
                isCJumped = true;
                CanMove = true;
                IsWallJumping = 1;
                ClimbingJumpDir = new Vector2(ClimbingJumpSpeed.x * IsWallJumping, ClimbingJumpSpeed.y);
                //将玩家当前所有的动量清零
                Rig.velocity = Vector2.zero; 
                //施加一个力，让玩家飞出去
                Rig.velocity += ClimbingJumpDir * ClimbingJumpForce;
                //Rig.velocity = new Vector2(Rig.velocity.x / 2, JumpingSpeed);
                StartCoroutine(ClimbingJump());
            }
            else if (canCJump && !isCJumped && IsBesideRightWall)
            {
                isCJumped = true;
                CanMove = true;
                IsWallJumping = -1;
                ClimbingJumpDir = new Vector2(ClimbingJumpSpeed.x * IsWallJumping, ClimbingJumpSpeed.y);
                //将玩家当前所有的动量清零
                Rig.velocity = Vector2.zero;
                //施加一个力，让玩家飞出去
                Rig.velocity += ClimbingJumpDir * ClimbingJumpForce;
                //Rig.velocity = new Vector2(Rig.velocity.x / 2, JumpingSpeed);
                StartCoroutine(ClimbingJump());
            }
            else if(canCJump)
            {
                canCJump = false;
            }
        }
        #endregion

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            GetDamage();
        }
    }
    void FixedUpdate()
    {
        #region 初始化
        m_animator.SetFloat("VelocityX", Rig.velocity.x);
        m_animator.SetFloat("VelocityY", Rig.velocity.y);
        m_animator.SetInteger("Horizontal", (int)Input.GetAxisRaw("Horizontal"));
        m_animator.SetInteger("Vertical", (int)Input.GetAxisRaw("Vertical"));
        #endregion

        #region 左右移动
        if (CanMove) 
        {
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                Rig.velocity = new Vector2(Mathf.SmoothDamp(Rig.velocity.x, WalkSpeed * Time.fixedDeltaTime * 60, ref velocityX, AccelerateTime), Rig.velocity.y);
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                Rig.velocity = new Vector2(Mathf.SmoothDamp(Rig.velocity.x, WalkSpeed * Time.fixedDeltaTime * -60, ref velocityX, AccelerateTime), Rig.velocity.y);
            }
            else
            {
                Rig.velocity = new Vector2(Mathf.SmoothDamp(Rig.velocity.x, 0, ref velocityX, DecelerateTime), Rig.velocity.y);
            }
            //else
            //{
            //    if (Input.GetAxisRaw("Horizontal") > 0)
            //    {
            //        Rig.velocity = new Vector2(Mathf.SmoothDamp(Rig.velocity.x, WalkSpeed * Time.fixedDeltaTime * 30, ref velocityX, AccelerateTime), Rig.velocity.y);
            //        //SmoothDamp四个参数分别为：当前速度，目标速度，加速度，用多长时间从0达到目标速度
            //    }
            //    else if (Input.GetAxisRaw("Horizontal") < 0)
            //    {
            //        Rig.velocity = new Vector2(Mathf.SmoothDamp(Rig.velocity.x, WalkSpeed * Time.fixedDeltaTime * -30, ref velocityX, AccelerateTime), Rig.velocity.y);
            //    }
            //    else
            //    {
            //        Rig.velocity = new Vector2(Mathf.SmoothDamp(Rig.velocity.x, 0, ref velocityX, DecelerateTime), Rig.velocity.y);
            //    }
            //}
        }
        #endregion

        #region 重力调整器
        if (GravityModifier)
        {
            if (Rig.velocity.y < 0)    //当玩家下坠的时候
            {
                Rig.velocity += Vector2.up * Physics2D.gravity.y * (FallMultiplier - 1) * Time.fixedDeltaTime * 1.5f;
            }
            else if (Rig.velocity.y > 0 && Input.GetAxis("Jump") != 1)       //当玩家松开跳跃键的时候
            {
                Rig.velocity += Vector2.up * Physics2D.gravity.y * (LowJumpMultiplier - 1) * Time.fixedDeltaTime * 1.5f;
            }
        }
        #endregion

        #region 冲刺
        if (Input.GetAxis("Dash") == 1 && !WasDashed)
        {
            WasDashed = true;
            DashingDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (DashingDir.x == 0 && DashingDir.y == 0)
            {
                DashingDir.x = FaceToRight;
            }
            if (Input.GetAxis("Vertical") == 0 || Input.GetAxis("Horizontal") == 0) 
            {
                DashingDir *= 1.414f;
            }
            //将玩家当前所有的动量清零
            Rig.velocity = Vector2.zero;
            //施加一个力，让玩家飞出去
            Rig.velocity += DashingDir * DashForce;
            StartCoroutine(Dash());
        }

        if (IsOnGround && Input.GetAxisRaw("Dash") == 0)
        {
            WasDashed = false;
        }
        #endregion

        #region 玩家受伤
        if(canGetDamage)
        {
            if(Input.GetKeyDown(KeyCode.P))
            {
                GetDamage();
            }
        }
        #endregion
    }
    
    

    IEnumerator Dash()
    {
        IsDashing = true;
        //关闭玩家的移动和跳跃的功能
        CanMove = false;
        CanJump = false;
        //关闭重力调整器
        GravityModifier = false;
        //关闭重力影响
        Rig.gravityScale = 0;
        //施加空气阻力(Rigidbody.Drag)
        DOVirtual.Float(DashMaxForce, 0, DashDuration, (x) => Rig.drag = x);
        //等待一段时间
        yield return new WaitForSeconds(DashWaitTime);
        //开启所有关闭的东西
        CanMove = true;
        CanJump = true;
        GravityModifier = true;
        Rig.gravityScale = gravityScale;
        IsDashing = false;
    }
    IEnumerator ClimbingJump()
    {
        m_animator.SetBool("ClimbingJump", true);
        //关闭玩家的移动和跳跃的功能
        CanMove = false;
        CanJump = false;
        //关闭重力调整器
        GravityModifier = false;
        //关闭重力影响
        Rig.gravityScale = 0;
        //施加空气阻力(Rigidbody.Drag)
        DOVirtual.Float(ClimbingJumpMaxForce, 0, ClimbingJumpDuration, (x) => Rig.drag = x);
        //等待一段时间
        yield return new WaitForSeconds(ClimbingJumpWaitTime);
        //开启所有关闭的东西
        CanMove = true;
        CanJump = true;
        GravityModifier = true;
        Rig.gravityScale = gravityScale;
        m_animator.SetBool("ClimbingJump", false);
        
    }
    IEnumerator GetDamageIE()
    {
        m_animator.SetBool("Hurt", true);
        //关闭玩家的移动和跳跃的功能
        CanMove = false;
        CanJump = false;
        //关闭重力调整器
        GravityModifier = false;
        //关闭重力影响
        Rig.gravityScale = 0;
        //施加空气阻力(Rigidbody.Drag)
        DOVirtual.Float(GDMaxForce, 0, GDDuration, (x) => Rig.drag = x);
        //等待一段时间
        yield return new WaitForSeconds(GDWaitTime);
        //开启所有关闭的东西
        CanMove = true;
        CanJump = true;
        GravityModifier = true;
        Rig.gravityScale = gravityScale;
        yield return new WaitForSeconds(IneffectiveTime);
        isDamaging = false;
        m_animator.SetBool("Hurt", false);
    }

    public void GetDamage()
    {
        if (!isDamaging)
        {
            isDamaging = true;
            GDDir = new Vector2(-FaceToRight, 1);
            //将玩家当前所有的动量清零
            Rig.velocity = Vector2.zero;
            //施加一个力，让玩家飞出去
            Rig.velocity += GDDir * GDForce;
            StartCoroutine(GetDamageIE());
        }
    }

    bool OnGround()
    {
        Collider2D Coll = Physics2D.OverlapBox((Vector2)transform.position + PointOffset, GroundSize, 0, GroundLayerMask);
        if (Coll != null)
        {
            canCJump = true;
            isCJumped = false;
            LatestCJStatus = 0;
            //CanMove = true;
            m_animator.SetBool("IsOnGround", true);
            IsWallJumping = 0;
            return true;
        }
        else
        {
            m_animator.SetBool("IsOnGround", false);
            return false;
        }
    }
    bool BesideRightWall()
    {
        Collider2D Coll = Physics2D.OverlapBox((Vector2)transform.position + RightWallPointOffset, WallSize, 0, WallLayerMask);
        if (Coll != null)
        {
            if (isCJumped && LatestCJStatus == IsWallJumping)
            {
                isCJumped = false;
            }
            FaceToRight = 1;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            return true;
        }
        else
        {
            return false;
        }
    }
    bool BesideLeftWall()
    {
        Collider2D Coll = Physics2D.OverlapBox((Vector2)transform.position + LeftWallPointOffset, WallSize, 0, WallLayerMask);
        if (Coll != null)
        {
            if (isCJumped && LatestCJStatus == IsWallJumping)
            {
                isCJumped = false;
            }
            FaceToRight = -1;
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            return true;
        }
        else
        {
            return false;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)transform.position + PointOffset, GroundSize);
        Gizmos.DrawWireCube((Vector2)transform.position + RightWallPointOffset, WallSize);
        Gizmos.DrawWireCube((Vector2)transform.position + LeftWallPointOffset, WallSize);
    }
}
