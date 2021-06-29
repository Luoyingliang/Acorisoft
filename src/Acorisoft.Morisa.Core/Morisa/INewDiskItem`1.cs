namespace Acorisoft.Morisa
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TInfo"></typeparam>
    public interface INewDiskItem<out TInfo>: INewItem<TInfo> where TInfo : notnull
    {
        /// <summary>
        /// 获取新的内容所在的目录。
        /// </summary>
        string Directory { get; }
        
        
        /// <summary>
        /// 获取新的内容所在的文件路径。
        /// </summary>
        string FileName { get; }
    }
}