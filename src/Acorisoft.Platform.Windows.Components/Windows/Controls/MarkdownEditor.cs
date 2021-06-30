using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Web.WebView2.Wpf;

namespace Acorisoft.Platform.Windows.Controls
{
    
    public class MarkdownEditor : EditorBase
    {
        static MarkdownEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MarkdownEditor), new FrameworkPropertyMetadata(typeof(MarkdownEditor)));
        }

       
        protected override async Task OnInitialized(WebView2 webView)
        {
            await base.OnInitialized(webView);
            
            if (!string.IsNullOrEmpty(Url))
            {
                webView.CoreWebView2.Navigate(Url);
            }


        }

        /// <summary>
        /// ���浱ǰ�༭�������ݡ�
        /// </summary>
        /// <returns>���ص�ǰ�༭���������ַ�����</returns>
        public async Task<string> SaveAsync()
        {
            if (Browser.CoreWebView2 == null)
            {
                await Browser.EnsureCoreWebView2Async();
            }

            return await Browser.ExecuteScriptAsync("saveImpl()");
        }

        public async Task LoadAsync(string json)
        {
            if (Browser.CoreWebView2 == null)
            {
                await Browser.EnsureCoreWebView2Async();
            }

            //
            //
            Browser.CoreWebView2.PostWebMessageAsString(json);

        }

    }
}