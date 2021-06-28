using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Acorisoft.Platform.Windows;
using Acorisoft.Studio.Imgur;
using LiteDB;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Core.Raw;

namespace Acorisoft.Studio
{
    [ComVisible(true)]
    public class MarkdownDocument
    {
        public string getMarkdown() => Markdown;
        public string Markdown { get; set; }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MarkdownDocument _document;
        private LocalImgurSystem _local;

        public MainWindow()
        {
            InitializeComponent();
            _local = new LocalImgurSystem();
            this.Loaded += OnLoaded;
            this.MouseDoubleClick += (o, e) => SaveMarkdownDocument();
            //Browser.NavigationStarting += OnNavigationStarting;
        }

        private void OnNavigationStarting(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs e)
        {
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _document = new MarkdownDocument
            {
                Markdown = "### Helow"
            };

            // Browser.JavascriptObjectRepository.Register("host", _document, BindingOptions.DefaultBinder);
            // Browser.Address = @"file://D:/Repo/HyperMD/ai1.html";
            // Browser.ShowDevTools();
            // Browser.Source = new Uri(@"file://D:/Repo/HyperMD/ai1.html");
        }

        private bool _flag;
        //
        // Test for Post value to Dom
        public async void SaveMarkdownDocument()
        {
            if (_flag)
            {
                Host.Cancel();
                await Task.Delay(400);
                Browser.Visibility = Visibility.Visible;
            }
            else
            {
                var ms = new MemoryStream();
                await Browser.CoreWebView2.CapturePreviewAsync(CoreWebView2CapturePreviewImageFormat.Png, ms);
                var bi = Interop.GetImageSourceFromStream(ms);
                Thumbnail.Source = bi;
                Browser.Visibility = Visibility.Collapsed;
                Host.Await();
            }

            _flag = !_flag;
            //const string save = "saveImpl()";
            // var response = await Browser.EvaluateScriptAsync(save);
            // dynamic result = response.Success ? response.Result ?? "null" : response.Message;
            // var md = response.Result.ToString();
            //
            // const string load = @"
            //     (async function()
            //     {
            //             await CefSharp.BindObjectAsync(""host"");
            //             host.getMarkdown().then(function(md){                            
            //             editor.setValue(md)
            //             })
            //     })();
            //     ";
            // response = await Browser.EvaluateScriptAsync(load);
            // await Browser.ExecuteScriptAsync(save);
        }
    }
}