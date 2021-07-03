using System;
using MediatR;

namespace Acorisoft.Morisa.Core
{
    public interface IDocumentSubEngine : IDisposable, INotificationHandler<ComposeOpenRequest> , INotificationHandler<ComposeCloseRequest>
    {
        
    }
}