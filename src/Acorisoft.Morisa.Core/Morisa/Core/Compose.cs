using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Acorisoft.ComponentModel;
using Acorisoft.Morisa.Internals;
using Acorisoft.Morisa.IO;
using LiteDB;
// ReSharper disable ConvertToAutoProperty
// ReSharper disable ConvertToAutoPropertyWhenPossible

namespace Acorisoft.Morisa.Core
{
    /// <summary>
    /// <see cref="Compose"/> 类型表示一个创作。
    /// </summary>
    internal class Compose : Disposable, ICompose
    {
        private readonly ILiteDatabase _database;
        private readonly ILiteCollection<ReadWriteAcl> _acl;
        private readonly string _path;

        internal Compose(string path, bool isOverride = true)
        {
            var file = Path.Combine(path, Constants.MainDatabaseName);
            
            if (!isOverride && File.Exists(file))
            {
                throw new InvalidOperationException(SR.CannotCreateDatabaseInExitsProject);
            }
            
            _database = new LiteDatabase(new ConnectionString
            {
                Filename = file,
                InitialSize = Constants.InitSize,
            });
            
            _acl = _database.GetCollection<ReadWriteAcl>(Constants.AclMoniker);
            _path = path;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnDisposeManagedCore()
        {
            _database?.Dispose();
        }

        /// <summary>
        /// 创建文件系统结构
        /// </summary>
        public void BuildHierarchy()
        {
            GetDirectoryOrCreate(ImageDirectory);
            GetDirectoryOrCreate(FileDirectory);
            GetDirectoryOrCreate(BrushesDirectory);
            GetDirectoryOrCreate(AutoSaveDirectory);
            GetDirectoryOrCreate(ThumbnailDirectory);
            GetDirectoryOrCreate(VideoDirectory);
        }

        private static string GetDirectoryOrCreate(string directory)
        {
            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }

            return directory;
        }



        /// <summary>
        /// 返回指定文档子系统对指定的集合的访问权限。
        /// </summary>
        /// <param name="subEngine"></param>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        public ResourcePermission GetResourcePermission(DocumentSubEngine subEngine , string collectionName)
        {
            if (_acl == null)
            {
                throw new InvalidOperationException(SR.CannotGetPermission);
            }

            if (subEngine == null)
            {
                return ResourcePermission.Denied;
            }

            if (!_acl.Exists(BsonHelper.Eq(collectionName)))
            {
                return ResourcePermission.Denied;
            }

            //
            // 获取ACL
            var acl = _acl.FindById(collectionName);
            var key = subEngine.GetType().AssemblyQualifiedName;
            
            if (acl.OwnerType != key)
            {
                return acl.WhiteList?.Any(x => x == key) ?? false? ResourcePermission.FullControl : acl.Fallback;
            }

            return ResourcePermission.FullControl;
        }

        /// <summary>
        /// 在受限的环境中创建属性集合。
        /// </summary>
        /// <param name="instance">调用的对象。调用的对象必须与传入的参数一致。</param>
        /// <returns>返回当前创作集中存储的属性集合。</returns>
        public PropertyCollection GetPropertyCollection(DocumentSubEngine instance)
        {

            return new PropertyCollection(
                instance,
                _database.GetCollection<BsonDocument>(Constants.PropertyCollectionMoniker));
        }
        
        internal PropertyCollection GetPropertyCollection(DocumentEngine instance) =>new PropertyCollection(
            instance,
            _database.GetCollection<BsonDocument>(Constants.PropertyCollectionMoniker));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public DatabaseCollection<T> GetDatabaseCollection<T>(string key, DocumentSubEngine instance)
        {
            return new DatabaseCollection<T>(
                instance.GetType().AssemblyQualifiedName, 
                key, 
                _acl,
                _database.GetCollection<T>(key));
        }

        public sealed override string ToString()
        {
            return _path;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public string VideoDirectory => Path.Combine(_path, "Videos");
        
        /// <summary>
        /// 
        /// </summary>
        public string ImageDirectory => Path.Combine(_path, "Images");
        
        /// <summary>
        /// 
        /// </summary>
        public string AutoSaveDirectory => Path.Combine(_path, "AutoSave");
        
        /// <summary>
        /// 
        /// </summary>
        public string FileDirectory => Path.Combine(_path, "Files");
        
        /// <summary>
        /// 0
        /// </summary>
        public string BrushesDirectory => Path.Combine(_path, "Brushes");
        
        /// <summary>
        /// 
        /// </summary>
        public string ThumbnailDirectory => Path.Combine(_path, "Thumbnail");
        
        /// <summary>
        /// 
        /// </summary>
        public string Directory => _path;
    }
}