using PixelAdventure.Core.Events;
using PixelAdventure.Core.Events.EventTypes;
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
    [Header("死亡检测")]
    public float deathYThreshold = -10f; // 死亡Y轴阈值
    public LayerMask spikeLayerMask;     // 尖刺层掩码

    private int currentJumpCount; // 当前剩余跳跃次数

    // 状态管理
    private IState _currentState;
    private StateFactory _stateFactory;
    //精灵
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
        //初始化精灵 2d刚体
        _sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 获取输入
        HorizontalInput = Input.GetAxisRaw("Horizontal");
        JumpPressed = Input.GetButtonDown("Jump");
        // 死亡检测
        CheckDeathConditions();
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
    public void SetAnimationState(string stateValue)
    {
        animator?.SetTrigger(stateValue);
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
    /// <summary>
    /// 检测死亡条件
    /// 企业级项目中死亡检测应该放在FixedUpdate或单独的检测系统中
    /// </summary>
    private void CheckDeathConditions()
    {
        // 检测掉出地图
        if (transform.position.y < deathYThreshold)
        {
            Die(PlayerDeathEventArgs.DeathCause.FallOutOfMap);
            return;
        }

        // 检测尖刺碰撞（使用物理检测，性能更好）
        Collider2D spikeCollider = Physics2D.OverlapCircle(transform.position, 0.5f, spikeLayerMask);
        if (spikeCollider != null)
        {
            Die(PlayerDeathEventArgs.DeathCause.SpikeTrap);
        }
    }

    /// <summary>
    /// 玩家死亡处理
    /// 分离死亡逻辑和事件触发，便于维护
    /// </summary>
    /// <param name="cause">死亡原因</param>
    private void Die(PlayerDeathEventArgs.DeathCause cause)
    {
        Debug.Log($"Player died due to: {cause}");

        // 触发死亡事件
        EventManager.TriggerPlayerDeath(cause, transform.position);

        // 禁用玩家控制
        enabled = false;
        animator?.SetTrigger("Die");
        // 可以在这里添加死亡动画、音效等
    }
}