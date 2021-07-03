using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acorisoft.Morisa.Core;
using Acorisoft.Morisa.Internals;
using LiteDB;

namespace Acorisoft.Morisa.IO
{
    /// <summary>
    /// <see cref="PropertyCollection"/> 类型表示一个属性集合。
    /// </summary>
    /// <remarks>
    /// <para>属性集合用于读取和写入数据库中的单例对象。</para>
    /// </remarks>
    public class PropertyCollection
    {
        //
        // 因为PropertyCollection本身所有人都具有所有权，因此想要实现权限控制就不能使用Collection Level 的权限控制，
        // 只能使用更细粒度的权限控制。
        private readonly ILiteCollection<BsonDocument> _targetCollection;
        private readonly string _ownerType;
        private readonly Dictionary<Type, Func<object, BsonDocument>> _serializer;
        private readonly Dictionary<Type, Func<BsonDocument, object>> _deserializer;

        internal PropertyCollection(DocumentSubEngine owner, ILiteCollection<BsonDocument> collection)
        {
            if (owner == null)
            {
                throw new InvalidOperationException();
            }

            _targetCollection = collection;
            _ownerType = owner.GetType().AssemblyQualifiedName ?? throw new ArgumentNullException(nameof(owner));
            _serializer = new Dictionary<Type, Func<object, BsonDocument>>();
            _deserializer = new Dictionary<Type, Func<BsonDocument, object>>();
        }
        
        internal PropertyCollection(DocumentEngine owner, ILiteCollection<BsonDocument> collection)
        {
            if (owner == null)
            {
                throw new InvalidOperationException();
            }

            _targetCollection = collection;
            _ownerType = owner.GetType().AssemblyQualifiedName ?? throw new ArgumentNullException(nameof(owner));
            _serializer = new Dictionary<Type, Func<object, BsonDocument>>();
            _deserializer = new Dictionary<Type, Func<BsonDocument, object>>();
        }

        public void Register<T>(Func<object, BsonDocument> serializer, Func<BsonDocument, object> deserializer)
        {
            if (serializer == null || deserializer == null)
            {
                return;
            }

            _serializer.TryAdd(typeof(T), serializer);
            _deserializer.TryAdd(typeof(T), deserializer);
        }

        private BsonDocument SerializeFromLocalSerializer(Type type, object instance)
        {
            return _serializer.TryGetValue(type, out var serializer) ? serializer(instance) : null;
        }
        
        private T DeserializeFromLocalDeserializer<T>(Type type, BsonDocument document)
        {
            return _deserializer.TryGetValue(type, out var deserialize) ? (T)deserialize(document) : default(T);
        }

        public T GetProperty<T>(Func<T> factory = null)
        {
            var key = typeof(T).AssemblyQualifiedName;
            
            if (_targetCollection.Exists(BsonHelper.Eq(key)))
            {
                var sourceDocument = _targetCollection.FindById(key);
                return BsonMapper.Global.ToObject<T>(sourceDocument) ??
                       DeserializeFromLocalDeserializer<T>(typeof(T), sourceDocument);
            }
            else
            {
                if (factory == null)
                {
                    return default(T);
                }
                
                var instance = factory();
                //
                // WriteBack
                SetProperty(instance);

                return instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="factory"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Task<T> GetPropertyAsync<T>(Func<T> factory = null)
        {
            T GetPropertyAsyncImpl()
            {
                return GetProperty<T>(factory);
            }

            return Task.Run(GetPropertyAsyncImpl);
        }
        
        /// <summary>
        /// 写入一个属性
        /// </summary>
        /// <param name="property">要写入的属性。</param>
        public void SetProperty(object property)
        {
            if (property == null)
            {
                return;
            }

            var propertyType = property.GetType();

            if (propertyType.IsInterface)
            {
                return;
            }

            //
            //
            var document = BsonMapper.Global.ToDocument(property) ?? SerializeFromLocalSerializer(propertyType, property);

            //
            //
            if (document == null)
            {
                throw new InvalidOperationException(string.Format(SR.CannotSerializeDocument, propertyType));
            }

            //
            // 判断是否存在
            if (_targetCollection.Exists(BsonHelper.Eq(propertyType.AssemblyQualifiedName)))
            {
                var sourceDocument = _targetCollection.FindById(propertyType.AssemblyQualifiedName);
                if (sourceDocument.TryGetValue(Constants.AclMoniker, out var acl))
                {
                    if (acl.AsString != _ownerType)
                    {
                        throw new InvalidOperationException(
                            string.Format(
                                SR.CannotSetProperty, propertyType));
                    }
                }
                else
                {
                    //
                    // Mark
                    sourceDocument[Constants.AclMoniker] = _ownerType;
                    
                    //
                    // WriteBack
                    _targetCollection.Upsert(sourceDocument);
                }
            }
            else
            {
                //
                // Update Acl Property
                document[Constants.AclMoniker] = _ownerType;
                
                //
                // WriteBack
                _targetCollection.Upsert(document);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public Task SetPropertyAsync(object property)
        {
            void SetPropertyAsyncImpl()
            {
                SetProperty(property);
            }

            return Task.Run(SetPropertyAsyncImpl);
        }
    }
}