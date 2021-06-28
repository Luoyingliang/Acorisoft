using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acorisoft.Platform.Windows.ViewModels;

namespace Acorisoft.Platform.Windows
{
    public struct DialogParam
    {
        /// <summary>
        /// 获取或设置当前对话框的参数。
        /// </summary>
        public IEnumerable<IDialogViewModel> Dialogs { get; set; }
        
        /// <summary>
        /// 获取或设置对话框上下文参数。
        /// </summary>
        public IViewModel Context { get; set; }
    }

    public interface IDialogSession
    {
        /// <summary>
        /// 
        /// </summary>
        bool IsCompleted { get; }
        
        /// <summary>
        /// 获取内容
        /// </summary>
        object Result { get; }
    }
    
    /// <summary>
    /// 
    /// </summary>
    public interface IDialogSupportService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IDialogSession> OpenDialog(DialogParam param);
        
        /// <summary>
        /// 获取对话框视图模型。
        /// </summary>
        IObservable<IDialogViewModel> DialogStream { get; }
    }
}