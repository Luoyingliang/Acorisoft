using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acorisoft.Morisa.Core;
using Acorisoft.Morisa.Resources;
using Acorisoft.Platform.Windows;
using Acorisoft.Platform.Windows.Services;
using Splat;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Acorisoft.Morisa.PoW.Items.Abilities;
using Acorisoft.Platform;
using Acorisoft.Morisa.Converters;

namespace Acorisoft.Morisa.PoW.Converters
{
    public class AbilityIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is IAbilityDocument ability)
            {
                if(ability.Icon is ImageResource && Locator.Current.GetService<IDocumentFileManager>() is IDocumentFileManager service)
                {
                    return ImageConverter.GetImageSource(ability.Icon, service);
                }
                else
                {
                    return PerformanceCategoryFallback(ability.Category);
                }
            }

            return Xaml.GetResource<DrawingImage>("Pow.Sword.Cross.Image");
        }

        protected virtual ImageSource PerformanceCategoryFallback(Category category)
        {
            return category switch
            {
                Category.Fedora => Xaml.GetResource<DrawingImage>("Pow.Sword.Cross.Image"),
                _ => Xaml.GetResource<DrawingImage>("Pow.Sword.Cross.Image")

            };
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
