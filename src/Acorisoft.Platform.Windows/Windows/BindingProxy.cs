using System.Windows;

namespace Acorisoft.Platform.Windows
{
    public class BindingProxy :Freezable
    {
        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }

        public static readonly DependencyProperty DataProperty = DependencyProperty.Register(
            "Data", typeof(object), typeof(BindingProxy), new PropertyMetadata(default(object)));

        public object Data
        {
            get => (object) GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }
    }
}