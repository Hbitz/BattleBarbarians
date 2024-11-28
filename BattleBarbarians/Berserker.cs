using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBarbarians
{
    internal class Berserker : Character
    {
        private int baseAttackPower; // Unique to berserker class
        bool wasInBerserkState = false;
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

        public override void PerformAttack(Character target)
        {
            // Reset our base attack power so we don't get a stacking 30% buff every turn
            AttackPower = baseAttackPower;

            // To add some identity to our berserker, he gets a 30% damage bonus when below half HP
            if (Health < MaxHealth / 2)
            {
                wasInBerserkState = true; // Start tracking our berserk state
                AttackPower = (int)(AttackPower * 1.3);
                Console.WriteLine($"{Name} is in a berserk state, increasing damage by 30%!");
            }
            else if (wasInBerserkState && Health > MaxHealth / 2)
            {
                wasInBerserkState = false;
                Console.WriteLine($"{Name} is no longer in berserk state.");
            }

            int attackChoice = ChooseAttack();

            Attack selectedAttack = Attacks[attackChoice];
            Console.WriteLine($"{Name} använder {selectedAttack.Name}!");
            target.TakeDamage(selectedAttack.Damage);
        }

    }
}

