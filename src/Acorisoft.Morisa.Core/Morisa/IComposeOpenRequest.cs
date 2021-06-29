using System;
using MediatR;

namespace Acorisoft.Morisa
{
    public interface IComposeOpenRequest : INotification
    {
        /// <summary>
        /// 获取打开的创作集。
        /// </summary>
        IMorisaCompose Compose { get; }
    }

    public class ComposeOpenRequest : IComposeOpenRequest
    {
        public ComposeOpenRequest(IMorisaCompose compose)
        {
            Compose = compose ?? throw new ArgumentNullException(nameof(compose));
        }
        
        /// <summary>
        /// 获取打开的创作集。
        /// </summary>
        public IMorisaCompose Compose { get; }
    }
}