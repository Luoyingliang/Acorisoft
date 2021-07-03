using System;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Acorisoft.Morisa.IO;
using Acorisoft.Morisa.Resources;
using DryIoc;
using MediatR;
using Unit = System.Reactive.Unit;
using Disposable = Acorisoft.ComponentModel.Disposable;
using System.Threading;
// ReSharper disable ClassNeverInstantiated.Global

// ReSharper disable UnusedMember.Global

namespace Acorisoft.Morisa.Core
{
    public class DocumentEngine : Disposable , IDocumentEngine , IDocumentEngineAwaiter
    {
        //--------------------------------------------------------------------------------------------------------------
        //
        // Avoid Boxing Methods
        //
        //--------------------------------------------------------------------------------------------------------------
        private readonly BehaviorSubject<ICompose> _composeStream;
        private readonly BehaviorSubject<bool> _isOpenStream;
        private readonly Subject<Unit> _composeOpenStream;
        private readonly Subject<Unit> _composeOpenCompletedStream;
        private readonly Subject<ComposeProperty> _propertyStream;
        private readonly CompositeDisposable _disposable;
        private readonly IMediator _mediator;
        
        
        
        
        
        
        //--------------------------------------------------------------------------------------------------------------
        //
        // Avoid Boxing Methods
        //
        //--------------------------------------------------------------------------------------------------------------
        private Compose _compose;
        private bool _isOpen;
        private string _path;
        private PropertyCollection _propertyCollection;
        private int _requestCount;

        
        //--------------------------------------------------------------------------------------------------------------
        //
        // Avoid Boxing Methods
        //
        //--------------------------------------------------------------------------------------------------------------
        private DocumentEngine(IMediator mediator)
        {
            _composeStream = new BehaviorSubject<ICompose>(null);
            _isOpenStream = new BehaviorSubject<bool>(false);
            _propertyStream = new Subject<ComposeProperty>();
            _composeOpenCompletedStream = new Subject<Unit>();
            _composeOpenStream = new Subject<Unit>();

            _disposable = new CompositeDisposable
            {
                _composeStream,
                _isOpenStream,
                _propertyStream,
                _composeOpenStream,
                _composeOpenCompletedStream
            };
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        
        
        
        
        //--------------------------------------------------------------------------------------------------------------
        //
        // Protected Methods
        //
        //--------------------------------------------------------------------------------------------------------------

        protected override void OnDisposeManagedCore()
        {
            if (!_disposable.IsDisposed)
            {
                _disposable.Dispose();
            }

            if (!_compose?.IsDisposed ?? false)
            {
                _compose?.Dispose();
            }
        }


        protected CompositeDisposable Disposables => _disposable;


        //--------------------------------------------------------------------------------------------------------------
        //
        // Public Methods
        //
        //--------------------------------------------------------------------------------------------------------------


        #region Public Methods

        //--------------------------------------------------------------------------------------------------------------
        //
        //  CreateAsync & LoadAsync & CloseAsync
        //
        //--------------------------------------------------------------------------------------------------------------

        #region CreateAsync & LoadAsync & CloseAsync


        public async Task CreateAsync(string folder, ComposeProperty property , bool isOverride = false)
        {
            if (property == null)
            {
                throw new InvalidOperationException(SR.DocumentEngine_PropertyWasNull);
            }
            
            if (string.IsNullOrEmpty(folder))
            {
                throw new InvalidOperationException(SR.DocumentEngine_FolderWasNull);
                
            }

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            
            _path = folder;
            
            //
            // 创建内容
            var compose = new Compose(folder, isOverride);
            
            //
            // 写入属性
            _propertyCollection = compose.GetPropertyCollection(this);

            //
            //
            await _propertyCollection.SetPropertyAsync(property);

            //
            // 加载
            await LoadAsync(compose);
        }


        public async Task LoadAsync(string folder)
        {
            if (string.IsNullOrEmpty(folder))
            {
                throw new InvalidOperationException(SR.DocumentEngine_FolderWasNull);
            }
            
            if (!Directory.Exists(folder))
            {
                throw new InvalidOperationException(SR.DocumentEngine_Load_FolderWasNull);
            }

            if (!string.IsNullOrEmpty(_path) && _path == folder)
            {
                throw new InvalidOperationException(SR.DocumentEngine_Load_Again);
            }

            _path = folder;
            
            //
            // 创建内容
            var compose = new Compose(folder, false);
            
            //
            // 加载
            await LoadAsync(compose);
        }

        private async Task LoadAsync(Compose compose, ComposeProperty property = null)
        {
            //
            // 获取属性集合
            _propertyCollection ??= compose.GetPropertyCollection(this);
            
            //
            // 获取属性
            property ??= _propertyCollection.GetProperty<ComposeProperty>();

            //
            // 释放之前的实例
            _compose?.Dispose();
            
            //
            //
            _compose = compose;
            _isOpen = true;
            
            //
            // 推送
            _composeStream.OnNext(_compose);
            _propertyStream.OnNext(property);
            _isOpenStream.OnNext(_isOpen);

            //
            // 推送
            await _mediator.Publish(new ComposeCloseRequest());
            await _mediator.Publish(new ComposeOpenRequest(compose));
        }


        public async Task CloseAsync()
        {
            await _mediator.Publish(new ComposeCloseRequest());
            //
            //
            _compose = null;
            _isOpen = false;
            _path = string.Empty;
            
            //
            // 推送
            _composeStream.OnNext(_compose);
            _propertyStream.OnNext(null);
            _isOpenStream.OnNext(_isOpen);
        }

        #endregion



        //--------------------------------------------------------------------------------------------------------------
        //
        // IDocumentFileManager
        //
        //--------------------------------------------------------------------------------------------------------------

        #region IDocumentFileManager

        public Stream OpenImage(ImageResource resource)
        {
            if(resource == null)
            {
                return null;
            }

            if(resource.First == Guid.Empty)
            {
                return null;
            }

            if (!_isOpen)
            {
                return null;
            }

            var fileName = Path.Combine(_compose.ImageDirectory, resource.First.ToString("N"));

            if (!File.Exists(fileName))
            {
                return null;
            }

            using var fs = new FileStream(fileName, FileMode.Open);
            var ms = new MemoryStream();
            fs.CopyTo(ms);
            ms.Position = 0;
            return ms;
        }


        public Task<Guid> UploadImageAsync(string sourceFileName)
        {
            if (!_isOpen)
            {
                throw new InvalidOperationException(SR.DocumentEngine_InvalidOperation_ComposeNotOpen);
            }

            return Task.Run(()=> UploadAsync(sourceFileName, _compose.ImageDirectory));
        }

        public Task<Guid> UploadFileAsync(string sourceFileName)
        {
            if (!_isOpen)
            {
                throw new InvalidOperationException(SR.DocumentEngine_InvalidOperation_ComposeNotOpen);
            }

            return Task.Run(() => UploadAsync(sourceFileName, _compose.FileDirectory));
        }

        private Task<Guid> UploadAsync(string sourceFileName , string folder)
        {
            return Task.Run(() =>
            {               

                if (!File.Exists(sourceFileName))
                {
                    throw new InvalidOperationException(SR.DocumentEngine_InvalidOperation_FileNotFound);
                }

                var id = Guid.NewGuid();
                var fileName = Path.Combine(folder, id.ToString("N"));
                var retryTime = 10;

                //
                // 重试10次避免冲突。
                while (File.Exists(fileName))
                {
                    retryTime--;

                    id = Guid.NewGuid();
                    fileName = Path.Combine(folder, id.ToString("N"));

                    if (retryTime <= 0)
                    {
                        break;
                    }
                }

                using var sourceStream = new FileStream(sourceFileName, FileMode.Open);
                using var targetStream = new FileStream(fileName, FileMode.Create);

                sourceStream.CopyTo(targetStream);

                return id;
            });
        }

        #endregion IDocumentFileManager



        //--------------------------------------------------------------------------------------------------------------
        //
        // IDocumentPropertyManager
        //
        //--------------------------------------------------------------------------------------------------------------

        #region IDocumentPropertyManager

        public async Task UpdatePropertyAsync(ComposeProperty property)
        {
            if(property == null)
            {
                return;
            }

            
            //
            //
            await _propertyCollection.SetPropertyAsync(property);

            //
            //
            _propertyStream.OnNext(property);
        }

        #endregion



        //--------------------------------------------------------------------------------------------------------------
        //
        // IDocumentEngineAwaiter
        //
        //--------------------------------------------------------------------------------------------------------------

        #region IDocumentEngineAwaiter

        void IDocumentEngineAwaiter.Release()
        {
            if (!_isOpen)
            {
                return;
            }

            Interlocked.Decrement(ref _requestCount);

            if (_requestCount <= 0)
            {
                _composeOpenCompletedStream.OnNext(Unit.Default);
            }
        }

        void IDocumentEngineAwaiter.WaitOne()
        {
            if (!_isOpen)
            {
                return;
            }

            if (_requestCount <= 0)
            {
                _composeOpenStream.OnNext(Unit.Default);
            }

            Interlocked.Increment(ref _requestCount);

            
        }

        #endregion



        #endregion



        //--------------------------------------------------------------------------------------------------------------
        //
        // Public Properties
        //
        //--------------------------------------------------------------------------------------------------------------
        public IObservable<Unit> ComposeOpenStream => _composeOpenStream;
        public IObservable<Unit> ComposeOpenCompletedStream => _composeOpenCompletedStream;
        public IObservable<ICompose> ComposeStream => _composeStream;
        public IObservable<bool> IsOpenStream => _isOpenStream;
        public IObservable<ComposeProperty> PropertyStream => _propertyStream;
        public ICompose Compose => _compose;

        
        
        
        
        
        #region Singleton

        

        private static DocumentEngine _instance;
        public static DocumentEngine CreateEngine(IMediator mediator , IContainer container)
        {
            // ReSharper disable once InvertIf
            if (_instance == null)
            {
                _instance = new DocumentEngine(mediator);

                //
                // TODO: Register DocumentEngine
                container.RegisterInstance<IDocumentEngine>(_instance);
                container.UseInstance<IDocumentFileManager>(_instance);
                container.UseInstance<IDocumentPropertyManager>(_instance);
                container.UseInstance<IDocumentEngineAwaiter>(_instance);
            }

            return _instance;
        }

        #endregion
    }
}