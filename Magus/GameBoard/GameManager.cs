using Magus.Entity;
using Magus.Util;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using Magus.UI;
using Magus.Items;
using System.Security.Policy;

namespace Magus.GameBoard
{
    public class GameManager
    {
        //Main Form
        private Form form;

        //Game Components
        private Panel camera;
        private PictureBox[,] cameraView;
        private Grid gameBoard;

        //Custom UI
        private UIComponents UIC;

        //Character Manager
        private CharacterManager cManager;

        //Item Manager
        private ItemManager iManager;

        //Game State
        private MenuState menuState;

        //Turn State
        private TurnState turnState;

        //Enemy AI
        private AI ai;

        public GameManager(Form form)
        {
            this.form = form;
            UIC = new UIComponents(form, this);
            cManager = new CharacterManager(this);
            iManager = new ItemManager(this);
            form.MouseDown += new MouseEventHandler(UIDownClick);
            form.MouseUp += new MouseEventHandler(UIUPClick);
        }

        //-----------------------------------------------------
        //Game State Changer
        //-----------------------------------------------------
        public void ChangeGameState(MenuState state)
        {
            UIC.ClearButtons();
            form.Controls.Clear();
            form.CreateGraphics().Clear(Color.Black);
            switch (state)
            {
                case MenuState.Splash:
                    menuState = state;
                    SetSplash();
                    break;
                case MenuState.MainMenu:
                    menuState = state;
                    SetMainMenu();
                    break;
                case MenuState.Game:
                    menuState = state;
                    SetGame();
                    break;
                case MenuState.Death:
                    menuState = state;
                    SetDeath();
                    break;
                case MenuState.HallOfFame:
                    menuState = state;
                    SetHallOfFame();
                    break;
                case MenuState.TryAgain:
                    menuState = state;
                    SetTryAgain();
                    break;
                default:
                    break;
            }
        }

        //-----------------------------------------------------
        //Splash Screen
        //-----------------------------------------------------
        private void SetSplash()
        {
            Splash();
        }

        //Creates UI Components for the splash screen and adds a panel overlay to capture events
        private void Splash()
        {
            UIC.SplashScreen();
            form.Controls.Add(CreateScreenPanel());
            cManager.CreateGenericPlayer();
        }

        //Creates a transparent panel overlay over the form to capture mouse events
        private Panel CreateScreenPanel()
        {
            Panel panel = new Panel();
            panel.Size = form.Size;
            panel.BackColor = Color.Transparent;
            panel.MouseDown += new MouseEventHandler(Splash_Click);
            return panel;
        }

        //Mouse event from Splash Screen
        private void Splash_Click(object sender, EventArgs e)
        {
            form.MouseDown -= new MouseEventHandler(Splash_Click);
            ChangeGameState(MenuState.MainMenu);
        }

        //-----------------------------------------------------
        //Main Menu Screen
        //-----------------------------------------------------
        private void SetMainMenu()
        {
            UIC.SelectionScreen();
        }

        //-----------------------------------------------------
        //Game Screen
        //-----------------------------------------------------
        private void SetGame()
        {
            UIC.SetIconClickEvent(false);
            StartGame();
        }

        //Creates and loads game information to start playing
        private void StartGame()
        {
            turnState = TurnState.player;
            CreateCamera();
            gameBoard = new Grid();
            gameBoard.CreateWorld();
            ReadWorldFile();
            cameraView = new PictureBox[Constants.CAMERA_VIEW_SIZE, Constants.CAMERA_VIEW_SIZE];
            iManager.GenerateItems();
            cManager.GenerateCharacters();
            FillArr();
            SetCamera();
            ai = new AI(this);
            DrawPlayArea();
            UIC.GameButtons();
            camera.Parent.KeyDown += new KeyEventHandler(this.form_KeyDown);
        }

        //Creates the camera game panel
        public void CreateCamera()
        {
            camera = new Panel();
            camera.Size = new Size(Constants.CAMERA_PANEL_WIDTH, Constants.CAMERA_PANEL_HEIGHT);
            camera.Location = new Point(Constants.CAMERA_OFFSET_X, 0);
            camera.BackColor = Color.Black;
            form.Controls.Add(camera);
        }

        //Creates the UI for the game
        private void DrawPlayArea()
        {
            UIC.DrawStoneBox(0, 0, 159, 410);
            UIC.DrawStoneBox(0, 400, 620, 79);
            UIC.FillRect(Constants.GREY, new Location(3, 400), 154, 10);
            UIC.StatusPanel();
            UIC.Status();
            UIC.StatusBar();
        }

        //-----------------------------------------------------
        //Death Screen
        //-----------------------------------------------------
        private void SetDeath()
        {
            StartDeath();
        }

        private void StartDeath()
        {
            cManager.DisposeAllCharacters();
            UIC.MagusScreen();
        }

        //-----------------------------------------------------
        // Hall Of Fame
        //-----------------------------------------------------

        private void SetHallOfFame()
        {
            UIC.HallOfFame();
        }

        //-----------------------------------------------------
        // Try Again
        //-----------------------------------------------------

        private void SetTryAgain()
        {
            UIC.TryAgainScreen();
        }

        //-----------------------------------------------------
        // End UI Screens
        //-----------------------------------------------------

        public void UIDownClick(object sender, MouseEventArgs e)
        {
            UIC.ClickDownUI(new Location(e.X, e.Y));
        }

        public void UIUPClick(object sender, MouseEventArgs e)
        {
            UIC.ClickUPUI(new Location(e.X, e.Y));
        }

        //TODO make sure items don't spawn on the player
        public void SpawnItems()
        {
            Random rng = new Random();
            int count = 0;
            while (count < 5000)
            {
                int x_coordinate = rng.Next(1, Constants.GAMEBOARD_SIZE_X - 1);
                int y_coordinate = rng.Next(1, Constants.GAMEBOARD_SIZE_Y - 1);
                if (!gameBoard.GetGridTile(x_coordinate, y_coordinate).IsBlocked)
                {
                    iManager.PlaceItem(x_coordinate, y_coordinate);
                    count++;
                }
            }
        }
        private void form_KeyDown(object o, KeyEventArgs e)
        {
            Console.WriteLine(e.KeyCode);
            Location l = Direction(e);
            l.X += cManager.Player.Location.X;
            l.Y += cManager.Player.Location.Y;
            if (gameBoard.GetGridTile(l).IsWall)
                return;
            //MovePlayerToLocation(l);
            //Console.WriteLine("X: " + l.X + " Y: " + l.Y);
            if(cManager.Player.Moves > 0)
            {
                UIC.DrawStatusDisplay();
                ai.GoTowards(cManager.Player, l.X, l.Y);
                SetCamera();
            }
        }

        //Sets the camera view to having the player in the middle 
        public void SetCamera()
        {
            Image characterimage = null;
            Image itemImage = null;
            int cameraBoundX = cManager.Player.Location.X - (Constants.CAMERA_VIEW_SIZE / 2);
            int cameraBoundY = cManager.Player.Location.Y - (Constants.CAMERA_VIEW_SIZE / 2);

            for (int x = 0; x < Constants.CAMERA_VIEW_SIZE; x++)
            {
                for (int y = 0; y < Constants.CAMERA_VIEW_SIZE; y++)
                {
                    if (cameraBoundX + x < 0 || cameraBoundX + x > Constants.GAMEBOARD_SIZE_X - 1 || cameraBoundY + y < 0 || cameraBoundY + y > Constants.GAMEBOARD_SIZE_Y - 1)
                        cameraView[x, y].Image = IdToPicture(-1);
                    else
                        cameraView[x, y].Image = IdToPicture(gameBoard.GetGridTile(cameraBoundX + x, cameraBoundY + y).BackGround);
                    itemImage = iManager.GetItemImage(cameraBoundX + x, cameraBoundY + y);
                    if(itemImage != null)
                    {
                        cameraView[x, y].Image = itemImage;
                    }
                    characterimage = cManager.GetCharacterImage(cameraBoundX + x, cameraBoundY + y);
                    if (characterimage != null)
                    {
                        cameraView[x, y].Image = characterimage;
                    }
                    cameraView[Constants.CAMERA_VIEW_SIZE / 2, Constants.CAMERA_VIEW_SIZE / 2].Image = cManager.Player.Icon;
                    cameraView[x, y].Update();
                }
            }
            camera.Update();
        }

        //Sets each cell to the correct texture which was collected from WORLD.MGS
        private Bitmap IdToPicture(int id)
        {
            switch (id)
            {
                case (int)TERRAIN.NotVisible:
                    return Properties.Resources.NotVisible;
                case (int)TERRAIN.StoneFloor:
                    return Properties.Resources.StonePavement;
                case (int)TERRAIN.Earth:
                    return Properties.Resources.Earth;
                case (int)TERRAIN.YellowFlower:
                    return Properties.Resources.YellowFlower;
                case (int)TERRAIN.Gravel:
                    return Properties.Resources.Gravel;
                case (int)TERRAIN.WoodFloor:
                    return Properties.Resources.WoodFloor;
                case (int)TERRAIN.Water:
                    return Properties.Resources.Water;
                case (int)TERRAIN.CobblestonePavement:
                    return Properties.Resources.CobblestonePavement;
                case (int)TERRAIN.StoneWall:
                    return Properties.Resources.StoneWall;
                case (int)TERRAIN.BrickWall:
                    return Properties.Resources.BrickWall;
                case (int)TERRAIN.Grass:
                    return Properties.Resources.Grass;
                case (int)TERRAIN.Sand:
                    return Properties.Resources.Sand;
                case (int)TERRAIN.WoodWall:
                    return Properties.Resources.WoodWall;
                case (int)TERRAIN.Tree:
                    return Properties.Resources.Tree;
                case (int)TERRAIN.Planks:
                    return Properties.Resources.Planks;
                case (int)TERRAIN.Bush:
                    return Properties.Resources.Bush;
                case (int)TERRAIN.Gate:
                    return Properties.Resources.Gate;
                case (int)TERRAIN.Mountain:
                    return Properties.Resources.Mountain;
                case (int)TERRAIN.Portal:
                    return Properties.Resources.Portal;
                case (int)TERRAIN.Marsh:
                    return Properties.Resources.Marsh;
                case (int)TERRAIN.StonesInWater:
                    return Properties.Resources.StonesInWater;
                case (int)TERRAIN.RedFlower:
                    return Properties.Resources.RedFlower;
                case (int)TERRAIN.PalmTree:
                    return Properties.Resources.PalmTree;
                case (int)TERRAIN.Door:
                    return Properties.Resources.Door;
            }
            return null;
        }

        //Fills the cameraView with picture boxes to be manipulated with images
        private void FillArr()
        {
            for (int x = 0; x < Constants.CAMERA_VIEW_SIZE; x++)
            {
                for (int y = 0; y < Constants.CAMERA_VIEW_SIZE; y++)
                {
                    int width = camera.Width / Constants.CAMERA_VIEW_SIZE;
                    int height = camera.Height / Constants.CAMERA_VIEW_SIZE;
                    cameraView[x, y] = new PictureBox();
                    cameraView[x, y].Size = new Size(width, height);
                    cameraView[x, y].Location = new Point(x * width, y * height);
                    cameraView[x, y].MouseClick += new MouseEventHandler(Cell_Click);
                    camera.Controls.Add(cameraView[x, y]);
                }
            }
        }

        //Event which runs when clicking on a cell in the camera view
        private void Cell_Click(object sender, MouseEventArgs e)
        {
            if (UIC.GetInteractionState())
                return;
            PictureBox cell = (PictureBox)sender;
            for (int x = 0; x < Constants.CAMERA_VIEW_SIZE; x++)
            {
                for (int y = 0; y < Constants.CAMERA_VIEW_SIZE; y++)
                {
                    if (cameraView[x, y].Equals(cell))
                    {
                        //Console.WriteLine("Cell Clicked, Camera Cell Position: " + x + "," + y); //For Debugging gives the camera position clicked
                        Location worldLocation = GetCellWorldPositionFromCamera(x, y);
                        Location newWorldPosition = MouseDirection(worldLocation);
                        if(cManager.Player.Moves > 0)
                        {
                            if (cManager.LocationContainsCharacter(newWorldPosition))
                            {
                                if (!cManager.Player.Location.IsMatch(newWorldPosition))
                                {
                                    if (e.Button.Equals(MouseButtons.Right))
                                    {
                                        cManager.Player.Opponent = cManager.FindCharacter(newWorldPosition.X, newWorldPosition.Y);
                                        ai.Combat(cManager.Player);
                                    }
                                    if (e.Button.Equals(MouseButtons.Left))
                                    {

                                        UIC.SetInteractionState(true);
                                        UIC.HostileInteration(cManager.FindCharacter(newWorldPosition.X, newWorldPosition.Y));
                                    }
                                }
                            }
                            else
                            {
                                ai.GoTowards(cManager.Player, newWorldPosition.X, newWorldPosition.Y);
                                SetCamera();
                                UIC.DrawStatusDisplay();
                            }
                        }
                    }
                }
            }
        }

        //Gets the new location where a player will move to based on a mouse click
        private Location MouseDirection(Location location)
        {
            Location playerLocation = cManager.Player.Location;
            Location newLocation = new Location(0, 0, playerLocation);
            if (location.X > playerLocation.X)
                newLocation.X++;
            if (location.X < playerLocation.X)
                newLocation.X--;
            if (location.Y > playerLocation.Y)
                newLocation.Y++;
            if (location.Y < playerLocation.Y)
                newLocation.Y--;
            return newLocation;
        }


        //Gets the X and Y coordinate of a selected cell based on clicked cell from the camera
        //Checking needs to be done if the X and Y is outside of the world bounds
        private Location GetCellWorldPositionFromCamera(int x, int y)
        {
            Character player = cManager.Player;
            int wx = GetWorldPositionX(x, player.Location.X);
            int wy = GetWorldPositionY(y, player.Location.Y);
            Console.WriteLine("World position of cell: " + wx + "," + wy); //For Debugging gives the world position of a cell clicked in the camera
            return new Location(wx, wy);
        }

        //Gets the X World Coordinate from the clicked cell in the camera
        private int GetWorldPositionX(int cx, int px)
        {
            if (cx < Constants.CAMERA_VIEW_SIZE / 2)
                return px - (Constants.CAMERA_VIEW_SIZE / 2 - cx);
            else if (cx > Constants.CAMERA_VIEW_SIZE / 2)
                return px + (cx - Constants.CAMERA_VIEW_SIZE / 2);
            else
                return px;
        }

        //Gets the Y World Coordinate from the clicked cell in the camera
        private int GetWorldPositionY(int cy, int py)
        {
            if (cy < Constants.CAMERA_VIEW_SIZE / 2)
                return py - (Constants.CAMERA_VIEW_SIZE / 2 - cy);
            else if (cy > Constants.CAMERA_VIEW_SIZE / 2)
                return py + (cy - Constants.CAMERA_VIEW_SIZE / 2);
            else
                return py;
        }

        //Sets a players Location to a given location
        private void MovePlayerToLocation(Location location)
        {
            Move(cManager.Player, location);
        }

        //Sets a characters Location to a given location
        private void Move(Character c, Location location) // remove a movement point from the character
        {
            c.Location = location;
        }

        //Returns a location or the direction where a player moves to
        private Location Direction(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    return new Location(0, -1);
                case Keys.A:
                    return new Location(-1);
                case Keys.S:
                    return new Location(0, 1);
                case Keys.D:
                    return new Location(1);
            }
            return new Location();
        }

        //Reads world file and sets the flags and terrain ids to the corresponding grid squares
        private void ReadWorldFile()
        {
            byte[] worldFile = Properties.Resources.WORLD;
            int byteOffset = 0;
            if (worldFile.Length >= 0)
            {
                for (int x = 0; x < Constants.GAMEBOARD_SIZE_X; x++)
                {
                    for (int y = 0; y < Constants.GAMEBOARD_SIZE_Y; y++)
                    {
                        gameBoard.GetGridTile(x, y).Flag = worldFile[byteOffset * 2];
                        gameBoard.GetGridTile(x, y).BackGround = worldFile[byteOffset * 2 + 1];
                        gameBoard.GetGridTile(x, y).IsWall = CreateTreadables(gameBoard.GetGridTile(x, y).BackGround);
                        byteOffset++;
                    }
                }
            }
        }

        //Sets a tile as walkable or not
        private bool CreateTreadables(int background)
        {
            switch (background)
            {
                case (int)TERRAIN.Water:
                case (int)TERRAIN.StoneWall:
                case (int)TERRAIN.BrickWall:
                case (int)TERRAIN.WoodWall:
                case (int)TERRAIN.Tree:
                case (int)TERRAIN.Mountain:
                case (int)TERRAIN.Marsh:
                case (int)TERRAIN.PalmTree:
                    return true;
                default:
                    return false;
            }
        }

        //Checks if a tile is blocked due to havign a character or wall
        public bool IsTileBlocked(int x, int y)
        {
            if (x > Constants.GAMEBOARD_SIZE_X || x < 0 || y > Constants.GAMEBOARD_SIZE_Y || y < 0)
                return true;
            return gameBoard.GetGridTile(x, y).IsWall || cManager.LocationContainsCharacter(new Location(x, y));
        }

        public bool IsTileWoodenOrStone(int x, int y)
        {
            return (gameBoard.GetGridTile(x, y).BackGround == (int)TERRAIN.WoodFloor ||
                gameBoard.GetGridTile(x, y).BackGround == (int)TERRAIN.StoneFloor);
        }

        public CharacterManager GetCharacterManager()
        {
            return cManager;
        }

        public UIComponents GetUI()
        {
            return UIC;
        }

        public void MovePanelForward()
        {
            foreach (Control control in form.Controls)
                if (control is Panel)
                    control.BringToFront();
        }

        //Creates the iteraction panel for npcs
        public Panel CreateInteractionPanel(int x, int y, int w, int h)
        {
            Panel panel = new Panel();
            panel.Location = new Point(x, y);
            panel.Width = w;
            panel.Height = h;
            panel.MouseDown += new MouseEventHandler(UIDownClick);
            panel.MouseUp += new MouseEventHandler(UIUPClick);
            form.Controls.Add(panel);
            panel.BringToFront();
            return panel;
        }

        //sets a tile to be blocked
        public void SetBlocked(int x, int y)
        {
            gameBoard.GetGridTile(new Location(x, y)).IsBlocked = true;
        }

        //Gets the next class in the list
        public void GetNextClass()
        {
            if (cManager.Player.CharacterClass != (ClassTypes)8)
            {
                cManager.Player.CharacterClass++;
            }
            else
            {
                cManager.Player.CharacterClass = (ClassTypes)0;
            }
            cManager.Update();
            UIC.UpdateMenuPictureBox();
        }

        //Gets the previous class in the list
        public void GetPreviousClass()
        {
            if (cManager.Player.CharacterClass != (ClassTypes)0)
            {
                cManager.Player.CharacterClass--;
            }
            else
            {
                cManager.Player.CharacterClass = (ClassTypes)8;
            }
            cManager.Update();
            UIC.UpdateMenuPictureBox();
        }

        //Brings the camera to the front of the screen
        public void BringGameForward()
        {
            camera.BringToFront();
        }

        //Changes turn state to have ai and player take turns
        public void ChangeTurnState(TurnState state)
        {
            turnState = state;
            if (state == TurnState.enemy)
            {
                cManager.RunAITurn(ai);
                UIC.EnableDisableButtons(true);
            }
            else
            {
                UIC.EnableDisableButtons(false);
                cManager.ResetMoveCount();
            }
            if(cManager.Player.HP > 0)
                UIC.DrawStatusDisplay();
        }

        public Grid GetGameBoard()
        {
            return gameBoard;
        }
    }
}
