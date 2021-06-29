namespace Acorisoft.Morisa.Core
{
    /// <summary>
    /// <see cref="ISubmoduleAwaiter"/> 表示一个文档系统请求中心，用于实现等待所有子模块操作支持。
    /// </summary>
    public interface ISubmoduleAwaiter
    {
        /// <summary>
        /// 设置一个请求标志。
        /// </summary>
        void WaitOne();
        
        /// <summary>
        /// 取消一个请求标志。
        /// </summary>
        void Release();
    }
}