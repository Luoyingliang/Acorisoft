using Acorisoft.Morisa.Documents.Items;

namespace Acorisoft.Morisa.PoW.Items
{
    public abstract class FullItemDocument : ItemDocument, IFullItemDocument
    {
        /// <summary>
        /// 获取或设置文档的描述。
        /// </summary>
        public string Whisper { get; set; }
    }
}