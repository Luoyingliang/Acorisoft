using System;
using System.Windows;
using Acorisoft.Platform.Windows.ViewModels;
using ImTools;
using ReactiveUI;

namespace Acorisoft.Platform.Windows.Views
{
    public class DialogView<TViewModel>: ReactiveUserControl<TViewModel> where TViewModel : DialogViewModel, IDialogViewModel
    {
        protected DialogView()
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
            ViewModel?.Stop();
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