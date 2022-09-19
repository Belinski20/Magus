using Magus.Entity;
using Magus.GameBoard;
using Magus.Util;
using System;

namespace Magus.UI
{
    public class TransitionButton : ButtonBase
    {
        private MenuState state;
        private GameManager g;

        public TransitionButton(Location loc1, Location loc2, MenuState state, string message, GameManager g)
        {
            this.g = g;
            TopLeft = loc1;
            BottomRight = loc2;
            this.state = state;
            Message = message;
            IsPressed = false;
            IsActive = true;
        }

        public override void Run()
        {
            Console.WriteLine("Transitioning to " + state.ToString());
            g.ChangeGameState(state);
        }
    }
}
