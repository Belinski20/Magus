using Magus.GameBoard;
using Magus.Util;
using System;

namespace Magus.UI
{
    public class NextClassButton : ButtonBase
    {
        GameManager manager;
        public NextClassButton(Location loc1, Location loc2, string message, GameManager manager)
        {
            TopLeft = loc1;
            BottomRight = loc2;
            Message = message;
            IsPressed = false;
            IsActive = true;
            this.manager = manager;
        }

        public override void Run()
        {
            manager.GetNextClass();
            Console.WriteLine("Next Class Button Hit");
        }
    }
}
