using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acorisoft.Platform.Windows.ViewModels;
using Acorisoft.Studio.Views;

namespace Acorisoft.Studio.ViewModels
{
    [ViewModelParing(typeof(MockViewModel), typeof(MockView))]
    public class MockViewModel : PageViewModel
    {
    }
}
