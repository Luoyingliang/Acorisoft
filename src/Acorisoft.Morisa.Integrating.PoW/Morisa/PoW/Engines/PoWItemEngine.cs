using Acorisoft.Morisa.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acorisoft.Morisa.PoW.Engines
{
    public class PoWItemEngine : DocumentSubEngine
    {
        public PoWItemEngine(IDocumentEngineAwaiter awaiter) : base(awaiter)
        {
        }
    }
}
