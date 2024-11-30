using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;

namespace BattleBarbarians
{
    internal class TwoHeadedOgre : Character
    {
        private Random random;

        public TwoHeadedOgre()
            : base(
                  "Two-Headed Ogre", // Boss name
                  500,               // HP
                  100,               // Mana
                  1,                // Attack Power modifier
                  new List<Attack>
                  {
                      new Attack("Crushing Blow", 10, 5, "A devastating attack from the brute head."),
                      new Attack("Fireball", 80, 60, "A fiery magical blast from the cunning head."),
                      new Attack("Dual Strike", 55, 40, "Both heads attack simultaneously, causing massive damage.")
                  })
        {
            random = new Random();
        }

        public override void PerformAttack(Character target)
        {
            int selectedAttackIndex = random.Next(Attacks.Count);
            Attack selectedAttack = Attacks[selectedAttackIndex];

            if (Mana >= selectedAttack.ManaCost)
            {
                Console.WriteLine($"{Name} uses {selectedAttack.Name}, causing {selectedAttack.Damage} damage to {target.Name}!");
                target.TakeDamage(selectedAttack.Damage);
                Mana -= selectedAttack.ManaCost;
            }
            else
            {
                Console.WriteLine($"{Name} tries to use {selectedAttack.Name}, but doesn't have enough mana! Using Crushing Blow instead.");
                Attack fallbackAttack = Attacks[0]; 
                target.TakeDamage(fallbackAttack.Damage);
                Mana -= fallbackAttack.ManaCost;
            }

            RegenerateHealth();
        }

        // Ogres are tough creatures with extreme vitality that quickly regenerates their wounds
        private void RegenerateHealth()
        {
            const int regenerationAmount = 10;
            Health += regenerationAmount;
            if (Health > MaxHealth) Health = MaxHealth;
            Console.WriteLine($"{Name} regenerates {regenerationAmount} HP. Current HP: {Health}/{MaxHealth}.");
        }
    }
}

