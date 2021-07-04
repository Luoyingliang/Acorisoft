using System;
using Acorisoft.Morisa.Resources;

namespace Acorisoft.Morisa.Documents.Items
{
    public interface IItemDocument
    {
        /// <summary>
        /// 获取或设置物品的唯一标识符。
        /// </summary>
        Guid Id { get; set; }
        
        
        /// <summary>
        /// 获取或设置物品的名称。
        /// </summary>
        string Name { get; set; }
        
        /// <summary>
        /// 获取或设置物品的图标。
        /// </summary>
        ImageResource Icon { get; set; }
    }
}