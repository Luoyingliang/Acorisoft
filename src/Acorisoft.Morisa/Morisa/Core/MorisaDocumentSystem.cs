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

// ReSharper disable InvertIf
// ReSharper disable ConvertToAutoPropertyWithPrivateSetter
// ReSharper disable ArrangeDefaultValueWhenTypeNotEvident
// ReSharper disable ArrangeObjectCreationWhenTypeEvident
namespace Acorisoft.Morisa.Core
{

    //
    /// <summary>
    /// 
    /// </summary>
    public class MorisaDocumentSystem : Disposable, IMorisaDocumentSystem, IMorisaObjectManager
    {
        //--------------------------------------------------------------------------------------------------------------
        //
        // Constants
        //
        //--------------------------------------------------------------------------------------------------------------


        private const string MainDatabaseName = "Morisa.md2v1";
        private const string ObjectCollectionName = "Metadatas";


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
        
        [Obsolete]
        private readonly Dictionary<Type, ResourcePermission> _composePermission;

        private readonly PermissionManager _permissionManager;

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
        private ILiteCollection<BsonDocument> _objCollection;


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
            _permissionManager = new PermissionManager();
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
        // MorisaObjectManager Implementations
        // 
        //--------------------------------------------------------------------------------------------------------------

        #region MorisaObjectManager Implementations

        internal BsonDocument GetObjectIntern(string key)
        {
            if (!_isOpen)
            {
                return null;
            }

            if (_compose == null)
            {
                return null;
            }

            //
            // 获取对象集合
            _objCollection ??= (_compose as MorisaCompose)?.GetDatabase()?.GetCollection(ObjectCollectionName);

            if (_objCollection == null)
            {
                throw new InvalidOperationException();
            }

            return _objCollection.FindById(key);
        }

        internal void SetObjectIntern(string key, BsonDocument document)
        {
            if (!_isOpen)
            {
                return;
            }

            if (_compose == null)
            {
                return;
            }

            _objCollection.Upsert(key, document);
        }

        /// <summary>
        /// 获取一个对象。
        /// </summary>
        /// <typeparam name="T">将要获取的对象实例类型。</typeparam>
        /// <returns>返回该类型的对象实例。如果对象不存在则返回 default 。</returns>
        public T GetObject<T>() where T : notnull
        {
            if (!_isOpen)
            {
                return default(T);
            }

            if (_compose == null)
            {
                return default(T);
            }

            //
            // 获取对象集合
            _objCollection ??= (_compose as MorisaCompose)?.GetDatabase()?.GetCollection(ObjectCollectionName);

            if (_objCollection == null)
            {
                throw new InvalidOperationException();
            }

            //
            // 获取键
            var key = typeof(T).ToString();

            //
            // 序列化
            var bson = _objCollection.FindById(key);

            //
            // 返回默认值 或者 反序列化
            return bson == null ? default(T) : BsonMapper.Global.Deserialize<T>(bson);
        }

        /// <summary>
        /// 获取一个对象。
        /// </summary>
        /// <param name="type">要获取的对象类型。</param>
        /// <returns>返回该类型的对象实例。如果对象不存在则返回 default 。</returns>
        public object GetObject(Type type)
        {
            if (!_isOpen)
            {
                return null;
            }

            if (_compose == null)
            {
                return null;
            }

            if (type == null)
            {
                return null;
            }

            //
            // 获取对象集合
            _objCollection ??= (_compose as MorisaCompose)?.GetDatabase()?.GetCollection(ObjectCollectionName);

            if (_objCollection == null)
            {
                throw new InvalidOperationException();
            }

            //
            // 获取键
            var key = type.ToString();

            //
            // 序列化
            var bson = _objCollection.FindById(key);

            //
            // 返回默认值 或者 反序列化
            return bson == null ? null : BsonMapper.Global.ToObject(type, bson);
        }

        /// <summary>
        /// 在异步操作中完成获取一个对象的方法。
        /// </summary>
        /// <typeparam name="T">将要获取的对象实例类型。</typeparam>
        /// <returns>返回可等待获取对象操作的任务实例。</returns>
        public Task<T> GetObjectAsync<T>() where T : notnull
        {
            return Task.Run(GetObject<T>);
        }

        /// <summary>
        /// 在异步操作中完成获取一个对象。
        /// </summary>
        /// <param name="type">要获取的对象类型。</param>
        /// <returns>返回可等待获取对象操作的任务实例。</returns>
        public Task<object> GetObjectAsync(Type type)
        {
            object GetObjectAsyncImpl()
            {
                return GetObject(type);
            }

            return Task.Run(GetObjectAsyncImpl);
        }


        /// <summary>
        /// 设置一个对象。
        /// </summary>
        /// <typeparam name="T">要设置的对象类型。</typeparam>
        /// <returns>返回设置的该对象。</returns>
        public T SetObject<T>()
        {
            if (!_isOpen)
            {
                return default(T);
            }

            if (_compose == null)
            {
                return default(T);
            }
            //
            // 获取实例
            var instance = Activator.CreateInstance<T>();

            //
            // 获取键
            var key = typeof(T).ToString();

            //
            // 获得BsonDocument
            var bson = BsonMapper.Global.Serialize(instance).AsDocument;

            //
            // 插入操作
            _objCollection.Upsert(key, bson);

            return instance;
        }

        /// <summary>
        /// 设置一个对象。
        /// </summary>
        /// <param name="instance">要设置的类型实例。</param>
        /// <typeparam name="T">要设置的对象类型。</typeparam>
        /// <returns>返回设置的该对象。</returns>
        public T SetObject<T>(T instance)
        {
            if (!_isOpen)
            {
                return default(T);
            }

            if (_compose == null)
            {
                return default(T);
            }
            
            if (EqualityComparer<T>.Default.Equals(instance, default(T)))
            {
                return instance;
            }

            //
            // 获取键
            var key = typeof(T).ToString();

            //
            // 获得BsonDocument
            var bson = BsonMapper.Global.Serialize(instance).AsDocument;

            //
            // 插入操作
            _objCollection.Upsert(key, bson);

            return instance;
        }

        /// <summary>
        /// 设置一个对象。
        /// </summary>
        /// <param name="factory">返回要设置实例的工厂方法。</param>
        /// <typeparam name="T">要设置的对象类型。</typeparam>
        /// <returns>返回设置的该对象。</returns>
        public T SetObject<T>(Func<T> factory)
        {
            if (!_isOpen)
            {
                return default(T);
            }

            if (_compose == null)
            {
                return default(T);
            }
            
            if (factory == null)
            {
                return default(T);
            }

            var instance = factory();

            if (EqualityComparer<T>.Default.Equals(instance, default(T)))
            {
                return instance;
            }

            //
            // 获取键
            var key = typeof(T).ToString();

            //
            // 获得BsonDocument
            var bson = BsonMapper.Global.Serialize(instance).AsDocument;

            //
            // 插入操作
            _objCollection.Upsert(key, bson);

            return instance;
        }

        /// <summary>
        /// 在异步操作中完成设置对象。
        /// </summary>
        /// <typeparam name="T">要设置的对象类型。</typeparam>
        /// <returns>返回可等待设置对象操作的任务实例。</returns>
        public Task<T> SetObjectAsync<T>()
        {
            return Task.Run(SetObject<T>);
        }

        /// <summary>
        /// 在异步操作中完成设置对象。
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>返回可等待设置对象操作的任务实例。</returns>
        public Task<T> SetObjectAsync<T>(T instance)
        {
            T SetObjectAsyncImpl()
            {
                return SetObject<T>(instance);
            }

            return Task.Run(SetObjectAsyncImpl);
        }

        /// <summary>
        /// 在异步操作中完成设置对象。
        /// </summary>
        /// <param name="factory">返回要设置实例的工厂方法。</param>
        /// <typeparam name="T">要设置的对象类型。</typeparam>
        /// <returns>返回可等待设置对象操作的任务实例。</returns>
        public Task<T> SetObjectAsync<T>(Func<T> factory)
        {
            if (factory == null)
            {
                return Task.FromResult(default(T));
            }

            T SetObjectAsyncImpl()
            {
                var instance = factory();
                return SetObject<T>(instance);
            }

            return Task.Run(SetObjectAsyncImpl);
        }

        #endregion


        //--------------------------------------------------------------------------------------------------------------
        //
        // Permission Implementations
        // 
        //--------------------------------------------------------------------------------------------------------------


        #region Permission Ver1

        private const string PermissionDictionaryKey = "69CBEFF8-809B-41AC-B2DF-C9D7AE0FBACB";

        //
        // 第一版的权限管理操作伴随着风险，我们还是实验起了第二种权限管理方式
        //
        // 
        [Obsolete]
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

        [Obsolete]
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

        [Obsolete]
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
            return !_composePermission.TryGetValue(typeof(T), out var permission)
                ? ResourcePermission.V1_None
                : permission;
        }

        /// <summary>
        /// 获取指定类型的权限。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ResourcePermission GetPermission(Type type)
        {
            return type == null ? ResourcePermission.V1_None :
                !_composePermission.TryGetValue(type, out var permission) ? ResourcePermission.V1_None : permission;
        }


        public void UpdatePermission()
        {
        }

        #endregion


        #region Permission Ver2

        private const string PermissionVersion2Key = "Permission@7AFAE979-7DAD-4A4E-82A4-EC4F1B483391";

        internal class PermissionPersistent
        {
            [BsonField("o")] public Type Owner { get; set; }

            [BsonField("t")] public Type Target { get; set; }

            [BsonField("p")] public ResourcePermission Permission { get; set; }
        }

        internal sealed class PermissionList : Dictionary<Type, ResourcePermission>
        {
        }

        internal class PermissionManager
        {
            private const string PermissionPersistentListCountName = "c";
            private const string PermissionPersistentListDataName = "a";
            private const string P2OwnerName = "o";
            private const string P2TargetName = "t";
            private const string P2PersistentName = "p";
            private readonly List<PermissionPersistent> _p2List;
            private readonly Dictionary<Type, PermissionList> _pVec;

            public PermissionManager()
            {
                _p2List = new List<PermissionPersistent>();
                _pVec = new Dictionary<Type, PermissionList>();
            }

            public void Load(IEnumerable<PermissionPersistent> pl)
            {
                foreach (var permission in pl)
                {
                    if (!_pVec.TryGetValue(permission.Owner, out var vec))
                    {
                        vec = new PermissionList();
                        vec.Add(permission.Target, permission.Permission);
                    }
                    else
                    {
                        if (!vec.TryGetValue(permission.Target, out var rp))
                        {
                            vec.TryAdd(permission.Target,permission.Permission);
                        }
                    }
                }
            }

            public ResourcePermission GetPermission(Type operatorType, Type targetType)
            {
                return ResourcePermission.Denied;
            }

            public BsonDocument Save() => Serialize(_p2List);

            //
            // 序列化 PermissionPersistent
            public static BsonDocument Serialize(PermissionPersistent p2)
            {
                return new BsonDocument
                {
                    {P2OwnerName, p2.Owner.AssemblyQualifiedName},
                    {P2TargetName, p2.Target.AssemblyQualifiedName},
                    {P2PersistentName, new BsonValue((int) p2.Permission)}
                };
            }
            
            //
            // 序列化 List<PermissionPersistent>
            public static BsonDocument Serialize(List<PermissionPersistent> pl)
            {
                return new BsonDocument()
                {
                    {PermissionPersistentListCountName, pl.Count},
                    {PermissionPersistentListDataName, new BsonArray(pl.Select(Serialize))}
                };
            }

            public static PermissionPersistent DeserializePermissionPersistent(BsonValue value)
            {
                var document = value.AsDocument;
                return new PermissionPersistent
                {
                    Owner = Type.GetType(document[P2OwnerName].AsString),
                    Target = Type.GetType(document[P2TargetName].AsString),
                    Permission = (ResourcePermission) document[P2PersistentName].AsInt32
                };
            }

            public static List<PermissionPersistent> DeserializeAll(BsonValue value)
            {
                var document = value.AsDocument;
                var count = document[PermissionPersistentListCountName].AsInt32;
                var list = new List<PermissionPersistent>(count);
                var array = document[PermissionPersistentListDataName].AsArray;
                for (var i = 0; i < count; i++)
                {
                    list.Add(DeserializePermissionPersistent(array[i]));
                }

                return list;
            }
        }

        internal void LoadPermission()
        {
            if (!_isOpen)
            {
                return;
            }

            if (_compose == null)
            {
                return;
            }

            if (_objCollection.Exists(Query.EQ("_id", PermissionVersion2Key)))
            {
                //
                // 序列化
                var document = GetObjectIntern(PermissionVersion2Key);

                //
                // 序列化全部
                var pl = PermissionManager.DeserializeAll(document);

                //
                // 加载全部
                _permissionManager.Load(pl);
            }
            else
            {
                InitializePermission();
                SavePermission();
            }
        }

        internal void InitializePermission()
        {
            
        }

        internal void SavePermission()
        {
            if (!_isOpen)
            {
                return;
            }

            if (_compose == null)
            {
                return;
            }

            //
            //
            var document = _permissionManager.Save();
            
            //
            //
            _objCollection.Upsert(PermissionVersion2Key, document);
        }
        
        
        #endregion


        //--------------------------------------------------------------------------------------------------------------
        //
        // IMorisaPropertyManager Implementations
        // 
        //--------------------------------------------------------------------------------------------------------------

        #region IMorisaPropertyManager Implementations

        /// <summary>
        /// 同步写入方法
        /// </summary>
        /// <param name="property">要写入的属性实例。</param>
        public void SetProperty(object property)
        {
            if (property == null)
            {
                return;
            }

            if (!_isOpen || _compose == null)
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
            // TODO : Considering About enable User Data ACL
            // if (GetPermission(operatorType) != ResourcePermission.V1_FullControl)
            // {
            //     return;
            // }

            //
            // 写入属性
            _objCollection.Upsert(key, bson);
        }

        /// <summary>
        /// 在一个异步操作中写入属性
        /// </summary>
        /// <param name="property">要写入的属性实例。</param>
        /// <returns>返回一个可以等待的实例</returns>
        public Task SetPropertyAsync(object property)
        {
            void SetPropertyAsyncImpl()
            {
                SetProperty(property);
            }

            return Task.Run(SetPropertyAsyncImpl);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetProperty<T>()
        {
            if (!_isOpen || _compose == null)
            {
                throw new InvalidOperationException("无效的操作");
            }

            if (_objCollection == null)
            {
                throw new InvalidOperationException("无效的操作");
            }

            //
            // 检测权限
            // TODO : Considering About enable User Data ACL
            // if (GetPermission(operatorType) == ResourcePermission.V1_None)
            // {
            //     return default(T);
            // }

            var key = typeof(T).ToString();
            var bson = _objCollection.FindById(key);

            return BsonMapper.Global.Deserialize<T>(bson);
        }


        /// <summary>
        /// 在一个异步操作中写入属性
        /// </summary>
        /// <returns>返回一个可以等待的实例</returns>
        public Task<T> GetPropertyAsync<T>()
        {
            T GetPropertyAsyncImpl()
            {
                return GetProperty<T>();
            }

            return Task.Run(GetPropertyAsyncImpl);
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

        public async Task CreateAsync(string folder, string name)
        {
            await CreateAsync(new NewDiskItem<MorisaComposeProperty>(
                folder, 
                GetDatabaseNameFromFolder(folder),
                name, 
                new MorisaComposeProperty
                {
                    Name = name,
                    
                }));
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
                var compose = MorisaCompose.Open(folder, dbFileName);

                //
                // 构建项目结构
                compose.BuildHierarchy();

                //
                // 更改
                _compose = compose;
                _isOpen = true;


                //
                // 加载权限
                // TODO : Considering About enable User Data ACL
                // InitializePermission();
                // SavePermission();

                //
                // 写入属性
                SetProperty(newItem.Info);
                _propertyStream.OnNext(newItem.Info);

                //
                // 推送更改通知到数据流
                _composeStream.OnNext(_compose);
                _isOpenStream.OnNext(_isOpen);


                //
                // 推送更改通知到子模块
                await _mediator.Publish(new ComposeCloseRequest());
                await _mediator.Publish(new ComposeOpenRequest(compose));
            }
            catch (Exception ex)
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
                var compose = MorisaCompose.Open(folder, dbFileName);

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
                // TODO : Considering About enable User Data ACL
                // LoadPermission();

                //
                // 读取属性
                var property = GetProperty<MorisaComposeProperty>();
                _propertyStream.OnNext(property);

                //
                // 推送更改通知到子模块
                await _mediator.Publish(new ComposeCloseRequest());
                await _mediator.Publish(new ComposeOpenRequest(compose));
            }
            catch (Exception ex)
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