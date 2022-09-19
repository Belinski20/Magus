using Magus.GameBoard;
using Magus.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magus.Entity
{
    class CharacterGenerator
    {
        Random rng = new Random();
        GameManager manager;

        public CharacterGenerator (GameManager manager)
        {
            this.manager = manager;
        }
        public void GenerateCharacters()
        {
            SpawnDemonPrinces();
            SpawnDarkOne();
        }

        private void SpawnDarkOne()
        {
            Character c = new Character();
            EnemyTypes char_class = EnemyTypes.TheDarkOne;
            c = new Character();
            c.IsNasty = true;
            c.Location.X = Constants.DARK_ONE_SPAWN_X;
            c.Location.Y = Constants.DARK_ONE_SPAWN_Y;
            c.EnemyType = char_class;
            c.Speed = StatUtil.GetByKey((EnemyTypes)char_class, StatConstants.speedMap);
            c.Strength = StatUtil.GetByKey((EnemyTypes)char_class, StatConstants.strengthMap);
            c.HealthStat = StatUtil.GetByKey((EnemyTypes)char_class, StatConstants.healthMap);
            c.Skill = StatUtil.GetByKey((EnemyTypes)char_class, StatConstants.skillMap);
            c.Wisdom = StatUtil.GetByKey((EnemyTypes)char_class, StatConstants.wisdomMap);
            c.IsFooled = false;
            c.Icon = StatUtil.GetCharacterImageByKey((EnemyTypes)char_class, StatConstants.iconMap);
            c.MP = c.MaxMana = c.ManaStat;
            c.MaxHp = c.HP = c.HealthStat;
            c.Moves = c.Speed;
        }

        private void SpawnDemonPrinces()
        {
            Character c;
            for (int i = 0; i < 3; i++)
            {
                EnemyTypes char_class = EnemyTypes.DemonPrince;
                c = new Character();
                c.IsNasty = true;
                c.Location.X = Constants.DEMON_PRINCE_SPAWN_X + i;
                c.Location.Y = Constants.DEMON_PRINCE_SPAWN_Y;
                c.EnemyType = char_class;
                c.Speed = StatUtil.GetByKey((EnemyTypes)char_class, StatConstants.speedMap);
                c.Strength = StatUtil.GetByKey((EnemyTypes)char_class, StatConstants.strengthMap);
                c.HealthStat = StatUtil.GetByKey((EnemyTypes)char_class, StatConstants.healthMap);
                c.Skill = StatUtil.GetByKey((EnemyTypes)char_class, StatConstants.skillMap);
                c.Wisdom = StatUtil.GetByKey((EnemyTypes)char_class, StatConstants.wisdomMap);
                c.IsFooled = false;
                c.Icon = StatUtil.GetCharacterImageByKey((EnemyTypes)char_class, StatConstants.iconMap);
                c.MP = c.MaxMana = c.ManaStat;
                c.MaxHp = c.HP = c.HealthStat;
                c.Moves = c.Speed;
            }
        }

        public void SpawnCharacters(ArrayList characters)
        {
            int count = 0;
            int spawn_x_coordinate = 0;
            int spawn_y_coordinate = 0;
            while (count < Constants.NUM_BAD_GUYS)
            {
                spawn_x_coordinate = rng.Next(0, Constants.GAMEBOARD_SIZE_X);
                spawn_y_coordinate = rng.Next(0, Constants.GAMEBOARD_SIZE_Y);
                if (!manager.IsTileBlocked(spawn_x_coordinate, spawn_y_coordinate))
                {
                    EnemyTypes char_class = GetClass(spawn_x_coordinate, spawn_y_coordinate);
                    int char_rank = StatUtil.GetByKey((EnemyTypes)char_class, StatConstants.rankMap);
                    Character c = new Character();
                    c.IsNasty = true;
                    c.Location.X = spawn_x_coordinate;
                    c.Location.Y = spawn_y_coordinate;
                    c.EnemyType = char_class;
                    c.Speed = StatUtil.GetByKey((EnemyTypes)char_class, StatConstants.speedMap);
                    c.Strength = StatUtil.GetByKey((EnemyTypes)char_class, StatConstants.strengthMap);
                    c.HealthStat = StatUtil.GetByKey((EnemyTypes)char_class, StatConstants.healthMap);
                    c.Skill = StatUtil.GetByKey((EnemyTypes)char_class, StatConstants.skillMap);
                    c.Wisdom = StatUtil.GetByKey((EnemyTypes)char_class, StatConstants.wisdomMap);
                    c.IsFooled = false;
                    c.Icon = StatUtil.GetCharacterImageByKey((EnemyTypes)char_class, StatConstants.iconMap);
                    c.Moves = c.Speed;
                    characters.Add(c);
                    //manager.SetBlocked(spawn_x_coordinate, spawn_y_coordinate);
                    Console.WriteLine("Made Character #" + count);
                    Console.WriteLine("Character Type: " + char_class);
                    Console.WriteLine("Rank: " + char_rank);
                    Console.WriteLine("Location: " + c.Location.X + ", " + c.Location.Y + "\n");
                    count++;
                }
            }
        }

        private EnemyTypes GetClass(int x, int y)
        {
            int distance = distanceFromSpawn(x, y);
            ArrayList ValidRankList = GetValidSpawnList(distance);
            ArrayList ValidEnemyList = GetValidEnemyList(ValidRankList);
            if (ValidEnemyList.Count == 0)
                return EnemyTypes.SmallOne;
            int random = rng.Next(0, ValidEnemyList.Count);   
            Console.WriteLine("Random Number: " + random);
            return (EnemyTypes)ValidEnemyList[random];
        }

        private int distanceFromSpawn(int enemy_x, int enemy_y)
        {
            int xDistance = Math.Abs(enemy_x - Constants.PLAYER_SPAWN_X);
            int yDistance = Math.Abs(enemy_y - Constants.PLAYER_SPAWN_Y);

            //pythagorean theorem to find distance
            //cast to an int for ease of use
            int spawnDistance = Math.Max(xDistance, yDistance);
            return spawnDistance;

        }

        public void SpawnPlayer(ArrayList characters)
        {
            //CharacterGenerator cg = new CharacterGenerator();
            //Random rng = new Random();
            //int count = 0;
            //Character player = new Character(Constants.PLAYER_SPAWN_X, Constants.PLAYER_SPAWN_Y);
            //player.IsPlayer = true;
            //characters.Add(player);
        }

        private bool IsValidSpawn(int rank, int distanceFromSpawn)
        {
            return rank <= distanceFromSpawn && rank + 50 >= distanceFromSpawn;
        }

        private ArrayList GetValidSpawnList(int distanceFromSpawn)
        {
            ArrayList validSpawnList = new ArrayList();
            foreach (int rank in Constants.rankList)
            {
                if(IsValidSpawn(rank, distanceFromSpawn) && rank != -1)
                {
                    validSpawnList.Add(rank);
                }
            }
            return validSpawnList;
        }

        private ArrayList GetValidEnemyList(ArrayList validSpawnList)
        {
            ArrayList validEnemyList = new ArrayList();
            foreach(int rank in validSpawnList)
            {
                foreach(KeyValuePair <EnemyTypes, int> pair in StatConstants.rankMap)
                {
                    if(pair.Value == rank)
                        validEnemyList.Add(pair.Key);
                }
                
            }

            return validEnemyList;
        }


    }
}
