using System;
namespace PixelAdventure.Core.Events.EventTypes
{
    /// <summary>
    /// 玩家复活事件参数类
    /// 继承自EventArgs，包含复活相关的详细信息
    /// </summary>
    public class PlayerRespawnEventArgs : EventArgs
    {
        /// <summary>
        /// 复活位置
        /// </summary>
        public UnityEngine.Vector3 RespawnPosition { get; private set; }
        public float Timestamp { get; private set; }
        /// <summary>
        /// 构造函数 - 创建一个包含复活位置的事件参数实例
        /// </summary>
        /// <param name="respawnPosition">复活位置</param>
        public PlayerRespawnEventArgs(UnityEngine.Vector3 respawnPosition)
        {
            RespawnPosition = respawnPosition;
            Timestamp = UnityEngine.Time.time;
        }
    }
}
