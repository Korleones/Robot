using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCTRL : MonoBehaviour
{
    [Header("移动")]
    public float WalkSpeed;
    public float AccelerateTime;
    public float DecelerateTime;
    bool CanMove = true;
    [Header("跳跃")]
    public float JumpingSpeed;
    public float FallMultiplier;
    public float LowJumpMultiplier;
    bool CanJump = true;
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
    bool IsDashing;
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
    [Header("爬墙跳")]
    Vector2 ClimbingJumpDir;
    public float ClimbingJumpForce;
    public float ClimbingJumpMaxForce;
    public float ClimbingJumpDuration;
    public float ClimbingJumpWaitTime;
    public Vector2 ClimbingJumpSpeed;

    int FaceToRight = 1;
    Rigidbody2D Rig;
    float velocityX;
    public bool IsOnGround;
    bool IsJumping;
    public bool IsClimbing;
    bool IsBesideRightWall;
    bool IsBesideLeftWall;
    public int IsWallJumping = 0;
    bool TimerOpen = false;
    float Timer = 0f;
    Animator m_animator;


    private void Awake()
    {
        Rig = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (Input.GetAxisRaw("Horizontal") > 0) FaceToRight = 1;
        else if (Input.GetAxisRaw("Horizontal") < 0) FaceToRight = -1;
        IsOnGround = OnGround();     //判断当前角色是否碰到地面
        IsBesideRightWall = BesideRightWall();    //判断当前角色是否碰到右边的墙
        IsBesideLeftWall = BesideLeftWall();    //判断当前角色是否碰到左边的墙
        if (!IsOnGround && (IsBesideRightWall || IsBesideLeftWall))
        {
            IsClimbing = true;
        }
        else
        {
            TimerOpen = false;
            Timer = 0f;
            IsClimbing = false;
        }

        #region 计时器
        if(TimerOpen)
        {
            Timer += Time.deltaTime;
        }
        #endregion
        #region 爬墙
        if (IsClimbing && Input.GetAxis("Vertical") >= 0)
        {
            if (Rig.velocity.y < 0)
            {
                TimerOpen = true;
                if(Timer >= 0.5f)
                {
                    Rig.velocity = new Vector2(Rig.velocity.x, -WallVelocity * Timer * Timer * 4f);

                }
                else 
                    Rig.velocity = new Vector2(Rig.velocity.x, -WallVelocity);

            }
        }
        #endregion

        #region 跳跃
        if (CanJump)
        {
            if (Input.GetAxis("Jump") == 1 && !IsJumping && IsOnGround)
            {
                m_animator.SetBool("Jump", true);
                Rig.velocity = new Vector2(Rig.velocity.x / 10f, JumpingSpeed);
                IsJumping = true;
            }
            if (IsOnGround && Input.GetAxis("Jump") == 0)
            {
                IsJumping = false;
            }
        }
        //if (CanMove && Input.GetButtonUp("Jump") && Rig.velocity.y > 0)
        //{
        //    Rig.velocity = new Vector2(Rig.velocity.x, 0);
        //}
        #endregion

        #region 爬墙跳
        if (Input.GetButtonDown("Jump") && IsClimbing)
        {
            if (IsBesideLeftWall && IsWallJumping <= 0)
            {
                IsWallJumping = 1;
                ClimbingJumpDir = new Vector2(ClimbingJumpSpeed.x * IsWallJumping, ClimbingJumpSpeed.y);
                //将玩家当前所有的动量清零
                Rig.velocity = Vector2.zero;
                //施加一个力，让玩家飞出去
                Rig.velocity += ClimbingJumpDir * ClimbingJumpForce;
                //Rig.velocity = new Vector2(Rig.velocity.x / 2, JumpingSpeed);
                StartCoroutine(ClimbingJump());
            }
            else if (IsBesideRightWall && IsWallJumping >= 0)
            {
                IsWallJumping = -1;
                ClimbingJumpDir = new Vector2(ClimbingJumpSpeed.x * IsWallJumping, ClimbingJumpSpeed.y);
                //将玩家当前所有的动量清零
                Rig.velocity = Vector2.zero;
                //施加一个力，让玩家飞出去
                Rig.velocity += ClimbingJumpDir * ClimbingJumpForce;
                //Rig.velocity = new Vector2(Rig.velocity.x / 2, JumpingSpeed);
                StartCoroutine(ClimbingJump());
            }
        }
        #endregion
    }
    void FixedUpdate()
    {
        m_animator.SetFloat("VelocityX", Rig.velocity.x);
        m_animator.SetFloat("VelocityY", Rig.velocity.y);
        m_animator.SetInteger("Horizontal", (int)Input.GetAxisRaw("Horizontal"));
        m_animator.SetInteger("Vertical", (int)Input.GetAxisRaw("Vertical"));

        #region 左右移动
        if (CanMove)
        {
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                m_animator.SetBool("Run", true);
                Rig.velocity = new Vector2(Mathf.SmoothDamp(Rig.velocity.x, WalkSpeed * Time.fixedDeltaTime * 60, ref velocityX, AccelerateTime), Rig.velocity.y);
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                m_animator.SetBool("Run", true);
                Rig.velocity = new Vector2(Mathf.SmoothDamp(Rig.velocity.x, WalkSpeed * Time.fixedDeltaTime * -60, ref velocityX, AccelerateTime), Rig.velocity.y);
            }
            else
            {
                m_animator.SetBool("Run", false);
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
            IsDashing = true;
            WasDashed = true;
            DashingDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (DashingDir.x == 0 && Input.GetAxis("Vertical") == 0)
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



    }

    IEnumerator Dash()
    {
        //关闭玩家的移动和跳跃的功能
        CanMove = false;
        CanJump = false;
        //关闭重力调整器
        GravityModifier = false;
        //关闭重力影响
        Rig.gravityScale = 0;
        //施加空气阻力(Rigidbody.Drag)
        DOVirtual.Float(DashMaxForce, 0, DashDuration, RigidbodyDrag);
        //等待一段时间
        yield return new WaitForSeconds(DashWaitTime);
        //开启所有关闭的东西
        CanMove = true;
        CanJump = true;
        GravityModifier = true;
        Rig.gravityScale = 5;
        Rig.drag = 0.5f;
    }
    //IEnumerator OnTheWall()
    //{
    //    //关闭玩家的移动和跳跃的功能
    //    CanMove = false;
    //    CanJump = false;
    //    //关闭重力调整器
    //    GravityModifier = false;
    //    //关闭重力影响
    //    Rig.gravityScale = 0;
    //    Rig.velocity = new Vector2(Rig.velocity.x, 0);
    //    yield return new WaitForSeconds(WallWaitTime);
    //    CanMove = true;
    //    CanJump = true;
    //    GravityModifier = true;
    //    Rig.gravityScale = 1;
    //}
    IEnumerator ClimbingJump()
    {
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
        Rig.gravityScale = 5;
        
    }

    bool OnGround()
    {
        Collider2D Coll = Physics2D.OverlapBox((Vector2)transform.position + PointOffset, GroundSize, 0, GroundLayerMask);
        if (Coll != null)
        {
            m_animator.SetBool("IsOnGround", true);
            IsWallJumping = 0;
            return true;
        }
        else
        {
            //m_animator.SetBool("");
            return false;
        }
    }
    bool BesideRightWall()
    {
        Collider2D Coll = Physics2D.OverlapBox((Vector2)transform.position + RightWallPointOffset, WallSize, 0, WallLayerMask);
        if (Coll != null)
        {
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
            return true;
        }
        else
        {
            return false;
        }
    }
    public void RigidbodyDrag(float x)
    {
        Rig.drag = x;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)transform.position + PointOffset, GroundSize);
        Gizmos.DrawWireCube((Vector2)transform.position + RightWallPointOffset, WallSize);
        Gizmos.DrawWireCube((Vector2)transform.position + LeftWallPointOffset, WallSize);
    }
}
