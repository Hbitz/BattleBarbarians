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
        bool wasInBerserkState = false;
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

        public override void PerformAttack(Character target)
        {
            // Kontrollera om vi är i Berserk State
            if (Health < MaxHealth / 2)
            {
                wasInBerserkState = true;
                Console.WriteLine($"{Name} is in a berserk state, increasing damage by 30%!");
            }
            else if (wasInBerserkState && Health >= MaxHealth / 2)
            {
                wasInBerserkState = false;
                Console.WriteLine($"{Name} is no longer in a berserk state.");
            }

            // Below we'll create a meny of attacks and item usages using Spectre.Console selection.
            // We use a boolean to track if an action is performed. Unless they attempt to seelct an attack they don't have mana for, all actions will set actionPerformed to true

            bool actionPerformed = false; 
            while (!actionPerformed)
            {
                // Skapa val för attacker
                var attackOptions = new Dictionary<Attack, bool>();
                foreach (var attack in Attacks)
                {
                    bool isAvailable = Mana >= attack.ManaCost;
                    attackOptions[attack] = isAvailable;
                }

                // Menu for attacks and items
                var promptChoices = new Dictionary<string, Action>();

                foreach (var option in attackOptions)
                {
                    string description = option.Value
                        ? $"{option.Key.Name} - Damage: {CalculateDamage(AttackPower, option.Key)}, Mana Cost: {option.Key.ManaCost}"
                        : $"{option.Key.Name} - Damage: {CalculateDamage(AttackPower, option.Key)}, Mana Cost: {option.Key.ManaCost}" + Markup.Escape("[Not enough mana]");

                    // Action for when player selects an attack
                    promptChoices[description] = () =>
                    {
                        if (option.Value)
                        {
                            HandleAttack(option.Key, target);
                            actionPerformed = true;
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[red]You cannot use this attack right now![/]");
                        }
                    };
                }

                // Add items to menu
                foreach (var item in Inventory.GetAllItems())
                {
                    string description = $"{item.Key.Name} x{item.Value}";
                    promptChoices[description] = () =>
                    {
                        HandleItem(item.Key);
                        actionPerformed = true;
                    };
                }

                // Present the menu to player
                string selectedAction = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]What do you want to do?[/]")
                        .PageSize(10)
                        .AddChoices(promptChoices.Keys));

                // Do the 
                promptChoices[selectedAction]();

            }
        }

        private void HandleAttack(Attack attack, Character target)
        {
            Console.WriteLine($"{Name} uses {attack.Name}!");
            Mana -= attack.ManaCost;

            int totalDmg = Health < MaxHealth / 2 // We used to rely on bool wasInBerserkerState to see if we added the 30% dmg boost, but a simple ternary is viable too
                ? Convert.ToInt32(CalculateDamage(AttackPower, attack) * 1.3)
                : Convert.ToInt32(CalculateDamage(AttackPower, attack));

            target.TakeDamage(totalDmg);
        }

        private void HandleItem(Item item)
        {
            Console.WriteLine($"{Name} uses {item.Name}!");
            Inventory.UseItem(item, this);
        }
    }
}

