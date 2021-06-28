using System;
using System.Reactive;

namespace Acorisoft.Platform.Windows.Services
{
    public interface IAwaitService
    {
        /// <summary>
        /// 等待操作完成。
        /// </summary>
        void Await();
        
        /// <summary>
        /// 更新内容
        /// </summary>
        /// <param name="content">提示内容</param>
        void Update(string content);
        
        /// <summary>
        /// 关闭窗口
        /// </summary>
        void Cancel();
        
        IObservable<Unit> BeginAwait { get; }
        IObservable<Unit> EndAwait { get; }
        IObservable<string> UpdateAwaitContent { get; }
    }
}