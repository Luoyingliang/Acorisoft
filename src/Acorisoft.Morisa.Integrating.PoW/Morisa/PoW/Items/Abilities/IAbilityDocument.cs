namespace Acorisoft.Morisa.PoW.Items.Abilities
{
    /// <summary>
    /// <see cref="IAbilityDocument"/> 接口表示一个能力文档接口类型，用于描述能力的所有属性。
    /// </summary>
    public interface IAbilityDocument : IValuableItemDocument
    {
        /// <summary>
        /// 获取或设置能力的类型
        /// </summary>
        AbilityType Type { get; set; }

        /// <summary>
        /// 获取或设置标签
        /// </summary>
        string Labels { get; set; }
        
        /// <summary>
        /// 获取或设置能力的分类
        /// </summary>
        Category Category { get; set; }
        
        /// <summary>
        /// 获取或设置能力的故事板。
        /// </summary>
        Storyboard Storyboard { get; set; }
        
        /// <summary>
        /// 代价部分技能词条分部。
        /// </summary>
        AbilityEntryPart Cost { get; set; }
        
        /// <summary>
        /// 常规部分技能词条分部。
        /// </summary>
        AbilityEntryPart General { get; set; }
        
        /// <summary>
        /// 进化部分技能词条分部。
        /// </summary>
        AbilityEntryPart Evolution { get; set; }
        
        /// <summary>
        /// 解锁部分技能词条分部。
        /// </summary>
        AbilityEntryPart Unlocked { get; set; }
        
        /// <summary>
        /// 隐藏部分技能词条分部。
        /// </summary>
        AbilityEntryPart Hidden { get; set; }
        
        /// <summary>
        /// 获取或设置能力的精神内核。
        /// </summary>
        AbilitySpritCore Sprit { get; set; }

        /// <summary>
        /// 动作描述
        /// </summary>
        string Motion { get; set; }

        /// <summary>
        /// 主观使用描述。
        /// </summary>
        string Subjectivity { get; set; }
    }
}