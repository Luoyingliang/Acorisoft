using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Acorisoft.Platform.Windows.Converters
{
    public class EnumConverter  : ObjectDataProvider
    {
        public object GetEnumValues(Enum enumObj)
        {
            var attribute = enumObj.GetType().GetRuntimeField(enumObj.ToString()).
                GetCustomAttributes(typeof(DescriptionAttribute), false).
                SingleOrDefault() as DescriptionAttribute;
            return attribute == null ? enumObj.ToString() : attribute.Description;
        }

        public List<object> GetEnumValuesFriendlyName(Type type)
        {
            var shortListOfApplicationGestures = Enum.GetValues(type).OfType<Enum>().Select(GetEnumValues).ToList();
            return
                shortListOfApplicationGestures;
        }
    }
}
