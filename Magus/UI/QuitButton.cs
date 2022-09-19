using Magus.Util;
using System;
using System.Windows.Forms;

namespace Magus.UI
{
    public class QuitButton : ButtonBase
    {
        private Form form;
        public QuitButton(Location loc1, Location loc2, string message, Form form)
        {
            TopLeft = loc1;
            BottomRight = loc2;
            Message = message;
            IsPressed = false;
            this.form = form;
            IsActive = true;
        }

        public override void Run()
        {
            Console.WriteLine("Quit Button Hit");
            form.Close();
        }
    }
}
