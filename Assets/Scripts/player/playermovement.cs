using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class playermovement : MonoBehaviour
{
    
    [Header("速度")]
    public float moveSpeed=7f;


    private float dirX = 0f;

    private Rigidbody2D rb; // 角色的刚体组件

    private SpriteRenderer sprite;//动画翻转
    private Animator anim;//动画设置
    void Awake()
    {
        // 在游戏开始时获取同一游戏对象上的Rigidbody2D组件引用
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX *moveSpeed, rb.velocity.y);
        
        
        UpdateAnimatorState();
    }
    private void UpdateAnimatorState()
    {
        if(dirX>0f)
        {
            anim.SetBool("running", true);//运行running动画
            sprite.flipX = false;//动画X轴不翻转
        }
        else if(dirX<0f)
        {
            anim.SetBool("running", true);//运行running动画
            sprite.flipX = true;//动画X轴翻转
            
            
        }
        else
        {
            anim.SetBool("running", false);//速度为0运行静止动画
        }
    }
}
