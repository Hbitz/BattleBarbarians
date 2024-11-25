using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BattleBarbarians
{
    internal class Warrior : Character<Warrior>
    {
        public Warrior(string name) 
            : base(
                  name, // Player name
                  120,  // HP
                  50,   // Mana
                  30,   // Attack power
                  new List<Attack> { 
                      new Attack("Slash", 25, 10, "A powerful slashing attack.") 
                  }
              )
        {
            
        }

        public override void PerformAttack(Character<Warrior> target)
        {
            Console.WriteLine($"{Name} attacks {target.Name} with Slash, causing {AttackPower} damage.");
            target.Health -= AttackPower;
        }
    }
}
