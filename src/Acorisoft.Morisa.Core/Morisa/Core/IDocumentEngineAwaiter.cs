namespace Acorisoft.Morisa.Core
{
    public interface IDocumentEngineAwaiter
    {
        void WaitOne();
        void Release();
    }
}