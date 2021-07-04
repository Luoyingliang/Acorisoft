using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Acorisoft.ComponentModel;
using Acorisoft.Platform.Windows;
using Acorisoft.Platform.Windows.ViewModels;
using Splat;

namespace Acorisoft.Platform.Windows.Services
{
    internal class DialogSession : IDialogSession
    {
        internal DialogSession(IDialogViewModel vm)
        {
            ViewModel = vm ?? throw new ArgumentNullException(nameof(vm));
            Tcs = new TaskCompletionSource<IDialogSession>();
        }
        public bool IsCompleted { get; set; }
        public object Result { get; set; }
        public TaskCompletionSource<IDialogSession> Tcs { get; }
        public Task<IDialogSession> Task => Tcs.Task;
        public IDialogViewModel ViewModel { get; }

        public T GetResult<T>()
        {
            if (((IDialogQueryable)ViewModel).GetResult<T>() is T value)
            {
                return value;
            }
            return default(T);
        }
    }

    public class DialogSupportService  : Disposable,  IDialogSupportService
    {
        private readonly Subject<IDialogViewModel> _dialog;
        private readonly Subject<Unit> _opening;
        private readonly Subject<Unit> _closing;
        private readonly Subject<IDialogViewModel> _updating;
        private readonly Stack<DialogSession> _stack;

        public DialogSupportService()
        {
            _closing = new Subject<Unit>();
            _dialog = new Subject<IDialogViewModel>();
            _opening = new Subject<Unit>();
            _stack = new Stack<DialogSession>();
            _updating = new Subject<IDialogViewModel>();
        }
        
        protected override void OnDisposeManagedCore()
        {
            _opening.Dispose();
            _dialog.Dispose();
            _closing.Dispose();
            _updating.Dispose();
            _stack.Clear();
        }

        public Task<IDialogSession> OpenDialog(IDialogViewModel vm)
        {
            if (vm == null)
            {
                Debug.WriteLine("对话框为空");
                return Task.FromResult((IDialogSession)null);
            }

            var session = new DialogSession(vm);
            if (_stack.Count == 0)
            {
                _stack.Push(session);
                
                //
                // 推送视图模型
                _dialog.OnNext(vm);
                
                //
                // 打开对话框
                _opening.OnNext(Unit.Default);

                //
                //
                var queryable = (IDialogQueryable) vm;
                
                //
                // 设置
                queryable.LoadedOrResume(session.Tcs, session, PopScope);
            }
            else
            {
                
                //
                //
                var lastScope = _stack.Peek();
                
                //
                //
                var queryable = (IDialogQueryable) lastScope.ViewModel;
                
                //
                // 取消
                queryable.Suspend();

                //
                //
                queryable = (IDialogQueryable) vm;
                
                //
                //
                _stack.Push(session);
                
                //
                // 推送视图模型
                _dialog.OnNext(vm);
                
                //
                // 打开对话框
                _updating.OnNext(vm);
                
                //
                // 设置
                queryable.LoadedOrResume(session.Tcs, session, PopScope);
            }

            return session.Task;
        }

        private void PopScope()
        {
            _stack.Pop();


            if (_stack.Count <= 0)
            {
                _closing.OnNext(Unit.Default);
                return;
            }

            var lastScope = _stack.Peek();
                
            //
            //
            var queryable = (IDialogQueryable) lastScope.ViewModel;

            //
            // Resume
            queryable.LoadedOrResume(lastScope.Tcs, lastScope, PopScope);
            
            //
            // Updating
            _updating.OnNext(lastScope.ViewModel);
        }

        public IObservable<IDialogViewModel> DialogStream => _dialog;
        public IObservable<Unit> DialogOpeningStream => _opening;
        public IObservable<Unit> DialogClosingStream => _closing;
        public IObservable<IDialogViewModel> DialogUpdatingStream => _updating;
    }
}