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
    }
}