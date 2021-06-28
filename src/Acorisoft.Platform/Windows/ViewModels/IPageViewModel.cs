using System;

namespace Acorisoft.Platform.Windows.ViewModels
{
    public interface IPageViewModel : IViewModel
    {
        /// <summary>
        /// 接受视图模型参数。
        /// </summary>
        /// <param name="parameter"></param>
        /// <remarks>
        /// 用于跳转参数接收。
        /// </remarks>
        void ReceiveParameter(ViewModelParameter parameter);
        
        /// <summary>
        /// 获取标题数据流。
        /// </summary>
        IObservable<string> TitleStream { get; }
    }
}