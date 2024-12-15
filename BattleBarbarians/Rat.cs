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

