using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Acorisoft.Platform.Windows.Services;
using ReactiveUI;
using Splat;
// ReSharper disable CA1822

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

        protected Task<IDialogSession> Dialog<TDialog>() where TDialog : DialogViewModel, IDialogViewModel
        {
            var vm = Locator.Current.GetService<TDialog>();
            return ServiceHost.DialogSupportService.OpenDialog(vm);
        }
        
        protected async Task<T> Dialog<TDialog, T>() where TDialog : DialogViewModel, IDialogViewModel
        {
            var vm = Locator.Current.GetService<TDialog>();
            var session = await ServiceHost.DialogSupportService.OpenDialog(vm);

            if (session.IsCompleted )
            {
                if (session.GetResult<T>() is T value)
                {
                    return value;
                }
                else
                {
                    if (session.Result is T newVal)
                    {
                        return newVal;
                    }
                }
            }

            return default(T);
        }
        
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
            // ???????????????????????????????????????????????????Dispose(bool disposing)????????????
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // ???????????????????????????????????????????????????Dispose(bool disposing)????????????
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
        /// ???????????????????????????????????????
        /// </summary>
        /// <typeparam name="T">?????????????????????????????????</typeparam>
        /// <param name="source">???????????????</param>
        /// <param name="value">??????????????????</param>
        /// <param name="name">????????????????????????????????????</param>
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
        /// ?????????????????????????????????????????????
        /// </summary>
        /// <param name="name">????????????????????????????????????????????????</param>
        protected void RaiseUpdating(string name)
        {
            this.RaisePropertyChanging(name);
        }
        /// <summary>
        /// ???????????????????????????????????????
        /// </summary>
        /// <param name="name">??????????????????????????????????????????</param>
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