using System;
using System.Threading.Tasks;
using MediatR;

namespace Acorisoft.Morisa.Core
{
    /// <summary>
    /// <see cref="IMorisaDocumentSystem"/> 接口表示一个抽象的文档系统，用于描述文档系统的功能支持细节。
    /// </summary>
    public interface IMorisaDocumentSystem : IDisposable , ISubmoduleAwaiter , IMorisaPropertyManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        Task CreateAsync(string folder, string name);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newItem"></param>
        /// <returns></returns>
        Task CreateAsync(INewDiskItem<MorisaComposeProperty> newItem);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        Task LoadAsync(string folder);
        
        /// <summary>
        /// 获取当前文档系统是否已经打开创作。
        /// </summary>
        bool IsOpen { get; }
        
        /// <summary>
        /// 中介器
        /// </summary>
        IMediator Mediator { get; }

        /// <summary>
        /// 获取当前的打开创作
        /// </summary>
        IMorisaCompose Compose { get; }

        /// <summary>
        /// 获取当前的文档系统的打开文件操作标志数据流
        /// </summary>
        IObservable<bool> IsOpenStream { get; }
        
        /// <summary>
        /// 获取当前的创作数据流
        /// </summary>
        IObservable<IMorisaCompose> ComposeStream { get; }
        
        /// <summary>
        /// 获取当前的文档系统开始打开创作标志数据流。
        /// </summary>
        IObservable<System.Reactive.Unit> ComposeOpening { get; }
        
        /// <summary>
        /// 获取当前的文档系统完成打开创作标志数据流。
        /// </summary>
        IObservable<System.Reactive.Unit> ComposeOpenCompleted { get; }
    }
}