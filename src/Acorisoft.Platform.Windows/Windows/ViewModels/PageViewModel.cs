using System;
using System.Reactive.Subjects;
using Acorisoft.Platform.Windows.Services;
using Acorisoft.Platform.Windows.ViewModels;

namespace Acorisoft.Platform.Windows.ViewModels
{
    public class PageViewModel : ViewModel, IPageViewModel
    {
        private readonly Subject<string> _title;

        protected PageViewModel()
        {
            _title = new Subject<string>();
        }
        

        protected override void OnDisposeManagedCore()
        {
            _title?.Dispose();
            base.OnDisposeManagedCore();
        }

        /// <summary>
        /// 获取全局的 <see cref="IExtraViewSupportService"/> 接口服务。
        /// </summary>
        protected IExtraViewSupportService ExtraViewSupportService => ServiceHost.ExtraViewSupportService;


        protected ViewModelParameter Parameter { get; private set; }

        
        
        #region IPageViewModel Interface Members

        


        /// <summary>
        /// 接受视图模型参数。
        /// </summary>
        /// <param name="parameter"></param>
        /// <remarks>
        /// 用于跳转参数接收。
        /// </remarks>
        void IPageViewModel.ReceiveParameter(ViewModelParameter parameter)
        {
            Parameter = parameter;
        }
        
        /// <summary>
        /// 获取标题数据流。
        /// </summary>
        IObservable<string> IPageViewModel.TitleStream => _title;
        
        
        #endregion
    }
}