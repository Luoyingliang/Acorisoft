using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace Acorisoft.Studio.Imgur
{
    /// <summary>
    /// <see cref="LocalImgurSystem"/> 表示一个本地的图床系统
    /// </summary>
    public class LocalImgurSystem
    {
        private readonly HttpListener _server;
        private readonly Task _listenMethods;

        public LocalImgurSystem()
        {
            _server = new HttpListener();
            _server.Prefixes.Add("http://localhost:8008/");
            _server.Start();
            _listenMethods = Task.Run(Loop);
        }

        async void Loop()
        {
            while (true)
            {
                var context = await _server.GetContextAsync();
                var request = context.Request;
                var reponse = context.Response;
                var json = @"{success: 1,file: {url: 'file:///E:/%E5%A3%81%E7%BA%B8/1%20(2).jpg'}";

                try
                {
                    if (request.HttpMethod == "Post")
                    {
                        
                        var byteList = new List<byte>();
                        var byteArr = new byte[2048];
                        int readLen = 0;
                        int len = 0;
                        //接收客户端传过来的数据并转成字符串类型
                        do
                        {
                            readLen = request.InputStream.Read(byteArr, 0, byteArr.Length);
                            len += readLen;
                            byteList.AddRange(byteArr);
                        } while (readLen != 0);
                        var data1 = Encoding.UTF8.GetString(byteList.ToArray(),0, len);
                    }
                }
                catch
                {
                    
                }

                var acrh = request.Headers["Access-Control-Request-Headers"];
                var acro = request.Headers["Host"];
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
    }
}