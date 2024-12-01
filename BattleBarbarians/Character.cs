﻿using System;
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

        public Character(string name, int health, int mana, double attackPower, List<Attack> attacks)
        {
            Name = name;
            Health = MaxHealth = health;
            Mana = MaxMana = mana;
            AttackPower = attackPower;
            Attacks = attacks ?? new List<Attack>();
        }

        public double CalculateDamage(double attackPower, Attack attack)
        {
            return Convert.ToInt32(attackPower * attack.Damage);
        }

        public abstract void PerformAttack(Character target);

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
