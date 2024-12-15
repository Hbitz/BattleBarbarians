using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBarbarians
{
    internal class HallOfFameEntry
    {
        public string Name { get; set; }  // Player name, user input
        public string CharacterType { get; set; }  
        public int Health { get; set; } 
        public int MaxHealth { get; set; }
        public int Mana { get; set; }   
        public int MaxMana { get; set; }
        public double AttackPower { get; set; } 
    }
}
