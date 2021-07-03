// ReSharper disable InconsistentNaming
namespace Acorisoft.Morisa
{
    public static class Constants
    {
        /// <summary>
        /// LiteDB 的ID字段名
        /// </summary>
        public const string IdMoniker = "_id";

        //
        // DocumentEngine Constants
        //
        public const string MainDatabaseName = "MainDB.Md2v1";
        public const int InitSize = 33554432;
        
        
        //
        // URI Schemes Constants
        //
        public const string ImageURIScheme = "http://example.m/image/{0}";
        public const string FileURIScheme = "http://example.m/file/{0}";
        
        
        //
        // PropertyCollection Constants
        //
        /// <summary>
        /// PropertyCollection中用于控制访问的字段
        /// </summary>
        public const string AclMoniker = "_acl";

        /// <summary>
        /// 
        /// </summary>
        public const string PropertyCollectionMoniker = "Metadatas";
    }
}