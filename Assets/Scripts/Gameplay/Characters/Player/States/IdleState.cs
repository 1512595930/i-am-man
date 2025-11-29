/// <summary>
/// 空闲状态
/// </summary>
using UnityEngine;
public class IdleState : IState
{
    public void Enter(PlayerController player)
    {
        player.SetAnimationState(0); // Idle动画
    }

    public void Update(PlayerController player)
    {
        // 状态转换逻辑
        if (Mathf.Abs(player.HorizontalInput) > 0.1f)
        {
            player.ChangeState(StateType.Run); // 移动到奔跑
        }
        else if (player.JumpPressed && player.IsGrounded)
        {
            player.ChangeState(StateType.Jump); // 跳跃
        }

        player.UpdateSpriteOrientation();
    }

    public void FixedUpdate(PlayerController player) { }
    public void Exit(PlayerController player) { }
    public string GetStateName() => "Idle";
}