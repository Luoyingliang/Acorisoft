using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Acorisoft.Platform.Windows.Services;
using ReactiveUI;
using Splat;

namespace Acorisoft.Platform.Windows.ViewModels
{
    public abstract class ViewModel : ReactiveObject, IRoutableViewModel, IViewModel
    {
        
        private bool _disposedValue;
        
        
        //--------------------------------------------------------------------------------------------------------------
        //
        // Navigate Methods
        //
        //--------------------------------------------------------------------------------------------------------------
        #region Navigate Methods
        
        public void Navigate<TViewModel>() where TViewModel : PageViewModel, IPageViewModel
        {
            ServiceHost.NavigateSupportService.Navigate<TViewModel>();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <typeparam name="TViewModel"></typeparam>
        public void Navigate<TViewModel>(ViewModelParameter param) where TViewModel : IPageViewModel
        {
            ServiceHost.NavigateSupportService.Navigate<TViewModel>(param);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="param"></param>
        public void Navigate(Type type, ViewModelParameter param)
        {
            ServiceHost.NavigateSupportService.Navigate(type, param);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="param"></param>
        public void Navigate(IPageViewModel vm, ViewModelParameter param)
        {
            ServiceHost.NavigateSupportService.Navigate(vm, param);
        }
        
        #endregion
        
        
        //--------------------------------------------------------------------------------------------------------------
        //
        // IDisposable Interface Members
        //
        //--------------------------------------------------------------------------------------------------------------

        #region IDisposable Interface Members

        

        protected virtual void OnDisposeManagedCore()
        {
            
        }

        protected virtual void OnDisposeUnmanagedCore()
        {
            
        }
        
        protected void Dispose(bool disposing)
        {
            if (_disposedValue)
            {
                return;
            }
            
            if (disposing)
            {
                OnDisposeManagedCore();
            }

            OnDisposeUnmanagedCore();
            _disposedValue = true;
        }

        ~ViewModel()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        
        
        #endregion
        
        
        //--------------------------------------------------------------------------------------------------------------
        //
        // Navigate Methods
        //
        //--------------------------------------------------------------------------------------------------------------
        
        public virtual void Start()
        {
        }

        public virtual void Stop()
        {
        }

        /// <summary>
        /// 设置指定字段的值并通知更改
        /// </summary>
        /// <typeparam name="T">指定要设置的字段类型。</typeparam>
        /// <param name="source">原始字段。</param>
        /// <param name="value">要设置的值。</param>
        /// <param name="name">设置新值的属性名，缺省。</param>
        protected bool Set<T>(ref T source, T value, [CallerMemberName] string name = "")
        {
            if (EqualityComparer<T>.Default.Equals(source, value))
            {
                return false;
            }

            this.RaisePropertyChanging(name);
            source = value;
            this.RaisePropertyChanged(name);

            return true;
        }


        /// <summary>
        /// 手动推送属性值正在变化的通知。
        /// </summary>
        /// <param name="name">指定属性值正在发生变化的属性名。</param>
        protected void RaiseUpdating(string name)
        {
            this.RaisePropertyChanging(name);
        }
        /// <summary>
        /// 手动推送属性值变化的通知。
        /// </summary>
        /// <param name="name">指定属性值发生变化的属性名。</param>
        protected void RaiseUpdated([CallerMemberName]string name = "")
        {
            this.RaisePropertyChanged(name);
        }
        
        /// <summary>
        /// 
        /// </summary>
        public virtual string UrlPathSegment => string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public IScreen HostScreen => Locator.Current.GetService<IScreen>();

    }
}