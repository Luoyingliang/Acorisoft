using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Acorisoft.Morisa.Xaml
{
    /// <summary>
    /// 
    /// </summary>
    public class AbilityCardView : AbilityView
    {
        static AbilityCardView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AbilityCardView), new FrameworkPropertyMetadata(typeof(AbilityCardView)));
        }
    }
}
