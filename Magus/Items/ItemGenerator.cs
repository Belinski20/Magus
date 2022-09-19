using Magus.GameBoard;
using Magus.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magus.Items
{
    class ItemGenerator
    {
        Random rng = new Random();
        GameManager manager;

        public ItemGenerator(GameManager manager)
        {
            this.manager = manager;
        }

      public ArrayList SpawnItems()
      {

         ArrayList iList = new ArrayList();
         int count = 0;
          int spawn_x_coordinate = 0;
          int spawn_y_coordinate = 0;
          while (count < Constants.ItemSpawnCount)
          {
              spawn_x_coordinate = rng.Next(0, Constants.GAMEBOARD_SIZE_X);
              spawn_y_coordinate = rng.Next(0, Constants.GAMEBOARD_SIZE_Y);
              if (!manager.IsTileBlocked(spawn_x_coordinate, spawn_y_coordinate) && manager.IsTileWoodenOrStone(spawn_x_coordinate, spawn_y_coordinate))
              {
               ItemTypes itemTypes = (ItemTypes) rng.Next(0, 87);
                  Item i = new Item();
                  i.X = spawn_x_coordinate;
                  i.Y = spawn_y_coordinate;
                  i.Name = itemTypes;
                  i.Requires = StatUtil.GetCharacterStatsByKey(itemTypes, StatConstants.RequiresMap);
                  i.Icon = StatUtil.GetCharacterImageByKey(itemTypes, StatConstants.itemIconMap);
                  iList.Add(i);
                  count++;
              }
          }
         return iList;
      }

     


   }
}
