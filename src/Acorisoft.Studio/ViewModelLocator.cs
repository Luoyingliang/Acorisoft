using System;
using Acorisoft.Studio.ViewModels;

namespace Acorisoft.Studio
{
    public static class ViewModelLocator
    {
        private static readonly Lazy<AppViewModel> AppViewModelLazyInstance = new Lazy<AppViewModel>(new AppViewModel());

        /// <summary>
        /// 
        /// </summary>
        public static AppViewModel AppViewModel => AppViewModelLazyInstance.Value;
    }
}