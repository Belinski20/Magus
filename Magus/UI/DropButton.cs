using Magus.Util;
using System;

namespace Magus.UI
{
    public class DropButton : ButtonBase 
    {
        public DropButton(Location loc1, Location loc2, string message)
        {
            TopLeft = loc1;
            BottomRight = loc2;
            Message = message;
            IsPressed = false;
            IsActive = true;
        }

        public override void Run()
        {
            Console.WriteLine("Drop Button Hit");
        }
    }
}
