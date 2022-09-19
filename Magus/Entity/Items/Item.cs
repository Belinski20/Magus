using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magus.Items
{
    public class Item
    {
        public string Name { get; set; }
        public string Kind { get; set; }
        public string Requires { get; set; }
        public int Weight { get; set; }
        public int Icon { get; set; }
        public int Value { get; set; }
        public int Price { get; set; }

        public Item(string itemName, string itemKind, int itemWeight,
            string itemRequires, int itemIcon, int itemValue, int itemPrice)
        {
            Name = itemName;
            Kind = itemKind;
            Weight = itemWeight;
            Requires = itemRequires;
            Icon = itemIcon;
            Value = itemValue;
            Price = itemPrice;
        }

    }
}
