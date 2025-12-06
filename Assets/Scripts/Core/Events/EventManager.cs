using System;
using UnityEngine;
using PixelAdventure.Core.Events.EventTypes;

namespace PixelAdventure.Core.Events
{
    /// <summary>
    /// 全局事件管理器（单例模式）
    /// 负责事件的注册、注销和触发
    /// 企业级项目中通常使用集中式事件管理，便于维护和调试
    /// </summary>
    public class EventManager : MonoBehaviour
    {
        private static EventManager _instance;
        
        /// <summary>
        /// 玩家死亡事件
        /// 使用EventHandler委托类型，符合.NET事件规范
        /// </summary>
        public static event EventHandler<PlayerDeathEventArgs> OnPlayerDeath;
        /// <summary>
        /// 玩家复活事件
        /// </summary>
        public static event EventHandler<PlayerRespawnEventArgs> OnPlayerRespawn;
        /// <summary>
        /// 单例实例（线程安全）
        /// 企业级项目需要确保单例的线程安全性
        /// </summary>
        public static EventManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    // 在场景中查找现有实例
                    _instance = FindObjectOfType<EventManager>();
                    
                    // 如果没有找到，创建新的GameObject并挂载
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("EventManager");
                        _instance = go.AddComponent<EventManager>();
                        DontDestroyOnLoad(go); // 跨场景不销毁
                    }
                }
                return _instance;
            }
        }
        
        /// <summary>
        /// 触发玩家死亡事件
        /// 企业级项目需要添加空引用检查，避免空指针异常
        /// </summary>
        /// <param name="cause">死亡原因</param>
        /// <param name="deathPosition">死亡位置</param>
        public static void TriggerPlayerDeath(PlayerDeathEventArgs.DeathCause cause, Vector3 deathPosition)
        {
            var eventArgs = new PlayerDeathEventArgs(cause, deathPosition);
            OnPlayerDeath?.Invoke(null, eventArgs); // 事件名.Invoke(发送者, 事件参数);
        }
        /// <summary>
        /// 触发玩家复活事件
        /// </summary>
        public static void TriggerPlayerRespawn(Vector3 RespawnPosition)
        {
            var eventArgs = new PlayerRespawnEventArgs(RespawnPosition);
            OnPlayerRespawn?.Invoke(null, eventArgs);
        }

        /// <summary>
        /// 清理事件订阅（防止内存泄漏）
        /// 在场景切换或对象销毁时调用
        /// </summary>
        public static void ClearAllSubscriptions()
        {
            OnPlayerDeath = null;
        }
        
        private void OnDestroy()
        {
            // 对象销毁时清理静态事件引用，防止内存泄漏
            ClearAllSubscriptions();
        }
    }
}