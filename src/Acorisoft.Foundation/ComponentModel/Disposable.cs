using System;

// ReSharper disable ConvertToAutoProperty
namespace Acorisoft.ComponentModel
{
    public abstract class Disposable : IDisposable
    {
        private bool _disposedValue;

        protected virtual void OnDisposeManagedCore()
        {
            
        }

        protected virtual void OnDisposeUnmanagedCore()
        {
            
        }
        
        protected void Dispose(bool disposing)
        {
            if (_disposedValue)
            {
                return;
            }
            
            if (disposing)
            {
                OnDisposeManagedCore();
            }

            OnDisposeUnmanagedCore();
            _disposedValue = true;
        }

        ~Disposable()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public bool IsDisposed => _disposedValue;
    }
}