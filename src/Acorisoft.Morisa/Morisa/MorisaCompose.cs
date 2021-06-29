using System.IO;
using LiteDB;

namespace Acorisoft.Morisa
{
    class MorisaCompose : IMorisaCompose
    {
        private const int InitSize = 33554432;
        private const string AutoSaveDirectoryName = "AutoSave";
        private const string AudioDirectoryName = "Audio";
        private const string ImageDirectoryName = "Images";
        private const string VideoDirectoryName = "Videos";
        private const string ThumbnailsDirectoryName = "Thumbnails";
        private const string BrushesDirectoryName = "Brushes";
        private const string CacheDirectoryName = "Cache";
        
        //
        // 这是个简单的密码
        private const string SPwd = "p62347.12";
        
        
        
     
        private readonly LiteDatabase _database;
        private readonly string _directory;

        private MorisaCompose(LiteDatabase database, string directory)
        {
            _database = database;
            _directory = directory;
        }

        private static string GetDirectoryIfNotExists(params string[] path)
        {
            var directory = Path.Combine(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return directory;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        public ILiteCollection<BsonDocument> GetCollection(string collectionName)
        {
            return _database.GetCollection(collectionName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collectionName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ILiteCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }
        
        public void BuildHierarchy()
        {
            GetDirectoryIfNotExists(_directory, AudioDirectoryName);
            GetDirectoryIfNotExists(_directory, AutoSaveDirectoryName);
            GetDirectoryIfNotExists(_directory, BrushesDirectoryName);
            GetDirectoryIfNotExists(_directory, CacheDirectoryName);
            GetDirectoryIfNotExists(_directory, ImageDirectoryName);
            GetDirectoryIfNotExists(_directory, ThumbnailsDirectoryName);
            GetDirectoryIfNotExists(_directory, VideoDirectoryName);
        }

        internal ILiteDatabase GetDatabase() => _database;

        public static MorisaCompose Open( string directory,string fileName)
        {
            return new MorisaCompose(new LiteDatabase(new ConnectionString
            {
                Filename = fileName,
                InitialSize = InitSize,
            }), directory);
        }
    }
}