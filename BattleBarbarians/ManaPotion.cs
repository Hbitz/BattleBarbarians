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
        public ManaPotion(string name, string description, int manaAmount)
            : base(name, description)
        {
            ManaAmount = manaAmount;
            
        }

        public override void Use(Character target)
        {
            Console.WriteLine($"{target.Name} uses {Name}, restoring {ManaAmount} Mana.");
            target.Mana += ManaAmount;
            if (target.Mana > target.MaxMana)
            {
                target.Mana = target.MaxMana;
            }
        }
    }
}
