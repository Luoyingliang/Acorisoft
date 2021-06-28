using System;
using System.Windows;
using Acorisoft.Platform.Windows.ViewModels;
using ImTools;
using ReactiveUI;

namespace Acorisoft.Platform.Windows.Views
{
    public abstract class PageView<TViewModel> : ReactiveUserControl<TViewModel> where TViewModel : PageViewModel, IPageViewModel
    {
        protected PageView()
        {
            this.WhenActivated(d =>
            {
                d(this.WhenAnyValue(x => x.ViewModel)
                          .Do(OnBindingDataContext)
                          .BindTo(this, x => x.DataContext));
            });
            
            //
            //
            this.Loaded += OnLoaded;
            this.Unloaded += OnUnloaded;
        }

        protected virtual void OnUnloaded(object sender, RoutedEventArgs e)
        {
            
        }

        protected virtual void OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel?.Start();
        }

        protected virtual void OnBindingDataContext(IObservable<TViewModel> sequence)
        {
            
        }
    }
}