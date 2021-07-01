using Acorisoft.Morisa.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Acorisoft.Morisa.Xaml
{
    public class AbilityItemView : AbilityView
    {
        static AbilityItemView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AbilityItemView), new FrameworkPropertyMetadata(typeof(AbilityItemView)));
        }



    }
}
