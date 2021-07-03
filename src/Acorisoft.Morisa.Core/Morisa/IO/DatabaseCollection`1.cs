using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Acorisoft.Morisa.Internals;
using LiteDB;

namespace Acorisoft.Morisa.IO
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DatabaseCollection<T> : ILiteCollection<T>
    {
        private readonly CollectionReader<T> _reader;
        private readonly CollectionWriter<T> _writer;

        internal DatabaseCollection(
            string callee, 
            string collectionName, 
            ILiteCollection<ReadWriteAcl> acl,
            ILiteCollection<T> collection,
            ResourcePermission fallback = ResourcePermission.ReadOnly)
        {
            ReadWriteAcl permission;
            
            if (!acl.Exists(BsonHelper.Eq(collectionName)))
            {
                //
                // Override Permission
                permission = new ReadWriteAcl
                {
                    OwnerType = callee,
                    WhiteList = new List<string>(),
                    Collection = collectionName,
                    Fallback = fallback,
                };

                //
                // 更新权限。
                _ = acl.Upsert(permission);
            }
            else
            {
                
                //
                // 获取ACL
                permission = acl.FindById(collectionName);

                //
                // 判断
                if (permission.OwnerType != callee) 
                {
                    if (permission.WhiteList.Any(x => x == callee))
                    {
                        _writer = new CollectionWriter<T>(collection);
                    }
                    else
                    {
                        throw new InvalidOperationException(SR.CannotInitializeCollection);
                    }
                }

                _reader = new CollectionReader<T>(collection);
            }
        }

        public int Count() => _reader.Count();

        public int Count(BsonExpression predicate) => _reader.Count(predicate);

        public int Count(string predicate, BsonDocument parameters)=> _reader.Count(predicate, parameters);

        public int Count(string predicate, params BsonValue[] args)=> _reader.Count(predicate, args);

        public int Count(Expression<Func<T, bool>> predicate) => _reader.Count(predicate);

        public int Count(Query query) => _reader.Count(query);

        public bool Delete(BsonValue id)
        {
            if(_writer == null)
            {
                throw new InvalidOperationException(SR.CannotWrite);
            }

            return _writer.Delete(id);
        }

        public int DeleteAll()
        {
            if (_writer == null)
            {
                throw new InvalidOperationException(SR.CannotWrite);
            }
            return _writer.DeleteAll();
        }

        public int DeleteMany(BsonExpression predicate)
        {
            if (_writer == null)
            {
                throw new InvalidOperationException(SR.CannotWrite);
            }
            return _writer.DeleteMany(predicate);
        }

        public int DeleteMany(string predicate, BsonDocument parameters)
        {
            if (_writer == null)
            {
                throw new InvalidOperationException(SR.CannotWrite);
            }
            return _writer.DeleteMany(predicate, parameters);
        }

        public int DeleteMany(string predicate, params BsonValue[] args)
        {
            if (_writer == null)
            {
                throw new InvalidOperationException(SR.CannotWrite);
            }
            return _writer.DeleteMany(predicate, args);
        }

        public int DeleteMany(Expression<Func<T, bool>> predicate)
        {
            if (_writer == null)
            {
                throw new InvalidOperationException(SR.CannotWrite);
            }
            return _writer.DeleteMany(predicate);
        }

        public bool DropIndex(string name)
        {
            if (_writer == null)
            {
                throw new InvalidOperationException(SR.CannotWrite);
            }
            return _writer.DropIndex(name);
        }

        public bool EnsureIndex(string name, BsonExpression expression, bool unique = false)
        {
            if (_writer == null)
            {
                throw new InvalidOperationException(SR.CannotWrite);
            }
            return _writer.EnsureIndex(name, expression, unique);
        }

        public bool EnsureIndex(BsonExpression expression, bool unique = false)
        {
            if (_writer == null)
            {
                throw new InvalidOperationException(SR.CannotWrite);
            }
            return _writer.EnsureIndex(expression, unique);
        }

        public bool EnsureIndex<K>(Expression<Func<T, K>> keySelector, bool unique = false)
        {
            if (_writer == null)
            {
                throw new InvalidOperationException(SR.CannotWrite);
            }

            return _writer.EnsureIndex<K>(keySelector, unique);
        }

        public bool EnsureIndex<K>(string name, Expression<Func<T, K>> keySelector, bool unique = false)
        {
            if (_writer == null)
            {
                throw new InvalidOperationException(SR.CannotWrite);
            }
            return _writer.EnsureIndex<K>(name, keySelector, unique);
        }

        public bool Exists(BsonExpression predicate)
        {
            return _reader.Exists(predicate);
        }

        public bool Exists(string predicate, BsonDocument parameters)
        {
            return _reader.Exists(predicate, parameters);
        }

        public bool Exists(string predicate, params BsonValue[] args)
        {
            return _reader.Exists(predicate, args);
        }

        public bool Exists(Expression<Func<T, bool>> predicate)
        {
            return _reader.Exists(predicate);
        }

        public bool Exists(Query query)
        {
            return _reader.Exists(query);
        }

        public IEnumerable<T> Find(BsonExpression predicate, int skip = 0, int limit = int.MaxValue)
        {
            return _reader.Find(predicate, skip, limit);
        }

        public IEnumerable<T> Find(Query query, int skip = 0, int limit = int.MaxValue)
        {
           return _reader.Find(query, skip, limit);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate, int skip = 0, int limit = int.MaxValue)
        {
            return _reader.Find(predicate, skip, limit);
        }

        public IEnumerable<T> FindAll()
        {
            return _reader.FindAll();
        }

        public T FindById(BsonValue id)
        {
            return _reader.FindById(id);
        }

        public T FindOne(BsonExpression predicate)
        {
            return _reader.FindOne(predicate);
        }

        public T FindOne(string predicate, BsonDocument parameters)
        {
            return _reader.FindOne(predicate, parameters);
        }

        public T FindOne(BsonExpression predicate, params BsonValue[] args)
        {
            return _reader.FindOne(predicate, args);
        }

        public T FindOne(Expression<Func<T, bool>> predicate)
        {
            return _reader.FindOne(predicate);
        }

        public T FindOne(Query query)
        {
            return _reader.FindOne(query);
        }

        public ILiteCollection<T> Include<K>(Expression<Func<T, K>> keySelector)
        {
           return _reader.Include(keySelector);
        }

        public ILiteCollection<T> Include(BsonExpression keySelector)
        {
            return _reader.Include(keySelector);
        }

        public BsonValue Insert(T entity)
        {
            if (_writer == null)
            {
                throw new InvalidOperationException(SR.CannotWrite);
            }

            return _writer.Insert(entity);
        }

        public void Insert(BsonValue id, T entity)
        {
            if (_writer == null)
            {
                throw new InvalidOperationException(SR.CannotWrite);
            }

            _writer.Insert(id, entity);
        }

        public int Insert(IEnumerable<T> entities)
        {
            if (_writer == null)
            {
                throw new InvalidOperationException(SR.CannotWrite);
            }

            return _writer.Insert(entities);
        }

        public int InsertBulk(IEnumerable<T> entities, int batchSize = 5000)
        {
            if (_writer == null)
            {
                throw new InvalidOperationException(SR.CannotWrite);
            }
            return _writer.InsertBulk(entities, batchSize);
        }

        public long LongCount()
        {
            return _reader.LongCount();
        }

        public long LongCount(BsonExpression predicate)
        {
            return _reader.LongCount(predicate);
        }

        public long LongCount(string predicate, BsonDocument parameters)
        {
            return _reader.LongCount(predicate, parameters);
        }

        public long LongCount(string predicate, params BsonValue[] args)
        {
           return _reader.LongCount(predicate, args);
        }

        public long LongCount(Expression<Func<T, bool>> predicate)
        {
            return _reader.LongCount(predicate);
        }

        public long LongCount(Query query)
        {
            return _reader.LongCount(query);
        }

        public BsonValue Max(BsonExpression keySelector)
        {
            return _reader.Max(keySelector);
        }

        public BsonValue Max()
        {
            return _reader.Max();
        }

        public K Max<K>(Expression<Func<T, K>> keySelector)
        {
            return _reader.Max(keySelector);
        }

        public BsonValue Min(BsonExpression keySelector)
        {
            return _reader.Min(keySelector);
        }

        public BsonValue Min()
        {
            return _reader.Min();
        }

        public K Min<K>(Expression<Func<T, K>> keySelector)
        {
            return _reader.Min(keySelector);
        }

        public ILiteQueryable<T> Query()
        {
            return _reader.Query();
        }

        public bool Update(T entity)
        {
            if (_writer == null)
            {
                throw new InvalidOperationException(SR.CannotWrite);
            }
            return _writer.Update(entity);
        }

        public bool Update(BsonValue id, T entity)
        {
            if (_writer == null)
            {
                throw new InvalidOperationException(SR.CannotWrite);
            }
            return _writer.Update(id, entity);
        }

        public int Update(IEnumerable<T> entities)
        {
            if (_writer == null)
            {
                throw new InvalidOperationException(SR.CannotWrite);
            }
            return _writer.Update(entities);
        }

        public int UpdateMany(BsonExpression transform, BsonExpression predicate)
        {
            if (_writer == null)
            {
                throw new InvalidOperationException(SR.CannotWrite);
            }
            return _writer.UpdateMany(transform, predicate);
        }

        public int UpdateMany(Expression<Func<T, T>> extend, Expression<Func<T, bool>> predicate)
        {
            if (_writer == null)
            {
                throw new InvalidOperationException(SR.CannotWrite);
            }
            return _writer.UpdateMany(extend, predicate);
        }

        public bool Upsert(T entity)
        {
            if (_writer == null)
            {
                throw new InvalidOperationException(SR.CannotWrite);
            }
            return _writer.Upsert(entity);
        }

        public int Upsert(IEnumerable<T> entities)
        {
            if (_writer == null)
            {
                throw new InvalidOperationException(SR.CannotWrite);
            }
            return _writer.Upsert(entities);
        }

        public bool Upsert(BsonValue id, T entity)
        {
            if (_writer == null)
            {
                throw new InvalidOperationException(SR.CannotWrite);
            }
            return _writer.Upsert(id, entity);
        }
        public string Name => _reader.Name;

        public BsonAutoId AutoId => _reader.AutoId;

        public EntityMapper EntityMapper => _reader.EntityMapper;

    }
}