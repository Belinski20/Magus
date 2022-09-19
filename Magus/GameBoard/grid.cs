using Magus.Util;
using System;

namespace Magus.GameBoard
{
    public class Grid
    {

        private GridSquare[,] world;
        public Grid()
        {
            world = new GridSquare[Constants.GAMEBOARD_SIZE_X, Constants.GAMEBOARD_SIZE_Y];
        }

        public void CreateWorld()
        {
            for (int x = 0; x < Constants.GAMEBOARD_SIZE_X; x++)
                for (int y = 0; y < Constants.GAMEBOARD_SIZE_Y; y++)
                {
                    world[x, y] = new GridSquare(x, y);
                }
        }

        public GridSquare GetGridTile(int x, int y)
        {
            return world[x, y];
        }

        public GridSquare GetGridTile(Location location)

        { 
                return world[location.X, location.Y];
        }

    }
}
