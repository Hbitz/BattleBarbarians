using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BattleBarbarians
{
    internal class Dwarf : Character
    {
        // The third class, Dwarf, is supposed to be a hard-mode character with overall nerfed attack power combined with a randomness to all of his attacks
        private Random random; 

        public Dwarf(string name)
            : base(
                  name, // Player name
                  100,  // HP
                  45,   // Mana
                  1,   // Attack Power modifier
                  new List<Attack> {
                      new Attack("Lucky Shot", 20, 5, "A quick shot with a random outcome."),
                      new Attack("Double or Nothing", 50, 20, "A risky double attack with a chance to miss.")
                  }
            )
        {
            random = new Random();
        }
        protected override void ApplySpecialMechanics()
        {
            // Implement special mechanics, if needed
        }

        // Dwarf-specific change: We use ResolveAttack to run the attack through dwarf's randomness and extra attack logic
        public override void HandleAttack(Attack attack, Character target)
        {
            Console.WriteLine($"{Name} uses {attack.Name}!");
            Mana -= attack.ManaCost;

            double damage = ResolveAttack(attack.Name, target);
            target.TakeDamage((int)damage);
        }

        private double ResolveAttack(string attackName, Character target)
        {
            switch (attackName)
            {
                case "Lucky Shot":
                    return LuckyShot(target); // Dwarf-specifik logic
                case "Double or Nothing":
                    return DoubleOrNothing(target); // Dwarf-specifik logic
                default:
                    return CalculateDamage(AttackPower, Attacks.FirstOrDefault(a => a.Name == attackName)); // Standard attacklogic
            }
        }

        private double LuckyShot(Character target)
        {
            // Dwarf-specific logic for Lucky Shot
            Attack luckyShot = Attacks.FirstOrDefault(a => a.Name == "Lucky Shot");
            double dmg = 0;
            int chance = random.Next(1, 101); // Get a number between 1 and 100 to determine luck
            string attackStatus = "normal";
            string attackInfo = $"{Name} attacks {target.Name} with Lucky Shot, causing {dmg} damage!";

            if (chance <= 20) // 20% chance for a critical hit
            {
                attackStatus = "crit";
                dmg = CalculateDamage(AttackPower, luckyShot) * 2;
                attackInfo = $"{Name} lands a critical hit on {target.Name} with Lucky Shot, causing {dmg} damage!";
            }
            else if (chance <= 60) // 40% chance for a normal hit
            {
                dmg = CalculateDamage(AttackPower, luckyShot);
            }
            else // 40% chance for a low dmg attack
            {
                attackStatus = "low";
                dmg = CalculateDamage(AttackPower, luckyShot) * 0.5;
                attackInfo = $"{Name} attacks {target.Name} with Lucky Shot, but {Name} is too drunk and the shot only grazes his target causing {dmg} damage.";
            }

            Console.WriteLine(attackInfo);
            return dmg;
        }

        private double DoubleOrNothing(Character target)
        {
            // Dwarf-specific logic for Double or Nothing
            Attack attack = Attacks.FirstOrDefault(a => a.Name == "Double or Nothing");
            double dmg = 0;
            int chance = random.Next(1, 101);

            if (chance <= 50) // 50% chance to succeed
            {
                dmg = CalculateDamage(AttackPower, attack) * 2;
                Console.WriteLine($"{Name} hits {target.Name} with a double strike, dealing {dmg} damage!");
            }
            else
            {
                Console.WriteLine($"{Name} misses the Double or Nothing attack!");
            }

            return dmg;
        }
    }
}

