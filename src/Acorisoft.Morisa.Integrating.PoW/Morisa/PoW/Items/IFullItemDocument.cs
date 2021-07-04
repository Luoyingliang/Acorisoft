using Acorisoft.Morisa.Documents.Items;

namespace Acorisoft.Morisa.PoW.Items
{
    public interface IFullItemDocument : IItemDocument
    {
        /// <summary>
        /// 获取或设置文档的描述。
        /// </summary>
        string Whisper { get; set; }
    }
}