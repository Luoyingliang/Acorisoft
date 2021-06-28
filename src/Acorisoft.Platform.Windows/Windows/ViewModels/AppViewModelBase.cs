using System;
using System.Globalization;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Acorisoft.Platform.Windows.Services;
using ReactiveUI;
using Splat;

namespace Acorisoft.Platform.Windows.ViewModels
{
    /// <summary>
    /// <see cref="AppViewModelBase"/> 表示一个应用视图模型基类。
    /// </summary>
    public class AppViewModelBase : ViewModel, IAppViewModel, IScreen ,IDisposable
    {
        //--------------------------------------------------------------------------------------------------------------
        //
        // Protected & Public Fields
        //
        //--------------------------------------------------------------------------------------------------------------
        
        protected readonly CompositeDisposable Disposable;
        protected readonly IFullLogger Logger;

        //--------------------------------------------------------------------------------------------------------------
        //
        // Private Fields
        //
        //--------------------------------------------------------------------------------------------------------------
        
        private string _title;
        
        
        
        
        
        //--------------------------------------------------------------------------------------------------------------
        //
        // Constructors
        //
        //--------------------------------------------------------------------------------------------------------------
        
        protected AppViewModelBase()
        {
            Disposable = new CompositeDisposable();
            Logger = Locator.Current.GetService<ILogManager>()?.GetLogger(this.GetType());
            Router = new RoutingState();
            
            var disposable1 = NavigateSupportService.PageStream
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(OnNavigating);

            var disposable2 = ExtraViewSupportService.ExtraViewParamStream.ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(DispatchExtraView);
            
            Disposable.Add(disposable1);
            Disposable.Add(disposable2);
        }


        protected override void OnDisposeManagedCore()
        {
            //
            //
            _newPageTitleDisposable?.Dispose();
            
            //
            //
            base.OnDisposeManagedCore();
        }


        //--------------------------------------------------------------------------------------------------------------
        //
        // Navigating & NavigatingFilter & IScreen Interface Members
        //
        //--------------------------------------------------------------------------------------------------------------
        
        #region Navigating & NavigatingFilter & IScreen Interface Members

        private IPageViewModel _oldPage;
        private IPageViewModel _newPage;
        private IDisposable _newPageTitleDisposable;

        private async void OnNavigating(IPageViewModel page)
        {
            // ReSharper disable once MergeCastWithTypeCheck
            if (page is not IRoutableViewModel)
            {
                return;
            }
            
            if (OnNavigateFiltering(_oldPage, page))
            {
                _oldPage = _newPage;
                _newPage = page;
                _newPageTitleDisposable?.Dispose();
                _newPageTitleDisposable = _newPage.TitleStream.ObserveOn(RxApp.MainThreadScheduler).Subscribe(x => Title = x);
                Title = await _newPage.TitleStream.LastAsync();
                var result = await Router.Navigate.Execute((IRoutableViewModel) page);
            }
            else
            {
                Logger.Info($"{page.GetType()}的导航已被过滤");
            }
        }
        
        protected virtual bool OnNavigateFiltering(IPageViewModel oldPage, IPageViewModel currentPage)
        {
            return true;
        }
        
        
        /// <summary>
        /// Screen
        /// </summary>
        public RoutingState Router { get; }
        
        /// <summary>
        /// 获取或设置当前程序的标题。
        /// </summary>
        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        #endregion

        
        
        
        
        
        
        
        //--------------------------------------------------------------------------------------------------------------
        //
        // ExtraViewSupportService Interface Members
        //
        //--------------------------------------------------------------------------------------------------------------
        
        #region ExtraViewSupportService Interface Members

        private IViewModel _context;
        private IViewModel _tool;
        private IViewModel _extra;
        private IViewModel _quick;

        private void DispatchExtraView(ExtraViewParam param)
        {
            _context = param.ContextView;
            _tool = param.ToolView;
            _extra = param.ExtraView;
            _quick = param.QuickView;
            
            RaiseUpdated(nameof(QuickView));
            RaiseUpdated(nameof(ToolView));
            RaiseUpdated(nameof(ContextView));
            RaiseUpdated(nameof(ExtraView));
        }

        /// <summary>
        /// 获取快捷视图。
        /// </summary>
        public IViewModel QuickView => _quick;
        
        /// <summary>
        /// 获取工具视图。
        /// </summary>
        public IViewModel ToolView => _tool;
        
        /// <summary>
        /// 获取上下文视图。
        /// </summary>
        public IViewModel ContextView => _context;
        
        /// <summary>
        /// 获取额外视图。
        /// </summary>
        public IViewModel ExtraView => _extra;
        
        #endregion
        
        

        
        
        
        
        
        
        
        /// <summary>
        /// 获取全局的 <see cref="INavigateSupportService"/> 接口服务。
        /// </summary>
        protected static INavigateSupportService NavigateSupportService => ServiceHost.NavigateSupportService;
        
        
        /// <summary>
        /// 获取全局的 <see cref="IExtraViewSupportService"/> 接口服务。
        /// </summary>
        protected static IExtraViewSupportService ExtraViewSupportService => ServiceHost.ExtraViewSupportService;

    }
}