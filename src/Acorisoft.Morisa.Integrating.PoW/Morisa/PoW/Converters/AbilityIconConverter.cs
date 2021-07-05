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
    public class AbilityIconConverter : IMultiValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is IAbilityDocument ability)
            {
                
            }

            return Xaml.GetResource<DrawingImage>("Pow.Sword.Cross.Image");
        }

        private ImageSource GetImageSourceFromImageResource(ImageResource resource)
        {
            if(Locator.Current.GetService<IDocumentFileManager>() is IDocumentFileManager service)
            {
                return ImageConverter.GetImageSource(resource, service);
            }
            else
            {
                return PerformanceCategoryFallback(Category.Fedora);
            }
        }

        protected virtual ImageSource PerformanceCategoryFallback(Category category)
        {
            return category switch
            {
                Category.Fedora => Xaml.GetResource<DrawingImage>("Pow.Fedora"),
                Category.Vision => Xaml.GetResource<DrawingImage>("Pow.Eye"),
                Category.Strengthen => Xaml.GetResource<DrawingImage>("Pow.Arm"),
                Category.Transformation => Xaml.GetResource<DrawingImage>("Pow.Wizard"),
                Category.Forging => Xaml.GetResource<DrawingImage>("Pow.Hammer"),
                Category.Shield => Xaml.GetResource<DrawingImage>("Pow.Shield"),
                Category.Battle => Xaml.GetResource<DrawingImage>("Pow.Sword.Cross.Image"),
                Category.Support => Xaml.GetResource<DrawingImage>("Pow.Support"),
                Category.Sentinel => Xaml.GetResource<DrawingImage>("Pow.Chess"),
                _ => Xaml.GetResource<DrawingImage>("Pow.Sword.Cross.Image")

            };
        }


        //public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    throw new NotImplementedException();
        //}
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if(values == null)
            {
                return null;
            }

            ImageResource image = null;
            var category = Category.Battle;
            
            foreach (var value in values)
            {
                if (value is ImageResource res)
                {
                    image = res;
                }
                else if (value is Category val)
                {
                    category = val;
                }
            }

            if (image != null)
            {
                return GetImageSourceFromImageResource(image);
            }

            return PerformanceCategoryFallback(category);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
