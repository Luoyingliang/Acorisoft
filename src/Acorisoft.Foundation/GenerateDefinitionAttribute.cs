using System;

namespace Acorisoft
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class GenerateDefinitionAttribute : Attribute
    {
        public GenerateDefinitionAttribute(string xmlns)
        {
            
        }
    }
    
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class SkipGenerateDefinitionAttribute : Attribute
    {
        public SkipGenerateDefinitionAttribute(string clrns)
        {
            
        }
    }
}