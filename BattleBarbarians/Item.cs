using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBarbarians
{
    internal abstract class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public Item(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public abstract void Use(Character target);
        //public Action<Character> Use { get; set; }

        // Use is a delegate that represents what the item does upon usage.
        //public Item(string name, string description, Action<Character> use)
        //{
        //    Name = name;
        //    Description = description;
        //    Use = use;
        //}

        // Överskriver Equals och GetHashCode för att jämföra baserat på namn
        // Vi gör detta för att jämföra Namnen på objekten, istället för referrenserna till det.
        // När items lagras som dictionary i CharacterInventory låter denna ändringen göra så att vi enkelt kan visa hur många items av en sort det finns.
        // Tex "Health potions - 4"
        public override bool Equals(object obj)
        {
            if (obj is Item item)
            {
                return Name == item.Name; // Jämför namn
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode(); // Använd namn för att generera hashkod
        }


    }
}
