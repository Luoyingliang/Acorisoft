using System;
using Acorisoft.Morisa.Documents;
using Acorisoft.Morisa.Documents.Items;
using Acorisoft.Morisa.Resources;

namespace Acorisoft.Morisa.PoW.Items.Abilities
{
    public class AbilityDocument : ValuableItemDocument, IAbilityDocument
    {
        /// <summary>
        /// 获取或设置标签
        /// </summary>
        public string Labels { get; set; }
        
        /// <summary>
        /// 获取或设置能力的分类
        /// </summary>
        public Category Category { get; set; }
        
        /// <summary>
        /// 获取或设置能力的故事板。
        /// </summary>
        public Storyboard Storyboard { get; set; }
        
        /// <summary>
        /// 代价部分技能词条分部。
        /// </summary>
        public AbilityEntryPart Cost { get; set; }
        
        /// <summary>
        /// 常规部分技能词条分部。
        /// </summary>
        public AbilityEntryPart General { get; set; }
        
        /// <summary>
        /// 进化部分技能词条分部。
        /// </summary>
        public AbilityEntryPart Evolution { get; set; }
        
        /// <summary>
        /// 解锁部分技能词条分部。
        /// </summary>
        public AbilityEntryPart Unlocked { get; set; }
        
        /// <summary>
        /// 隐藏部分技能词条分部。
        /// </summary>
        public AbilityEntryPart Hidden { get; set; }
        
        /// <summary>
        /// 获取或设置能力的精神内核。
        /// </summary>
        public AbilitySpritCore Sprit { get; set; }

        /// <summary>
        /// 动作描述
        /// </summary>
        public string Motion { get; set; }

        /// <summary>
        /// 主观使用描述。
        /// </summary>
        public string Subjectivity { get; set; }
    }
}