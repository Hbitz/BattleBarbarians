using System;
using System.Collections.Generic;
using System.Linq;
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

        public override void PerformAttack(Character target)
        {
            int attackChoice = ChooseAttack();
            Attack selectedAttack = Attacks[attackChoice];

            int totalDmg = (int)ResolveAttack(selectedAttack.Name, target);
            Mana -= selectedAttack.ManaCost;
            target.TakeDamage(totalDmg);

        }

        private double ResolveAttack(string attackName, Character target)
        {
            switch (attackName)
            {
                case "Lucky Shot":
                    return LuckyShot(target);
                case "Double or Nothing":
                    return DoubleOrNothing(target);
                default:
                    return LuckyShot(target); // Fallback för okända attacker
            }
        }


        public double LuckyShot(Character target)
        {
            Attack luckyShot = Attacks[0];
            double dmg = 0;
            int chance = random.Next(1, 101); // Get a number between 1 and 100 for luck
            string attackStatus = "normal";
            string attackInfo = "";


            if (chance <= 20) // 20% chance for a critical hit
            {
                attackStatus = "crit";
                dmg = (int)(CalculateDamage(AttackPower, luckyShot) * 2);
            }
            else if (chance <= 60) // 40% chance for a normal hit
            {
                dmg = (int)(CalculateDamage(AttackPower, luckyShot));
            }
            else // 40% chance for a low damage attack
            {
                attackStatus = "low";
                dmg = (int)(CalculateDamage(AttackPower, luckyShot) * 0.5); // Half damage
            }


            switch (attackStatus)
            {
                case "normal":
                    attackInfo = $"{Name} attacks {target.Name} with Lucky Shot, causing {dmg} damage!";
                    break;
                case "crit":
                    attackInfo = $"{Name} lands a critical hit with Lucky Shot, causing {dmg} damage!";
                    break;
                case "low":
                    attackInfo = $"{Name} attacks {target.Name} with Lucky Shot, but {Name} is too drunk and the shot only grazes his target causing {dmg} damage.";
                    break;
            }

            Console.WriteLine(attackInfo);
            return dmg;
        }

        // Additional ability: Double or Nothing with high risk and reward
        public double DoubleOrNothing(Character target)
        {
            Attack attack = Attacks[1];
            double dmg = 0;
            int chance = random.Next(1, 101); // Random number between 1 and 100

            if (chance <= 50) // 50% chance to succeed
            {
                dmg = (int)(CalculateDamage(AttackPower, attack) * 2);
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

