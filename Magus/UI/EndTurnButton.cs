using Magus.GameBoard;
using Magus.Util;
using System;

namespace Magus.UI
{
    public class EndTurnButton : ButtonBase
    {
        private GameManager manager;
        public EndTurnButton(Location loc1, Location loc2, string message, GameManager manager)
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
            //manager.runAITurn
            //reset player moves
            Console.WriteLine("End Turn Button Hit");
            manager.ChangeTurnState(TurnState.enemy);
        }
    }
}
