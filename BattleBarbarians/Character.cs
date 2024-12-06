using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace BattleBarbarians
{
    internal abstract class Character
    {
        // Properties
        public string Name { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int Mana { get; set; }
        public int MaxMana { get; set; }
        public double AttackPower { get; set; }
        public List<Attack> Attacks { get; set; } // Generic lista with attacks
        // Because Inventory logic could be extensive and because it's not needed by all characters(rat, troll, etc), we create a InventoryManager class.
        public CharacterInventory Inventory { get; set; }

        public Character(string name, int health, int mana, double attackPower, List<Attack> attacks)
        {
            Name = name;
            Health = MaxHealth = health;
            Mana = MaxMana = mana;
            AttackPower = attackPower;
            Attacks = attacks ?? new List<Attack>();
            Inventory = new CharacterInventory();
        }

        // Abstract method used to apply a character's special behaviors
        protected abstract void ApplySpecialMechanics();

        public double CalculateDamage(double attackPower, Attack attack)
        {
            return Convert.ToInt32(attackPower * attack.Damage);
        }

        public abstract void PerformAttack(Character target);

        // New main method of attacking for all playable characters
        public virtual void PerformAttack2(Character target)
        {
            ApplySpecialMechanics(); // Use class-specifics mechanics

            bool actionPerformed = false;

            while (!actionPerformed)
            {
                // Skapa meny för attacker
                var attackOptions = Attacks.ToDictionary(attack => attack, attack => Mana >= attack.ManaCost);

                //Old
                //var attackOptions = new Dictionary<Attack, bool>();
                //foreach (var attack in Attacks)
                //{
                //    bool isAvailable = Mana >= attack.ManaCost;
                //    attackOptions[attack] = isAvailable;
                //}

                // Menu for attacks and items
                var promptChoices = new Dictionary<string, Action>();

                foreach (var option in attackOptions)
                {
                    string description = option.Value
                        ? $"{option.Key.Name} - Damage: {CalculateDamageNew(option.Key)}, Mana: {option.Key.ManaCost}"
                        : $"{option.Key.Name} - Damage: {CalculateDamage(AttackPower, option.Key)}, Mana Cost: {option.Key.ManaCost}" + Markup.Escape("[Not enough mana]");
                    promptChoices[description] = () =>
                    {
                        if (option.Value)
                        {
                            HandleAttack(option.Key, target);
                            actionPerformed = true;
                        }
                        else
                        {
                            Console.WriteLine("Not enough mana!");
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
                        .Title("Choose your action:")
                        .PageSize(10)
                        .AddChoices(promptChoices.Keys));
                promptChoices[selectedAction]();
            }
        }

        // New method of using CalcuateDamage, now draws directly from attackPower rather than relying on double-type input
        protected virtual int CalculateDamageNew(Attack attack)
        {
            return Convert.ToInt32(AttackPower * attack.Damage); 
        }



        // Old method of choosing attacks, before inventory was implemeneted. No longer used
        // Todo - validation
        public int ChooseAttack()
        {
            var choices = new List<string>();
            for (int i = 0; i < Attacks.Count; i++)
            {
                choices.Add($"{Attacks[i].Name} Skada: {CalculateDamage(AttackPower, Attacks[i])}, manacost: {Attacks[i].ManaCost}");
            }

            int attackChoice = 0; // Temporarily empty

            // Validaiton to ensure you can only select an attack you have enough mana for.
            bool validAttack = false;
            while (validAttack == false)
            {

                string selectedAttack = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title($"{Name}, Välj en attack:")
                    .AddChoices(choices)
                    .HighlightStyle(new Style(foreground: Color.Red))
                );
                attackChoice = choices.IndexOf(selectedAttack);

                if (Mana < Attacks[attackChoice].ManaCost)
                {
                    Console.WriteLine("You don't have enough mana to use that attack");
                }
                else
                {
                    validAttack = true;
                }
            }
            return attackChoice;
        }

        // Unused, not referenced by program. Safe to remove
        public void ChooseItem()
        {
            var choices = new List<string>();
            if (Inventory.IsEmpty())
            {
                Console.WriteLine("Inventory empty");
                return;
            }

            var items = Inventory.GetAllItems();

            // Creates a list of our items
            var itemNames = items.Select(item => $"{item.Key.Name} ({item.Value})").ToList();

            // Create menu to let player select 
            var chosenItemName = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Choose an item to use:[/]")
                    .PageSize(10) 
                    .AddChoices(itemNames)
            );

            // Hitta föremålet användaren valde
            var chosenItem = items.FirstOrDefault(item =>
                $"{item.Key.Name} ({item.Value})" == chosenItemName).Key;

            // Kontrollera om ett föremål har valts
            if (chosenItem != null)
            {
                Inventory.UseItem(chosenItem, this);
            }
        }


        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health < 0) Health = 0;
            Console.WriteLine($"{Name} took {damage} damage and now has {Health}/{MaxHealth} HP.");
        }

        public void RecoverMana(int amount)
        {
            Mana += amount;
            if (Mana > MaxMana) Mana = MaxMana;
            Console.WriteLine($"{Name} recovered {amount} Mana and now has {Mana}/{MaxMana} Mana.");
        }

        public bool IsAlive()
        {
            return Health > 0;
        }

        public override string ToString()
        {
            return $"{Name} - HP: {Health}/{MaxHealth}, Mana: {Mana}/{MaxMana}, Attack Power: {AttackPower}";
        }

        public virtual void ShowInventory()
        {
            Inventory.ShowInventory();
        }
        public virtual void GetInventoryChoices()
        {

        }

        public virtual void HandleAttack(Attack attack, Character target)
        {
            Console.WriteLine($"{Name} uses {attack.Name}!");
            Mana -= attack.ManaCost;

            int damage = CalculateDamageNew(attack);
            target.TakeDamage(damage);
        }

        protected virtual void HandleItem(Item item)
        {
            Console.WriteLine($"{Name} uses {item.Name}!");
            Inventory.UseItem(item, this);
        }
    }
}
