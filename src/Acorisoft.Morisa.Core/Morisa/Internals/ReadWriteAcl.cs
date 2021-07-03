using System.Collections;
using System.Collections.Generic;
using LiteDB;

namespace Acorisoft.Morisa.Internals
{
    internal class ReadWriteAcl
    {
        [BsonId]
        public string Collection { get; set; }
        
        [BsonField("o")]
        public string OwnerType { get; set; }
        
        [BsonField("w")]
        public List<string> WhiteList { get; set; }
        
        [BsonField("f")]
        public ResourcePermission Fallback { get; set; }
    }
    
    /*
     * let collection = this.GetCollection<T>(string); // get owner type
     * let 
     */
}