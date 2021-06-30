using System.Threading.Tasks;
using System.Windows;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Acorisoft.ComponentModel;
using Acorisoft.Platform.Windows;
using Microsoft.Web.WebView2.Core;
using System.Reactive.Subjects;

// ReSharper disable ReplaceSubstringWithRangeIndexer
// ReSharper disable MergeIntoNegatedPattern


namespace Acorisoft.Platform.Windows.Controls
{
    public class RichTextEditor : EditorBase
    {
        
        public delegate string HookUploadHandler(byte[] data);

        public struct UploadDataPacket
        {
            public Guid Id { get; set; }

            public byte[] Data { get; set; }
        }

        //--------------------------------------------------------------------------------------------------------------
        //
        // Nested Classes
        //
        //--------------------------------------------------------------------------------------------------------------

        #region Nested Classes & PostDataPacketReader

        private class PostDataPacket
        {
            /// <summary>
            /// 0=> 参数
            /// 1=> 文件
            /// </summary>
            public int Type = 0;

            public byte[] Datas;
        }

        private static class PostDataPacketReader
        {
            private static bool CompareBytes(IReadOnlyList<byte> source, byte[] comparison)
            {
                try
                {
                    var count = source.Count;
                    if (source.Count != comparison.Length)
                        return false;
                    for (var i = 0; i < count; i++)
                        if (source[i] != comparison[i])
                            return false;
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            private static byte[] ReadLineAsBytes(Stream input)
            {
                var resultStream = new MemoryStream();
                while (true)
                {
                    var data = input.ReadByte();
                    resultStream.WriteByte((byte) data);
                    if (data == 10)
                        break;
                }

                resultStream.Position = 0;
                var dataBytes = new byte[resultStream.Length];
                resultStream.Read(dataBytes, 0, dataBytes.Length);
                return dataBytes;
            }

            /// <summary>
            /// 获取Post过来的参数和数据
            /// </summary>
            /// <returns></returns>
            public static IEnumerable<PostDataPacket> GetPostDataPackets(HttpListenerContext context)
            {
                var request = context.Request;
                try
                {
                    var postDataPackets = new List<PostDataPacket>();
                    if (request.ContentType == null || request.ContentType.Length <= 20 ||
                        string.Compare(request.ContentType.Substring(0, 20), "multipart/form-data;",
                            StringComparison.OrdinalIgnoreCase) != 0)
                    {
                        return postDataPackets;
                    }

                    var firstPost = request.ContentType.Split(';').Skip(1).ToArray();
                    var boundary = string.Join(";", firstPost).Replace("boundary=", "").Trim();
                    var chunkBoundary = Encoding.UTF8.GetBytes("--" + boundary + "\r\n");
                    var endBoundary = Encoding.UTF8.GetBytes("--" + boundary + "--\r\n");
                    var input = request.InputStream;
                    var resultStream = new MemoryStream();
                    var canMoveNext = true;
                    PostDataPacket data = null;

                    while (canMoveNext)
                    {
                        var currentChunk = ReadLineAsBytes(input);
                        if (!Encoding.UTF8.GetString(currentChunk).Equals("\r\n"))
                        {
                            resultStream.Write(currentChunk, 0, currentChunk.Length);
                        }

                        if (CompareBytes(chunkBoundary, currentChunk))
                        {
                            var result = new byte[resultStream.Length - chunkBoundary.Length];
                            resultStream.Position = 0;
                            resultStream.Read(result, 0, result.Length);
                            if (result.Length > 0)
                                data!.Datas = result;
                            data = new PostDataPacket();
                            postDataPackets.Add(data);
                            resultStream.Dispose();
                            resultStream = new MemoryStream();
                        }
                        else if (Encoding.UTF8.GetString(currentChunk).Contains("Content-Disposition"))
                        {
                            var result = new byte[resultStream.Length - 2];
                            resultStream.Position = 0;
                            resultStream.Read(result, 0, result.Length);
                            resultStream.Dispose();
                            resultStream = new MemoryStream();
                        }
                        else if (Encoding.UTF8.GetString(currentChunk).Contains("Content-Type"))
                        {
                            data!.Type = 1;
                            resultStream.Dispose();
                            resultStream = new MemoryStream();
                        }
                        else if (CompareBytes(endBoundary, currentChunk))
                        {
                            var result = new byte[resultStream.Length - endBoundary.Length - 2];
                            resultStream.Position = 0;
                            resultStream.Read(result, 0, result.Length);
                            data!.Datas = result;
                            resultStream.Dispose();
                            canMoveNext = false;
                        }
                    }

                    return postDataPackets;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }


            public static async Task Feedback(HttpListenerContext context, string file)
            {
                var reponse = context.Response;
                var content = $@"{{ ""success"": 1,""file"": {{ ""url"": ""{file}"" }} }}";
                context.Response.ContentType = "application/json"; //告诉客户端返回的ContentType类型为纯文本格式，编码为UTF-8
                context.Response.AddHeader("Content-type", "application/json"); //添加响应头信息
                context.Response.StatusCode = 200;
                context.Response.StatusDescription = "200";
                var data = Encoding.UTF8.GetBytes(content);
                await reponse.OutputStream.WriteAsync(data.AsMemory(0, data.Length));
                await reponse.OutputStream.DisposeAsync();
            }
        }

        private class EditorJsUploadHook : Disposable
        {
            private readonly HttpListener _server;
            private readonly Task _listenMethods;
            private bool _continue;

            public EditorJsUploadHook(params string[] prefix)
            {
                _server = new HttpListener();
                foreach (var p in prefix)
                {
                    _server.Prefixes.Add(p);
                }

                _server.Start();
                _continue = true;
                _listenMethods = Task.Run(Loop);
            }

            private async void Loop()
            {
                // image.js 262
                while (_continue)
                {
                    try
                    {
                        var context = await _server.GetContextAsync();
                        var packet = PostDataPacketReader.GetPostDataPackets(context).FirstOrDefault(x => x.Type == 1);
                        if (packet == null)
                        {
                            continue;
                        }

                        var result = OnUploaded?.Invoke(packet.Datas);
                        await PostDataPacketReader.Feedback(context, result);
                    }
                    catch
                    {
                    }
                }
                
            }

            protected override void OnDisposeManagedCore()
            {
                _continue = false;
                _listenMethods.Dispose();
            }

            public event HookUploadHandler OnUploaded;
        }

        #endregion








        //--------------------------------------------------------------------------------------------------------------
        //
        // Static Constructor & Fields
        //
        //--------------------------------------------------------------------------------------------------------------

        static RichTextEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MarkdownEditor),
                new FrameworkPropertyMetadata(typeof(MarkdownEditor)));
        }






        
        
        //--------------------------------------------------------------------------------------------------------------
        //
        // Private Fields
        //
        //--------------------------------------------------------------------------------------------------------------





        private readonly EditorJsUploadHook _hook;
        private readonly Subject<UploadDataPacket> _upload;
        private string _internRtfText;
        private TaskCompletionSource<string> _saveTask;





        //--------------------------------------------------------------------------------------------------------------
        //
        // Constructors
        //
        //--------------------------------------------------------------------------------------------------------------

        public RichTextEditor()
        {
            _hook = new EditorJsUploadHook("http://localhost:8008/");
            _upload = new Subject<UploadDataPacket>();
            _hook.OnUploaded += OnUploaded;
        }

        protected override void OnWebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            _internRtfText = e.WebMessageAsJson;
            if(_saveTask != null)
            {
                _saveTask.SetResult(_internRtfText);
                _saveTask = null;
            }
        }


        private string OnUploaded(byte[] data)
        {
            var info = new UploadDataPacket
            {
                Data = data,
                Id = Guid.NewGuid()
            };

            _upload.OnNext(info);
            
            return "resx://d.jpg";
        }


        /// <summary>
        /// 保存当前编辑器的内容。
        /// </summary>
        /// <returns>返回当前编辑器的内容字符串。</returns>
        public async Task<string> SaveAsync()
        {
            if(Browser.CoreWebView2 == null)
            {
                await Browser.EnsureCoreWebView2Async();
            }

            //
            //
            Browser.CoreWebView2.PostWebMessageAsString("save");

            //
            //
            _saveTask = new TaskCompletionSource<string>();

            //
            //
            return await _saveTask.Task;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task ToggleReadOnlyMode()
        {
            if(Browser.CoreWebView2 == null)
            {
                await Browser.EnsureCoreWebView2Async();
            }

            //
            //
            Browser.CoreWebView2.PostWebMessageAsString("toggle");
        }

        public async Task Load(string json)
        {
            if (Browser.CoreWebView2 == null)
            {
                await Browser.EnsureCoreWebView2Async();
            }

            //
            //
            Browser.CoreWebView2.PostWebMessageAsJson(json);

        }




        //--------------------------------------------------------------------------------------------------------------
        //
        // Override Methods
        //
        //--------------------------------------------------------------------------------------------------------------



        protected override async Task OnInitialized(WebView2 webView)
        {
            await base.OnInitialized(webView);

            
            if (!string.IsNullOrEmpty(Url))
            {
                webView.CoreWebView2.Navigate(Url);
                webView.CoreWebView2.PostWebMessageAsJson(@"{ ""blocks"" :[]}");
            }
        }

        protected override void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _upload.Dispose();
            _hook.Dispose();

            base.OnUnloaded(sender, e);
        }
    }
}