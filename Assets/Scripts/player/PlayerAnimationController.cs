using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("组件引用")]
    private Animator animator;
    private SpriteRenderer sprite;
    private playermovement movementScript;
    private BetterJumpController jumpScript;
    [Header("动画状态")]
    public MovementState currentState = MovementState.idle;
    public enum MovementState
    {
        idle, running, jumping, falling
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        movementScript = GetComponent<playermovement>();
        jumpScript = GetComponent<BetterJumpController>();
    }

    void Update()
    {
        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        MovementState state;

        // 处理跳跃/下落状态（优先级最高）
        if (jumpScript.IsJumping())
        {
            state = MovementState.jumping;
        }
        else if (jumpScript.IsFalling())
        {
            state = MovementState.falling;
        }
        // 处理水平移动状态
        else if (movementScript.GetDirectionX() != 0)
        {
            state = MovementState.running;
            // 处理朝向
            sprite.flipX = movementScript.GetDirectionX() < 0;
        }
        else
        {
            state = MovementState.idle;
        }

        animator.SetInteger("state", (int)state);
    }
}

