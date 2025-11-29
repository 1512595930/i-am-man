/// <summary>
/// ±¼ÅÜ×´Ì¬
/// </summary>
using UnityEngine;
public class RunState : IState
{
    public void Enter(PlayerController player)
    {
        player.SetAnimationState(1); // Run¶¯»­
    }

    public void Update(PlayerController player)
    {
        // Ó¦ÓÃÒÆ¶¯
        player.ApplyMovement(player.moveSpeed);

        // ×´Ì¬×ª»»
        if (Mathf.Abs(player.HorizontalInput) < 0.1f)
        {
            player.ChangeState(StateType.Idle); // »Øµ½¿ÕÏÐ
        }
        else if (player.JumpPressed && player.IsGrounded)
        {
            player.ChangeState(StateType.Jump); // ÌøÔ¾
        }
        else if (!player.IsGrounded)
        {
            // ¿ÕÖÐ×´Ì¬ÅÐ¶Ï
            if (player.Velocity.y > 0)
                player.ChangeState(StateType.Jump);
            else
                player.ChangeState(StateType.Fall);
        }

        player.UpdateSpriteOrientation();
    }

    public void FixedUpdate(PlayerController player) { }
    public void Exit(PlayerController player) { }
    public string GetStateName() => "Run";
}