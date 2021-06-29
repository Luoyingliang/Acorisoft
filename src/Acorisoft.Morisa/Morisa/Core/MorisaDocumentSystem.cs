using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MediatR;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using DryIoc;
using LiteDB;
using Unit = System.Reactive.Unit;
using Disposable = Acorisoft.ComponentModel.Disposable;

namespace Acorisoft.Morisa.Core
{
    // ##Resharper diable comments
    //
    // ReSharper disable InvertIf
    // ReSharper disable ConvertToAutoPropertyWithPrivateSetter
    // ReSharper disable ArrangeDefaultValueWhenTypeNotEvident
    //
    /// <summary>
    /// 
    /// </summary>
    public class MorisaDocumentSystem : Disposable, IMorisaDocumentSystem
    {
        //--------------------------------------------------------------------------------------------------------------
        //
        // Constants
        //
        //--------------------------------------------------------------------------------------------------------------
        
        
        
        private const string MainDatabaseName = "Morisa.md2v1";
        
        
        
        
        //--------------------------------------------------------------------------------------------------------------
        //
        // Private Readonly Fields
        //
        //--------------------------------------------------------------------------------------------------------------
        
        
        private readonly CompositeDisposable _disposable;
        private readonly Subject<Unit> _composeOpening;
        private readonly Subject<Unit> _composeOpenCompleted;
        private readonly BehaviorSubject<MorisaComposeProperty> _propertyStream;
        private readonly BehaviorSubject<bool> _isOpenStream;
        private readonly BehaviorSubject<IMorisaCompose> _composeStream;
        private readonly IMediator _mediator;
        private readonly Dictionary<Type, ResourcePermission> _composePermission; 

        //--------------------------------------------------------------------------------------------------------------
        //
        // Private Fields
        //
        //--------------------------------------------------------------------------------------------------------------
        
        
        //
        // 用于实现等待全局子模块的读取操作。
        private int _waitingRequestCount;
        private bool _isOpen;
        private IMorisaCompose _compose;


        //--------------------------------------------------------------------------------------------------------------
        //
        // Constructors
        //
        //--------------------------------------------------------------------------------------------------------------
        private MorisaDocumentSystem(IMediator mediator)
        {
            _disposable = new CompositeDisposable();
            _composeOpening = new Subject<Unit>();
            _composeOpenCompleted = new Subject<Unit>();
            _isOpenStream = new BehaviorSubject<bool>(false);
            _composeStream = new BehaviorSubject<IMorisaCompose>(null);
            _composePermission = new Dictionary<Type, ResourcePermission>();
            _propertyStream = new BehaviorSubject<MorisaComposeProperty>(null);
            _waitingRequestCount = 0;
            _mediator = mediator;


            _disposable.Add(_composeOpening);
            _disposable.Add(_composeOpenCompleted);
            _disposable.Add(_isOpenStream);
            _disposable.Add(_composeStream);
            _disposable.Add(_propertyStream);
        }
        
        
        
        
        
        
        //--------------------------------------------------------------------------------------------------------------
        //
        // Singleton
        // 
        //--------------------------------------------------------------------------------------------------------------

        #region Singleton

        private static MorisaDocumentSystem _instance;

        /// <summary>
        /// 创建一个文档系统实例。
        /// </summary>
        /// <param name="container">容器</param>
        /// <returns>返回一个新的</returns>
        public static MorisaDocumentSystem Create(IContainer container)
        {
            if (_instance == null)
            {
                _instance = new MorisaDocumentSystem(container.Resolve<IMediator>());
                //
                // 注册
                container.RegisterInstance<IMorisaDocumentSystem>(_instance);
                container.UseInstance<ISubmoduleAwaiter>(_instance);
            }


            return _instance;
        }

        #endregion
        
        
        //--------------------------------------------------------------------------------------------------------------
        //
        // Permission Implementations
        // 
        //--------------------------------------------------------------------------------------------------------------



        #region Permission

        private const string PermissionDictionaryKey = "69CBEFF8-809B-41AC-B2DF-C9D7AE0FBACB";
        private const string ObjectCollectionName = "Metadatas";
        
        private ILiteCollection<BsonDocument> _objCollection;

        private void LoadPermissiongDictionary()
        {
            if (!_isOpen || _compose is not MorisaCompose compose)
            {
                ThrowHelper.ThrowInvalidOperation();
                return;
            }
            
            //
            // 清空权限
            _composePermission.Clear();

            //
            // Resharper disable 
            var database = compose.GetDatabase();
            
            //
            //
            _objCollection = database.GetCollection(ObjectCollectionName);
            
            //
            //
            var bson = _objCollection.FindById(PermissionDictionaryKey);

            if (bson != null)
            {
                //
                // 反序列化
                var pDic = BsonMapper.Global.Deserialize<Dictionary<Type, ResourcePermission>>(bson);

                //
                //
                foreach (var (key, value) in pDic)
                {
                    _composePermission.Add(key, value);
                }
            }
            else
            {
                InitializePermissionDictionary();
            }
        }

        private void InitializePermissionDictionary()
        {
            if (_objCollection == null)
            {
                return;
            }
            
            _composePermission.Add(typeof(MorisaDocumentSystem), ResourcePermission.V1_FullControl);
            _composePermission.Add(typeof(IMorisaDocumentSystem), ResourcePermission.V1_FullControl);
            
            //
            // 保存
            SavePermissiongDictionary();
        }

        private void SavePermissiongDictionary()
        {
            if (_composePermission.Count <= 0)
            {
                return;
            }

            //
            // 序列化
            var bson = BsonMapper.Global.ToDocument(_composePermission);
            
            //
            //
            _objCollection.Upsert(PermissionDictionaryKey, bson);
        }

        
        

        /// <summary>
        /// 获取指定类型的权限。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ResourcePermission GetPermission<T>()
        {
            return !_composePermission.TryGetValue(typeof(T), out var permission) ? ResourcePermission.None : permission;
        }

        /// <summary>
        /// 获取指定类型的权限。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ResourcePermission GetPermission(Type type)
        {
            return  type == null ? ResourcePermission.None : !_composePermission.TryGetValue(type, out var permission) ? ResourcePermission.None : permission;
        }
        
        #endregion
        
        
        
        
        //--------------------------------------------------------------------------------------------------------------
        //
        // IMorisaPropertyManager Implementations
        // 
        //--------------------------------------------------------------------------------------------------------------

        #region IMorisaPropertyManager Implementations


        /// <summary>
        /// 内部写入操作
        /// </summary>
        /// <param name="property">要写入的属性实例。</param>
        internal void SetProperty(object property) => SetProperty(property, GetType());

        /// <summary>
        /// 内部写入操作
        /// </summary>
        /// <param name="property">要写入的属性实例。</param>
        internal void SetProperty(MorisaComposeProperty property)
        {
            SetProperty((object)property);
            _propertyStream.OnNext(property);
        }

        /// <summary>
        /// 内部读取属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        internal T GetProperty<T>() => GetProperty<T>(GetType());

        
        /// <summary>
        /// 内部读取属性
        /// </summary>
        internal MorisaComposeProperty GetProperty()
        {
            var property = GetProperty<MorisaComposeProperty>();
            _propertyStream.OnNext(property);
            return property;
        }
        
        /// <summary>
        /// 同步写入方法
        /// </summary>
        /// <param name="property">要写入的属性实例。</param>
        /// <param name="operatorType">操作此方法的类型。传递调用该方法的实例类型。</param>
        public void SetProperty(object property, Type operatorType)
        {
            if (property == null)
            {
                return;
            }

            if (!_isOpen ||_compose == null)
            {
                return;
            }

            if (_objCollection == null)
            {
                return;
            }
            
            //
            // 序列化为BsonDocument
            var bson = BsonMapper.Global.ToDocument(property);

            //
            // 获取键
            var key = property.GetType().ToString();

            if (bson == null)
            {
                //
                // TODO : add another serialize method
                return;
            }
            
            //
            // 检测权限
            if (GetPermission(operatorType) != ResourcePermission.V1_FullControl)
            {
                return;
            }

            //
            // 写入属性
            _objCollection.Upsert(key, bson);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operatorType"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetProperty<T>(Type operatorType)
        {

            if (!_isOpen ||_compose == null)
            {
                throw new InvalidOperationException("无效的操作");
            }

            if (_objCollection == null)
            {
                throw new InvalidOperationException("无效的操作");
            }

            //
            // 检测权限
            if (GetPermission(operatorType) == ResourcePermission.None)
            {
                return default(T);
            }

            var key = typeof(T).ToString();
            var bson = _objCollection.FindById(key);
            
            return BsonMapper.Global.Deserialize<T>(bson);
        }


        /// <summary>
        /// 获取属性流。
        /// </summary>
        public IObservable<MorisaComposeProperty> PropertyStream => _propertyStream;
        
        
        #endregion
        
        
        
        
        
        
        //--------------------------------------------------------------------------------------------------------------
        //
        // IMorisaDocumentSystem Implementations
        // 
        //--------------------------------------------------------------------------------------------------------------

        #region IMorisaDocumentSystem Implementations

        

        private static string GetDatabaseNameFromFolder(string folder)
        {
            return Path.Combine(folder, MainDatabaseName);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newItem"></param>
        public async Task CreateAsync(INewDiskItem<MorisaComposeProperty> newItem)
        {
            if (newItem == null)
            {
                ThrowHelper.ThrowArgumentNull(nameof(newItem));
                return;
            }

            //
            // 务必保证 Directory 属性不为空
            if (string.IsNullOrEmpty(newItem!.Directory))
            {
                ThrowHelper.ThrowArgumentNull("newItem.Directory");
                return;
            }

            //
            // 检测属性是否正确
            if (!MorisaComposeProperty.Validate(newItem!.Info))
            {
                ThrowHelper.ThrowArgumentError();
                return;
            }

            var folder = newItem.Directory;
            
            //
            // 判断目录是否存在，如果不存在则创建
            if (!Directory.Exists(newItem.Directory))
            {
                Directory.CreateDirectory(newItem.Directory);
            }

            //
            // 获取数据库地址
            var dbFileName = GetDatabaseNameFromFolder(folder);

            //
            // 检测数据库是否存在
            if (string.IsNullOrEmpty(dbFileName))
            {
                ThrowHelper.ThrowFileNotFound(dbFileName);
            }

            try
            {
                //
                // 打开创作集
                var compose = MorisaCompose.Open(dbFileName);
                
                //
                // 构建项目结构
                compose.BuildHierarchy();

                //
                // 更改
                _compose = compose;
                _isOpen = true;
                
                
                //
                // 加载权限
                InitializePermissionDictionary();
                
                //
                // 写入属性
                SetProperty(newItem.Info);
                
                //
                // 推送更改通知到数据流
                _composeStream.OnNext(_compose);
                _isOpenStream.OnNext(_isOpen);
                
                
                //
                // 推送更改通知到子模块
                await _mediator.Publish(new ComposeCloseRequest());
                await _mediator.Publish(new ComposeOpenRequest(compose));
            }
            catch(Exception ex)
            {
                throw new AggregateException("打开创作集的时候遇到错误", ex);
            }
        }
        
        /// <summary>
        /// 加载指定的创作集。
        /// </summary>
        /// <param name="folder">指定要加载的创作集文件夹位置。</param>
        /// <exception cref="AggregateException">打开失败时弹出的异常</exception>
        /// <exception cref="ArgumentNullException">指定要加载的创作集文件夹位置为空</exception>
        /// <exception cref="FileNotFoundException">指定要加载的创作集不存在。</exception>
        public async Task LoadAsync(string folder)
        {
            if (string.IsNullOrEmpty(folder))
            {
                ThrowHelper.ThrowArgumentNull(folder);
            }

            //
            // 获取数据库地址
            var dbFileName = GetDatabaseNameFromFolder(folder);

            //
            // 检测数据库是否存在
            if (string.IsNullOrEmpty(dbFileName))
            {
                ThrowHelper.ThrowFileNotFound(dbFileName);
            }

            try
            {
                //
                // 打开创作集
                var compose = MorisaCompose.Open(dbFileName);
                
                //
                // 构建项目结构
                compose.BuildHierarchy();

                //
                // 更改
                _compose = compose;
                _isOpen = true;
                
                //
                // 推送更改通知到数据流
                _composeStream.OnNext(_compose);
                _isOpenStream.OnNext(_isOpen);
                
                //
                // 加载权限
                LoadPermissiongDictionary();

                //
                // 读取属性
                GetProperty();
                
                //
                // 推送更改通知到子模块
                await _mediator.Publish(new ComposeCloseRequest());
                await _mediator.Publish(new ComposeOpenRequest(compose));
            }
            catch(Exception ex)
            {
                throw new AggregateException("打开创作集的时候遇到错误", ex);
            }
        }
        
        
        
        

        /// <summary>
        /// 获取当前文档系统是否已经打开创作。
        /// </summary>
        public bool IsOpen => _isOpen;

        /// <summary>
        /// 中介器
        /// </summary>
        public IMediator Mediator => _mediator;

        /// <summary>
        /// 获取当前的打开创作
        /// </summary>
        public IMorisaCompose Compose => _compose;
        
        /// <summary>
        /// 获取当前的文档系统的打开文件操作标志数据流
        /// </summary>
        public IObservable<bool> IsOpenStream => _isOpenStream;

        /// <summary>
        /// 获取当前的创作数据流
        /// </summary>
        public IObservable<IMorisaCompose> ComposeStream => _composeStream;

        /// <summary>
        /// 获取当前的文档系统开始打开创作标志数据流。
        /// </summary>
        public IObservable<Unit> ComposeOpening => _composeOpening;

        /// <summary>
        /// 获取当前的文档系统完成打开创作标志数据流。
        /// </summary>
        public IObservable<Unit> ComposeOpenCompleted => _composeOpenCompleted;

        #endregion
        
        
        
        
        
        
        
        
        
        
        
        //--------------------------------------------------------------------------------------------------------------
        //
        // Protected Properties
        //
        //--------------------------------------------------------------------------------------------------------------
        
        
        protected CompositeDisposable Disposable => _disposable;
        
        
        
        
        
        
        
        
        //--------------------------------------------------------------------------------------------------------------
        //
        // ISubmoduleAwaiter Implementations
        //
        //--------------------------------------------------------------------------------------------------------------

        #region ISubmoduleAwaiter

        /// <summary>
        /// 设置一个请求标志。
        /// </summary>
        void ISubmoduleAwaiter.WaitOne()
        {
            if (_waitingRequestCount == 0 && _composeOpening.HasObservers)
            {
                _composeOpening.OnNext(Unit.Default);
            }

            Interlocked.Increment(ref _waitingRequestCount);
        }

        /// <summary>
        /// 取消一个请求标志。
        /// </summary>
        void ISubmoduleAwaiter.Release()
        {
            Interlocked.Decrement(ref _waitingRequestCount);

            if (_waitingRequestCount == 0 && _composeOpenCompleted.HasObservers)
            {
                _composeOpenCompleted.OnNext(Unit.Default);
            }
        }

        #endregion
        
        
        
        
        
        
        
        
        
        
        
        
        //--------------------------------------------------------------------------------------------------------------
        //
        // Public Properties
        //
        //--------------------------------------------------------------------------------------------------------------
        
    }
}