using System;
using System.Windows.Forms;
using Magus.GameBoard;

namespace Magus
{
    public partial class Form1 : Form
    {
        private GameManager board;
        public Form1()
        {
            InitializeComponent();
            board = new GameManager(this);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            board.ChangeGameState(UI.MenuState.Splash);
        }
    }
}
