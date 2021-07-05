using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Acorisoft.Platform.Windows.Controls
{
    public class GeometryButton : Button
    {
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            "Icon", 
            typeof(Geometry), 
            typeof(GeometryButton), 
            new PropertyMetadata(default(Geometry)));

        public static readonly DependencyProperty IsFillModeProperty = DependencyProperty.Register(
            "IsFillMode", 
            typeof(bool), 
            typeof(GeometryButton), 
            new PropertyMetadata(Xaml.Box(false)));

        public bool IsFillMode
        {
            get => (bool) GetValue(IsFillModeProperty);
            set => SetValue(IsFillModeProperty, value);
        }
        public Geometry Icon
        {
            get => (Geometry) GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
    }
}