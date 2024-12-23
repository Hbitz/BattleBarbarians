using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;

namespace BattleBarbarians
{
    internal class Rat : Character
    {
        public string Description { get; set; }
        // Easy and predictable enemy.
        public Rat()
            : base(
                  "Giant rat", // Fiendens namn
                  130,   // HP
                  0,    // No mana
                  1,   // Attack Power modifier
                  new List<Attack>
                  {
                      new Attack("Bite", 15, 0, "A quick bite with sharp teeth.")
                  }
            )
        {
            Description = "The rat is an easy enemy with moderate health and weak attacks.";
        }
        public string GetDescription()
        {
            return Description;
        }
        protected override void ApplySpecialMechanics()
        {
            // Implement special mechanics
        }

        public override void PerformAttack(Character target)
        {
            Attack selectedAttack = Attacks[0];
            Console.WriteLine($"{Name} attacks {target.Name} with Bite, causing {selectedAttack.Damage} damage.");
            target.Health -= selectedAttack.Damage;
        }
    }
}

