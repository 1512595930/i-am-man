using UnityEngine;
using UnityEngine.UI;
using PixelAdventure.Core.Events;
using PixelAdventure.Core.Events.EventTypes;

namespace PixelAdventure.UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("UI引用")]
        [SerializeField] private Text scoreText;          // 拖入ScoreText
        [SerializeField] private Text orangeCountText;    // 拖入OrangeCountText
        [SerializeField] private Text appleCountText;     // 拖入AppleCountText

        [Header("分数规则")]
        [SerializeField] private int orangeValue = 10;    // 每个橙子多少分
        [SerializeField] private int appleValue = 20;     // 每个苹果多少分

        // 记录数据
        private int totalScore = 0;
        private int orangesCollected = 0;
        private int applesCollected = 0;

        private void Start()
        {
            // 订阅收集事件
            EventManager.OnPlayerCollectItem += OnItemCollected;

            // 初始化UI显示
            UpdateAllUI();
        }

        private void OnItemCollected(object sender, ItemCollectedEventArgs e)
        {
            // 根据物品类型处理
            switch (e.CollectedItemType)
            {
                case ItemCollectedEventArgs.ItemType.Orange:
                    orangesCollected += e.Quantity;
                    totalScore += orangeValue * e.Quantity;
                    break;

                case ItemCollectedEventArgs.ItemType.Apple:
                    applesCollected += e.Quantity;
                    totalScore += appleValue * e.Quantity;
                    break;
            }

            // 更新UI
            UpdateAllUI();

            // 控制台输出
            Debug.Log($"收集: {e.CollectedItemType} x{e.Quantity}, 当前分数: {totalScore}");
        }

        private void UpdateAllUI()
        {
            // 更新分数显示
            if (scoreText != null)
                scoreText.text = $"分数: {totalScore}";

            // 更新橙子数量
            if (orangeCountText != null)
                orangeCountText.text = $"橙子: {orangesCollected}";

            // 更新苹果数量
            if (appleCountText != null)
                appleCountText.text = $"苹果: {applesCollected}";
        }

        private void OnDestroy()
        {
            // 取消订阅事件
            EventManager.OnPlayerCollectItem -= OnItemCollected;
        }
    }
}