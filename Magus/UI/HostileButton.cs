
using Magus.Entity;
using Magus.Util;
using System;

namespace Magus.UI
{
    public class HostileButton : ButtonBase
    {
        private UIComponents uic;
        private Character pc;
        private Character c;
        public HostileButton(Location loc1, Location loc2, string message, UIComponents uic, Character pc, Character c)
        {
            TopLeft = loc1;
            BottomRight = loc2;
            Message = message;
            IsPressed = false;
            IsActive = true;
            this.uic = uic;
            this.pc = pc;
            this.c = c;
        }

        public override void Run()
        {
            Random rnd = new Random();
            Console.WriteLine("Hostile Button Hit");
            switch(Message)
            {
                case "Push":
                    if (pc.Moves == 0)
                        return;
                    //Add Push Method
                    break;
                case "Booh!":
                    if (pc.Moves == 0)
                        return;
                    
                    if(true) // if(!c.isFooled && Rand(10) < 2)
                    {
                        //c.IsFleeing = 17;
                        //Message(c.Class + " looks scared.");
                    }
                    //else
                    //Message("Ha ha ha!");
                    //c.IsFooled = true;
                    pc.Moves--;
                    break;
                case "!#$%&?*!!":
                    if (pc.Moves == 0)
                        return;
                    
                    if (!c.IsFooled && rnd.Next(10) < 2)
                    {
                        c.IsConfused = true;
                        uic.Message(c.EnemyType + " scratches its head");
                    }
                    else
                    {
                        uic.Message(c.EnemyType + ": #$%&?*! yourself!");
                    }
                    c.IsFooled = true;
                    pc.Moves--;
                    break;
                case "Cancel":
                    uic.RemoveButtonFromInteraction(4);
                    uic.BringGameForward();
                    uic.SetInteractionState(false);
                    uic.ResetInteractionPanel();
                    uic.EnableDisableButtons(true);
                    break;
                default:
                    break;
            }
            uic.DrawStatusDisplay();
        }
    }
}
