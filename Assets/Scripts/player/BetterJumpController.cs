using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterJumpController : MonoBehaviour
{
    [Header("重力倍率")]
    public float fallMultiplier = 2.5f;   // 下落重力倍数
    public float lowJumpMultiplier = 2f;  // 短按跳跃重力倍数
    [Header("跳跃高度")]
    public float jumpForce = 7f;
    [Header("多段跳设置")]
    public int maxJumpCount = 1; // 最大跳跃次数（1=单段跳）
    public int extraJumpsFromItem = 1; // 道具给予的额外跳跃次数
    [Header("地面检测参数")]
    [SerializeField] private Transform groundCheck; // 用于定位检测点的空物体
    public float groundCheckDistance = 0.1f;// 检测距离
    public LayerMask groundLayer;//LayerMask  图层掩码

    private Rigidbody2D rb; // 角色的刚体组件
    private bool isJumpKeyHeld; // 标记跳跃键是否被按住
    private bool isGrounded;
    private int currentJumpCount; // 当前剩余跳跃次数
    private bool hasExtraJump; // 是否拥有额外跳跃能力

    void Awake()
    {
        // 在游戏开始时获取同一游戏对象上的Rigidbody2D组件引用
        rb = GetComponent<Rigidbody2D>();
        currentJumpCount = maxJumpCount;//初始化跳跃次数
    }
    public void setcurrentJumpCount()
    {
        currentJumpCount++;
    }
    void Update()
    {
        bool previousGrounded = isGrounded;
        // 每一帧检测跳跃键（默认为空格键）是否被按住
        // 将输入状态存储在一个变量中，以便在FixedUpdate中使用
        isJumpKeyHeld = Input.GetButton("Jump");

        CheckGrounded();// 地面检测逻辑

        if (!previousGrounded && isGrounded)
        {
            currentJumpCount = maxJumpCount;
            hasExtraJump = false;
        }
            if (Input.GetButtonDown("Jump"))
            {
                // 在地面上且有跳跃次数
                if (isGrounded && currentJumpCount > 0)
                {
                    Jump();
                    currentJumpCount--;
                }
                // 在空中但有基础跳跃次数
                else if (!isGrounded && currentJumpCount > 0)
                {
                    Jump();
                    currentJumpCount--;
                }
              
            }
    }

    void FixedUpdate()
    {
        // FixedUpdate通常用于处理物理计算，调用频率固定

        // 情况1：角色正处于下落阶段（垂直速度小于0）
        if (rb.velocity.y < 0)
        {
            // 应用更大的下落重力倍数
            // 原理：在物理引擎原有重力(Physics2D.gravity)的基础上，额外施加一个向下的力
            // (fallMultiplier - 1) 确保了：
            // - 当fallMultiplier=2.5时，额外施加1.5倍的标准重力，总重力为2.5倍
            // Vector2.up * 一个负数，实质是Vector2.down的方向
            // Time.fixedDeltaTime 用于确保力的大小与帧率无关
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        // 情况2：角色正在上升，但玩家松开了跳跃键（实现短按小跳）
        else if (rb.velocity.y > 0 && !isJumpKeyHeld)
        {
            // 应用一个中等大小的重力倍数，使角色在松开键后快速减速上升，转为下落
            // 这创造了"长按跳得高，短按跳得低"的手感
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }
    void CheckGrounded()
    {
        // 地面检测逻辑（需要根据你的角色碰撞器类型调整）

        // 发射射线
        // 起点: groundCheck.position
        // 方向: Vector2.down (向下)
        // 距离: groundCheckDistance
        // 层级: whatIsGround (只检测指定层)
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);

        // 如果射线碰到了东西，且碰到的物体在 whatIsGround 层上，则认为在地面上
        isGrounded = (hit.collider != null);

        // 以下是可选的调试代码，可以在Scene视图中看到射线
        Debug.DrawRay(groundCheck.position, Vector2.down * groundCheckDistance, isGrounded ? Color.green : Color.red);
    }
    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
    public bool IsJumping()
    {
        return !isGrounded && rb.velocity.y > 0;
    }

    public bool IsFalling()
    {
        return !isGrounded && rb.velocity.y < 0;
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }
    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 20;
        style.normal.textColor = Color.white;

        GUI.Label(new Rect(10, 10, 300, 30), "在地面: " + isGrounded, style);
        GUI.Label(new Rect(10, 40, 300, 30), "剩余跳跃: " + currentJumpCount, style);
        GUI.Label(new Rect(10, 70, 300, 30), "有额外跳: " + hasExtraJump, style);
        GUI.Label(new Rect(10, 100, 300, 30), "垂直速度: " + rb.velocity.y.ToString("F2"), style);
    }
}