using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class playermovement : MonoBehaviour
{
    
    [Header("速度")]
    public float moveSpeed = 7f;
    private float dirX = 0f;
    private Rigidbody2D rb; // 角色的刚体组件

   

    void Awake()
    {
        // 在游戏开始时获取同一游戏对象上的Rigidbody2D组件引用
        rb = GetComponent<Rigidbody2D>();
        
    }

    void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX *moveSpeed, rb.velocity.y);
        
    }
    public float GetDirectionX()
    {
        return dirX;
    }
}
