using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;

namespace BattleBarbarians
{
    internal class Goblin : Character
    {
        private Random random;

        public Goblin()
            : base(
                  "Goblin", // Enemy name
                  80,   // HP
                  30,   // Mana
                  1,   // Attack Power modifier
                  new List<Attack> {
                      new Attack("Sneaky Stab", 15, 5, "A quick and sneaky stab."),
                      new Attack("Goblin Smash", 30, 20, "A wild and reckless smash with high damage.")
                  }
            )
        {
            random = new Random();
        }

        public override void PerformAttack(Character target)
        {
            int attackIndex = random.Next(Attacks.Count);
            Attack chosenAttack = Attacks[attackIndex];

            // Handle attacks when insufficient mana
            if (chosenAttack.Name == "Goblin Smash" && Mana < chosenAttack.ManaCost)
            {
                Console.WriteLine($"{Name} doesn't have enough mana for {chosenAttack.Name} and defaults to Sneaky Stab!");
                // Fallback to default
                chosenAttack = Attacks[0];
                Console.WriteLine($"{Name} uses {chosenAttack.Name} on {target.Name}, dealing {chosenAttack.Damage} damage!");
                target.TakeDamage(chosenAttack.Damage);
            }

            else
            {
                Mana -= chosenAttack.ManaCost;
                Console.WriteLine($"{Name} uses {chosenAttack.Name} on {target.Name}, dealing {chosenAttack.Damage} damage!");
                target.TakeDamage(chosenAttack.Damage);
            }
        }
    }
}

