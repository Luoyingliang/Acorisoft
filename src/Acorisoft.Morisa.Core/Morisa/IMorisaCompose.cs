using LiteDB;

namespace Acorisoft.Morisa
{
    /// <summary>
    /// <see cref="IMorisaCompose"/> 表示一个创作。
    /// </summary>
    public interface IMorisaCompose
    {
        /// <summary>
        /// 获取数据集合。
        /// </summary>
        /// <param name="collectionName">指定要获取的数据集合名。</param>
        /// <returns>返回一个数据集合。</returns>
        ILiteCollection<BsonDocument> GetCollection(string collectionName);
        
        /// <summary>
        /// 获取数据集合。
        /// </summary>
        /// <param name="collectionName">指定要获取的数据集合名。</param>
        /// <typeparam name="T">要获取的数据集合类型。</typeparam>
        /// <returns>返回一个数据集合。</returns>
        ILiteCollection<T> GetCollection<T>(string collectionName);
        
        /// <summary>
        /// 构建创作集目录结构
        /// </summary>
        void BuildHierarchy();
    }
}