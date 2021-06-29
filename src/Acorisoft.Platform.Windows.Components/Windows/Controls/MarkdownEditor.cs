using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Web.WebView2.Wpf;

namespace Acorisoft.Platform.Windows.Controls
{
    [ComVisible(true)]
    public class EditorJSBridge
    {
        public void uploadFile(string fileNameOrUrl)
        {
            Debug.WriteLine($"data is not null : {fileNameOrUrl != null}");
            Debug.WriteLine($"data length : {fileNameOrUrl?.Length ?? 0}");
            Received?.Invoke(fileNameOrUrl);
        }

        public string test()
        {
            return "testing";
        }

        public event Action<string> Received;
    }
    
    public class MarkdownEditor : EditorBase
    {
        static MarkdownEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MarkdownEditor), new FrameworkPropertyMetadata(typeof(MarkdownEditor)));
        }

        private EditorJSBridge _bridge;

        protected override async Task OnInitialized(WebView2 webView)
        {
            await base.OnInitialized(webView);

            _bridge = new EditorJSBridge();
            _bridge.Received += (data) =>
            {

            };
            webView.CoreWebView2.AddHostObjectToScript("bridge", _bridge);
            
            if (!string.IsNullOrEmpty(Url))
            {
                webView.CoreWebView2.Navigate(Url);
            }


        }

        public void GetMarkdown()
        {
            
        }
        
    }
}