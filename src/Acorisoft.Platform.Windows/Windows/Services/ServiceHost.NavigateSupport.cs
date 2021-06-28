using System;
using System.Reactive.Subjects;
using Acorisoft.ComponentModel;
using Acorisoft.Platform.Windows.ViewModels;
using Acorisoft.Platform.Windows;
using ReactiveUI;
using Splat;

namespace Acorisoft.Platform.Windows.Services
{
    public class NavigateSupportService : Disposable, INavigateSupportService
    {
        private readonly Subject<IPageViewModel> _page;
        private readonly IFullLogger _logger;
        private readonly ViewModelParameter Empty = new ViewModelParameter();

        public NavigateSupportService()
        {
            _page = new Subject<IPageViewModel>();
            _logger = Locator.Current.GetService<ILogManager>()?.GetLogger(typeof(NavigateSupportService));
        }

        protected sealed override void OnDisposeManagedCore()
        {
            if (!_page.IsDisposed)
            {
                _page.Dispose();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        public void Navigate<TViewModel>() where TViewModel : IPageViewModel
        {
            if (Locator.CurrentMutable.HasRegistration(typeof(TViewModel)))
            {
                var page = Locator.Current.GetService<TViewModel>();
                Navigate(page, Empty);
            }
            else
            {
                _logger.Info($"{typeof(TViewModel)} 类型不存在，无法导航");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <typeparam name="TViewModel"></typeparam>
        public void Navigate<TViewModel>(ViewModelParameter param) where TViewModel : IPageViewModel
        {
            if (Locator.CurrentMutable.HasRegistration(typeof(TViewModel)))
            {
                var page = Locator.Current.GetService<TViewModel>();
                Navigate(page, param ??  Empty);
            }
            else
            {
                _logger.Info($"{typeof(TViewModel)} 类型不存在，无法导航");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="param"></param>
        public void Navigate(Type type, ViewModelParameter param)
        {
            if (type == null)
            {
                return;
            }
            
            if (type.IsAssignableFrom(typeof(IPageViewModel)))
            {
                var page = (IPageViewModel)Locator.Current.GetService(type);
                Navigate(page, param ?? Empty);
            }
            else
            {
                _logger.Info($"{type.FullName} 类型不是 IPageViewModel 接口的实现，无法导航");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="param"></param>
        public void Navigate(IPageViewModel vm, ViewModelParameter param)
        {
            if (vm == null)
            {
                return;
            }
            
            //
            // 接受参数。
            vm.ReceiveParameter(param);

            if (!_page.HasObservers)
            {
                _logger.Info($"导航服务没有观察者，导航结束");
                return;
            }
            //
            // 路由
            _page.OnNext(vm);
        }

        /// <summary>
        /// 获取 <see cref="NavigateSupportService"/> 的视图模型流 
        /// </summary>
        public IObservable<IPageViewModel> PageStream => _page;
    }
}