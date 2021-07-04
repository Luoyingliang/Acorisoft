using Acorisoft.Morisa.Documents;

namespace Acorisoft.Morisa.PoW.Items
{
    public abstract class ValuableItemDocument : FullItemDocument, IValuableItemDocument
    {
        /// <summary>
        /// 获取或设置物品的稀有度。
        /// </summary>
        public Rarity Rarity { get; set; }
    }
}