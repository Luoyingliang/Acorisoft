using System.Threading.Tasks;
using System.Windows.Input;
using Acorisoft.Platform.Windows.Services;

namespace Acorisoft.Platform.Windows.ViewModels
{
    public interface IDialogViewModel : IViewModel
    {
        ICommand Finish { get; }
        ICommand Cancel { get; }
    }

}