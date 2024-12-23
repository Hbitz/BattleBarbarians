using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBarbarians
{
    internal class BestiaryEntry
    {
        public string Name { get; set; }
        public string Description { get; set; } // Currently set, but unused
        public List<Attack> Attacks { get; set; }
        public bool IsDiscovered { get; set; }

        public BestiaryEntry(string name, string description, List<Attack> attacks)
        {
            Name = name;
            Description = description;
            IsDiscovered = false;
            Attacks = attacks;
        }
    }
}
