using System;

namespace Acorisoft.Morisa.Documents
{
    public interface IDocumentIndex
    {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
    }
}