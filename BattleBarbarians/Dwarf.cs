﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBarbarians
{
    internal class Dwarf : Character<Dwarf>
    {
        // The third class, Dwarf, is supposed to be a hard-mode character with overall nerfed attack power combined with a randomness to all of his attacks
        private Random random; 

        public Dwarf(string name)
            : base(
                  name, // Player name
                  100,  // HP
                  45,   // Mana
                  20,   // Attack power
                  new List<Attack> {
                      new Attack("Lucky Shot", 20, 5, "A quick shot with a random outcome."),
                      new Attack("Double or Nothing", 50, 20, "A risky double attack with a chance to miss.")
                  }
            )
        {
            random = new Random();
        }

        public override void PerformAttack(Character<Dwarf> target)
        {
            int chance = random.Next(1, 101); // Get a number between 1 and 100 for luck

            if (chance <= 20) // 20% chance for a critical hit
            {
                int criticalDamage = (int)(AttackPower * 2); // Double damage on critical hit
                Console.WriteLine($"{Name} lands a critical hit with Lucky Shot, causing {criticalDamage} damage!");
                target.Health -= criticalDamage;
            }
            else if (chance <= 60) // 40% chance for a normal hit
            {
                Console.WriteLine($"{Name} attacks {target.Name} with Lucky Shot, causing {AttackPower} damage.");
                target.Health -= AttackPower;
            }
            else // 40% chance for a low damage attack
            {
                int lowDamage = (int)(AttackPower * 0.5); // Half damage
                Console.WriteLine($"{Name} attacks {target.Name} with Lucky Shot, but {Name} is too drunk the shot only grazes his target and causes {lowDamage} damage.");
                target.Health -= lowDamage;
            }
        }

        // Additional ability: Double or Nothing with high risk and reward
        public void DoubleOrNothing(Character<Dwarf> target)
        {
            int chance = random.Next(1, 101); // Random number between 1 and 100

            if (chance <= 50) // 50% chance to succeed
            {
                Console.WriteLine($"{Name} hits {target.Name} with a double strike, dealing {AttackPower * 2} damage!");
                target.Health -= AttackPower * 2;
            }
            else
            {
                Console.WriteLine($"{Name} misses the Double or Nothing attack!");
            }
        }
    }
}
