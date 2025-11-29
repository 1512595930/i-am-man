using UnityEngine;

/// <summary>
/// 跳跃状态 - 修复了立即退出的问题
/// </summary>
public class JumpState : IState
{
    private float stateTimer = 0f;
    


    public void Enter(PlayerController player)
    {
        
        stateTimer = 0f;
        player.SetAnimationState(2); // Jump动画
        player.ApplyJump(); // 应用跳跃力
    }

    public void Update(PlayerController player)
    {
        stateTimer += Time.deltaTime;

        // 允许空中移动
        player.ApplyMovement(player.moveSpeed * 0.6f);

        if (player.Velocity.y < Mathf.Epsilon)
        {
            player.ChangeState(StateType.Fall); // 切换到下落
        }
        else if (player.IsGrounded && stateTimer > 0.1f)
        {
            player.ChangeState(StateType.Idle); // 落地回到空闲
        }

        player.UpdateSpriteOrientation();
    }

    public void FixedUpdate(PlayerController player) 
    {
        Vector2 velocity = player.rb.velocity;

        if (velocity.y > 0 && !player.JumpHeld) // 上升且未按住跳跃
        {
            velocity += Vector2.up * Physics2D.gravity.y * (player.lowJumpMultiplier - 1) * Time.fixedDeltaTime;

        }
        player.rb.velocity = velocity;
    }

    public void Exit(PlayerController player) { }
    public string GetStateName() => "Jump";
}