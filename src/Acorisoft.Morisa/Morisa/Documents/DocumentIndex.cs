using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acorisoft.Morisa.Documents
{
    public abstract class DocumentIndex : IDocumentIndex
    {
        /// <summary>
        /// 
        /// </summary>
        [BsonId]
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
    }
}
