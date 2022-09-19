using Magus.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Magus.UI
{
    public class NoButton : ButtonBase
    {
        Form form;
        public NoButton(Location loc1, Location loc2, string message, Form form)
        {
            TopLeft = loc1;
            BottomRight = loc2;
            Message = message;
            IsPressed = false;
            IsActive = true;
            this.form = form;
        }

        public override void Run()
        {
            Console.WriteLine("No Button Hit");
            form.Close();
        }
    }
}
