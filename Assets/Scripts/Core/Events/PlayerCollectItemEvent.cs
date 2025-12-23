using UnityEngine;
using PixelAdventure.Core.Events;
using PixelAdventure.Core.Events.EventTypes;

namespace PixelAdventure.Gameplay.Event
{
    public class PlayerCollectItemEvent : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            EventManager.OnPlayerCollectItem += OnPlayerCollectItem;
        }

        private void OnPlayerCollectItem(object sender, ItemCollectedEventArgs e)
        {
            Debug.Log("Player Collect  " + e.Quantity);
        }

        // Update is called once per frame
        private void OnDestroy()
        {
            // 取消事件订阅
            EventManager.OnPlayerCollectItem -= OnPlayerCollectItem;
        }
    }
}
