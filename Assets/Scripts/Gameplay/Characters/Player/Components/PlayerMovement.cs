using UnityEngine;

// 移动控制 - 处理物理移动和跳跃
public class PlayerMovement : MonoBehaviour
{
    [Header("移动设置")]
    public float moveSpeed = 5f;
    public float acceleration = 10f;
    public float deceleration = 8f;

    [Header("跳跃设置")]
    public float jumpForce = 7f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [Header("地面检测")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer = 1;

    private Rigidbody2D rb;
    private bool isGrounded;

    // 公共属性
    public bool IsGrounded => isGrounded;
    public bool IsFalling => !isGrounded && rb.velocity.y < -0.1f;

    public Vector2 Velocity => rb.velocity;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) Debug.LogError("缺少Rigidbody2D组件!");
    }

    void Update()
    {
        CheckGrounded();
    }

    void FixedUpdate()
    {
        ApplyBetterJumpPhysics();
    }

    // 地面检测
    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // 调试显示
        Debug.DrawRay(groundCheck.position, Vector2.down * groundCheckRadius,
                     isGrounded ? Color.green : Color.red);
    }

    // 更好的跳跃物理（可变高度跳跃）
    private void ApplyBetterJumpPhysics()
    {
        if (rb.velocity.y < 0)
        {
            // 下落时增加重力
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            // 短按跳跃时跳跃高度较低
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    // 水平移动
    public void Move(float horizontalInput)
    {

        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
    }

    // 执行跳跃
    public void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            Debug.Log("执行跳跃，速度: " + rb.velocity);
        }
    }
}