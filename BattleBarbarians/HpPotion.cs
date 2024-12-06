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
        public HpPotion(string name, string description)
           : base(name, description)
        {
            
        }

        public override void Use(Character target)
        {
            Console.WriteLine($"{target.Name} uses {Name}, restoring 50 HP.");
            target.Health += 50;
            if (target.Health > target.MaxHealth)
            {
                target.Health = target.MaxHealth;
            }
        }
    }
}
