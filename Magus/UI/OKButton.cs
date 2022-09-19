using Magus.Entity;
using Magus.Util;
using System;

namespace Magus.UI
{
    class OKButton : ButtonBase
    {
        UIComponents ui;
        public OKButton(Location loc1, Location loc2, string message, UIComponents ui)
        {
            TopLeft = loc1;
            BottomRight = loc2;
            Message = message;
            IsPressed = false;
            this.ui = ui;
            IsActive = true;
        }

        public override void Run()
        {
            Console.WriteLine("OK Button Hit");
            ui.HideUI();
            ui.ModifyCharacter();
        }
    }
}
