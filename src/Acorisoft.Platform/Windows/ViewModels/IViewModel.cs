namespace Acorisoft.Platform.Windows.ViewModels
{
    /// <summary>
    /// <see cref="IViewModel"/> 表示一个视图模型
    /// </summary>
    public interface IViewModel
    {
        /// <summary>
        /// 开始该视图模型的生命周期
        /// </summary>
        void Start();
        
        /// <summary>
        /// 结束该视图模型的生命周期
        /// </summary>
        void Stop();
    }
}