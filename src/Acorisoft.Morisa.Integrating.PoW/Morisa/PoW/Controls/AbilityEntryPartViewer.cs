using System.Windows;
using System.Windows.Controls;

namespace Acorisoft.Morisa.PoW.Controls
{
    public class AbilityEntryPartViewer : ItemsControl
    {
        static AbilityEntryPartViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AbilityEntryPartViewer), new FrameworkPropertyMetadata(typeof(AbilityEntryPartViewer)));
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", 
            typeof(string), 
            typeof(AbilityEntryPartViewer),
            new PropertyMetadata(default(string)));

        public string Title
        {
            get => (string) GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
    }
}