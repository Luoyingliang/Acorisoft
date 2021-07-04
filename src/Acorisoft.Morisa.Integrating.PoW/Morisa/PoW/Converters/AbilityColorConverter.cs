using Acorisoft.Morisa.Documents;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Acorisoft.Morisa.PoW.Converters
{
    public class AbilityColorConverter : IValueConverter
    {
        private static readonly SolidColorBrush _one = new SolidColorBrush(Color.FromRgb(208, 208, 208));
        private static readonly SolidColorBrush _two = new SolidColorBrush(Color.FromRgb(47, 120, 255));
        private static readonly SolidColorBrush _three = new SolidColorBrush(Color.FromRgb(145, 50, 200));
        private static readonly SolidColorBrush _four = new SolidColorBrush(Color.FromRgb(255, 150, 0));
        private static readonly SolidColorBrush _five = new SolidColorBrush(Color.FromRgb(207, 71, 71));


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is not Rank rank)
            {
                return _one;
            }

            return rank switch
            {
                Rank.Two => _two,
                Rank.Three => _three,
                Rank.Four => _four,
                Rank.Five => _five,
                _ => _one
            };

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
