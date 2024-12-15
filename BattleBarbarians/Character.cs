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
        public List<Attack> Attacks { get; set; }
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

        //public abstract void PerformAttack(Character target);

        public virtual void PerformAttack(Character target)
        {
            ApplySpecialMechanics(); // Use class-specifics mechanics

            bool actionPerformed = false;

            while (!actionPerformed)
            {
                // Skapa meny för attacker
                var attackOptions = Attacks.ToDictionary(attack => attack, attack => Mana >= attack.ManaCost);

                // Menu for attacks and items
                var promptChoices = new Dictionary<string, Action>();

                foreach (var option in attackOptions)
                {
                    string description = option.Value
                        ? $"{option.Key.Name} - Damage: {CalculateDamageNew(option.Key)}, Mana: {option.Key.ManaCost}"
                        : $"{option.Key.Name} - Damage: {CalculateDamageNew(option.Key)}, Mana: {option.Key.ManaCost}" + Markup.Escape("[Not enough mana]");
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
