using UnityEngine;

/// <summary>
/// 下落状态
/// </summary>
public class FallState : IState
{
    public void Enter(PlayerController player)
    {
        player.SetAnimationState(3); // Fall动画
    }

    public void Update(PlayerController player)
    {
        // 允许空中移动
        player.ApplyMovement(player.moveSpeed);

        // 状态转换
        if (player.IsGrounded||player.Velocity.y == 0)
        {
            player.ChangeState(StateType.Idle); // 落地回到空闲
        }

        player.UpdateSpriteOrientation();
    }

    public void FixedUpdate(PlayerController player)
    {
        Vector2 velocity = player.rb.velocity;
        velocity += Vector2.up * Physics2D.gravity.y * (player.fallMultiplier - 1) * Time.fixedDeltaTime;
        player.rb.velocity = velocity;
    }
    public void Exit(PlayerController player) { }
    public string GetStateName() => "Fall";
}