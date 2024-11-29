using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;

namespace BattleBarbarians
{
    internal class GameController
    {
        private BattleManager battleManager;

        public GameController()
        {
            battleManager = new BattleManager();
        }

        public void StartGame()
        {
            Character selectedCharacter = CharacterSelection();

            if (selectedCharacter != null)
            {
                Console.WriteLine($"Du valde {selectedCharacter.Name}! Spelet börjar nu!");
                battleManager.StartBattle(selectedCharacter);
            }
            else
            {
                Console.WriteLine("Ogiltigt val, försök igen.");
            }
        }

        private Character CharacterSelection()
        {
            Console.WriteLine("Välj din karaktär:");
            Console.WriteLine("1. Dwarf");
            Console.WriteLine("2. Berserker");
            Console.WriteLine("3. Warrior");

            int choice = 0;

            while (choice < 1 || choice > 3)
            {
                Console.Write("Välj en karaktär (1-3): ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out choice) && choice >= 1 && choice <= 3)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Ogiltigt val, försök igen.");
                }
            }

            // Skapa rätt karaktär baserat på spelarens val
            return choice switch
            {
                1 => new Dwarf("Dwarf"), 
                2 => new Berserker("Berserker"),
                3 => new Warrior("Warrior"), 
                _ => null
            };
        }
    }
}
