using System;

namespace Acorisoft.Morisa.Documents
{
    public class AbilityStorySet
    {

        public sealed override string ToString()
        {
            return Name;
        }

        public string Name { get; set; }
        public Guid Id { get; set; }
    }
}