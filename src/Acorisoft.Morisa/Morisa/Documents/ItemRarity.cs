using System;
using System.Collections;
using System.Collections.Generic;

#nullable enable

namespace Acorisoft.Morisa.Documents
{
    /// <summary>
    /// <see cref="ItemRarity"/> 类型表示一个稀有度
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    public abstract class ItemRarity : IItemRarity, IComparable<ItemRarity> 
    {
        public virtual int CompareTo(ItemRarity? other)
        {
            return ComparableHelper.LessThen();
        }

        public int CompareTo(object? obj)
        {
            if (obj is ItemRarity newItem)
            {
                return CompareTo(newItem);
            }

            return ComparableHelper.LessThen();
        }
    }
}