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
            // To add some identity to our berserker, he gets a 30% damage bonus when below half HP
            if (Health < MaxHealth / 2)
            {
                wasInBerserkState = true; // Start tracking our berserk state
                Console.WriteLine($"{Name} is in a berserk state, increasing damage by 30%!");
            }
            else if (wasInBerserkState && Health > MaxHealth / 2)
            {
                wasInBerserkState = false;
                Console.WriteLine($"{Name} is no longer in berserk state.");
            }



            var actionOptions = new Dictionary<string, bool>();

            foreach (var attack in Attacks)
            {
                string actionText = $"Attack: {attack.Name} - Damage: {CalculateDamage(AttackPower, attack)}, Mana Cost: {attack.ManaCost})";
                // This validation allows us to display options for all attacks, but only accept the input where we have enough mana.
                bool isAvailable = Mana >= attack.ManaCost;
                if (!isAvailable)
                {
                    actionText += Markup.Escape(" [Not enough mana]");
                }
                actionOptions[actionText] = isAvailable;
            }

            foreach (var item in Inventory.GetAllItems())
            {
                string actionText = $"Item: {item.Key.Name} x{item.Value}";
                actionOptions[actionText] = true; // Alla föremål är tillgängliga
            }

            // Konvertera alternativen till en lista för SelectionPrompt
            var promptChoices = actionOptions.Keys.ToList();

            string selectedAction = "";
            do
            {
                selectedAction = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]What do you want to do?[/]")
                        .PageSize(10)
                        .AddChoices(promptChoices));

                // As even disabled options are shown, control that the selectedAction is a enabled option
                if (!actionOptions[selectedAction])
                {
                    AnsiConsole.MarkupLine("[red]You cannot select this option![/]");
                }

            } while (!actionOptions[selectedAction]); // Repetera tills användaren väljer ett giltigt val

            
            if (selectedAction.StartsWith("Attack:"))
            {
                HandleAttack(selectedAction, target);
            }
            else if (selectedAction.StartsWith("Item:"))
            {
                HandleItem(selectedAction);
            }
        }

        private void HandleAttack(string action, Character target)
        {
            // Todo - Are there any better alternatives to getting the attack?
            string attackName = action.Substring(8).Split('-')[0].Trim();
            var attack = Attacks.First(a => a.Name == attackName);
            
            if (Mana < attack.ManaCost)
            {
                Console.WriteLine("[red]You don't have enough mana for this attack.[/]");
                return;
            }

            Console.WriteLine($"{Name} uses {attack.Name}!");
            Mana -= attack.ManaCost;

            int totalDmg = Health < MaxHealth / 2 // We used to rely on bool wasInBerserkerState to see if we added the 30% dmg boost, but a simple ternary is viable too
                ? Convert.ToInt32(CalculateDamage(AttackPower, attack) * 1.3)
                : Convert.ToInt32(CalculateDamage(AttackPower, attack));

            target.TakeDamage(totalDmg);
        }

        private void HandleItem(string action)
        {
            // Get the item name and use it
            string itemName = action.Substring(5).Split('x')[0].Trim();
            var item = Inventory.GetAllItems().Keys.First(i => i.Name == itemName);

            Inventory.UseItem(item, this);
        }
    }
}

