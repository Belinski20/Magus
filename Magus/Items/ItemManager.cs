using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magus.Entity;
using Magus.GameBoard;
using System.Media;
using Magus.Util;
using Magus.Properties;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Magus.Items
{
    class ItemManager
    {
        public ItemGenerator iGen;
        public GameManager manager;
        public ArrayList items;
        public Hashtable itemsInUse;
        Item i = new Item();

        //Used for adding player stats based on items
        int mod;

        public ItemManager(GameManager manager)
        {
            items = new ArrayList();
            itemsInUse = new Hashtable();
            this.manager = manager;
            iGen = new ItemGenerator(manager);
            mod = 5;
        }

        public Image GetItemImage(int x, int y)
        {
            foreach(Item i in items)
            {
                if (i.X == x && i.Y == y)
                    return i.Icon;
            }
            return null;
        }

        //Adds items to the list each character has
        public void AddItem(Item item)
        {
            items.Add(item);
        }

      public void GenerateItems()
      {
         items = iGen.SpawnItems();
      }
      /*
        //Associates items with characters
        public void AddItemToInventory(Character character, Item item)
        {
            AddItem(item);
            itemsInUse.Add(character, items);
        }

        //Removes item from list
        public void RemoveItem(Character character, Item item)
        {
            items.Remove(item);
            itemsInUse.Remove(character);
            itemsInUse.Add(character, items);
            Console.WriteLine(item + " has been removed.");
        }

        //Returns number of items in list
        public int HowManyItems()
        {
            return items.Count;
        }

        //Set state to "weilding"
        public void WieldItem(Character character, Item item)
        {
            if (item.State == "weilding")
                character.skill -= mod;
            RemoveItem(character, item);
            item.State = "weilding";
            AddItemToInventory(character, item);
        }

        //Set to carry wielding item
        public void UnWieldItem(Character character, Item item)
        {
            RemoveItem(character, item);
            item.State = "carrying";
            AddItemToInventory(character, item);
            if (item.State == "weilding")
                character.skill += mod;
        }

        //Set state to "wearing"
        public void WearItem(Character character, Item item)
        {
            RemoveItem(character, item);
            item.State = "wearing";
            AddItemToInventory(character, item);

            switch(item.Kind)
            {
                case "O_GREENAMULET":
                    character.Skill += mod + item.Value;
                    break;

                case "O_BLUEAMULET":
                    character.Wisdom += mod + item.Value;
                    break;

                case "O_YELLOWAMULE":
                    character.Speed += Math.Max(0, 1 + item.Value / 2);
                    break;

                case "O_REDGLOVES":
                    character.Strength += mod + Math.Max(0, item.Value);
                    break;
            }
        }

        //Set state to "carrying" for wearable item
        public void UnWearItem(Character character, Item item)
        {
            RemoveItem(character, item);
            item.State = "carrying";
            AddItemToInventory(character, item);

            switch (item.Kind)
            {
                case "O_GREENAMULET":
                    character.Skill -= mod + item.Value;
                    break;

                case "O_BLUEAMULET":
                    character.Wisdom -= mod + item.Value;
                    break;

                case "O_YELLOWAMULE":
                    character.Speed -= Math.Max(0, 1 + item.Value / 2);
                    break;

                case "O_REDGLOVES":
                    character.Strength -= mod + Math.Max(0, item.Value);
                    break;
            }
        }

        //Returns true if the item can be comined, false otherwise.
        public bool MayBeComined(Item item)
        {
            switch(item.Kind)
            {
                case "O_CLASS_WEAPON":
                case "O_CLASS_MIXEDWEAPON":
                    return true;
            }
            return false;
        }

        //Use the item
        public void Use(Character character, Item item)
        {
            if (item.State == "wielding" && item.Kind == "O_CLASS_SPELL")
            {
                UnWieldItem(character, item);
            }
            if (item == null || !MayUse(character, item))
                return;

            switch (item.Kind)
            {
                case "O_CLASS_WEAPON":
                case "O_CLASS_THROWINGWEAPON":
                case "O_CLASS_RANGEDWEAPON":
                case "O_CLASS_SPELL":
                case "O_CLASS_MIXEDWEAPON":
                case "O_CLASS_SPECIAL":
                case "O_CLASS_POTION":
                    Item[] arr = (Item[])itemsInUse[character];
                    int index = Array.IndexOf(arr, item);
                    if (item.State == "wielding")
                    {
                        if (arr[index + 1].State == "wielding")
                        {
                            return;
                        }
                        if (!MayBeComined(item) || !MayBeComined(arr[index + 1]))
                            return;
                    }
                    WieldItem(character, item);
                    break;

                case "O_CLASS_AARMOR":
                    if (character.Moves >= 2)
                    {
                        WearItem(character, item);
                        character.Moves -= 2;
                    }
                    break;
                case "O_CLASS_GADGET":
                    if (character.Moves > 0)
                    {
                        WearItem(character, item);
                        character.Moves--;
                    }
                    break;

            }
        }

        //Returns the number associated with the string
        public int getNum(Item it)
        {
            int num = 0;
            switch (it.Requires)
            {
                case "O_REQ_NOTHING":
                    num = 0;
                    break;
                case "O_REQ_ONEHAND":
                    num = 1;
                    break;
                case "O_REQ_TWOHANDS":
                    num = 2;
                    break;
                case "O_REQ_HEAD":
                    num = 4;
                    break;
                case "O_REQ_BODY":
                    num = 8;
                    break;
                case "O_REQ_HANDS":
                    num = 16;
                    break;
                case "O_REQ_NECK":
                    num = 32;
                    break;
                case "O_REQ_FINGER":
                    num = 64;
                    break;
                case "O_REQ_SHOLDERS":
                    num = 128;
                    break;
            }
            return num;
        }

        //Determines if the character can use the item.
        public bool MayUse(Character character, Item item)
        {
            int holds = 0;
            int hands = 2;
            int num = 0;
            Item it = null;
            Item[] arr = (Item[])itemsInUse[character];
            int loc = Array.IndexOf(arr, item);
            if (loc != -1 && arr[loc].State == "wielding")
                it = arr[loc];
            if (it != null)
            {
                num = getNum(it);
                holds |= num;
                hands -= num & 3;
            }
            if (loc != -1 && arr[loc].State == "wearing")
                it = arr[loc];
            if (it != null)
            {
                num = getNum(it);
                holds |= num;
                hands -= num & 3;
            }
            if ((getNum(it) & 3) > hands)
                return false;
            return ((getNum(it) & 0xFFFC & holds)) == 0;
        }

        //Stop using the item.
        public void StopUsing(Character character, Item item)
        {
            if (item == null)
                return;

            switch(item.Kind)
            {
                case "O_CLASS_SPELL":
                case "O_CLASS_THROWINGWEAPON":
                case "O_CLASS_MIXEDWEAPON":
                    UnWieldItem(character, item);
                    break;
                case "O_CLASS_WEAPON":
                case "O_CLASS_SPECIAL":
                case "O_CLASS_RANGEDWEAPON":
                case "O_CLASS_POTION":
                    if (character.Moves > 0)
                    {
                        UnWieldItem(character, item);
                        character.Moves--;
                    }
                    break;
                case "O_CLASS_ARMOR":
                case "O_CLASS_GADGET":
                    if (character.Moves > 0)
                    {
                        UnWearItem(character, item);
                        character.Moves--;
                    }
                    break;
            }
        }
      */
        //Places the item in the world
        public void PlaceItem(int x, int y)
        {
         
        }

      /*
        public void FillList(int x, int y)
        {
    
            AddItem(new Item("Sword", "O_CLASS_WEAPON", 4, "O_REQ_ONEHAND", 76, 9, 20, x, y));
            AddItem(new Item("2H-Sword", "O_CLASS_WEAPON", 8, "O_REQ_TWOHANDS", 74, 15, 75, x, y));
            AddItem(new Item("Axe", "O_CLASS_WEAPON", 5, "O_REQ_ONEHAND", 85, 10, 20, x, y));
            AddItem(new Item("Dagger", "O_CLASS_MIXEDWEAPON", 1, "O_REQ_ONEHAND", 88, 6, 10, x, y));
            AddItem(new Item("Bow", "O_CLASS_RANGEDWEAPON", 3, "O_REQ_TWOHANDS", 83, 9, 20, x, y));
            AddItem(new Item("Crossbow", "O_CLASS_RANGEDWEAPON", 4, "O_REQ_TWOHANDS", 82, 12, 20, x, y));
            AddItem(new Item("Throwing star", "O_CLASS_THROWINGWEAPON", 1, "O_REQ_ONEHAND", 89, 9, 5, x, y));
            AddItem(new Item("Fireblade", "O_CLASS_SPECIAL", 2, "O_REQ_ONEHAND", 173, 0, 150, x, y));
            AddItem(new Item("Helmet", "O_CLASS_ARMOR", 3, "O_REQ_HEAD", 91, 2, 20, x, y));
            AddItem(new Item("Shield", "O_CLASS_ARMOR", 3, "O_REQ_ONEHAND", 78, 4, 20, x, y));
            AddItem(new Item("Leather", "O_CLASS_ARMOR", 3, "O_REQ_BODY", 79, 2, 20, x, y));
            AddItem(new Item("Armour", "O_CLASS_ARMOR", 7, "O_REQ_BODY", 80, 6, 75, x, y));
            AddItem(new Item("Emerald", "O_CLASS_GADGET", 1, "O_REQ_NECK", 81, 0, 100, x, y));
            AddItem(new Item("Emerald ring", "O_CLASS_GADGET", 0, "O_REQ_FINGER", 90, 0, 150, x, y));
            AddItem(new Item("Arrows", "O_CLASS_ARROWS", 0, "O_REQ_NOTHING", 84, 0, 10, x, y));
            AddItem(new Item("Studded leather", "O_CLASS_ARMOR", 4, "O_REQ_BODY", 77, 3, 20, x, y));
            AddItem(new Item("Lightning bolt", "O_CLASS_SPELL", 0, "O_REQ_ONEHAND", 55, 10, 0, x, y));
            AddItem(new Item("FireBall", "O_CLASS_SPELL", 0, "O_REQ_ONEHAND", 60, 5, 0, x, y));
            AddItem(new Item("Portal", "O_CLASS_SPELL", 0, "O_REQ_ONEHAND", 56, 7, 0, x, y));
            AddItem(new Item("Air", "O_CLASS_SPELL", 0, "O_REQ_ONEHAND", 72, 7, 0, x, y));
            AddItem(new Item("Fire", "O_CLASS_SPELL", 0, "O_REQ_ONEHAND", 70, 10, 0, x, y));
            AddItem(new Item("Water", "O_CLASS_SPELL", 0, "O_REQ_ONEHAND", 68, 8, 0, x, y));
            AddItem(new Item("Water", "O_CLASS_SPELL", 0, "O_REQ_ONEHAND", 68, 8, 0, x, y));
            AddItem(new Item("Earth", "O_CLASS_SPELL", 0, "O_REQ_ONEHAND", 67, 8, 0, x, y));
            AddItem(new Item("Skeleton", "O_CLASS_SPELL", 0, "O_REQ_ONEHAND", 64, 15, 0, x, y));
            AddItem(new Item("Zombie", "O_CLASS_SPELL", 0, "O_REQ_ONEHAND", 65, 15, 0, x, y));
            AddItem(new Item("Vision", "O_CLASS_SPELL", 0, "O_REQ_ONEHAND", 69, 2, 0, x, y));
            AddItem(new Item("Phantom", "O_CLASS_SPELL", 0, "O_REQ_ONEHAND", 73, 3, 0, x, y));
            AddItem(new Item("Slayer", "O_CLASS_WEAPON", 8, "O_REQ_TWOHANDS", 122, 17, 150, x, y));
            AddItem(new Item("FastFeet", "O_CLASS_SPELL", 0, "O_REQ_ONEHAND", 54, 8, 0, x, y));
            AddItem(new Item("Heal", "O_CLASS_SPELL", 0, "O_REQ_ONEHAND", 53, 5, 20, x, y));
            AddItem(new Item("StoneAxe", "O_CLASS_WEAPON", 10, "O_REQ_TWOHANDS", 107, 15, 0, x, y));
            AddItem(new Item("Gloves", "O_CLASS_ARMOR", 1, "O_REQ_HANDS", 58, 1, 20, x, y));
            AddItem(new Item("Darkness", "O_CLASS_SPELL", 0, "O_REQ_ONEHAND", 57, 10, 0, x, y));
            AddItem(new Item("Darkness", "O_CLASS_SPELL", 0, "O_REQ_ONEHAND", 57, 10, 0, x, y));
            AddItem(new Item("Sabre", "O_CLASS_WEAPON", 7, "O_REQ_ONEHAND", 86, 10, 0, x, y));
            AddItem(new Item("Staff", "O_CLASS_WEAPON", 3, "O_REQ_TWOHANDS", 87, 8, 20, x, y));
            AddItem(new Item("Chaos", "O_CLASS_SPELL", 0, "O_REQ_ONEHAND", 66, 20, 0, x, y));
            AddItem(new Item("Demon", "O_CLASS_SPELL", 0, "O_REQ_ONEHAND", 63, 25, 0, x, y));
            AddItem(new Item("Cloak", "O_CLASS_ARMOR", 1, "O_REQ_SHOULDERS", 94, 1, 20, x, y));
            AddItem(new Item("Big shield", "O_CLASS_ARMOR", 6, "O_REQ_ONEHAND", 110, 6, 20, x, y));
            AddItem(new Item("Big helmet", "O_CLASS_ARMOR", 4, "O_REQ_HEAD", 111, 3, 20, x, y));
            AddItem(new Item("Opal ring", "O_CLASS_GADGET", 0, "O_REQ_FINGER", 97, 0, 150, x, y));
            AddItem(new Item("Club", "O_CLASS_WEAPON", 5, "O_REQ_ONEHAND", 119, 11, 20, x, y));
            AddItem(new Item("Topaz", "O_CLASS_GADGET", 1, "O_REQ_NECK", 123, 0, 300, x, y));
            AddItem(new Item("Opal", "O_CLASS_GADGET", 1, "O_REQ_NECK", 124, 0, 150, x, y));
            AddItem(new Item("Stonefoot", "O_CLASS_SPELL", 0, "O_REQ_ONEHAND", 129, 6, 0, x, y));
            AddItem(new Item("Lightning", "O_CLASS_SPELL", 0, "O_REQ_ONEHAND", 71, 15, 0, x, y));
            AddItem(new Item("DragonTooth", "O_CLASS_WEAPON", 4, "O_REQ_ONEHAND", 132, 13, 150, x, y));
            AddItem(new Item("Sun's Edge", "O_CLASS_WEAPON", 5, "O_REQ_ONEHAND", 133, 13, 150, x, y));
            AddItem(new Item("SilverBow", "O_CLASS_RANGEDWEAPON", 3, "O_REQ_TWOHANDS", 134, 11, 100, x, y));
            AddItem(new Item("Bubble", "O_CLASS_TRINKET", 0, "O_REQ_NOTHING", 92, 0, 150, x, y));
            AddItem(new Item("Faithful", "O_CLASS_ARROWS", 0, "O_REQ_NOTHING", 142, 0, 50, x, y));
            AddItem(new Item("SunBow", "O_CLASS_SPECIAL", 3, "O_REQ_TWOHANDS", 150, 11, 150, x, y));
            AddItem(new Item("Chock", "O_CLASS_SPECIAL", 4, "O_REQ_ONEHAND", 143, 2, 150, x, y));
            AddItem(new Item("BloodTaste", "O_CLASS_WEAPON", 9, "O_REQ_TWOHANDS", 144, 19, 200, x, y));
            AddItem(new Item("SunArmour", "O_CLASS_ARMOR", 4, "O_REQ_BODY", 149, 6, 150, x, y));
            AddItem(new Item("Elven cloak", "O_CLASS_ARMOR", 1, "O_REQ_SHOULDERS", 147, 3, 50, x, y));
            AddItem(new Item("Focus", "O_CLASS_GADGET", 1, "O_REQ_HANDS", 146, 0, 150, x, y));
            AddItem(new Item("Terror", "O_CLASS_SPELL", 0, "O_REQ_ONEHAND", 153, 7, 0, x, y));
            AddItem(new Item("Berzerk", "O_CLASS_SPELL", 0, "O_REQ_ONEHAND", 152, 3, 0, x, y));
            AddItem(new Item("Protection", "O_CLASS_SPELL", 0, "O_REQ_ONEHAND", 151, 2, 0, x, y));
            AddItem(new Item("Leadball", "O_CLASS_THROWINGWEAPON", 1, "O_REQ_ONEHAND", 145, 8, 10, x, y));
            AddItem(new Item("Wooden shield", "O_CLASS_ARMOR", 2, "O_REQ_ONEHAND", 163, 3, 10, x, y));
            AddItem(new Item("Wakizashi", "O_CLASS_WEAPON", 2, "O_REQ_ONEHAND", 164, 9, 20, x, y));
            AddItem(new Item("SunHelmet", "O_CLASS_ARMOR", 2, "O_REQ_HEAD", 157, 3, 70, x, y));
            AddItem(new Item("SunShield", "O_CLASS_ARMOR", 3, "O_REQ_ONEHAND", 158, 6, 70, x, y));
            AddItem(new Item("ThunderStorm", "O_CLASS_SPELL", 0, "O_REQ_ONEHAND", 159, 25, 0, x, y));
            AddItem(new Item("HyperSpace", "O_CLASS_SPELL", 0, "O_REQ_ONEHAND", 160, 20, 0, x, y));
            AddItem(new Item("Panic", "O_CLASS_SPELL", 0, "O_REQ_ONEHAND", 161, 20, 0, x, y));
            AddItem(new Item("Shooting star", "O_CLASS_SPECIAL", 0, "O_REQ_ONEHAND", 162, 0, 300, x, y));
            AddItem(new Item("Inferno", "O_CLASS_SPELL", 0, "O_REQ_ONEHAND", 165, 12, 0, x, y));
            AddItem(new Item("Negator", "O_CLASS_TRINKET", 3, "O_REQ_NOTHING", 114, 0, 150, x, y));
            AddItem(new Item("Chain'n'ball", "O_CLASS_WEAPON", 5, "O_REQ_TWOHANDS", 170, 10, 0, x, y));
            AddItem(new Item("SunGloves", "O_CLASS_ARMOR", 1, "O_REQ_HANDS", 171, 2, 20, x, y));
            AddItem(new Item("Delay", "O_CLASS_SPELL", 0, "O_REQ_ONEHAND", 172, 2, 0, x, y));
            AddItem(new Item("Ghostblade", "O_CLASS_SPECIAL", 4, "O_REQ_ONEHAND", 140, 9, 150, x, y));
            AddItem(new Item("Enchant", "O_CLASS_SPECIAL", 0, "O_REQ_ONEHAND", 178, 0, 100, x, y));
            AddItem(new Item("Purify", "O_CLASS_SPELL", 0, "O_REQ_ONEHAND", 179, 20, 0, x, y));
            AddItem(new Item("Blue", "O_CLASS_POTION", 1, "O_REQ_ONEHAND", 180, 0, 100, x, y));
            AddItem(new Item("Green", "O_CLASS_POTION", 1, "O_REQ_ONEHAND", 180, 0, 100, x, y));
            AddItem(new Item("Yellow", "O_CLASS_POTION", 1, "O_REQ_ONEHAND", 180, 0, 100, x, y));
            AddItem(new Item("Brown", "O_CLASS_POTION", 1, "O_REQ_ONEHAND", 180, 0, 100, x, y));
            AddItem(new Item("Red", "O_CLASS_POTION", 1, "O_REQ_ONEHAND", 180, 0, 100, x, y));
            AddItem(new Item("Grey", "O_CLASS_POTION", 1, "O_REQ_ONEHAND", 180, 0, 100, x, y));
            AddItem(new Item("White", "O_CLASS_POTION", 1, "O_REQ_ONEHAND", 180, 0, 100, x, y));
            AddItem(new Item("Purple", "O_CLASS_POTION", 1, "O_REQ_ONEHAND", 180, 0, 100, x, y));
            AddItem(new Item("Black", "O_CLASS_POTION", 1, "O_REQ_ONEHAND", 180, 0, 100, x, y));
        }
      */
    }

}
