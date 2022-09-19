using Magus.Entity;
using Magus.GameBoard;
using Magus.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magus.UI
{
    public class MinusButton : ButtonBase
    {
        Stats stat;
        GameManager manager;
        public MinusButton(Location loc1, Location loc2, Stats stat, GameManager manager)
        {
            TopLeft = loc1;
            BottomRight = loc2;
            Message = "-";
            IsPressed = false;
            IsActive = true;
            this.stat = stat;
            this.manager = manager;
        }

        public override void Run()
        {
            Console.WriteLine("Minus Button Hit");
            if (manager.GetCharacterManager().RemovePoint(stat))
                manager.GetUI().SkillPoints += 1;
            manager.GetUI().ModifyCharacter();
            manager.GetCharacterManager().UpdateStats();
        }
    }
}
