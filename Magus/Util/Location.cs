/* Created by: Zachary Meyer
 * A Util class which contains an X and Y value.
 * These values would correspond to a location on the world grid.
 */

namespace Magus.Util
{
    public class Location
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Location(int x = 0, int y = 0, Location location = null)
        {
            if(location != null)
            {
                this.X = location.X;
                this.Y = location.Y;
            }
            else
            {
                this.X = x;
                this.Y = y;
            }
            
        }

        public bool IsMatch(Location loc)
        {
            return this.X == loc.X && this.Y == loc.Y;
        }
    }
}
