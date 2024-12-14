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

        // Overriding Equals and GetHashCode to compare based on name of object, instead of referense to it.
        // As items in CharacterInventory are saved as a dictionary this allows use so we can easily visually display how many items of one kind there is.
        // "Health potions - 4" instead of four entries of "health potion" when displaying inventory.
        public override bool Equals(object obj)
        {
            if (obj is Item item)
            {
                return Name == item.Name; // Compare name
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode(); // Use name to generarte hashcode
        }


    }
}
