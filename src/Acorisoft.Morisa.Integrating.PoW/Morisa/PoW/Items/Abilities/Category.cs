using Acorisoft.Platform.Windows.Converters;
using System.ComponentModel;

namespace Acorisoft.Morisa.PoW.Items.Abilities
{
    public enum Category
    {
        /// <summary>
        /// 侦查
        /// </summary>
        [Description("侦查系")]
        Fedora,

        /// <summary>
        /// 战斗
        /// </summary>
        [Description("战斗系")]
        Battle,

        /// <summary>
        /// 防御
        /// </summary>
        [Description("防御系")]
        Shield,


        /// <summary>
        /// 辅助
        /// </summary>
        [Description("辅助系")]
        Support,

        /// <summary>
        /// 变身
        /// </summary>
        [Description("变身系")]
        Transformation,


        /// <summary>
        /// 哨兵
        /// </summary>
        [Description("Eye")]
        Vision,

        /// <summary>
        /// 强化
        /// </summary>
        [Description("强化系")]
        Strengthen,

        /// <summary>
        /// 锻造系
        /// </summary>
        [Description("锻造系")]
        Forging,

        /// <summary>
        /// 哨兵
        /// </summary>
        [Description("哨兵")]
        Sentinel,
    }
}