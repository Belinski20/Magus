using Magus.Util;
using System;
using System.Windows.Forms;

namespace Magus.UI
{
    public abstract class ButtonBase
    {
        public string Message { get; set; }
        public Location TopLeft { get; set; }
        public Location BottomRight { get; set; }
        public bool IsPressed { get; set; }
        public bool IsActive { get; set; }

        public bool IsClicked(Location clickedLoc)
        {
            if (TopLeft.X <= clickedLoc.X && clickedLoc.X <= BottomRight.X)
               if (TopLeft.Y <= clickedLoc.Y && clickedLoc.Y <= BottomRight.Y)
               {
                   IsPressed = true;
                   return true;
               }

            return false;
        }

        public virtual void Run()
        {
            Console.WriteLine("Base Button Click");
        }
    }
}
