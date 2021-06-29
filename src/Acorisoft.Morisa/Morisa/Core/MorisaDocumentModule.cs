using System;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Disposable = Acorisoft.ComponentModel.Disposable;

namespace Acorisoft.Morisa.Core
{
    public abstract class MorisaDocumentModule : Disposable, IMorisaDocumentModule
    {
        private readonly ISubmoduleAwaiter _awaiter;
        private readonly CompositeDisposable _disposable;
        private readonly BehaviorSubject<bool> _isOpenStream;
        private bool _isOpen;
        
        protected MorisaDocumentModule(ISubmoduleAwaiter awaiter)
        {
            _isOpen = false;
            _isOpenStream = new BehaviorSubject<bool>(_isOpen);
            _awaiter = awaiter ?? throw new ArgumentNullException(nameof(awaiter));
            _disposable = new CompositeDisposable
            {
                _isOpenStream
            };
        }

        /// <summary>
        /// 当创作集打开时调用该方法，在该方法中完成文档系统模块的数据初始化。
        /// </summary>
        /// <param name="compose">传递的创作集。</param>
        protected virtual void OnComposeOpen(IMorisaCompose compose)
        {
        }

        /// <summary>
        /// 当创作集关闭时调用该方法，在该方法中完成文档系统模块的数据清理。
        /// </summary>
        protected virtual void OnComposeClose()
        {
        }

        protected override void OnDisposeManagedCore()
        {
            OnComposeClose();
            _disposable.Dispose();
        }

        /// <summary>
        /// 处理创作集打开消息。
        /// </summary>
        /// <param name="notification">传入的请求实例。</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>返回等待该操作的任务实例。</returns>
        Task INotificationHandler<ComposeOpenRequest>.Handle(ComposeOpenRequest notification, CancellationToken cancellationToken)
        {
            void HandleImpl()
            {
                _awaiter.WaitOne();
                OnComposeOpen(notification.Compose);
                _awaiter.Release();
                _isOpen = true;
            }

            return Task.Run(HandleImpl, cancellationToken);
        }

        /// <summary>
        /// 处理创作集关闭消息。
        /// </summary>
        /// <param name="notification">传入的请求实例。</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>返回等待该操作的任务实例。</returns>
        Task INotificationHandler<ComposeCloseRequest>.Handle(ComposeCloseRequest notification, CancellationToken cancellationToken)
        {
            void HandleImpl()
            {
                _awaiter.WaitOne();
                OnComposeClose();
                _awaiter.Release();
                _isOpen = false;
            }

            return Task.Run(HandleImpl, cancellationToken);
        }

        protected CompositeDisposable Disposable => _disposable;

        /// <summary>
        /// 获取当前的文档模块的打开文件操作标志数据流
        /// </summary>
        public IObservable<bool> IsOpenStream => _isOpenStream;
        
        /// <summary>
        /// 获取当前文档模块是否已经打开创作。
        /// </summary>
        public bool IsOpen => _isOpen;
    }
}