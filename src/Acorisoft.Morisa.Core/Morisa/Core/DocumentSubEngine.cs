using System;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Acorisoft.Morisa.IO;
using MediatR;
using Disposable = Acorisoft.ComponentModel.Disposable;

// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ConvertToAutoPropertyWithPrivateSetter
// ReSharper disable ConvertToAutoProperty
#nullable enable

namespace Acorisoft.Morisa.Core
{
    /// <summary>
    /// <see cref="DocumentSubEngine"/> 表示一个文档子引擎，用于为应用程序提供文档内容读取、写入、保存支持。
    /// </summary>
    public abstract class DocumentSubEngine : Disposable, IDocumentSubEngine
    {
        //--------------------------------------------------------------------------------------------------------------
        //
        // Avoid Boxing Methods
        //
        //--------------------------------------------------------------------------------------------------------------
        private readonly IDocumentEngineAwaiter _awaiter;
        private readonly CompositeDisposable _disposable;
        private readonly BehaviorSubject<bool> _isOpenStream;

        
        
        
        
        
        
        
        
        //--------------------------------------------------------------------------------------------------------------
        //
        // Avoid Boxing Methods
        //
        //--------------------------------------------------------------------------------------------------------------
        private PropertyCollection? _propertyCollection;
        private Compose? _compose;
        private bool _isOpen;

        
        
        
        
        
        //--------------------------------------------------------------------------------------------------------------
        //
        // Avoid Boxing Methods
        //
        //--------------------------------------------------------------------------------------------------------------
        protected DocumentSubEngine(IDocumentEngineAwaiter awaiter)
        {
            _awaiter = awaiter;
            _isOpen = false;
            _isOpenStream = new BehaviorSubject<bool>(_isOpen);
            
            _disposable = new CompositeDisposable
            {
                _isOpenStream
            };
        }
        
        
        //--------------------------------------------------------------------------------------------------------------
        //
        // Avoid Boxing Methods
        //
        //--------------------------------------------------------------------------------------------------------------

        protected PropertyCollection GetPropertyCollection()
        {
            return !_isOpen
                ? throw new InvalidOperationException(SR.CannotInitializeCollectionWithoutOpenCompose)
                : _compose!.GetPropertyCollection(this);
        }

        protected DatabaseCollection<T> GetCollection<T>(string name)
        {
            return !_isOpen
                ? throw new InvalidOperationException(SR.CannotInitializeCollectionWithoutOpenCompose)
                : _compose!.GetDatabaseCollection<T>(name, this);
        }







        //--------------------------------------------------------------------------------------------------------------
        //
        // Avoid Boxing Methods
        //
        //--------------------------------------------------------------------------------------------------------------

        #region INotificationHandler<ComposeOpenRequest> & OnComposeOpen  Implementations

        protected virtual void OnComposeOpen(ICompose compose)
        {
            
        }
        
        Task INotificationHandler<ComposeOpenRequest>.Handle(ComposeOpenRequest notification, CancellationToken cancellationToken)
        {
            void HandleOpen()
            {
                //
                // 等待
                _awaiter.WaitOne();
                
                //
                // 获取创作集
                _compose = notification.InternalCompose;
                _propertyCollection = GetPropertyCollection();
                
                //
                // 子类中加载
                OnComposeOpen(notification.Compose);
                
                //
                // 释放
                _awaiter.Release();
                
                //
                // 推送通知
                _isOpen = true;
                _isOpenStream.OnNext(_isOpen);
            }

            return Task.Run(HandleOpen ,cancellationToken);
        }
        
        #endregion

        #region INotificationHandler<ComposeCloseRequest> & OnComposeClose  Implementations

        protected virtual void OnComposeClose()
        {
            
        }
        
        
        Task INotificationHandler<ComposeCloseRequest>.Handle(ComposeCloseRequest notification, CancellationToken cancellationToken)
        {
            void HandleClose()
            {
                //
                // 等待
                _awaiter.WaitOne();
                
                //
                // 释放
                _propertyCollection = null;
                OnComposeClose();
                
                //
                // 释放
                _awaiter.Release();
                
                
                //
                // 推送通知
                _isOpen = false;
                _isOpenStream.OnNext(_isOpen);
            }
            
            return Task.Run(HandleClose ,cancellationToken);
        }
        
        #endregion

        
        
        
        
        
        
        //--------------------------------------------------------------------------------------------------------------
        //
        // Avoid Boxing Methods
        //
        //--------------------------------------------------------------------------------------------------------------
        protected CompositeDisposable Disposable => _disposable;
        public IObservable<bool> IsOpenStream => _isOpenStream;
        public bool IsOpen => _isOpen;
        public PropertyCollection? PropertyCollection => _propertyCollection;
    }
}