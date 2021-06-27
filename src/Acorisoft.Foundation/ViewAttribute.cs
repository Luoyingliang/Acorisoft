using System;

namespace Acorisoft
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ViewModelParingAttribute : Attribute
    {
        public ViewModelParingAttribute(Type vmType, Type vType)
        {

        }
    }
}