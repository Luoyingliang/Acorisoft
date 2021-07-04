using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Acorisoft.Morisa.Documents;
using Acorisoft.Morisa.PoW.Items;

namespace Acorisoft.Morisa.PoW.Converters
{
    public class AbilityRarityConverter  : ObjectDataProvider
    {

        public List<object> GetFriendlyName(Type type)
        {
            var shortListOfApplicationGestures = Rarities.All.Select(x => (object)Rarities.GetName(x)).ToList();
            return shortListOfApplicationGestures;
        }
    }
}
