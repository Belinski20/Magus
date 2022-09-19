using Magus.GameBoard;
using System.Collections;
using System.Drawing;
using Magus.Util;
using System;
using System.Windows.Forms.VisualStyles;
using Magus.UI;
using Magus.Items;

/* Created by: Zachary Meyer
 * A manager class which contains a list of all characters in the game.
 * Allows for manipulating the list and getting specific characters.
 */
namespace Magus.Entity
{
    public class CharacterManager
    {
        CharacterGenerator cg;
        public Character Player { get; set; }
        private ArrayList characters;
        public CharacterManager(GameManager manager)
        {
            characters = new ArrayList();
            cg = new CharacterGenerator(manager);
        }

        //Generates the basic character
        public void CreateCharacter()
        {
            cg.SpawnPlayer(characters);
        }

        //Generates characters for the game
        public void GenerateCharacters()
        {
            cg.SpawnCharacters(characters);
        }

        //Finds a specific character based on the x, y coordinate in which it stands
        public Character FindCharacter(int x, int y)
        {
            foreach (Character character in characters)
            {
                if (character.Location.X == x && character.Location.Y == y)
                    return character;
            }
            return null;
        }

        public bool LocationContainsCharacter(Location loc)
        {
            if (Player.Location.X == loc.X && Player.Location.Y == loc.Y)
                return true;
            foreach (Character character in characters)
            {
                if (character.Location.X == loc.X && character.Location.Y == loc.Y)
                    return true;
            }
            return false;
        }

        //Removes the Opponent that the character has
        public void CancelOpponent(Character target)
        {
            foreach (Character character in characters)
            {
                if (character.Opponent == target)
                    character.Opponent = null;
            }
        }

        //Removes the character from the game character list
        public void DisposeCharacter(Character character)
        {
            characters.Remove(character);
        }

        public void DisposeAllCharacters()
        {
            characters.Clear();
        }

        public Image GetCharacterImage(int x, int y)
        {
            foreach (Character c in characters)
            {
                if (c.Location.X == x && c.Location.Y == y)
                {
                    return c.Icon;
                }
            }
            return null;
        }

      public bool RemovePoint(Stats stat)
        {
            switch (stat)
            {
                case Stats.Health:
                    if (Player.HealthStat > 10)
                    {
                        Player.HealthStat -= 10;
                        return true;
                    }
                    return false;
                case Stats.Mana:
                    if (Player.ManaStat > 0)
                    {
                        Player.ManaStat -= 10;
                        return true;
                    }
                    return false;
                case Stats.Strength:
                    if (Player.Strength > 0)
                    {
                        Player.Strength -= 1;
                        return true;
                    }
                    return false;
                case Stats.Agility:
                    if (Player.Agility > 0)
                    {
                        Player.Agility -= 1;
                        return true;
                    }
                    return false;
                case Stats.Wisdom:
                    if (Player.Wisdom > 0)
                    {
                        Player.Wisdom -= 1;
                        return true;
                    }
                    return false;
                default:
                    return false;
            }
        }

        public bool AddPoint(Stats stat, int points)
        {
            switch (stat)
            {
                case Stats.Health:
                    if (points > 0)
                    {
                        Player.HealthStat += 10;
                        return true;
                    }
                    return false;
                case Stats.Mana:
                    if (points > 0)
                    {
                        Player.ManaStat += 10;
                        return true;
                    }
                    return false;
                case Stats.Strength:
                    if (points > 0)
                    {
                        Player.Strength += 1;
                        return true;
                    }
                    return false;
                case Stats.Agility:
                    if (points > 0)
                    {
                        Player.Agility += 1;
                        return true;
                    }
                    return false;
                case Stats.Wisdom:
                    if (points > 0)
                    {
                        Player.Wisdom += 1;
                        return true;
                    }
                    return false;
                default:
                    return false;
            }
        }

        public void CreateGenericPlayer()
        {
            Player = new Character(Constants.PLAYER_SPAWN_X, Constants.PLAYER_SPAWN_Y);
            Player.CharacterClass = (ClassTypes)0;
            Player.IsPlayer = true;
            Player.Name = Player.CharacterClass.ToString();
            Update();
        }

        public void UpdateStats()
        {
            Player.HP = Player.HealthStat;
            Player.Mana = Player.ManaStat;
        }

        public void Update()
        {
            Player.Speed = StatUtil.GetCharacterStatsByKey(Player.CharacterClass, StatConstants.playerSpeedMap);
            Player.Strength = StatUtil.GetCharacterStatsByKey(Player.CharacterClass, StatConstants.playerStrengthMap);
            Player.Skill = StatUtil.GetCharacterStatsByKey(Player.CharacterClass, StatConstants.playerSkillMap);
            Player.HealthStat = StatUtil.GetCharacterStatsByKey(Player.CharacterClass, StatConstants.playerHealthMap);
            Player.Wisdom = StatUtil.GetCharacterStatsByKey(Player.CharacterClass, StatConstants.playerWisdomMap);
            Player.ManaStat = StatUtil.GetCharacterStatsByKey(Player.CharacterClass, StatConstants.playerPowerMap);
            Player.MP = Player.MaxMana = Player.ManaStat;
            Player.MaxHp = Player.HP = Player.HealthStat;
            Player.Level = LevelRanks.Nobody;
            Player.Name = Player.CharacterClass.ToString();
            Player.Moves = Player.Speed;
            if(Player.HP < Player.MaxHp)
            {
                Player.HP++;
                Player.HP += Math.Min(Player.Moves, Player.Speed);
            }
            if (!Player.isFemale)
            {
                Player.Icon = StatUtil.GetCharacterImageByKey(Player.CharacterClass, StatConstants.playerIconMapMale);
            }
            else
            {
                Player.Icon = StatUtil.GetCharacterImageByKey(Player.CharacterClass, StatConstants.playerIconMapFemale);
            }
        }

        public void RunAITurn(AI ai)
        {
            ai.RunAITurn(Player);
        }

        public void ResetMoveCount()
        {
            Player.Moves = Player.Speed;
            foreach (Character c in characters)
            {
                c.Moves = c.Speed;
            }
        }
    }
}

