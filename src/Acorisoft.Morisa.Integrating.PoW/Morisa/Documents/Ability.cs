using System;
using Acorisoft.Morisa.Resources;
using LiteDB;

namespace Acorisoft.Morisa.Documents
{
    /// <summary>
    /// <see cref="Ability"/> 类型表示能力。
    /// </summary>
    public class Ability
    {
        /*
         * 
         */
        
        /// <summary>
        /// 获取或设置唯一标识符。
        /// </summary>
        [BsonId]
        public Guid Id { get; set; }
        
        /// <summary>
        /// 获取或设置能力的名称。
        /// </summary>
        [BsonField("n")]
        public string Name { get; set; }
        
        /// <summary>
        /// 获取或设置能力的图标。
        /// </summary>
        [BsonField("i")]
        public DocumentImageResource Icon { get; set; }
        
        /// <summary>
        /// 获取或设置能力的分类。
        /// </summary>
        public string Category { get; set; }
    }
}