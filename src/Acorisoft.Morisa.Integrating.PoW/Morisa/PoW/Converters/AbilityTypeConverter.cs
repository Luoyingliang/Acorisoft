using Acorisoft.Morisa.Documents;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Acorisoft.Morisa.PoW.Items.Abilities;

namespace Acorisoft.Morisa.PoW.Converters
{
    public class AbilityTypeConverter : IValueConverter
    {


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not AbilityType type)
            {
                return "主动技能";
            }

            return type switch
            {
                AbilityType.Halo => "光环技能",
                AbilityType.Active => "主动技能",
                AbilityType.Passive => "被动技能",
                _ => "光环技能"
            };

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
