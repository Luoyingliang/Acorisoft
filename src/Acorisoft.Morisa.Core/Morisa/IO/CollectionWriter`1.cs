using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LiteDB;

// ReSharper disable InconsistentNaming
// ReSharper disable RedundantTypeArgumentsOfMethod
namespace Acorisoft.Morisa.IO
{
    public class CollectionWriter<T> : ILiteCollection<T>
    {
        private readonly ILiteCollection<T> _collection;
        
        internal  CollectionWriter(ILiteCollection<T> collection)  =>
            _collection = collection ?? throw new ArgumentNullException(nameof(collection));

        public ILiteCollection<T> Include<K>(Expression<Func<T, K>> keySelector)
        {
            return _collection.Include(keySelector);
        }

        public ILiteCollection<T> Include(BsonExpression keySelector)
        {
            return _collection.Include(keySelector);
        }

        public bool Upsert(T entity)
        {
            return _collection.Upsert(entity);
        }

        public int Upsert(IEnumerable<T> entities)
        {
            return _collection.Upsert(entities);
        }

        public bool Upsert(BsonValue id, T entity)
        {
            return _collection.Upsert(id, entity);
        }

        public bool Update(T entity)
        {
            return _collection.Update(entity);
        }

        public bool Update(BsonValue id, T entity)
        {
            return _collection.Update(id, entity);
        }

        public int Update(IEnumerable<T> entities)
        {
            return _collection.Update(entities);
        }

        public int UpdateMany(BsonExpression transform, BsonExpression predicate)
        {
            return _collection.UpdateMany(transform, predicate);
        }

        public int UpdateMany(Expression<Func<T, T>> extend, Expression<Func<T, bool>> predicate)
        {
            return _collection.UpdateMany(extend, predicate);
        }

        public BsonValue Insert(T entity)
        {
            return _collection.Insert(entity);
        }

        public void Insert(BsonValue id, T entity)
        {
            _collection.Insert(id, entity);
        }

        public int Insert(IEnumerable<T> entities)
        {
            return _collection.Insert(entities);
        }

        public int InsertBulk(IEnumerable<T> entities, int batchSize = 5000)
        {
            return _collection.InsertBulk(entities, batchSize);
        }

        public bool EnsureIndex(string name, BsonExpression expression, bool unique = false)
        {
            return _collection.EnsureIndex(name, expression, unique);
        }

        public bool EnsureIndex(BsonExpression expression, bool unique = false)
        {
            return _collection.EnsureIndex(expression, unique);
        }

        public bool EnsureIndex<K>(Expression<Func<T, K>> keySelector, bool unique = false)
        {
            return _collection.EnsureIndex(keySelector, unique);
        }

        public bool EnsureIndex<K>(string name, Expression<Func<T, K>> keySelector, bool unique = false)
        {
            return _collection.EnsureIndex(name, keySelector, unique);
        }

        public bool DropIndex(string name)
        {
            return _collection.DropIndex(name);
        }

        public ILiteQueryable<T> Query()
        {
            return _collection.Query();
        }

        public IEnumerable<T> Find(BsonExpression predicate, int skip = 0, int limit = 2147483647)
        {
            return _collection.Find(predicate, skip, limit);
        }

        public IEnumerable<T> Find(Query query, int skip = 0, int limit = 2147483647)
        {
            return _collection.Find(query, skip, limit);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate, int skip = 0, int limit = 2147483647)
        {
            return _collection.Find(predicate, skip, limit);
        }

        public T FindById(BsonValue id)
        {
            return _collection.FindById(id);
        }

        public T FindOne(BsonExpression predicate)
        {
            return _collection.FindOne(predicate);
        }

        public T FindOne(string predicate, BsonDocument parameters)
        {
            return _collection.FindOne(predicate, parameters);
        }

        public T FindOne(BsonExpression predicate, params BsonValue[] args)
        {
            return _collection.FindOne(predicate, args);
        }

        public T FindOne(Expression<Func<T, bool>> predicate)
        {
            return _collection.FindOne(predicate);
        }

        public T FindOne(Query query)
        {
            return _collection.FindOne(query);
        }

        public IEnumerable<T> FindAll()
        {
            return _collection.FindAll();
        }

        public bool Delete(BsonValue id)
        {
            return _collection.Delete(id);
        }

        public int DeleteAll()
        {
            return _collection.DeleteAll();
        }

        public int DeleteMany(BsonExpression predicate)
        {
            return _collection.DeleteMany(predicate);
        }

        public int DeleteMany(string predicate, BsonDocument parameters)
        {
            return _collection.DeleteMany(predicate, parameters);
        }

        public int DeleteMany(string predicate, params BsonValue[] args)
        {
            return _collection.DeleteMany(predicate, args);
        }

        public int DeleteMany(Expression<Func<T, bool>> predicate)
        {
            return _collection.DeleteMany(predicate);
        }

        public int Count()
        {
            return _collection.Count();
        }

        public int Count(BsonExpression predicate)
        {
            return _collection.Count(predicate);
        }

        public int Count(string predicate, BsonDocument parameters)
        {
            return _collection.Count(predicate, parameters);
        }

        public int Count(string predicate, params BsonValue[] args)
        {
            return _collection.Count(predicate, args);
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            return _collection.Count(predicate);
        }

        public int Count(Query query)
        {
            return _collection.Count(query);
        }

        public long LongCount()
        {
            return _collection.LongCount();
        }

        public long LongCount(BsonExpression predicate)
        {
            return _collection.LongCount(predicate);
        }

        public long LongCount(string predicate, BsonDocument parameters)
        {
            return _collection.LongCount(predicate, parameters);
        }

        public long LongCount(string predicate, params BsonValue[] args)
        {
            return _collection.LongCount(predicate, args);
        }

        public long LongCount(Expression<Func<T, bool>> predicate)
        {
            return _collection.LongCount(predicate);
        }

        public long LongCount(Query query)
        {
            return _collection.LongCount(query);
        }

        public bool Exists(BsonExpression predicate)
        {
            return _collection.Exists(predicate);
        }

        public bool Exists(string predicate, BsonDocument parameters)
        {
            return _collection.Exists(predicate, parameters);
        }

        public bool Exists(string predicate, params BsonValue[] args)
        {
            return _collection.Exists(predicate, args);
        }

        public bool Exists(Expression<Func<T, bool>> predicate)
        {
            return _collection.Exists(predicate);
        }

        public bool Exists(Query query)
        {
            return _collection.Exists(query);
        }

        public BsonValue Min(BsonExpression keySelector)
        {
            return _collection.Min(keySelector);
        }

        public BsonValue Min()
        {
            return _collection.Min();
        }

        public K Min<K>(Expression<Func<T, K>> keySelector)
        {
            return _collection.Min(keySelector);
        }

        public BsonValue Max(BsonExpression keySelector)
        {
            return _collection.Max(keySelector);
        }

        public BsonValue Max()
        {
            return _collection.Max();
        }

        public K Max<K>(Expression<Func<T, K>> keySelector)
        {
            return _collection.Max(keySelector);
        }

        public string Name
        {
            get => _collection.Name;
        }

        public BsonAutoId AutoId
        {
            get => _collection.AutoId;
        }

        public EntityMapper EntityMapper
        {
            get => _collection.EntityMapper;
        }
    }
}