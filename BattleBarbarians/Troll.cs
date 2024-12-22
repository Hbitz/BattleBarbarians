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
        public string Description { get; set; }

        public Troll()
            : base(
                  "Troll",
                  200, // High health
                  25,  // Mana
                  1,  // Attack Power modifier
                  new List<Attack> {
                      new Attack("Troll Smash", 15, 0, "A devastating smash with the troll's club."),
                      new Attack("Boulder Throw", 25, 10, "Throws a massive boulder at the enemy."),
                      new Attack("Crushing Blow", 55, 20, "A slow but incredibly powerful blow.")
                  }
            )
        {
            random = new Random();
            Description = "The troll has high health and a few strong attack abilities.";
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
            Attack chosenAttack = Attacks[random.Next(Attacks.Count)];

            if (Mana >= chosenAttack.ManaCost)
            {
                Mana -= chosenAttack.ManaCost;
                Console.WriteLine($"{Name} uses {chosenAttack.Name} on {target.Name}, dealing {chosenAttack.Damage} damage!");
                target.TakeDamage(chosenAttack.Damage);
            }
            else
            {
                Attack baseAttack = Attacks[0];
                // Fallback to basic attack if not enough mana
                Console.WriteLine($"{Name} tries to use {chosenAttack.Name} but doesn't have enough mana!");
                Console.WriteLine($"{Name} instead swings its club at {target.Name}, dealing {baseAttack.Damage} damage.");
                target.TakeDamage(baseAttack.Damage);
            }
        }
    }
}

