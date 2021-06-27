using System;
using System.Collections.Generic;
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
using CefSharp;
using CefSharp.DevTools.Network;

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

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += OnLoaded;
            this.MouseDoubleClick += (o, e) => SaveMarkdownDocument();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _document = new MarkdownDocument
            {
                Markdown = "### Helow"
            };

            Browser.JavascriptObjectRepository.Register("host", _document, BindingOptions.DefaultBinder);
            Browser.Address = @"file://D:/Repo/HyperMD/ai1.html";
            Browser.ShowDevTools();
        }

        //
        // Test for Post value to Dom
        public async void SaveMarkdownDocument()
        {
            const string save = "saveImpl()";
            var response = await Browser.EvaluateScriptAsync(save);
            dynamic result = response.Success ? response.Result ?? "null" : response.Message;
            var md = response.Result.ToString();

            const string load = @"
                (async function()
                {
                        await CefSharp.BindObjectAsync(""host"");
                        host.getMarkdown().then(function(md){                            
                        editor.setValue(md)
                        })
                })();
                ";
            response = await Browser.EvaluateScriptAsync(load);
        }
    }
}