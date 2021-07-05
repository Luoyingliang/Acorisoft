using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Acorisoft.Platform.Windows
{
    public static class Interop
    {
        public static ImageSource GetImageSource(Image image)
        {
            var ms = new MemoryStream();
            image.Save(ms, ImageFormat.Bmp);
            ms.Seek(0, SeekOrigin.Begin);
            var bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();
            return bi;
        }

        public static ImageSource GetImageSource(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            var bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = stream;
            bi.EndInit();
            return bi;
        }

        public static RenderTargetBitmap Snapshot(FrameworkElement element)
        {
            var dpi = VisualTreeHelper.GetDpi(element);
            var bitmap = new RenderTargetBitmap(
                (int)element.DesiredSize.Width,
                (int)element.DesiredSize.Height,
                dpi.DpiScaleX * dpi.PixelsPerInchX,
                dpi.DpiScaleY * dpi.PixelsPerInchY,
                PixelFormats.Default);
            bitmap.Render(element);
            bitmap.Freeze();
            return bitmap;
        }
    }
}