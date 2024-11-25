using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBarbarians
{
    internal class Attack
    {
        public string Name { get; set; }
        public int Damage { get; set; }
        public int ManaCost { get; set; }
        public string Description { get; set; }

        public Attack(string name, int damage, int manaCost, string description)
        {
            Name = name;
            Damage = damage;
            ManaCost = manaCost;
            Description = description;
        }
    }
}
