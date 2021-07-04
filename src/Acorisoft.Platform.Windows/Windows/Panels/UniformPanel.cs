using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Acorisoft.Platform.Windows.Panels
{
    public class UniformPanel : Panel
    {
        protected sealed override Size MeasureOverride(Size availableSize)
        {
            var height = 0d;
            var width = 0d;

            foreach (FrameworkElement element in Children)
            {
                element.Measure(availableSize);

                height = Math.Max(height, element.DesiredSize.Height);
                width = Math.Max(width, element.DesiredSize.Width);
            }            

            return new Size(width, height);
        }

        protected sealed override Size ArrangeOverride(Size finalSize)
        {
            var averageWidth = Math.Max(64, finalSize.Width / Children.Count);
            var x = 0d;
            foreach (FrameworkElement element in Children)
            {
                element.Arrange(new Rect(x, 0, averageWidth, finalSize.Height));
                x += averageWidth;
            }

            return finalSize;
        }
    }
}
