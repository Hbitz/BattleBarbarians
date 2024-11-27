using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;

namespace BattleBarbarians
{
    internal class Rat : Character<Rat>
    {
        // Easy and predictable enemy.
        public Rat()
            : base(
                  "Giant rat", // Fiendens namn
                  130,   // HP
                  0,    // No mana
                  30,   // Attack power
                  new List<Attack>
                  {
                      new Attack("Bite", 15, 0, "A quick bite with sharp teeth.")
                  }
            )
        {
        }

        public override void PerformAttack(Character<Rat> target)
        {
            Console.WriteLine($"{Name} attacks {target.Name} with Bite, causing {AttackPower} damage.");
            target.Health -= AttackPower;
        }
    }
}

