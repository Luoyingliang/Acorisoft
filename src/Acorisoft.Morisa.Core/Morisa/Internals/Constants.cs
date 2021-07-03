namespace Acorisoft.Morisa.Internals
{
    public static class Constants
    {
        /// <summary>
        /// LiteDB 的ID字段名
        /// </summary>
        public const string IdMoniker = "_id";

        public const string MainDatabaseName = "MainDB.Md2v1";
        public const int InitSize = 33554432;
        
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