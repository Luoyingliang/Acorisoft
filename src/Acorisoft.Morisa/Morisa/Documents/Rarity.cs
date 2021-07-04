using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acorisoft.Morisa.Documents
{
    public class Rarity : IEquatable<Rarity>, IComparable<Rarity>
    {
        /// <summary>
        /// 获取或设置当前物件的稀有度。
        /// </summary>
        public int Rank { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Rarity rarity ? Equals(rarity) : ReferenceEquals(this, obj);
        }

        public int CompareTo(Rarity other)
        {
            return Rank - other.Rank;
        }

        public bool Equals(Rarity other)
        {
            return other != null && Rank == other.Rank;
        }

        public override int GetHashCode()
        {
            return Rank.GetHashCode();
        }

        public static bool operator ==(Rarity left, Rarity right)
        {
            return left?.Equals(right) ?? right is null;
        }

        public static bool operator !=(Rarity left, Rarity right)
        {
            return !(left == right);
        }

        public static bool operator <(Rarity left, Rarity right)
        {
            return left is null ? !ReferenceEquals(right, null) : left.CompareTo(right) < 0;
        }

        public static bool operator <=(Rarity left, Rarity right)
        {
            return left is null || left.CompareTo(right) <= 0;
        }

        public static bool operator >(Rarity left, Rarity right)
        {
            return left is not null && left.CompareTo(right) > 0;
        }

        public static bool operator >=(Rarity left, Rarity right)
        {
            return left is null ? right is null : left.CompareTo(right) >= 0;
        }
    }
}
