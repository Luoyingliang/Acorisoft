using System;
using System.Reactive.Subjects;
using Acorisoft.Platform.Windows.ViewModels;
using Splat;

namespace Acorisoft.Platform.Windows.Services
{
    public class ExtraViewSupportService : IExtraViewSupportService
    {
        private readonly Subject<ExtraViewParam> _param;
        private readonly IFullLogger _logger;

        public ExtraViewSupportService()
        {
            _param = new Subject<ExtraViewParam>();
            _logger = Locator.Current.GetService<ILogManager>()?.GetLogger(typeof(NavigateSupportService));
        }
        
        public void ActivateExtraView(ExtraViewParam param)
        {
            if (!_param.HasObservers)
            {
                _logger.Info($"导航服务没有观察者，导航结束");
                return;
            }
            
            _param.OnNext(param);
        }

        public IObservable<ExtraViewParam> ExtraViewParamStream => _param;
    }
}