using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Spectre.Console;
using Figgle;

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
            string text = "BattleBarbarians!";
            string banner = FiggleFonts.Standard.Render(text);

            Console.WriteLine(banner);
            ShowGameInfo();
            Character selectedCharacter = CharacterSelection();

            if (selectedCharacter != null)
            {
                Console.Clear();
                battleManager.StartBattle(selectedCharacter);
            }
            else
            {
                Console.WriteLine("Ogiltigt val, försök igen.");
            }
        }

        public void ShowGameInfo()
        {
            Console.WriteLine("Hello and welcome to the game BattleBarbarians!");
            Console.WriteLine("This game is an action-rpg with turn based combat.");
            Console.WriteLine("The goal of the game is to defeat enemies, grow stronger and attain freedom by beating the boss on level 20.");
            Console.WriteLine();
            Console.WriteLine("There are three playable characters:");
            Console.WriteLine("\tBerserker - A fierce commander with multiple attacks and offensive capabilities");
            Console.WriteLine("\tWarrior - A standard fighter with good balance between offense and defense");
            Console.WriteLine("\tDwarf - A curageous but weak fighter which relies on luck. This if meant for hardcore players.");

        }

        private Character CharacterSelection()
        {
            // Skapa en lista med karaktärer att välja mellan
            var characters = new List<Character>
            {
                new Berserker("Berserker"),
                new Warrior("Warrior"),
                new Dwarf("Dwarf")
            };

                // Använd Spectre.Console Selection för att låta spelaren välja en karaktär
                var selectedCharacter = AnsiConsole
                    .Prompt(
                        new SelectionPrompt<string>()
                            .Title("Välj en karaktär:")
                            .AddChoices("Berserker", "Warrior", "Dwarf")
                    );

            Character player = selectedCharacter switch
            {
                "Berserker" => new Berserker("Berserker"),
                "Warrior" => new Warrior("Warrior"),
                "Dwarf" => new Dwarf("Dwarf"),
                _ => throw new InvalidOperationException("Ogiltigt val")
            };

            return player;
        }
    }
}
