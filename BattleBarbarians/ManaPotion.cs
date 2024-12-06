using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBarbarians
{
    internal class ManaPotion : Item
    {
        public int ManaAmount { get; set; }
        public ManaPotion(string name, string description)
            : base(name, description)
        {
            
        }

        public override void Use(Character target)
        {
            Console.WriteLine($"{target.Name} uses {Name}, restoring 30 Mana.");
            target.Mana += 30;
            if (target.Mana > target.MaxMana)
            {
                target.Mana = target.MaxMana;
            }
        }
    }
}
