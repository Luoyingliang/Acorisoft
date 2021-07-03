using Acorisoft.Morisa.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Acorisoft.Morisa.Xaml
{
    public class AbilityPartView :ItemsControl
    {
        static AbilityPartView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AbilityPartView), new FrameworkPropertyMetadata(typeof(AbilityPartView)));
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(AbilityPartView), new PropertyMetadata(null));


    }
}
