using Acorisoft.Morisa.PoW.ViewModels;
using Acorisoft.Morisa.PoW.Views;
using Acorisoft.Platform.Windows.ViewModels;

namespace Acorisoft.Studio.ViewModels
{
    /// <summary>
    /// <see cref="AppViewModel"/> 类型表示一个应用程序视图模型。用于表示应用程序级别的视图模型，提供应用程序级别的数据绑定支持。
    /// </summary>
    public class AppViewModel : AppViewModelBase
    {
        public AppViewModel()
        {
            Title = "首页";
        }

        public override void Start()
        {
            Navigate<AbilityEditViewModel>();
        }
    }
}