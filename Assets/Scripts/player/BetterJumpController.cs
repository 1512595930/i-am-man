using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterJumpController : MonoBehaviour
{
    [Header("重力倍率")]
    public float fallMultiplier = 2.5f;   // 下落重力倍数
    public float lowJumpMultiplier = 2f;  // 短按跳跃重力倍数

    private Rigidbody2D rb; // 角色的刚体组件
    private bool isJumpKeyHeld; // 标记跳跃键是否被按住

    void Awake()
    {
        // 在游戏开始时获取同一游戏对象上的Rigidbody2D组件引用
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 每一帧检测跳跃键（默认为空格键）是否被按住
        // 将输入状态存储在一个变量中，以便在FixedUpdate中使用
        isJumpKeyHeld = Input.GetButton("Jump");
    }

    void FixedUpdate()
    {
        // FixedUpdate通常用于处理物理计算，调用频率固定

        // 情况1：角色正处于下落阶段（垂直速度小于0）
        if (rb.velocity.y < 0)
        {
            // 应用更大的下落重力倍数
            // 原理：在物理引擎原有重力(Physics2D.gravity)的基础上，额外施加一个向下的力
            // (fallMultiplier - 1) 确保了：
            // - 当fallMultiplier=2.5时，额外施加1.5倍的标准重力，总重力为2.5倍
            // Vector2.up * 一个负数，实质是Vector2.down的方向
            // Time.fixedDeltaTime 用于确保力的大小与帧率无关
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        // 情况2：角色正在上升，但玩家松开了跳跃键（实现短按小跳）
        else if (rb.velocity.y > 0 && !isJumpKeyHeld)
        {
            // 应用一个中等大小的重力倍数，使角色在松开键后快速减速上升，转为下落
            // 这创造了"长按跳得高，短按跳得低"的手感
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }
}