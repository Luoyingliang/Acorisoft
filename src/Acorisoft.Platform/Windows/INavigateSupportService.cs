using System;
using Acorisoft.Platform.Windows.ViewModels;

namespace Acorisoft.Platform.Windows
{
    public interface INavigateSupportService : IDisposable
    {
        void Navigate<TViewModel>() where TViewModel : IPageViewModel;
        void Navigate<TViewModel>(ViewModelParameter param) where TViewModel : IPageViewModel;
        void Navigate(Type type, ViewModelParameter param);
        void Navigate(IPageViewModel vm, ViewModelParameter param);
        IObservable<IPageViewModel> PageStream { get; }
    }
}