namespace Acorisoft.Platform.Windows.ViewModels
{
    public interface IPageViewModel : IViewModel
    {
        void ReceiveParameter(ViewModelParameter parameter);
    }
}