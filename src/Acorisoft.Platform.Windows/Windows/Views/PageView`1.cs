using Acorisoft.Platform.Windows.ViewModels;
using ReactiveUI;

namespace Acorisoft.Platform.Windows.Views
{
    public abstract class PageView<TViewModel> : ReactiveUserControl<TViewModel> where TViewModel : PageViewModel, IPageViewModel
    {
        
    }
}