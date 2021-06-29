namespace Acorisoft.Morisa
{
    public interface INewItem<out TInfo> where TInfo : notnull
    {
        /// <summary>
        /// 获取名称。
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// 获取或设置额外的信息
        /// </summary>
        TInfo Info { get; }
    }
}