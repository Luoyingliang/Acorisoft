using System;

namespace Acorisoft.Morisa.PoW.Items.Abilities
{
    public class Storyboard
    {
        public sealed override string ToString()
        {
            return Name;
        }

        public string Name { get; set; }
        public Guid Id { get; set; }
    }
}