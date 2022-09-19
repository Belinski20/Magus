using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace Magus.Items
{
    public class Item
    {
        public ItemTypes Name { get; set; }

        public ItemClass Kind { get; set; }
        public ItemReq Requires { get; set; }
        public int Weight { get; set; }
        public Image Icon { get; set; }
        public int Value { get; set; }
        public int Price { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        //Refers to the state the item is in
        //Carrying is when a player has the item stored in the inventory
        //In use is when the item is being weilded or weared
        //None is a blank state
        public string State { get; set; }


        public Item()
        {
            State = "none";
        }

    }
}
