using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Acorisoft.Platform.Windows.Controls
{
    public class EnumOption : DependencyObject
    {

        public string Name
        {
            get => (string)GetValue(NameProperty);
            set => SetValue(NameProperty, value);
        }

        public Enum Enum
        {
            get => (Enum)GetValue(EnumProperty);
            set => SetValue(EnumProperty, value);
        }

        public static readonly DependencyProperty EnumProperty = DependencyProperty.Register(
            "Enum",
            typeof(Enum),
             typeof(EnumOption),
             new PropertyMetadata(null));


        public static readonly DependencyProperty NameProperty = DependencyProperty.Register(
            "Name",
            typeof(string),
             typeof(EnumOption),
             new PropertyMetadata("默认选项"));


    }
}
