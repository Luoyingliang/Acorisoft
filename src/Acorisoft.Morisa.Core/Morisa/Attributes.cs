using System;

namespace Acorisoft.Morisa
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ExplicitSerializerAttribute : Attribute
    {
        
    }
    
    [AttributeUsage(AttributeTargets.Class)]
    public class ExplicitDeserializerAttribute : Attribute
    {
        
    }
}