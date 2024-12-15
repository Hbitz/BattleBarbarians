using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BattleBarbarians
{
    // Currently, all enemy art are saved in TXT file in resource folder.
    // By changing the property "Build action: none" -> "Embedded resource" of the textfiles in our resource folder we can easily load it in our class.
    // By saving them in simple .txt we can at any time easily overlook, change and use our art, without needing to format it with newlines etc.
    internal class AsciiArtProvider
    {
        // Dict resourceName.txt: "art"
        private static Dictionary<string, string> _asciiArt;

        static AsciiArtProvider()
        {
            _asciiArt = new Dictionary<string, string>();
            LoadAsciiArt();
        }

        private static void LoadAsciiArt()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceNames = assembly.GetManifestResourceNames();

            // Filter resources to those that include "Resources" in their filepath
            foreach (var resourceName in resourceNames)
            {
                if (resourceName.Contains("Resources"))
                {
                    using var stream = assembly.GetManifestResourceStream(resourceName);
                    using var reader = new StreamReader(stream!);

                    // Example: Extract key from resource name
                    var key = resourceName.Substring("BattleBarbarians.Resources.".Length);
                    var art = reader.ReadToEnd();
                    _asciiArt[key] = art;
                }
            }
        }

        public static string GetAsciiArt(string characterName)
        {
            // Adding ".txt" as the names of our art is saved as artName.txt in _asciiArt dict.
            if (_asciiArt.TryGetValue(characterName + ".txt", out var art))
                return art;

            return $"ASCII art for '{characterName}' not found!";
        }
    }
}
