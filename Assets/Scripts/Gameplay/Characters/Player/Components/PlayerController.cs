using UnityEngine;
/// <summary>
/// 玩家主控制器 - 状态模式的上下文
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("组件引用")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform groundCheck;
    public Rigidbody2D rb { get; private set; }
    [Header("移动设置")]
    public float moveSpeed = 7f;
    public float jumpForce = 7f;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    [Header("重力倍率")]
    public float fallMultiplier = 2.5f;   // 下落重力倍数
    public float lowJumpMultiplier = 2f;  // 短按跳跃重力倍数
    [Header("多段跳设置")]
    public int maxJumpCount = 1; // 最大跳跃次数（1=单段跳）
    public bool JumpHeld => Input.GetButton("Jump");
    private int currentJumpCount; // 当前剩余跳跃次数

    // 状态管理
    private IState _currentState;
    private StateFactory _stateFactory;
    private SpriteRenderer _sprite;
    // 输入状态
    public float HorizontalInput { get; private set; }
    public bool JumpPressed { get; private set; }

    // 物理状态
    public bool IsGrounded { get; private set; }
    public Vector2 Velocity => rb.velocity;

    void Start()
    {
        // 初始化状态机
        _stateFactory = new StateFactory();
        ChangeState(StateType.Idle);

        _sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 获取输入
        HorizontalInput = Input.GetAxisRaw("Horizontal");
        JumpPressed = Input.GetButtonDown("Jump");

        // 地面检测
        IsGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // 更新当前状态
        _currentState?.Update(this);
    }

    void FixedUpdate()
    {
        _currentState?.FixedUpdate(this);
    }
    
    
    /// <summary>
    /// 改变玩家状态
    /// 【可扩展点】外部系统可以通过此方法强制改变状态
    /// </summary>
    public void ChangeState(StateType newStateType)
    {
        IState newState = _stateFactory.CreateState(newStateType);

        _currentState?.Exit(this);
        _currentState = newState;
        _currentState?.Enter(this);

        Debug.Log($"状态改变: {_currentState.GetStateName()}");
    }

    /// <summary>
    /// 设置动画状态
    /// </summary>
    public void SetAnimationState(int stateValue)
    {
        animator?.SetInteger("state", stateValue);
    }

    /// <summary>
    /// 应用移动力
    /// </summary>
    public void ApplyMovement(float speed)
    {
        rb.velocity = new Vector2(HorizontalInput * speed, rb.velocity.y);
    }

    /// <summary>
    /// 应用跳跃力
    /// </summary>
    public void ApplyJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    /// <summary>
    /// 更新精灵朝向
    /// </summary>
    public void UpdateSpriteOrientation()
    {
        if (Mathf.Abs(HorizontalInput) > Mathf.Epsilon)
        {
            _sprite.flipX = HorizontalInput < 0;
        }
    }
}