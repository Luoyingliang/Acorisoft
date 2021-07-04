using Acorisoft.Morisa.Core;
using Acorisoft.Morisa.Resources;
using Acorisoft.Platform.Windows;
using Acorisoft.Platform.Windows.Services;
using Splat;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Acorisoft.Morisa.Converters
{
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Locator.Current.GetService<IDocumentFileManager>() is IDocumentFileManager service)
            {
                if(value is not ImageResource resource)
                {
                    return FallbackImage();
                }

                return GetImageSource(resource, service);
            }
            return FallbackImage();
        }

        public static ImageSource GetImageSource(ImageResource resource, IDocumentFileManager service)
        {
            var stream = service.OpenImage(resource);

            return Interop.GetImageSource(stream);
        }


        protected ImageSource FallbackImage()
        {
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}