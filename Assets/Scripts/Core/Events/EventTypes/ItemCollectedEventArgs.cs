using System;
namespace PixelAdventure.Core.Events.EventTypes
{
    public class ItemCollectedEventArgs : EventArgs
    {
        public enum ItemType
        {
            Apple,
            Orange
        }
        public ItemType CollectedItemType { get; private set; }
        public int Quantity { get; private set; }
        public DateTime CollectionTime { get; private set; }
        public ItemCollectedEventArgs(ItemType itemType, int quantity)
        {
            CollectedItemType = itemType;
            Quantity = quantity;
            CollectionTime = DateTime.Now;
        }
    }

}
