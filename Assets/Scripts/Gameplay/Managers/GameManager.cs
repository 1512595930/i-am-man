using UnityEngine;
using PixelAdventure.Core.Events;
using PixelAdventure.Core.Events.EventTypes;

namespace PixelAdventure.Gameplay.Managers
{
    /// <summary>
    /// 游戏管理器 - 另一个观察者示例
    /// 负责处理玩家死亡时的游戏逻辑
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [Header("Respawn Settings")]
        [SerializeField] private Vector3 respawnPosition = new Vector3(0, 2, 0); // 复活点位置
        [SerializeField] private float respawnDelay = 1f;
        [SerializeField] private PlayerController _player; // 在 Inspector 中拖拽赋值
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
            
            if (!_isRespawning)
            {
                _isRespawning = true;
                StartCoroutine(RespawnCoroutine());
            }
        }
        /// <summary>
        /// 复活协程 - 游戏管理器统一控制复活流程
        /// </summary>
        private System.Collections.IEnumerator RespawnCoroutine()
        {
            // 等待UI倒计时（真实时间，不受Time.timeScale影响）
            yield return new WaitForSecondsRealtime(respawnDelay);

            // 执行复活逻辑
            RespawnPlayer();

            // 恢复游戏
            _isRespawning = false;

            // 触发复活事件，通知其他系统
            EventManager.TriggerPlayerRespawn();
        }
        /// <summary>
        /// 复活玩家 - 游戏管理器的核心职责
        /// </summary>
        private void RespawnPlayer()
        {
            ResetPhysicsState();
            _player.transform.position = respawnPosition;
            _player.SetAnimationState("Respawn");// 播放复活动画
            _player.ChangeState(StateType.Idle); // 切换到待机状态
            _player.enabled = true; // 确保玩家控制器启用
            Debug.Log("Player Respawned at " + respawnPosition);
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
            EventManager.OnPlayerDeath -= OnPlayerDeath;
        }
    }
}