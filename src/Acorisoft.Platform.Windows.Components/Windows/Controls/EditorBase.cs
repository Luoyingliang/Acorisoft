using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;

// ReSharper disable VirtualMemberNeverOverridden.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable CheckNamespace

#nullable enable

namespace Acorisoft.Platform.Windows.Controls
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

        protected EditorBase()
        {
            this.Unloaded += OnUnloaded;
        }

        protected virtual void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (Browser!.CoreWebView2 == null)
            {
                return;
            }

            Browser.CoreWebView2.NavigationStarting -= OnNavigationStarting;
            Browser.CoreWebView2.SourceChanged -= OnSourceChanged;
            Browser.CoreWebView2.ContentLoading -= OnContentLoading;
            Browser.CoreWebView2.HistoryChanged -= OnHistoryChanged;
            Browser.CoreWebView2.NavigationCompleted -= OnNavigationCompleted;
            Browser.CoreWebView2.WebMessageReceived -= OnWebMessageReceived;
            Browser.CoreWebView2.WebResourceRequested -= OnWebResourceRequested;
            Browser.CoreWebView2.WebResourceResponseReceived -= OnWebResourceResponseReceived;
            Browser.CoreWebView2InitializationCompleted -= OnCoreWebView2InitializationCompleted;
        }

        protected virtual async Task OnInitialized(WebView2 webView)
        {
        }

        protected virtual void OnNavigationStarting(object? sender, CoreWebView2NavigationStartingEventArgs e)
        {
        }

        protected virtual void OnNavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
        {
        }

        protected virtual void OnHistoryChanged(object? sender, object e)
        {
        }

        protected virtual void OnContentLoading(object? sender, CoreWebView2ContentLoadingEventArgs e)
        {
        }

        protected virtual void OnSourceChanged(object? sender, CoreWebView2SourceChangedEventArgs e)
        {
            
        }


        protected virtual void OnWebResourceResponseReceived(object? sender, CoreWebView2WebResourceResponseReceivedEventArgs e)
        {
        }
        
        
        protected virtual void OnCoreWebView2InitializationCompleted(object? sender, CoreWebView2InitializationCompletedEventArgs e)
        {
        }

        protected virtual async void OnWebResourceRequested(object? sender, CoreWebView2WebResourceRequestedEventArgs e)
        {
            //using var stream = new FileStream(@"E:\壁纸\1.png", FileMode.Open);
            //var ms = new MemoryStream();
            //stream.CopyTo(ms);
            //ms.Position = 0;
            //var response = Browser.CoreWebView2.Environment.CreateWebResourceResponse(ms, 200, "OK", "");
            //e.Response = response;
            //
            // if request failed will turn to this
            // e.Response.StatusCode = 200;
            
        }

        protected virtual void OnWebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
        }


        public override async void OnApplyTemplate()
        {
            // _server = new HttpListener();
            // _server.Prefixes.Add("http://localhost:8008/");
            // _server.Start();
            // _listenMethods = Task.Run(Loop);
            //
            //
            Thumbnail = (Image) GetTemplateChild(ThumbnailName)!;

            //
            // 获取
            Browser = (WebView2) GetTemplateChild(BrowserName)!;

            var op = new CoreWebView2EnvironmentOptions("--disable-web-security");
            //op.AdditionalBrowserArguments = "--proxy-server=http://localhost:8009";

            //
            // 创建设置
            var setting = await CoreWebView2Environment.CreateAsync(null, null, op);

            //
            // 初始化
            await Browser!.EnsureCoreWebView2Async(setting);

            //
            // 关闭右键菜单
            // Browser.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
            // Browser.CoreWebView2.Settings.AreDevToolsEnabled = false;
            Browser.CoreWebView2.NavigationStarting += OnNavigationStarting;
            Browser.CoreWebView2.SourceChanged += OnSourceChanged;
            Browser.CoreWebView2.ContentLoading += OnContentLoading;
            Browser.CoreWebView2.HistoryChanged += OnHistoryChanged;
            Browser.CoreWebView2.NavigationCompleted += OnNavigationCompleted;
            Browser.CoreWebView2.WebMessageReceived += OnWebMessageReceived;
            Browser.CoreWebView2.WebResourceRequested += OnWebResourceRequested;
            Browser.CoreWebView2.WebResourceResponseReceived += OnWebResourceResponseReceived;
            Browser.CoreWebView2.AddWebResourceRequestedFilter("*", CoreWebView2WebResourceContext.Image);
            Browser.CoreWebView2InitializationCompleted += OnCoreWebView2InitializationCompleted;
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
            if (Browser!.CoreWebView2 == null)
            {
                //
                // 确认WebView2正确加载。
                await Browser.EnsureCoreWebView2Async();
            }

            //
            // 截图
            var bitmap = await CaptureAsync();

            //
            //
            Thumbnail!.Source = bitmap;
            Thumbnail.Visibility = Visibility.Visible;
            Browser.Visibility = Visibility.Collapsed;
        }

        public async void Refresh()
        {
            if (Browser!.CoreWebView2 == null)
            {
                //
                // 确认WebView2正确加载。
                await Browser.EnsureCoreWebView2Async();
            }

            Browser.CoreWebView2!.Navigate(Url);
        }

        /// <summary>
        /// 截图
        /// </summary>
        /// <returns></returns>
        public async Task<ImageSource> CaptureAsync()
        {
            if (Browser!.CoreWebView2 == null)
            {
                //
                // 确认WebView2正确加载。
                await Browser.EnsureCoreWebView2Async();
            }

            //
            // 创建一个内存
            var ms = new MemoryStream();

            //
            // 等待创建完成
            await Browser.CoreWebView2!.CapturePreviewAsync(CoreWebView2CapturePreviewImageFormat.Png, ms);

            //
            // 导出图片
            return Interop.GetImageSource(ms);
        }

        /// <summary>
        /// 覆盖显示操作完成时，调用该方法。
        /// </summary>
        public async void DisableOverlayBehavior()
        {
            if (Browser!.CoreWebView2 == null)
            {
                //
                // 确认WebView2正确加载。
                await Browser.EnsureCoreWebView2Async();
            }
            //
            // 等待动画完成
            await Task.Delay(400);

            Thumbnail!.Visibility = Visibility.Collapsed;
            Browser!.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 获取缩略图
        /// </summary>
        protected Image? Thumbnail { get; private set; }

        /// <summary>
        /// 获取浏览器实例
        /// </summary>
        protected WebView2? Browser { get; private set; }


        public string Url { get; set; }                 
    }
}