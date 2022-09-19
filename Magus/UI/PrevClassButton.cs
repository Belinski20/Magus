using Magus.GameBoard;
using Magus.Util;
using System;

namespace Magus.UI
{
    public class PrevClassButton : ButtonBase
    {
        GameManager manager;
        public PrevClassButton(Location loc1, Location loc2, string message, GameManager manager)
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
            manager.GetPreviousClass();
            Console.WriteLine("Prev Class Button Hit");
        }
    }
}
