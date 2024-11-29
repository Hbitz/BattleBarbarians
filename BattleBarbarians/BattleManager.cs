using BattleBarbarians;
using Figgle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

internal class BattleManager
{
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

    public void StartBattle(Character player)
    {
        
        Character enemy = GenerateRandomEnemy();

        while (player.IsAlive() && enemy.IsAlive())
        {
            PrintBattleArtAndInfo(player, enemy);

            player.PerformAttack(enemy);

            if (enemy.IsAlive())
            {
                enemy.PerformAttack(player);
            }
        }

        // Restore HP/Mana on victroy
        if (player.IsAlive())
        {
            Console.WriteLine($"{player.Name} vinner!");
            player.Health = player.MaxHealth;  

            // Gives player a 50% chance for a reward
            GiveReward(player);
        }
        else
        {
            Console.WriteLine($"{enemy.Name} vinner!");
        }        
    }

    public void PrintBattleArtAndInfo(Character player, Character enemy)
    {
        // Clear the console every turn
        Console.Clear();
        // A banner with text-art
        string text = "BattleBarbarians!";
        string banner = FiggleFonts.Standard.Render(text);
        Console.WriteLine(banner);


        // Set enemyArt to our current enemy
        string enemyArt = ratArt;
        if (enemy.Name == "Goblin")
        {
            enemyArt = goblinArt;
        }
        else if (enemy.Name == "Troll")
        {
            enemyArt = trollArt;
        }


        // Dela upp ASCII-konststrängarna i rader, för att skriva ut rätt positionering för fienderna
        string[] berserkerLines = berserkerArt.Split('\n');
        string[] enemyLines = enemyArt.Split('\n');

        foreach (string line in enemyLines)
        {
            Console.WriteLine(new string(' ', 50) + line);
        }
        Console.WriteLine($"{' ',50}{enemy.Name} HP: {enemy.Health}/{enemy.MaxHealth}, MP: {enemy.Mana}/{enemy.MaxMana}");

        Console.WriteLine(berserkerArt);
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
            Console.WriteLine("Du får en belöning! Välj en:");
            Console.WriteLine("1. Permanent HP");
            Console.WriteLine("2. Permanent Mana");
            Console.WriteLine("3. Permanent Attack Power");

            //int rewardChoice = int.Parse(Console.ReadLine()); 
            if (!int.TryParse(Console.ReadLine(), out int rewardChoice) || rewardChoice < 1 || rewardChoice > 3)
            {
                Console.WriteLine("Ogiltigt val. Försök igen.");
                return;
            }

            switch (rewardChoice)
            {
                case 1:
                    player.MaxHealth += 50;
                    Console.WriteLine($"{player.Name} får 50 extra HP!");
                    break;
                case 2:
                    player.MaxMana += 20;
                    Console.WriteLine($"{player.Name} får 20 extra Mana!");
                    break;
                case 3:
                    player.AttackPower += 10;
                    Console.WriteLine($"{player.Name} får 10 extra Attack Power!");
                    break;
                default:
                    Console.WriteLine("Ogiltigt val.");
                    break;
            }
        }
        else
        {
            Console.WriteLine("Tyvärr, ingen belöning denna gång.");
        }
    }
}
