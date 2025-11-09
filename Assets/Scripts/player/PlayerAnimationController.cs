using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("组件引用")]
    private Animator _animator;
    private SpriteRenderer _sprite;
    private playermovement _movementScript;
    private BetterJumpController _jumpScript;
    [Header("动画参数配置")]
    [SerializeField] private string _stateParameter = "state";
    [Header("是否每帧都检查并更新朝向")]
    [SerializeField] public bool _updateOrientationEveryFrame = true;
    public MovementState CurrentState { get; private set; } = MovementState.Idle;//属性封装
   
    public enum MovementState
    {
        Idle, Running, Jumping, Falling
    }
    public event Action<MovementState, MovementState> OnStateChanged;//状态改变事件
    private MovementState _previousState;//上一个状态
    private bool _isInitialized = false;//
    void Start()
    {
        InitializeComponents();
    }

    void Update()
    {
        if (!_isInitialized) return;
        UpdateAnimationState();
        if(_updateOrientationEveryFrame==true)
        {
            UpdateSpriteOrientation();
        }
    }
    private void InitializeComponents()
    {
        _animator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        _movementScript = GetComponent<playermovement>();
        _jumpScript = GetComponent<BetterJumpController>();
        if (_animator == null)
        {
            Debug.LogError($"PlayerAnimationController: Animator组件缺失于 {gameObject.name}");
            return;
        }

        _isInitialized = true;//已初始化
    }

    private void UpdateAnimationState()
    {
        _previousState = CurrentState;
        if (ShouldEnterJumpState())
        {
            CurrentState = MovementState.Jumping;
        }
        else if (ShouldEnterFallState())
        {
            CurrentState = MovementState.Falling;
        }
        else if(ShouldEnterRunState())
        {
            CurrentState = MovementState.Running;
        }
        else
        {
            CurrentState = MovementState.Idle;
        }
        if(_previousState !=CurrentState)
        {
            UpdateAnimatorParameters();
            UpdateSpriteOrientation();
            OnStateChanged?.Invoke(_previousState, CurrentState);
        }
    }
    private bool ShouldEnterJumpState()
    {
        return _jumpScript != null && _jumpScript.IsJumping();
    }
    private bool ShouldEnterFallState()
    {
        return _jumpScript != null && _jumpScript.IsFalling();
    }
    private bool ShouldEnterRunState()
    {
        return _jumpScript != null && Mathf.Abs(_movementScript.GetDirectionX()) > Mathf.Abs(Mathf.Epsilon);
    }//Mathf.Epsilon是一个非常小的浮点数值，在Unity脚本中代表浮点数所能表示的最小正值。
    private void UpdateAnimatorParameters()
    {
        _animator.SetInteger(_stateParameter, (int)CurrentState);

        // 企业级技巧：使用哈希值提高性能
        // Animator.StringToHash(_stateParameter) 可以缓存起来重复使用
    }
    /// <summary>
    /// 更新精灵朝向
    /// </summary>
    private void UpdateSpriteOrientation()
    {
        if (_movementScript == null || _sprite == null) return;

        float directionX = _movementScript.GetDirectionX();

        // 只在水平方向有输入时更新朝向，保持当前朝向直到有新的输入
        if (Mathf.Abs(directionX) > Mathf.Epsilon)
        {//改进：避免微小抖动导致的频繁翻转if (Mathf.Abs(directionX) > _movementThreshold)
            _sprite.flipX = directionX < 0;
        }
    }
}

