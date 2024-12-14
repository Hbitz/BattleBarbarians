using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BattleBarbarians
{
    internal class HallOfFameManager
    {
        private const string FilePath = "hall_of_fame.json";
        private const int MaxEntriesPerType = 3; // Max entries per character. This is a speedrun contest.

        // Attempt to read from file, create empty list if it doesn't exist or if it can't be read.
        public List<HallOfFameEntry> ReadEntries()
        {
            if (!File.Exists(FilePath))
            {
                return new List<HallOfFameEntry>();
            }

            string json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<HallOfFameEntry>>(json) ?? new List<HallOfFameEntry>();
        }

        public void WriteEntry(HallOfFameEntry newEntry)
        {
            var entries = ReadEntries();

            // Only the first 3 entries of each character is allowed into the hall of fame.
            int countForType = entries.Count(e => e.CharacterType == newEntry.CharacterType);
            if (countForType >= MaxEntriesPerType)
            {
                Console.WriteLine("There are only so many times a feat could be considered an achievement. Once it's been done by the masses, it no longer has the value of a legendary tale.");
                Console.WriteLine($"Hall of Fame already has filled {MaxEntriesPerType} slots for {newEntry.CharacterType}.");
                return;
            }

            // Add entry and write to file
            entries.Add(newEntry);
            string json = JsonSerializer.Serialize(entries, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);

            Console.WriteLine("Your achievements has been recorded into the hall of fame!");
        }

        public void PrintHallOfFame()
        {
            var entries = ReadEntries();

            if (entries.Count == 0)
            {
                Console.WriteLine("There are no recorded entries. Be the first one!");
                return;
            }

            Console.WriteLine("Hall of Fame:");
            Console.WriteLine(new string('-', 30));

            foreach (var entry in entries)
            {
                Console.WriteLine($"Name: {entry.Name}");
                Console.WriteLine($"Type: {entry.CharacterType}");
                Console.WriteLine($"Health at end of boss battle: {entry.Health}/{entry.MaxHealth}");
                Console.WriteLine($"Mana at end of boss battle: {entry.Mana}/{entry.MaxMana}");
                Console.WriteLine($"Attackpower: {entry.AttackPower}");
                Console.WriteLine(new string('-', 30));
            }
        }
    }
}
