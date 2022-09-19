using Magus.Entity;
using Magus.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magus.Util
{
    class StatUtil
    {
        //will fix when branches are merged
        public static ArrayList getEnemyStats(EnemyTypes type)
        {
            ArrayList statList = new ArrayList();
            statList.Add(GetByKey(type, StatConstants.speedMap));
            statList.Add(GetByKey(type, StatConstants.strengthMap));
            statList.Add(GetByKey(type, StatConstants.wisdomMap));
            statList.Add(GetByKey(type, StatConstants.skillMap));
            statList.Add(GetByKey(type, StatConstants.healthMap));
            statList.Add(GetByKey(type, StatConstants.powerMap));
            return statList;
        }

        public static int GetByKey(EnemyTypes type, SortedDictionary<EnemyTypes, int> map)
        {
            foreach (KeyValuePair < EnemyTypes, int> pair in map )
            {
                if(pair.Key.Equals(type))
                {
                    return pair.Value;
                }
            }
            return -1;
        }
        public static EnemyTypes GetByValue(int rank, SortedDictionary<EnemyTypes, int> map)
        {
            foreach (KeyValuePair<EnemyTypes, int> pair in map)
            {
                if (pair.Value.Equals(rank))
                {
                    return pair.Key;
                }
            }
            return (EnemyTypes)0;
        }
        public static int GetCharacterStatsByKey<T>(T type, SortedDictionary<T, int> map)
        {
            foreach (KeyValuePair<T, int> pair in map)
            {
                if (pair.Key.Equals(type))
                {
                    return pair.Value;
                }
            }
            return -1;
        }
         public static ItemReq GetCharacterStatsByKey<T>(T type, SortedDictionary<T, ItemReq> map)
         {
         foreach (KeyValuePair<T, ItemReq> pair in map)
         {
            if (pair.Key.Equals(type))
            {
               return pair.Value;
            }
         }
         return 0f;
         }

      public static Image GetCharacterImageByKey<T>(T type, SortedDictionary<T, Image> map)
        {
            foreach (KeyValuePair<T, Image> pair in map)
            {
                if (pair.Key.Equals(type))
                {
                    return pair.Value;
                }
            }
            return null;
        }

    }
}
