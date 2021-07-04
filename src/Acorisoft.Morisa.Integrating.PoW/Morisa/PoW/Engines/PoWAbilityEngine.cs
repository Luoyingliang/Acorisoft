using Acorisoft.Morisa.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acorisoft.Morisa.PoW.Engines
{
    public class PoWAbilityEngine : DocumentSubEngine
    {
        public PoWAbilityEngine(IDocumentEngineAwaiter awaiter) : base(awaiter) 
        {
        }
    }
}
