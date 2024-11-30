using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBarbarians
{
    internal class Berserker : Character
    {
        bool wasInBerserkState = false;
        public Berserker(string name)
            : base(
                  name, // Player name
                  150,  // HP
                  40,   // Mana
                  1,   // Attack Power modifier
                  new List<Attack> {
                      new Attack("Furious Slash", 20, 5, "A brutal slash with a fury boost."),
                      new Attack("Berserk Charge", 50, 20, "Charge toward your target and strike a vital slash."),
                      new Attack("Raging whirlind", 85, 35, "Consumed by fury, unleash a furious barrage of attacks.")
                  }
            )
        {

        }

        public override void PerformAttack(Character target)
        {
            // To add some identity to our berserker, he gets a 30% damage bonus when below half HP
            if (Health < MaxHealth / 2)
            {
                wasInBerserkState = true; // Start tracking our berserk state
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
            Mana -= selectedAttack.ManaCost;

            if (wasInBerserkState)
            {
                Console.WriteLine("1. " + AttackPower);
                // Total dmg is attack power modifier * attack's damage * 30% berserker rage bonus
                int totalDmg = Convert.ToInt32(CalculateDamage(AttackPower, selectedAttack) * 1.3);
                target.TakeDamage(totalDmg);
            }
            else
            {
                Console.WriteLine("2, " + AttackPower);
                int totalDmg = Convert.ToInt32(CalculateDamage(AttackPower, selectedAttack));
                target.TakeDamage(totalDmg);
            }
        }

    }
}

