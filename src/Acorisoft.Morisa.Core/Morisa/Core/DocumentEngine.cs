using System;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Acorisoft.Morisa.IO;
using Acorisoft.Morisa.Resources;
using MediatR;
using Disposable = Acorisoft.ComponentModel.Disposable;
// ReSharper disable ClassNeverInstantiated.Global

// ReSharper disable UnusedMember.Global

namespace Acorisoft.Morisa.Core
{
    public class DocumentEngine : Disposable
    {
        //--------------------------------------------------------------------------------------------------------------
        //
        // Avoid Boxing Methods
        //
        //--------------------------------------------------------------------------------------------------------------
        private readonly BehaviorSubject<ICompose> _composeStream;
        private readonly BehaviorSubject<bool> _isOpenStream;
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
            _disposable = new CompositeDisposable
            {
                _composeStream,
                _isOpenStream,
                _propertyStream
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


        //--------------------------------------------------------------------------------------------------------------
        //
        // Public Methods
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

        #endregion IDocumentFileManager
        
        
        #region IDocumentPropertyManager
        
        #endregion
        
        
        
        
        
        
        //--------------------------------------------------------------------------------------------------------------
        //
        // Avoid Boxing Methods
        //
        //--------------------------------------------------------------------------------------------------------------
        public IObservable<ICompose> ComposeStream => _composeStream;
        public IObservable<bool> IsOpenStream => _isOpenStream;
        public IObservable<ComposeProperty> PropertyStream => _propertyStream;
        public ICompose Compose => _compose;

        
        
        
        
        
        #region Singleton

        

        private static DocumentEngine _instance;
        public static DocumentEngine CreateEngine(IMediator mediator , Action<DocumentEngine> registerCallback)
        {
            // ReSharper disable once InvertIf
            if (_instance == null)
            {
                _instance = new DocumentEngine(mediator);
                
                //
                // TODO: Register DocumentEngine
                registerCallback?.Invoke(_instance);
            }

            return _instance;
        }

        #endregion
    }
}