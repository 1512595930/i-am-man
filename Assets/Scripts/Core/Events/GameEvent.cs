using System;
using UnityEngine;

public static class GameEvents
{
    // 核心：声明一个事件（这就是观察者模式的本质）
    public static event Action OnPlayerDied;

    // 触发事件的方法
    public static void TriggerPlayerDied()
    {
        // 检查是否有监听者，然后触发事件
        OnPlayerDied?.Invoke();
        
    }
}