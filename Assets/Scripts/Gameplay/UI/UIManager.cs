using UnityEngine;
using UnityEngine.UI;
using PixelAdventure.Core.Events;
using PixelAdventure.Core.Events.EventTypes;

namespace PixelAdventure.Gameplay.UI
{
    /// <summary>
    /// UI管理器 - 演示观察者模式中的订阅者
    /// 负责处理玩家死亡时的UI反馈
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private GameObject deathPanel;     // 死亡提示面板
        [SerializeField] private Text deathReasonText;     // 死亡原因文本
        [SerializeField] private Text respawnTimerText;   // 复活倒计时文本
        [SerializeField] private float respawnDelay = 1f;
        private void Start()
        {
            // 订阅玩家死亡事件
            EventManager.OnPlayerDeath += OnPlayerDeath;

            // 初始化UI状态
            if (deathPanel != null)
                deathPanel.SetActive(false);
        }

        /// <summary>
        /// 玩家死亡事件处理函数
        /// 企业级项目中应该使用异步操作处理UI动画
        /// </summary>
        private void OnPlayerDeath(object sender, PlayerDeathEventArgs e)
        {
            // 显示死亡UI
            ShowDeathUI(e.Cause);

            // 开始复活倒计时
            StartCoroutine(RespawnCountdown());
        }

        /// <summary>
        /// 显示死亡UI
        /// </summary>
        /// <param name="cause">死亡原因</param>
        private void ShowDeathUI(PlayerDeathEventArgs.DeathCause cause)
        {
            if (deathPanel != null)
            {
                deathPanel.SetActive(true);

                // 根据死亡原因显示不同的文本
                string reasonText = GetDeathReasonText(cause);
                if (deathReasonText != null)
                    deathReasonText.text = reasonText;
            }
        }

        /// <summary>
        /// 获取死亡原因描述文本
        /// 企业级项目中应该使用本地化系统
        /// </summary>
        private string GetDeathReasonText(PlayerDeathEventArgs.DeathCause cause)
        {
            switch (cause)
            {
                case PlayerDeathEventArgs.DeathCause.FallOutOfMap:
                    return "你掉出了地图边界！";
                case PlayerDeathEventArgs.DeathCause.SpikeTrap:
                    return "你被尖刺陷阱刺穿了！";
                default:
                    return "你死了！";
            }
        }

        /// <summary>
        /// 复活倒计时协程
        /// 企业级项目中使用协程处理计时逻辑
        /// </summary>
        private System.Collections.IEnumerator RespawnCountdown()
        {
            float timer = respawnDelay;

            while (timer > 0)
            {
                if (respawnTimerText != null)
                    respawnTimerText.text = $"复活倒计时: {timer:F1}秒";

                timer -= Time.deltaTime;
                yield return null;
            }

            // 倒计时结束，隐藏UI并复活玩家
            HideDeathUI();
            RespawnPlayer();
        }

        /// <summary>
        /// 隐藏死亡UI
        /// </summary>
        private void HideDeathUI()
        {
            if (deathPanel != null)
                deathPanel.SetActive(false);
        }

        /// <summary>
        /// 复活玩家
        /// 企业级项目中应该通过游戏管理器处理复活逻辑
        /// </summary>
        private void RespawnPlayer()
        {
            // 这里可以添加复活逻辑，比如重置玩家位置、状态等
            
            Debug.Log("Player respawned!");
        }

        private void OnDestroy()
        {
            // 取消事件订阅，防止内存泄漏
            EventManager.OnPlayerDeath -= OnPlayerDeath;
        }
    }
}