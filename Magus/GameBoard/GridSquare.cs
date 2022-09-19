using Magus.Util;

namespace Magus.GameBoard
{
    public class GridSquare
    {
        public Location Location { get; set; }
        public int ForeGround { get; set;}
        public int BackGround { get; set; }
        public int Overlay { get; set; }
        public bool IsBlocked { get; set; }
        public int Flag { get; set; }
        public bool IsWall { get; set; }
        public bool IsDoor { get; set; }

        public GridSquare(int x = 0, int y = 0)
        {
            Location = new Location(x, y);
        }
    }
}
