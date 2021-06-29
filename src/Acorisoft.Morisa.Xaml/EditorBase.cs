using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Acorisoft.Platform.Windows;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;

namespace Acorisoft
{
    [TemplatePart(Name = BrowserName, Type = typeof(WebView2))]
    [TemplatePart(Name = ThumbnailName, Type = typeof(Image))]
    public abstract class EditorBase : Control
    {
        //
        // 引用:
        // https://www.cnblogs.com/TianFang/p/14398424.html
        //
        private const string BrowserName = "PART_Browser";
        private const string ThumbnailName = "PART_Thumbnail";


        protected virtual async Task OnInitialized(WebView2 webView)
        {
            
        }

        public override async void OnApplyTemplate()
        {
            //
            //
            Thumbnail = (Image) GetTemplateChild(ThumbnailName);
            
            //
            // 获取
            Browser = (WebView2) GetTemplateChild(BrowserName);

            //
            // 创建设置
            var setting = await CoreWebView2Environment.CreateAsync();
            
            //
            // 初始化
            await Browser!.EnsureCoreWebView2Async(setting);
            
            //
            // 关闭右键菜单
            Browser.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
            Browser.CoreWebView2.Settings.AreDevToolsEnabled = false;

            //
            // 初始化
            await OnInitialized(Browser);
            
            //
            // 基类调用
            base.OnApplyTemplate();
        }

        /// <summary>
        /// 当需要在本控件之上进行覆盖显示时，调用该方法。
        /// </summary>
        public async void EnableOverlayBehavior()
        {
            //
            // 截图
            var bitmap = await CaptureAsync();

            //
            //
            Thumbnail.Source = bitmap;
            Thumbnail.Visibility = Visibility.Visible;
            Browser.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 截图
        /// </summary>
        /// <returns></returns>
        public async Task<ImageSource> CaptureAsync()
        {
            //
            // 创建一个内存
            var ms = new MemoryStream();

            //
            // 等待创建完成
            await Browser.CoreWebView2.CapturePreviewAsync(CoreWebView2CapturePreviewImageFormat.Png, ms);

            //
            // 导出图片
            return Interop.GetImageSourceFromStream(ms);
        }

        /// <summary>
        /// 覆盖显示操作完成时，调用该方法。
        /// </summary>
        public async void DisableOverlayBehavior()
        {
            //
            // 等待动画完成
            await Task.Delay(400);
            
            Thumbnail.Visibility = Visibility.Collapsed;
            Browser.Visibility = Visibility.Visible;
        }
        
        protected Image Thumbnail { get; private set; }
        protected WebView2 Browser { get; private set; }
    }
}