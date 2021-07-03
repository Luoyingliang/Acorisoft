using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acorisoft.Morisa.Core;
using MediatR;

namespace Acorisoft.Morisa
{

    public class ComposeOpenRequest : INotification
    {
        internal ComposeOpenRequest(Compose compose) => InternalCompose = compose ?? throw new ArgumentNullException(nameof(compose));
        internal Compose InternalCompose { get; }
        public ICompose Compose => InternalCompose;
    }

    public class ComposeCloseRequest : INotification
    {

    }
}
