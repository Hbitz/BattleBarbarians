using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBarbarians
{
    internal class HpPotion : Item
    {
        public int HealAmount { get; set; }
        public HpPotion(string name, string description, int healAmount)
           : base(name, description)
        {
            HealAmount = healAmount;
            
        }

        public override void Use(Character target)
        {
            Console.WriteLine($"{target.Name} uses {Name}, restoring {HealAmount}.");
            target.Health += HealAmount;
            if (target.Health > target.MaxHealth)
            {
                target.Health = target.MaxHealth;
            }
        }
    }
}
