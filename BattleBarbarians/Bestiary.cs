using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBarbarians
{
    internal class Bestiary
    {
        private Dictionary<string, BestiaryEntry> entries = new Dictionary<string, BestiaryEntry>();
        public AsciiArtProvider AsciiArtProvider = new AsciiArtProvider();


        public void AddEntry(string name, string description, List<Attack> attacks)
        {
            if (!entries.ContainsKey(name))
            {
                entries[name] = new BestiaryEntry(name, description, attacks);
            }
        }

        public void Discover(string name)
        {
            if (entries.ContainsKey(name))
            {
                entries[name].IsDiscovered = true;
            }
        }

        // Todo - Enemies have descriptions, but currently unused. Maybe add HP?
        public void ShowDiscoveredEntries()
        {
            Console.WriteLine("Bestiary:");

            if (entries.Values.All(e => !e.IsDiscovered))
            {
                Console.WriteLine("You have not yet encountered any enemies!");
                Console.WriteLine("The first time you encounter their entry is added.");
                Console.WriteLine("");
            }
            else
            {
                foreach (var entry in entries.Values)
                {
                    if (entry.IsDiscovered)
                    {
                        Console.WriteLine(new string('-', 30));
                        Console.WriteLine($"{entry.Name}");
                        Console.WriteLine(AsciiArtProvider.GetAsciiArt(entry.Name));
                        Console.WriteLine("\nAttacks:");
                        foreach (Attack attack in entry.Attacks)
                        {
                            Console.WriteLine($"{attack.Name}, {attack.Description} Damage: {attack.Damage}, Manacost: {attack.ManaCost}");

                        }
                    }
                }
                Console.WriteLine(new string('-', 30));
                Console.WriteLine();
            }
        }
    }
}
