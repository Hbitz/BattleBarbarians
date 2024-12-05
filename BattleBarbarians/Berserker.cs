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

        //public void ShowInventory()
        //{
        //    Inventory.ShowInventory();
        //}

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


            // Lista över valbara alternativ
            var actionOptions = new Dictionary<string, bool>();

            // Lägg till attacker som val
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

            // Lägg till föremål som val
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
                // Visa prompt med alternativen
                selectedAction = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]What do you want to do?[/]")
                        .PageSize(10)
                        .AddChoices(promptChoices));

                // Kontrollera om valet är tillgängligt
                if (!actionOptions[selectedAction])
                {
                    AnsiConsole.MarkupLine("[red]You cannot select this option![/]");
                }

            } while (!actionOptions[selectedAction]); // Repetera tills användaren väljer ett giltigt val

            // Hantera valet
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
            // Extrahera attackens namn och leta upp den
            string attackName = action.Substring(8).Split('-')[0].Trim();
            var attack = Attacks.First(a => a.Name == attackName);

            // Kontrollera mana och applicera skada
            if (Mana < attack.ManaCost)
            {
                Console.WriteLine("[red]You don't have enough mana for this attack.[/]");
                return;
            }

            Console.WriteLine($"{Name} uses {attack.Name}!");
            Mana -= attack.ManaCost;

            int totalDmg = Health < MaxHealth / 2
                ? Convert.ToInt32(CalculateDamage(AttackPower, attack) * 1.3)
                : Convert.ToInt32(CalculateDamage(AttackPower, attack));

            target.TakeDamage(totalDmg);
        }

        private void HandleItem(string action)
        {
            // Extrahera föremålets namn och använd det
            string itemName = action.Substring(5).Split('x')[0].Trim();
            var item = Inventory.GetAllItems().Keys.First(i => i.Name == itemName);

            Inventory.UseItem(item, this);
        }

        public void PerformAttackTest(Character target)
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

            var actionOptions = new List<string>();
            foreach (var attack in Attacks)
            {
                actionOptions.Add($"Attack: {attack.Name}, Skada: {CalculateDamage(AttackPower, attack)}, manacost: {attack.ManaCost}");
            }
            foreach (var item in Inventory.GetAllItems())
            {
                actionOptions.Add($"Item: {item.Key.Name} x{item.Value}");
            }

            //attackChoice = choices.IndexOf(selectedAttack);
            string selectedAction = ""; // Empty for now

            bool validatedAction = false;
            while (!validatedAction)
            {
                //Todo Validation for mana cost
                selectedAction = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Choose an action:")
                    .AddChoices(actionOptions)
                    .HighlightStyle(new Style(foreground: Color.Red))
                );
                if (selectedAction.StartsWith("Attack:"))
                {


                    // This is such a bad way to handle getting an attack.
                    string selectedAttackName = selectedAction.Substring(8).Split(',')[0].Trim();
                    var selectedAttack = Attacks.First(a => a.Name == selectedAttackName);

                    if (Mana < selectedAttack.ManaCost)
                    {
                        Console.WriteLine("You don't have enough mana to use that attack");
                    }
                    else
                    {
                        validatedAction = true;
                    }

                    if (validatedAction)
                    {
                        Console.WriteLine($"{Name} använder {selectedAttack.Name}");
                        Mana -= selectedAttack.ManaCost;
                        if (wasInBerserkState)
                        {
                            // Total dmg is attack power modifier * attack's damage * 30% berserker rage bonus
                            int totalDmg = Convert.ToInt32(CalculateDamage(AttackPower, selectedAttack) * 1.3);
                            Console.WriteLine("1. " + totalDmg);
                            target.TakeDamage(totalDmg);
                        }
                        else
                        {
                            int totalDmg = Convert.ToInt32(CalculateDamage(AttackPower, selectedAttack));
                            Console.WriteLine("2, " + totalDmg);
                            target.TakeDamage(totalDmg);
                        }
                    }
                }
                else if (selectedAction.StartsWith("Item:"))
                {
                    // Hämta item-namn och använd föremål
                    string selectedItemName = selectedAction.Substring(5).Split('x')[0].Trim();
                    var selectedItem = Inventory.GetAllItems().Keys.First(i => i.Name == selectedItemName);

                    Inventory.UseItem(selectedItem, this);
                    validatedAction = true;
                }

            }


            //int attackChoice = ChooseAttack();

            //Attack selectedAttack2 = Attacks[attackChoice];
            //Console.WriteLine($"{Name} använder {selectedAttack2.Name}!");
            //Mana -= selectedAttack2.ManaCost;

            //if (wasInBerserkState)
            //{
            //    Console.WriteLine("1. " + AttackPower);
            //    // Total dmg is attack power modifier * attack's damage * 30% berserker rage bonus
            //    int totalDmg = Convert.ToInt32(CalculateDamage(AttackPower, selectedAttack2) * 1.3);
            //    target.TakeDamage(totalDmg);
            //}
            //else
            //{
            //    Console.WriteLine("2, " + AttackPower);
            //    int totalDmg = Convert.ToInt32(CalculateDamage(AttackPower, selectedAttack2));
            //    target.TakeDamage(totalDmg);
            //}
        }
        public  void PerformAttackTest2(Character target)
        {
            // Förbered lista med val (attacker och föremål)
            var actionOptions = new List<string>();
            // Lägg till attacker i listan
            foreach (var attack in Attacks)
            {
                actionOptions.Add($"Attack: {attack.Name} (Damage: {attack.Damage}, Mana Cost: {attack.ManaCost})");
            }
            // Lägg till föremål från inventory i listan
            foreach (var item in Inventory.GetAllItems())
            {
                actionOptions.Add($"Item: {item.Key.Name} x{item.Value}");
            }

            // Låt spelaren välja
            string action = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]What do you want to do?[/]")
                    .PageSize(10)
                    .AddChoices(actionOptions));

            // Hantera valet
            if (action.StartsWith("Attack:"))
            {
                // Hämta attacknamn och utför attack
                string selectedAttackName = action.Substring(8).Split('(')[0].Trim();
                var selectedAttack = Attacks.First(a => a.Name == selectedAttackName);

                Console.WriteLine($"{Name} uses {selectedAttack.Name}!");
                Mana -= selectedAttack.ManaCost;

                // Beräkna och applicera skada
                int totalDmg = Health < MaxHealth / 2
                    ? Convert.ToInt32(CalculateDamage(AttackPower, selectedAttack) * 1.3)
                    : Convert.ToInt32(CalculateDamage(AttackPower, selectedAttack));

                target.TakeDamage(totalDmg);
            }
            else if (action.StartsWith("Item:"))
            {
                // Hämta item-namn och använd föremål
                string selectedItemName = action.Substring(5).Split('x')[0].Trim();
                var selectedItem = Inventory.GetAllItems().Keys.First(i => i.Name == selectedItemName);

                Inventory.UseItem(selectedItem, this);
            }
        }

    }
}

