using Magus.Util;
using System.Drawing;

/* Created By: Zachary Meyer
 * A class which is used to create every entity character in the game
 */

namespace Magus.Entity
{
    public class Character
    {
        public int skill { get; set; }
        public string Name { get; set; }
        //
        public Image Icon { get; set; }
        public LevelRanks Level { get; set; }
        public int XP { get; set; }
        public int Moves { get; set; }
        public int Strength { get; set; }
        public int Skill { get; set; }
        public int Wisdom { get; set; }
        public int Load { get; set; }
        public int HP { get; set; }
        public int HealthStat { get; set; }
        public int MaxHp { get; set; }
        public int ManaStat { get; set; }
        public int MP { get; set; }
        public int MaxMana { get; set; }
        public int Mana { get; set; }
        public int Agility { get; set; }
        public int Speed { get; set;}
        public ClassTypes CharacterClass { get; set; }
        public EnemyTypes EnemyType { get; set; }
        public bool IsFooled { get; set; }
        public bool IsConfused { get; set; }
        //
        public bool IsPlayer { get; set; }
        public bool IsNasty { get; set; }
        public bool isFemale { get; set; }
        public Location Location { get; set; }
        public Character Opponent { get; set; }
        public Character(int x = 0, int y = 0)
        {
            Location = new Location(x, y);
        }
    }
}
