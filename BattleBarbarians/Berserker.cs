using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBarbarians
{
    internal class Berserker : Character<Berserker>
    {
        private int baseAttackPower; // Unique to berserker class
        public Berserker(string name)
            : base(
                  name, // Player name
                  150,  // HP
                  40,   // Mana
                  40,   // Attack power
                  new List<Attack> {
                      new Attack("Furious Slash", 40, 15, "A brutal slash with a fury boost."),
                      new Attack("Berserk Charge", 50, 20, "Charge toward your target and strike a vital slash."),
                      new Attack("Raging whirlind", 85, 35, "Consumed by fury, unleash a furious barrage of attacks.")
                  }
            )
        {
            baseAttackPower = AttackPower;
        }

        public override void PerformAttack(Character<Berserker> target)
        {
            // To add some identify to our berserker, he get's a 30% damage bonus when below half hp.
            if (Health < MaxHealth / 2)
            {
                AttackPower = (int)(AttackPower * 1.3);
                Console.WriteLine($"{Name} is in a berserk state, increasing damage by 30%!");
            }
            else
            {
                AttackPower = baseAttackPower;
                Console.WriteLine($"{Name} is no longer in berserk state");
            }

            // TODO - 
            Console.WriteLine($"{Name} attacks {target.Name} with Furious Slash, causing {AttackPower} damage.");
            target.Health -= AttackPower;
        }
    }
}

