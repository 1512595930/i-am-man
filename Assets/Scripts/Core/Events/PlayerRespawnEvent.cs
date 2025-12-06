using UnityEngine;
using PixelAdventure.Core.Events;
using PixelAdventure.Core.Events.EventTypes;

namespace PixelAdventure.Gameplay.Managers
{
    /// <summary>
    /// 游戏管理器 - 另一个观察者示例
    /// 负责处理玩家死亡时的游戏逻辑
    /// </summary>
    public class PlayerRespawnEvent : MonoBehaviour
    {
        [Header("Respawn Settings")]
        [SerializeField] private PlayerController _player;
        private void Start()
        {
            // 订阅玩家死亡事件
            EventManager.OnPlayerRespawn += OnPlayerRespawn;
        }
        private void OnPlayerRespawn(object sender, PlayerRespawnEventArgs e)
        {
            ResetPhysicsState();
            _player.transform.position = e.RespawnPosition;
            _player.SetAnimationState("Respawn");// 播放复活动画
            _player.ChangeState(StateType.Idle); // 切换到待机状态
            _player.enabled = true; // 确保玩家控制器启用
            
            Debug.Log("Player Respawned at " + e.RespawnPosition);
        }
        /// <summary>
        /// 重置物理状态，防止复活时受到影响
        /// </summary>
        private void ResetPhysicsState()
        {
            Rigidbody2D rb = _player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
        }
        private void OnDestroy()
        {
            // 取消事件订阅
            EventManager.OnPlayerRespawn-= OnPlayerRespawn;
        }
    }
}