using System;

namespace PixelAdventure.Core.Events.EventTypes
{
    /// <summary>
    /// 玩家死亡事件参数类
    /// 继承自EventArgs，包含死亡相关的详细信息
    /// </summary>
    public class PlayerDeathEventArgs : EventArgs
    {
        /// <summary>
        /// 死亡原因枚举
        /// 企业级项目中通常使用枚举定义明确的死亡类型，便于后续统计和分析
        /// </summary>
        public enum DeathCause
        {
            FallOutOfMap,    // 掉出地图
            SpikeTrap,       // 尖刺陷阱
            EnemyAttack,     // 敌人攻击
            Environmental    // 环境伤害
        }

        /// <summary>
        /// 死亡原因
        /// </summary>
        public DeathCause Cause { get; private set; }
        public UnityEngine.Vector3 DeathPosition { get; private set; }
        public float Timestamp { get; private set; }

        /// <summary>
        /// 构造函数 到时候new一个有死亡原因和位置的事件
        /// </summary>
        /// <param name="cause">死亡原因</param>
        /// <param name="deathPosition">死亡位置</param>
        public PlayerDeathEventArgs(DeathCause cause, UnityEngine.Vector3 deathPosition)
        {
            Cause = cause;
            DeathPosition = deathPosition;
            Timestamp = UnityEngine.Time.time;
        }
    }
}