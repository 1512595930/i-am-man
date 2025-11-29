using UnityEngine;

// 动画控制 - 管理角色动画和精灵朝向
public class PlayerAnimation : MonoBehaviour
{
    // 动画状态枚举
    public enum AnimationState { Idle = 0, Run = 1, Jump = 2, Fall = 3 }

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (animator == null) Debug.LogError("缺少Animator组件!");
        if (spriteRenderer == null) Debug.LogError("缺少SpriteRenderer组件!");
    }

    // 设置动画状态
    public void SetAnimationState(AnimationState state)
    {
        if (animator != null)
        {
            animator.SetInteger("state", (int)state);
            Debug.Log("设置动画状态: " + state);
        }
    }

    // 更新精灵朝向
    public void UpdateSpriteOrientation(float horizontalInput)
    {
        if (spriteRenderer != null && Mathf.Abs(horizontalInput) > 0.1f)
        {
            spriteRenderer.flipX = horizontalInput < 0;
        }
    }
}