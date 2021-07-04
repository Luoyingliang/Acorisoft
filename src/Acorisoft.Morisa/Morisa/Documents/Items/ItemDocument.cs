using System;
using Acorisoft.Morisa.Resources;
using LiteDB;

namespace Acorisoft.Morisa.Documents.Items
{
    /// <summary>
    /// <see cref="ItemDocument"/> 表示一个物体文档，用于描述物体的内容。
    /// </summary>
    public class ItemDocument : IItemDocument
    {
        /// <summary>
        /// 获取或设置物品的唯一标识符。
        /// </summary>
        [BsonId]
        public Guid Id { get; set; }


        /// <summary>
        /// 获取或设置物品的名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置物品的图标。
        /// </summary>
        public ImageResource Icon { get; set; }
    }
}