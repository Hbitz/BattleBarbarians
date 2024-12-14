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
                  200,  // HP
                  50,   // Mana
                  1,   // Attack power modifier
                  new List<Attack> { 
                      new Attack("Slash", 30, 0, "A powerful slashing attack.") 
                  }
              )
        {
            
        }
        protected override void ApplySpecialMechanics()
        {
            // Implement special mechanics
        }


    }
}
