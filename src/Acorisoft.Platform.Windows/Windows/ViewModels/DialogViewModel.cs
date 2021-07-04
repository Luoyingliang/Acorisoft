using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Acorisoft.Platform.Windows.Services;
#nullable enable
namespace Acorisoft.Platform.Windows.ViewModels
{
    internal interface IDialogQueryable
    {
        T GetResult<T>();
        void LoadedOrResume(TaskCompletionSource<IDialogSession> tcs, DialogSession session, Action action);
        void Suspend();
    }

    public class DialogViewModel : ViewModel, IDialogViewModel, IDialogQueryable
    {
        internal class Command : ICommand
        {
            private readonly Action _handler;

            internal Command(Action handler) => _handler = handler ?? throw new ArgumentNullException(nameof(handler));

            public bool CanExecute(object? parameter) => CommandCanExecute;

            public void Execute(object? parameter)
            {
                _handler();
            }

            public event EventHandler? CanExecuteChanged
            {
                add => CommandManager.RequerySuggested += value;
                remove => CommandManager.RequerySuggested -= value;
            }
            
            internal bool CommandCanExecute { get; set; }
        }

        private TaskCompletionSource<IDialogSession> _tcs;
        private DialogSession _session;
        private Action _callback;
        private readonly Command _finish;
        private readonly Command _cancel;

        protected DialogViewModel()
        {
            _finish = new Command(OnFinish);
            _cancel = new Command(OnCancel);
            _cancel.CommandCanExecute = true;
        }

        private void OnFinish()
        {
            if (_tcs == null || _session == null || _callback == null)
            {
                return;
            }

            _callback();
            _session.Result = this;
            _session.IsCompleted = true;
            _tcs.SetResult(_session);
        }

        private void OnCancel()
        {
            if (_tcs == null || _session == null || _callback == null)
            {
                return;
            }

            _callback();
            _session.Result = this;
            _session.IsCompleted = false;
            _tcs.SetResult(_session);
        }

        protected virtual object GetResult()
        {
            return this;
        }

        protected virtual bool CanFinish()
        {
            return false;
        }

        protected void RaiseUpdate()
        {
            _cancel.CommandCanExecute = true;
            _finish.CommandCanExecute = CanFinish();
        }

        T IDialogQueryable.GetResult<T>() => (T)GetResult();

        void IDialogQueryable.LoadedOrResume(TaskCompletionSource<IDialogSession> tcs, DialogSession session ,Action callback)
        {
            _tcs = tcs;
            _session = session;
            _callback = callback;
        }
        
        void IDialogQueryable.Suspend()
        {
            _tcs = null;
            _session = null;
            _callback = null;
        }
        public ICommand Finish => _finish;
        public ICommand Cancel => _cancel;
    }
}