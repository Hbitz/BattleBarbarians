using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace BattleBarbarians
{
    internal class Berserker : Character
    {    
        private bool IsInBerserkState { get; set; } = false;
        public Berserker(string name)
            : base(
                  name, // Player name
                  150,  // HP
                  40,   // Mana
                  1,   // Attack Power modifier
                  new List<Attack> {
                      new Attack("Furious Slash", 20, 0, "A brutal slash with a fury boost."),
                      new Attack("Berserk Charge", 50, 20, "Charge toward your target and strike a vital slash."),
                      new Attack("Raging whirlind", 85, 35, "Consumed by fury, unleash a furious barrage of attacks.")
                  }
            )
        {

        }

        public void UseHpPotion(Item healthPotion)
        {
            Inventory.UseItem(healthPotion, this);
        }

        public void UseMpPotion(Item manaPotion)
        {
            Inventory.UseItem(manaPotion, this);
        }

        // Introduce special-mechanick and behaviors of Berserker-class
        protected override void ApplySpecialMechanics()
        {
            if (Health < MaxHealth / 2 && !IsInBerserkState) // When you first enter berserker state
            {
                Console.WriteLine($"{Name} enters a Berserk State, gaining 30% bonus damage!");
                IsInBerserkState = true;

            }
            else if (Health >= MaxHealth / 2 && IsInBerserkState == true) // When you exit
            {
                Console.WriteLine($"{Name} calms down and exits Berserk State.");
                IsInBerserkState = false;
            }
            else if (IsInBerserkState == true) // When you are in berserker state
            {
                Console.WriteLine($"{Name} is in a Berserk State, gaining 30% bonus damage!");
            }
        }

        protected override int CalculateDamageNew(Attack attack)
        {
            int baseDamage = base.CalculateDamageNew(attack);
            return IsInBerserkState ? (int)(baseDamage * 1.3) : baseDamage;
        }

        // Old function, no longer used
        public override void PerformAttack(Character target)
        {
            Console.WriteLine("PerformAttack not implemeneted");
        }
    }
}

