using BattleBarbarians;
using Figgle;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

internal class BattleManager
{
        private HallOfFameManager _hallOfFameManager = new HallOfFameManager();

    // temp - Refactor all ASCII art to own file?
    private readonly string berserkerArt = @"
         O
        /|\
        / \
      (Player)";

    private readonly string ratArt = @"
 ()-().----.          .
  \""/` ___ ;_____.'
   ` ^^   ^^
";

    private readonly string trollArt = @"
   O
  /|\
   |
  / \
 /   \
(Troll)";

    private readonly string goblinArt = @" 
   O
  -|-
  / \
(Goblin)";

    private readonly string twoHeadedOgreArt = @"
             /\
   (_)(_)   /< \>
   /∞\/∞\  <\ >/>
  _\ö/\ö/_   ||
 /  O---O \_/|3
| /|:::::|\_/||
| ||~~~~~|   ()
mm |\___/|
   ||   ||
  /_|   |_\";
    private int level = 1;
    private bool isFinalLevel = false;

    public void StartBattle(Character player)
    {
        bool running = true;
        // Keep running game loop until we reach lvl 20 or player dies
        while (running)
        {
            // bool is used to run specific code on last level
            if (level == 20)
            {
                isFinalLevel = true; 
            }
            Character enemy = GenerateRandomEnemy();
            // If we are on the final level, switch enemy to the final boss
            if (isFinalLevel == true)
            {
                enemy = new TwoHeadedOgre();
            }

            while (player.IsAlive() && enemy.IsAlive())
            {
                PrintBattleArtAndInfo(player, enemy);
                player.ShowInventory();
                Console.WriteLine("\n");

                player.PerformAttack(enemy);

                if (enemy.IsAlive())
                {
                    enemy.PerformAttack(player);
                }
                player.RecoverMana(5);
                enemy.RecoverMana(5);
            }

            // Restore HP/Mana on victroy
            if (player.IsAlive())
            {
                Console.WriteLine($"{player.Name} wins!");
                if (!isFinalLevel)
                {
                    level++; // Increase level
                    // Gives player a 50% chance for a reward
                    GiveReward(player);
                    player.Health = player.MaxHealth;  
                    player.Mana = player.MaxMana;
                }
                else
                {
                    Console.WriteLine("Wow, you have defeated the final boss!");
                    Console.WriteLine("This is a tremendous achievement, one of a kind that will go down in history");

                    Console.Write("Please enter your name: ");
                    string playerName = Console.ReadLine();

                    var newEntry = new HallOfFameEntry
                    {
                        Name = playerName,
                        CharacterType = player.Name, // Character type of player
                        Health = player.Health,
                        MaxHealth = player.MaxHealth,
                        Mana = player.Mana,
                        MaxMana = player.MaxMana,
                        AttackPower = player.AttackPower,
                    };

                    _hallOfFameManager.WriteEntry(newEntry);
                    Console.WriteLine("\n");
                    running = false;
                }

            }
            else
            {
                Console.WriteLine($"{enemy.Name} vinner!");
                running = false;
            }
        }
    }



    public void PrintBattleArtAndInfo(Character player, Character enemy)
    {
        string text = "BattleBarbarians!";
        string banner = FiggleFonts.Standard.Render(text);
        string levelText = "Level " + level.ToString();
        string levelBanner = FiggleFonts.Avatar.Render(levelText);
        Console.WriteLine(banner);
        Console.WriteLine(levelBanner);

        // Get the art from our AsciiArtProvider class
        string playerArt = AsciiArtProvider.GetAsciiArt(player.GetType().Name);
        string enemyArt = AsciiArtProvider.GetAsciiArt(enemy.GetType().Name);

        // Split up ASCII-strings in rows in order to print out the right positioning for enemies
        string[] playerLines = playerArt.Split('\n');
        string[] enemyLines = enemyArt.Split('\n');

        foreach (string line in enemyLines)
        {
            Console.WriteLine(new string(' ', 50) + line);
        }
        Console.WriteLine($"{' ',50}{enemy.Name} HP: {enemy.Health}/{enemy.MaxHealth}, MP: {enemy.Mana}/{enemy.MaxMana}");

        foreach (string line in playerLines)
        {
            Console.WriteLine(line);
        }

        Console.WriteLine($"{player.Name} HP: {player.Health}/{player.MaxHealth}, MP: {player.Mana}/{player.MaxMana}");
        Console.WriteLine();
    }

    private Character GenerateRandomEnemy()
    {
        Random rand = new Random();
        int enemyType = rand.Next(1, 4);  
        switch (enemyType)
        {
            case 1:
                return new Rat();
            case 2:
                return new Goblin();
            case 3:
                return new Troll();
            default:
                return new Rat();
        }
    }

    private void GiveReward(Character player)
    {
        Random rand = new Random();
        int rewardChance = rand.Next(1, 101);

        if (rewardChance <= 50)
        {
            Console.WriteLine("You got a reward!");

            var rewardChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose your reward:")
                    .HighlightStyle(new Style(Color.White, Color.Black))
                    .AddChoices(
                    "[green]1. Permanent HP[/]", 
                    "[blue]2. Permanent Mana[/]",
                    "[red]3. Permanent Attack Power[/]",
                    "[green]4. Hp potion[/]",
                    "[blue]5. Mana potion[/]"
                    //"[white]6. Escape Scroll - Flee from one battle[/]"
                    )
            );

            // Map the selected choice to a reward
            switch (rewardChoice)
            {
                case "[green]1. Permanent HP[/]":
                    player.MaxHealth += 20;
                    Console.WriteLine($"{player.Name} gets 20 extra HP!");
                    break;
                case "[blue]2. Permanent Mana[/]":
                    player.MaxMana += 10;
                    Console.WriteLine($"{player.Name} gets 10 extra Mana!");
                    break;
                case "[red]3. Permanent Attack Power[/]":
                    player.AttackPower += 0.1;
                    Console.WriteLine($"{player.Name} gets 10% extra Attack Power!");
                    break;

                case "[green]4. Hp potion[/]":
                    player.Inventory.AddItem(new HpPotion("Health Potion", "Restores 50hp"));
                    break;
                case "[blue]5. Mana potion[/]":
                    player.Inventory.AddItem(new ManaPotion("Mana Potion", "Restores 35mp"));
                    break;
                default:
                    Console.WriteLine("Invalid choice."); // This case will rarely be hit due to the controlled selection
                    break;
            }
        }
        else
        {
            Console.WriteLine("No reward this time.");
        }
    }
}
