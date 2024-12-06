using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BattleBarbarians
{
    internal class Dwarf : Character
    {
        // The third class, Dwarf, is supposed to be a hard-mode character with overall nerfed attack power combined with a randomness to all of his attacks
        private Random random; 

        public Dwarf(string name)
            : base(
                  name, // Player name
                  100,  // HP
                  45,   // Mana
                  1,   // Attack Power modifier
                  new List<Attack> {
                      new Attack("Lucky Shot", 20, 5, "A quick shot with a random outcome."),
                      new Attack("Double or Nothing", 50, 20, "A risky double attack with a chance to miss.")
                  }
            )
        {
            random = new Random();
        }
        protected override void ApplySpecialMechanics()
        {
            // Implement special mechanics, if needed
        }

        // Old way of attacking. Playable characters no longer use PerformAttack
        public override void PerformAttack(Character target)
        {
            Console.WriteLine("PerformAttack dwarf not implemented");
            throw new NotImplementedException();
        }


        // Testing, attempted to combine logic between Character and Dwarf to resolve special effects and randomness of dwarf's attacks.
        // Solved it by using all logic from character, but overriding HandlAttack
        public void PerformAttack5(Character target)
        {
            // Välj attack med hjälp av basklassens mekanism
            int attackChoice = ChooseAttack(); // Välj attack från basklassen
            Attack selectedAttack = Attacks[attackChoice];

            // Validera om det finns tillräckligt med mana (använd basklassens logik för detta)
            if (Mana < selectedAttack.ManaCost)
            {
                Console.WriteLine("Not enough mana for this attack.");
                return;
            }

            // Minska mana
            Mana -= selectedAttack.ManaCost;

            // Dwarf-specifik attacklogik (t.ex. kritiska träffar, specialeffekter)
            double damage = ResolveAttack(selectedAttack.Name, target);  // Använd Dwarf-specifik logik för att beräkna skada

            // Skicka skada till målet
            //target.TakeDamage((int)damage);

            // Dwarf-specifika effekter efter attacken
            ResolveSpecialAttackEffects(selectedAttack, target);
        }

        // Dwarf-specific change: We use ResolveAttack to run the attack through dwarf's randomness and extra attack logic
        public override void HandleAttack(Attack attack, Character target)
        {
            Console.WriteLine($"{Name} uses {attack.Name}!");
            Mana -= attack.ManaCost;

            double damage = ResolveAttack(attack.Name, target);
            target.TakeDamage((int)damage);
        }

        private double ResolveAttack(string attackName, Character target)
        {
            switch (attackName)
            {
                case "Lucky Shot":
                    return LuckyShot(target); // Dwarf-specifik logik
                case "Double or Nothing":
                    return DoubleOrNothing(target); // Dwarf-specifik logik
                default:
                    return CalculateDamage(AttackPower, Attacks.FirstOrDefault(a => a.Name == attackName)); // Standard attacklogik
            }
        }

        private double LuckyShot(Character target)
        {
            // Dwarf-specifik logik för Lucky Shot
            Attack luckyShot = Attacks.FirstOrDefault(a => a.Name == "Lucky Shot");
            double dmg = 0;
            int chance = random.Next(1, 101); // Get a number between 1 and 100 to determine luck
            string attackStatus = "normal";
            string attackInfo = $"{Name} attacks {target.Name} with Lucky Shot, causing {dmg} damage!";

            if (chance <= 20) // 20% chance for a critical hit
            {
                attackStatus = "crit";
                dmg = CalculateDamage(AttackPower, luckyShot) * 2;
                attackInfo = $"{Name} lands a critical hit on {target.Name} with Lucky Shot, causing {dmg} damage!";
            }
            else if (chance <= 60) // 40% chance for a normal hit
            {
                dmg = CalculateDamage(AttackPower, luckyShot);
            }
            else // 40% chance for a low dmg attack
            {
                attackStatus = "low";
                dmg = CalculateDamage(AttackPower, luckyShot) * 0.5;
                attackInfo = $"{Name} attacks {target.Name} with Lucky Shot, but {Name} is too drunk and the shot only grazes his target causing {dmg} damage.";
            }

            Console.WriteLine(attackInfo);
            return dmg;
        }

        private double DoubleOrNothing(Character target)
        {
            // Dwarf-specifik logik för Double or Nothing
            Attack attack = Attacks.FirstOrDefault(a => a.Name == "Double or Nothing");
            double dmg = 0;
            int chance = random.Next(1, 101); // Randomt nummer mellan 1 och 100

            if (chance <= 50) // 50% chans att lyckas
            {
                dmg = CalculateDamage(AttackPower, attack) * 2;
                Console.WriteLine($"{Name} hits {target.Name} with a double strike, dealing {dmg} damage!");
            }
            else
            {
                Console.WriteLine($"{Name} misses the Double or Nothing attack!");
            }

            return dmg;
        }


        // Testing, now unused
        public void PerformAttack2Old(Character target)
        {

            
            int attackChoice = ChooseAttack();
            Attack selectedAttack = Attacks[attackChoice];
            // Validering av attacker och mana sker i basklassens PerformAttack
            if (Mana < selectedAttack.ManaCost)
            {
                Console.WriteLine("Not enough mana for this attack.");
                return;
            }
            Mana -= selectedAttack.ManaCost;

            // Anropa basklassens PerformAttack för att hantera gemensam logik
            //base.PerformAttack2(target);

            // Dwarf-specifik logik: Slumpmässig effekt på Lucky Shot och Double or Nothing
            ResolveSpecialAttackEffects(selectedAttack, target);
        }

        // Slumpmässig logik för att modifiera effekten av Dwarf:s attacker
        private void ResolveSpecialAttackEffects(Attack selectedAttack, Character target)
        {
            if (selectedAttack.Name == "Lucky Shot")
            {
                LuckyShot(target);  // Slumpande skada för Lucky Shot
            }
            else if (selectedAttack.Name == "Double or Nothing")
            {
                DoubleOrNothing(target);  // Högrisk för Double or Nothing
            }
        }

        // Lucky Shot: Slumpmässig skada med olika chanser (krit, normal eller låg)
        public void LuckyShotOld(Character target)
        {
            Attack luckyShot = Attacks[0];
            double dmg = 0;
            int chance = random.Next(1, 101);  // Slumpa mellan 1 och 100

            string attackStatus = "normal";
            if (chance <= 20)  // 20% chans för kritisk träff
            {
                attackStatus = "crit";
                dmg = CalculateDamage(AttackPower, luckyShot) * 2;  // Dubbel skada för kritisk träff
            }
            else if (chance <= 60)  // 40% chans för normal skada
            {
                dmg = CalculateDamage(AttackPower, luckyShot);
            }
            else  // 40% chans för låg skada
            {
                attackStatus = "low";
                dmg = CalculateDamage(AttackPower, luckyShot) * 0.5;  // Halv skada för låg träff
            }

            // Feedback om attacken
            string attackInfo = attackStatus switch
            {
                "normal" => $"{Name} attacks {target.Name} with Lucky Shot, causing {dmg} damage!",
                "crit" => $"{Name} lands a critical hit with Lucky Shot, causing {dmg} damage!",
                "low" => $"{Name} attacks {target.Name} with Lucky Shot, but {Name} is too drunk and the shot only grazes his target causing {dmg} damage.",
                _ => string.Empty
            };

            Console.WriteLine(attackInfo);
            target.TakeDamage((int)dmg);
        }

        // Double or Nothing: Högriskattack
        public void DoubleOrNothingOld(Character target)
        {
            Attack attack = Attacks[1];
            double dmg = 0;
            int chance = random.Next(1, 101);  // Slumpa mellan 1 och 100

            if (chance <= 50)  // 50% chans för lyckad attack
            {
                dmg = CalculateDamage(AttackPower, attack) * 2;  // Dubbel skada om lyckad
                Console.WriteLine($"{Name} hits {target.Name} with a double strike, dealing {dmg} damage!");
            }
            else  // 50% chans att missa
            {
                Console.WriteLine($"{Name} misses the Double or Nothing attack!");
            }

            target.TakeDamage((int)dmg);
        }

        //public override void PerformAttack2(Character target)
        //{
        //    int attackChoice = ChooseAttack();
        //    Attack selectedAttack = Attacks[attackChoice];

        //    int totalDmg = (int)ResolveAttack(selectedAttack.Name, target);
        //    Mana -= selectedAttack.ManaCost;
        //    target.TakeDamage(totalDmg);

        //}

        //private double ResolveAttack(string attackName, Character target)
        //{
        //    switch (attackName)
        //    {
        //        case "Lucky Shot":
        //            return LuckyShot(target);
        //        case "Double or Nothing":
        //            return DoubleOrNothing(target);
        //        default:
        //            return LuckyShot(target); // Fallback för okända attacker
        //    }
        //}


        //public double LuckyShot(Character target)
        //{
        //    Attack luckyShot = Attacks[0];
        //    double dmg = 0;
        //    int chance = random.Next(1, 101); // Get a number between 1 and 100 for luck
        //    string attackStatus = "normal";
        //    string attackInfo = "";


        //    if (chance <= 20) // 20% chance for a critical hit
        //    {
        //        attackStatus = "crit";
        //        dmg = (int)(CalculateDamage(AttackPower, luckyShot) * 2);
        //    }
        //    else if (chance <= 60) // 40% chance for a normal hit
        //    {
        //        dmg = (int)(CalculateDamage(AttackPower, luckyShot));
        //    }
        //    else // 40% chance for a low damage attack
        //    {
        //        attackStatus = "low";
        //        dmg = (int)(CalculateDamage(AttackPower, luckyShot) * 0.5); // Half damage
        //    }


        //    switch (attackStatus)
        //    {
        //        case "normal":
        //            attackInfo = $"{Name} attacks {target.Name} with Lucky Shot, causing {dmg} damage!";
        //            break;
        //        case "crit":
        //            attackInfo = $"{Name} lands a critical hit with Lucky Shot, causing {dmg} damage!";
        //            break;
        //        case "low":
        //            attackInfo = $"{Name} attacks {target.Name} with Lucky Shot, but {Name} is too drunk and the shot only grazes his target causing {dmg} damage.";
        //            break;
        //    }

        //    Console.WriteLine(attackInfo);
        //    return dmg;
        //}

        //// Additional ability: Double or Nothing with high risk and reward
        //public double DoubleOrNothing(Character target)
        //{
        //    Attack attack = Attacks[1];
        //    double dmg = 0;
        //    int chance = random.Next(1, 101); // Random number between 1 and 100

        //    if (chance <= 50) // 50% chance to succeed
        //    {
        //        dmg = (int)(CalculateDamage(AttackPower, attack) * 2);
        //        Console.WriteLine($"{Name} hits {target.Name} with a double strike, dealing {dmg} damage!");
        //    }
        //    else
        //    {
        //        Console.WriteLine($"{Name} misses the Double or Nothing attack!");
        //    }
        //    return dmg;
        //}
    }
}

