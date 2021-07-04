using Acorisoft.Morisa.Documents.Items;

namespace Acorisoft.Morisa.PoW.Items.Materials
{
    /// <summary>
    /// <see cref="IMaterialDocument"/> 接口表示一个材料文档的抽象接口。材料文档主要用于表示：植物、生物、矿石等项目。
    /// </summary>
    public interface IMaterialDocument : IFullItemDocument
    {
        /// <summary>
        /// 获取或设置材料的分类
        /// </summary>
        Category Category { get; set; }
    }
}