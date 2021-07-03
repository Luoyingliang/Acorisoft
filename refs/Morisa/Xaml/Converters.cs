using Acorisoft.Morisa.Documents;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Acorisoft.Morisa.Xaml
{
    public class RarityBackgroundBrushConverter : IValueConverter
    {
        private static readonly SolidColorBrush _one = new SolidColorBrush(Color.FromRgb(208, 208, 208));
        private static readonly SolidColorBrush _two = new SolidColorBrush(Color.FromRgb(47, 120, 255 ));
        private static readonly SolidColorBrush _three = new SolidColorBrush(Color.FromRgb(145, 50, 200));
        private static readonly SolidColorBrush _four = new SolidColorBrush(Color.FromRgb(255, 150, 0));
        private static readonly SolidColorBrush _five = new SolidColorBrush(Color.FromRgb(207,71,71));


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Rarity rarity = (value as AbilityRarity)?.GetRarity() ?? Rarity.One;

            return rarity switch
            {
                Rarity.Two => _two,
                Rarity.Three => _three,
                Rarity.Four => _four,
                Rarity.Five => _five,
                _ => _one
            };

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static RarityBackgroundBrushConverter _instance;

        public static IValueConverter Current
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new RarityBackgroundBrushConverter();
                }

                return _instance;
            }
        }
    }

    public class AbilityIconConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is not Ability ability)
            {
                return FallbackWithNullAbility();
            }

            return ability.Icon == null ? FallbackWithCategory(ability.Category) : ability.Icon;
        }

        protected object FallbackWithNullAbility()
        {
            return Acorisoft.Platform.Xaml.GetResource<DrawingImage>("Pow.Sword.Cross.Image");
        }

        protected object FallbackWithCategory(AbilityCategory category)
        {
            return Acorisoft.Platform.Xaml.GetResource<DrawingImage>("Pow.Sword.Cross.Image");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static AbilityIconConverter _instance;

        public static IValueConverter Current
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AbilityIconConverter();
                }

                return _instance;
            }
        }
    }
}
