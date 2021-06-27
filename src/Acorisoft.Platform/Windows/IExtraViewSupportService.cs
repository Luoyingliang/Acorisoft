using System;
using Acorisoft.Platform.Windows.ViewModels;

namespace Acorisoft.Platform.Windows
{
    public struct ExtraViewParam
    {
        public IViewModel QuickView { get; set; }
        public IViewModel ToolView { get; set; }
        public IViewModel ExtraView { get; set; }
        public IViewModel ContextView { get; set; }
    }

    public interface IExtraViewSupportService
    {
        void ActivateExtraView(ExtraViewParam param);
        
        IObservable<ExtraViewParam> ExtraViewParamStream { get; }
    }
}