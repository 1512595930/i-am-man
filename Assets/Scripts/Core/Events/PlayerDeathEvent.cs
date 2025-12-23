using UnityEngine;
using PixelAdventure.Core.Events;
using PixelAdventure.Core.Events.EventTypes;

namespace PixelAdventure.Gameplay.Event
{
    /// <summary>
    /// 游戏管理器 - 另一个观察者示例
    /// 负责处理玩家死亡时的游戏逻辑
    /// </summary>
    public class PlayerDeathEvent : MonoBehaviour
    {
        [Header("Respawn Settings")]
        [SerializeField] private Vector3 respawnPosition = new Vector3(0, 2, 0); // 复活点位置
        [SerializeField] private float respawnDelay = 1f;
        [SerializeField] private PlayerController _player;
        private bool _isRespawning = false;
        private void Start()
        {
            // 订阅玩家死亡事件
            EventManager.OnPlayerDeath += OnPlayerDeath;
        }

        /// <summary>
        /// 玩家死亡事件处理
        /// 负责游戏层面的逻辑处理
        /// </summary>
        private void OnPlayerDeath(object sender, PlayerDeathEventArgs e)
        {
            _player.SetAnimationState("Die");
            _player.enabled = false;
            if (!_isRespawning)
            {
                _isRespawning = true;
                StartCoroutine(RespawnCoroutine());
            }
        }
        /// <summary>
        /// 复活协程
        /// </summary>
        private System.Collections.IEnumerator RespawnCoroutine()
        {
            // 等待复活延迟
            yield return new WaitForSecondsRealtime(respawnDelay);

            // 触发复活事件，通知其他系统
            EventManager.TriggerPlayerRespawn(respawnPosition);
            _isRespawning = false;
        }
        private void OnDestroy()
        {
            // 取消事件订阅
            EventManager.OnPlayerDeath -= OnPlayerDeath;
        }
    }
}