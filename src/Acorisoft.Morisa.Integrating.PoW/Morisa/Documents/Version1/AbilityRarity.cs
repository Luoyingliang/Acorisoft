using System.Collections.Generic;
using Acorisoft.Properties;

#nullable enable

namespace Acorisoft.Morisa.Documents
{

    public enum Rarity
    {
        One,
        Two,
        Three,
        Four,
        Five,
    }


    /// <summary>
    /// <see cref="AbilityRarity"/> 类型表示一个能力的稀有度。
    /// </summary>
    /// <remarks>
    /// 采用5星稀有度制度。
    /// </remarks>
    public class AbilityRarity 
    {
        private readonly int _rarity;

        public AbilityRarity(int rarity)
        {
            _rarity = rarity;
        }

        internal int Value => _rarity;

        public sealed override bool Equals(object? obj)
        {
            if (obj is AbilityRarity rarity)
            {
                return rarity._rarity == _rarity;
            }

            return ReferenceEquals(obj, this);
        }

        public sealed override int GetHashCode()
        {
            return _rarity.GetHashCode();
        }

        public sealed override string ToString() => GetRarityFriendlyName(this);

        /// <summary>
        /// 获取当前稀有度的友好名称。
        /// </summary>
        /// <param name="rarity">指定要获取的稀有度</param>
        /// <returns>返回当前稀有度的友好名称。</returns>
        public static string GetRarityFriendlyName(AbilityRarity rarity)
        {
            return rarity._rarity switch
            {
                2 => SR.Rarity_Two,
                3 => SR.Rarity_Three,
                4 => SR.Rarity_Four,
                5 => SR.Rarity_Five,
                _ => SR.Rarity_One
            };
        }

        public static AbilityRarity One => new AbilityRarity(1);
        public static AbilityRarity Two => new AbilityRarity(2);
        public static AbilityRarity Three => new AbilityRarity(3);
        public static AbilityRarity Four => new AbilityRarity(4);
        public static AbilityRarity Five => new AbilityRarity(5);

        public static IEnumerable<AbilityRarity> Rarities => new AbilityRarity[]
        {
            One,
            Two,
            Three,
            Four,
            Five
        };

        public Rarity GetRarity()
        {
            return _rarity switch
            {
                2 => Rarity.Two,
                3 => Rarity.Three,
                4 => Rarity.Four,
                5 => Rarity.Five,
                _ => Rarity.One,
            };
        }
    }
}