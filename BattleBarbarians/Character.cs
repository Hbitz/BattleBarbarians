using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBarbarians
{
    internal abstract class Character<T> where T : Attack
    {
        // Properties
        public string Name { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int Mana { get; set; }
        public int MaxMana { get; set; }
        public int AttackPower { get; set; }
        public List<T> Attacks { get; set; } // Generic lista with attacks

        public Character(string name, int health, int mana, int attackPower, List<T> attacks)
        {
            Name = name;
            Health = MaxHealth = health;
            Mana = MaxMana = mana;
            AttackPower = attackPower;
            Attacks = attacks ?? new List<T>();
        }

        public virtual void PerformAttack(Character<T> target, T attack)
        {
            if (Mana >= attack.ManaCost)
            {
                Mana -= attack.ManaCost;
                target.TakeDamage(attack.Damage);
                Console.WriteLine($"{Name} used {attack.Name} on {target.Name} for {attack.Damage} damage!");
            }
            else
            {
                Console.WriteLine($"{Name} tried to use {attack.Name}, but didn't have enough Mana!");
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
    }
}
