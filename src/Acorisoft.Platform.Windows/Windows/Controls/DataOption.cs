using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Acorisoft.Platform.Windows.Controls
{
    public class DataOption : DependencyObject , IEquatable<DataOption>
    {
        public override string ToString()
        {
            return base.ToString();
        }

        public bool Equals(DataOption other)
        {
            return Data == other.Data;
        }

        public string Name
        {
            get => (string)GetValue(NameProperty);
            set => SetValue(NameProperty, value);
        }

        public object Data
        {
            get => (object)GetValue(EnumProperty);
            set => SetValue(EnumProperty, value);
        }

        public static readonly DependencyProperty EnumProperty = DependencyProperty.Register(
            "Enum",
            typeof(object),
             typeof(DataOption),
             new PropertyMetadata(null));


        public static readonly DependencyProperty NameProperty = DependencyProperty.Register(
            "Name",
            typeof(string),
             typeof(DataOption),
             new PropertyMetadata("默认选项"));


    }
}
