using System.Windows;
using System.Windows.Controls;

namespace Acorisoft.Platform.Windows.Panels
{
    /// <summary>
    /// <see cref="TilePanel"/> 类型表示一个平铺面板，该面板能够提供所有元素平铺布局的支持。
    /// </summary>
    /// <remarks>
    /// 该面板常规情况下（不使用Grid中的RowDefinition或者ColumnDefinition等布局）可以代替Grid。
    /// </remarks>
    public class TilePanel : Panel
    {
        // ReSharper disable once MemberCanBePrivate.Global
        protected static readonly Point Zero = new(0, 0);

        protected sealed override Size MeasureOverride(Size availableSize)
        {
            foreach (FrameworkElement element in Children)
            {
                element.Measure(availableSize);
            }

            return availableSize;
        }

        protected sealed override Size ArrangeOverride(Size finalSize)
        {
            foreach (FrameworkElement element in Children)
            {
                element.Arrange(new Rect(Xaml.Point_Zero, finalSize));
            }

            return base.ArrangeOverride(finalSize); 
        }
    }
}