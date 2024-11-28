using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;

namespace BattleBarbarians
{
    internal class Troll : Character
    {
        private Random random;

        public Troll()
            : base(
                  "Troll",
                  200, // High health
                  30,  // Mana
                  50,  // Strong base attack power
                  new List<Attack> {
                      new Attack("Troll Smash", 20, 5, "A devastating smash with the troll's club."),
                      new Attack("Boulder Throw", 30, 10, "Throws a massive boulder at the enemy."),
                      new Attack("Crushing Blow", 60, 20, "A slow but incredibly powerful blow.")
                  }
            )
        {
            random = new Random();
        }

        public override void PerformAttack(Character target)
        {
            Attack chosenAttack = Attacks[random.Next(Attacks.Count)];

            if (Mana >= chosenAttack.ManaCost)
            {
                Mana -= chosenAttack.ManaCost;
                Console.WriteLine($"{Name} uses {chosenAttack.Name} on {target.Name}, dealing {chosenAttack.Damage} damage!");
                target.TakeDamage(chosenAttack.Damage);
            }
            else
            {
                // Fallback to basic attack if not enough mana
                Console.WriteLine($"{Name} tries to use {chosenAttack.Name} but doesn't have enough mana!");
                Console.WriteLine($"{Name} instead swings its club at {target.Name}, dealing {AttackPower} damage.");
                target.TakeDamage(AttackPower);
            }
        }
    }
}

