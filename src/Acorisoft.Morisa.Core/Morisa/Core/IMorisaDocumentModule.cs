using System;
using MediatR;

namespace Acorisoft.Morisa.Core
{
    /// <summary>
    /// <see cref="IMorisaDocumentModule"/> 表示一个文档系统的子模块，用于为应用程序提供文档数据操作支持。
    /// </summary>
    public interface IMorisaDocumentModule : INotificationHandler<ComposeOpenRequest>, INotificationHandler<ComposeCloseRequest>
    {
        /// <summary>
        /// 获取当前的文档模块的打开文件操作标志数据流
        /// </summary>
        IObservable<bool> IsOpenStream { get; }
        
        /// <summary>
        /// 获取当前文档模块是否已经打开创作。
        /// </summary>
        bool IsOpen { get; }
    }
}