using Acorisoft.Morisa.Documents;
using Acorisoft.Morisa.Documents.Items;

namespace Acorisoft.Morisa.PoW.Items
{
    /// <summary>
    /// 
    /// </summary>
    public interface IValuableItemDocument : IFullItemDocument
    {
        /// <summary>
        /// 获取或设置物品的稀有度。
        /// </summary>
        Rarity Rarity { get; set; }
    }
}