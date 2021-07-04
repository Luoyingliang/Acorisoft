using Acorisoft.Morisa.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acorisoft.Morisa.PoW.Items
{
    public static class Rarities
    {
        public static string GetName(Rarity rarity)
        {
            return rarity.Rank switch
            {
                2 => "精英",
                3 => "稀有",
                4 => "传奇",
                5 => "史诗",
                _ => "普通",
            };
        }
        
        /// <summary>
        /// 普通
        /// </summary>
        public readonly static Rarity General = new Rarity { Rank = 1 };

        /// <summary>
        /// 精英
        /// </summary>
        public readonly static Rarity Elite = new Rarity { Rank = 2 };

        /// <summary>
        /// 稀有
        /// </summary>
        public readonly static Rarity Rare = new Rarity { Rank = 3 };

        /// <summary>
        /// 传奇
        /// </summary>
        public readonly static Rarity Legendary = new Rarity { Rank = 4 };

        /// <summary>
        /// 史诗
        /// </summary>
        public readonly static Rarity Epic = new Rarity { Rank = 5 };

        public readonly static Rarity[] All = new[]
        {
            General,
            Elite,
            Rare,
            Legendary,
            Epic
        };
    }
}
