using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Acorisoft.Platform.Windows;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;

// ReSharper disable MemberCanBePrivate.Global

namespace Acorisoft.Platform.Windows.Controls
{
    [TemplatePart(Name = BrowserName, Type = typeof(WebView2))]
    [TemplatePart(Name = ThumbnailName, Type = typeof(Image))]
    public abstract class EditorBase : Control
    {
        private HttpListener _server;
        private Task _listenMethods;

        async void Loop()
        {
            while (true)
            {
                var context = await _server.GetContextAsync();
                var request = context.Request;
                var reponse = context.Response;
                var json = @"{success: 1,file: {url: 'file:///E:/%E5%A3%81%E7%BA%B8/1%20(2).jpg'}";

                SaveFile(Encoding.UTF8, request.ContentType, request.InputStream);

                var length = GetBoundary(request.ContentType);
                var acrh = request.Headers["Access-Control-Request-Headers"];
                var acro = request.Headers["Host"];
                byte[] boundaryBytes = Encoding.UTF8.GetBytes(length);
                int boundaryLen = boundaryBytes.Length;
                context.Response.ContentType = "application/json"; //告诉客户端返回的ContentType类型为纯文本格式，编码为UTF-8
                context.Response.AddHeader("Content-type", "application/json"); //添加响应头信息
                context.Response.ContentEncoding = Encoding.UTF8;
                // context.Response.Headers.Add("Access-Control-Allow-Credentials: true");
                context.Response.Headers.Add($"Access-Control-Allow-Origin: {acro}");
                context.Response.Headers.Add($"Access-Control-Request-Headers: origin, x-requested-with");
                context.Response.StatusCode = 200;
                context.Response.StatusDescription = "200";
                var data = Encoding.UTF8.GetBytes(json);
                await reponse.OutputStream.WriteAsync(data.AsMemory(0, data.Length));
                await reponse.OutputStream.DisposeAsync();
            }
        }
        private static string GetBoundary(string ctype)
        {
            return "--" + ctype.Split(';')[1].Split('=')[1];
        }
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
            if (Browser.CoreWebView2 == null)
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


        protected virtual void OnWebResourceResponseReceived(object? sender,
            CoreWebView2WebResourceResponseReceivedEventArgs e)
        {
        }

        protected virtual void OnWebResourceRequested(object? sender, CoreWebView2WebResourceRequestedEventArgs e)
        {
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
            Thumbnail = (Image) GetTemplateChild(ThumbnailName);

            //
            // 获取
            Browser = (WebView2) GetTemplateChild(BrowserName);

            var op = new CoreWebView2EnvironmentOptions("--disable-web-security");

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
            if (Browser.CoreWebView2 == null)
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
            Thumbnail.Source = bitmap;
            Thumbnail.Visibility = Visibility.Visible;
            Browser.Visibility = Visibility.Collapsed;
        }

        public async void Refresh()
        {
            if (Browser.CoreWebView2 == null)
            {
                //
                // 确认WebView2正确加载。
                await Browser.EnsureCoreWebView2Async();
            }

            Browser.CoreWebView2.Navigate(Url);
        }

        /// <summary>
        /// 截图
        /// </summary>
        /// <returns></returns>
        public async Task<ImageSource> CaptureAsync()
        {
            if (Browser.CoreWebView2 == null)
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

        /// <summary>
        /// 获取缩略图
        /// </summary>
        protected Image Thumbnail { get; private set; }

        /// <summary>
        /// 获取浏览器实例
        /// </summary>
        protected WebView2 Browser { get; private set; }


        public string Url { get; set; }
        
        
        
        
        
        
        static void Main(string[] args)
{
    HttpListener listener = new HttpListener();
    listener.Prefixes.Add("http://localhost:8080/ListenerTest/");
    listener.Start();

    HttpListenerContext context = listener.GetContext();

    SaveFile(context.Request.ContentEncoding, GetBoundary(context.Request.ContentType), context.Request.InputStream);

    context.Response.StatusCode = 200;
    context.Response.ContentType = "text/html";
    using (StreamWriter writer = new StreamWriter(context.Response.OutputStream, Encoding.UTF8))
        writer.WriteLine("File Uploaded");

    context.Response.Close();

    listener.Stop();

}

private static void SaveFile(Encoding enc, String boundary, Stream input)
{
    Byte[] boundaryBytes = enc.GetBytes(boundary);
    Int32 boundaryLen = boundaryBytes.Length;

    using (FileStream output = new FileStream("data", FileMode.Create, FileAccess.Write))
    {
        Byte[] buffer = new Byte[1024];
        Int32 len = input.Read(buffer, 0, 1024);
        Int32 startPos = -1;

        // Find start boundary
        while (true)
        {
            if (len == 0)
            {
                throw new Exception("Start Boundaray Not Found");
            }

            startPos = IndexOf(buffer, len, boundaryBytes);
            if (startPos >= 0)
            {
                break;
            }
            else
            {
                Array.Copy(buffer, len - boundaryLen, buffer, 0, boundaryLen);
                len = input.Read(buffer, boundaryLen, 1024 - boundaryLen);
            }
        }

        // Skip four lines (Boundary, Content-Disposition, Content-Type, and a blank)
        for (Int32 i = 0; i < 4; i++)
        {
            while (true)
            {
                if (len == 0)
                {
                    throw new Exception("Preamble not Found.");
                }

                startPos = Array.IndexOf(buffer, enc.GetBytes("\n")[0], startPos);
                if (startPos >= 0)
                {
                    startPos++;
                    break;
                }
                else
                {
                    len = input.Read(buffer, 0, 1024);
                }
            }
        }

        Array.Copy(buffer, startPos, buffer, 0, len - startPos);
        len = len - startPos;

        while (true)
        {
            Int32 endPos = IndexOf(buffer, len, boundaryBytes);
            if (endPos >= 0)
            {
                if (endPos > 0) output.Write(buffer, 0, endPos-2);
                break;
            }
            else if (len <= boundaryLen)
            {
                throw new Exception("End Boundaray Not Found");
            }
            else
            {
                output.Write(buffer, 0, len - boundaryLen);
                Array.Copy(buffer, len - boundaryLen, buffer, 0, boundaryLen);
                len = input.Read(buffer, boundaryLen, 1024 - boundaryLen) + boundaryLen;
            }
        }
    }
}

private static Int32 IndexOf(Byte[] buffer, Int32 len, Byte[] boundaryBytes)
{
    for (Int32 i = 0; i <= len - boundaryBytes.Length; i++)
    {
        Boolean match = true;
        for (Int32 j = 0; j < boundaryBytes.Length && match; j++)
        {
            match = buffer[i + j] == boundaryBytes[j];
        }

        if (match)
        {
            return i;
        }
    }

    return -1;
}
    }
}