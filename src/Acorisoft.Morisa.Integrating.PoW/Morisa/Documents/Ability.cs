using System;
using System.Collections;
using System.Collections.Generic;
using Acorisoft.Morisa.Resources;
using Acorisoft.Properties;
using LiteDB;

namespace Acorisoft.Morisa.Documents
{
    public class AbilityPart : List<AbilityEntry>
    {

    }

    /// <summary>
    /// <see cref="Ability"/> 类型表示能力。
    /// </summary>
    public class Ability : IEquatable<Ability> , IComparable<Ability>
    {
        internal const string NameMoniker = "n";
        internal const string IconMoniker = "i";
        internal const string CategoryMoniker = "c";
        internal const string LabelsMoniker = "l";
        internal const string RarityMoniker = "r";
        internal const string RegularMoniker = "p";
        internal const string UnlockMoniker = "u";
        internal const string EvolutionMoniker = "e";
        internal const string HiddenMoniker = "h";
        internal const string StorySetMoniker = "s";

        public bool Equals(Ability? other)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(Ability? other)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public sealed override string ToString()
        {
            return string.IsNullOrEmpty(Name) ? SR.Ability_Empty : Name;
        }

        /// <summary>
        /// 获取或设置唯一标识符。
        /// </summary>
        [BsonId]
        public Guid Id { get; set; }
        
        /// <summary>
        /// 获取或设置能力的名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置情绪。
        /// </summary>
        public string Emotion { get; set; }
        
        /// <summary>
        /// 获取或设置能力的图标。
        /// </summary>
        public DocumentImageResource Icon { get; set; }
        
        /// <summary>
        /// 获取或设置能力的分类。
        /// </summary>
        public AbilityCategory Category { get; set; }
        
        /// <summary>
        /// 获取或设置能力的标签。
        /// </summary>
        /// <remarks>
        /// 例如：
        /// <para>战斗系、飞行、</para>
        /// </remarks>
        public AbilityLabel Labels { get; set; }
        
        /// <summary>
        /// 获取或设置能力的稀有度。
        /// </summary>
        public AbilityRarity Rarity { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<AbilityEntry> Cost { get; set; }

        /// <summary>
        /// 常规部分
        /// </summary>
        public IEnumerable<AbilityEntry> Regular { get; set; }
        
        /// <summary>
        /// 解锁部分
        /// </summary>
        public IEnumerable<AbilityEntry> Unlock { get; set; }
        
        /// <summary>
        /// 进化部分
        /// </summary>
        public IEnumerable<AbilityEntry> Evolution { get; set; }
        
        /// <summary>
        /// 隐藏部分
        /// </summary>
        public IEnumerable<AbilityEntry> Hidden { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<AbilityEntry> Zone { get; set; }

        /// <summary>
        /// 故事集
        /// </summary>
        public AbilityStorySet StorySet { get; set; } 
    }
}