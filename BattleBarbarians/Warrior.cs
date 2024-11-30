using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BattleBarbarians
{
    internal class Warrior : Character
    {
        public Warrior(string name) 
            : base(
                  name, // Player name
                  120,  // HP
                  50,   // Mana
                  1,   // Attack power modifier
                  new List<Attack> { 
                      new Attack("Slash", 25, 0, "A powerful slashing attack.") 
                  }
              )
        {
            
        }

        public override void PerformAttack(Character target)
        {
            int attackChoice = ChooseAttack();

            Attack selectedAttack = Attacks[attackChoice];
            Console.WriteLine($"{Name} attacks {target.Name} with {selectedAttack.Name}, causing {AttackPower} damage.");
            target.TakeDamage(selectedAttack.Damage);
        }
    }
}
