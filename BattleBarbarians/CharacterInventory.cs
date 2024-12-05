using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBarbarians
{
    internal class CharacterInventory
    {
        private Dictionary<Item, int> _items;
        //private List<Item> _items;

        public CharacterInventory()
        {
            _items = new Dictionary<Item, int>();
        }

        public void AddItem(Item item)
        {
            if (_items.ContainsKey(item))
            {
                _items[item]++;
            }
            else
            {
                _items.Add(item, 1);
            }
            Console.WriteLine($"{item.Name} added to inventory.");
        }

        // Visa föremålen i inventariet
        public void ShowInventory()
        {
            Console.WriteLine("Inventory:");
            foreach (var item in _items)
            {
                Console.WriteLine($"{item.Key.Name} - {item.Value}");
            }
        }

        public Dictionary<Item, int> GetAllItems()
        {
            return _items;
            //return new Dictionary<Item, int>(_items); // Return a copy of inventory
        }

        public bool IsEmpty()
        {
            return !_items.Any();
        }

        public void UseItem(Item item, Character target)
        {
            if (_items.ContainsKey(item) && _items[item] > 0)
            {
                item.Use(target);
                _items[item]--;

                if (_items[item] == 0)
                {
                    _items.Remove(item);
                }
            }
            else
            {
                Console.WriteLine($"You don't have {item.Name} in your inventory");
            }
        }

        //// Använd ett föremål från inventariet
        //public void UseItem(Item item, Character target)
        //{
        //    if (_items.Contains(item))
        //    {
        //        _items.Remove(item);
        //        item.Use(target);
        //        Console.WriteLine($"{item.Name} has been used.");
        //    }
        //    else
        //    {
        //        Console.WriteLine($"{item.Name} is not in the inventory.");
        //    }
        //}

    }
}
