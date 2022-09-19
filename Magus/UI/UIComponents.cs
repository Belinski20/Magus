using Magus.Entity;
using Magus.GameBoard;
using Magus.Util;
using System;
using System.Collections;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace Magus.UI
{
    public class UIComponents
    {
        public int SkillPoints { get; set; }

        private GameManager gManager;
        private Form form;
        private ArrayList buttons;
        private bool InteractionMenuOpen = false;
        private Panel InteractionPanel = null;
        public UIComponents(Form form, GameManager gManager)
        {
            this.gManager = gManager;
            this.form = form;
            buttons = new ArrayList();
            SkillPoints = 0;
        }

        //Creates a Box which looks like it is made of stone
        public void DrawStoneBox(int x, int y, int w, int h, Panel panel = null)
        {
            FillRect(Constants.GREY, new Location(x, y), w, h, panel);
            BorderRect(Constants.LIGHT_GREY, new Location(x, y), w, h, panel);
            BorderRect(Constants.LIGHT_GREY, new Location(x + 1, y + 1), w - 1, h - 1, panel);
            BorderRect(Constants.LIGHT_GREY, new Location(x + 2, y + 2), w - 2, h - 2, panel);
            Line(Constants.DARK_GREY, new Location(x + w, y), new Location(x + w, y + h), panel);
            Line(Constants.DARK_GREY, new Location(x + w, y + h), new Location(x + 1, y + h), panel);
            Line(Constants.DARK_GREY, new Location(x + w - 1, y + 1), new Location(x + w - 1, y + h - 1), panel);
            Line(Constants.DARK_GREY, new Location(x + w - 2, y + 2), new Location(x + w - 2, y + h - 2), panel);
            Line(Constants.DARK_GREY, new Location(x + w - 1, y + h - 1), new Location(x + 1, y + h - 1), panel);
            Line(Constants.DARK_GREY, new Location(x + w - 2, y + h - 2), new Location(x + 2, y + h - 2), panel);

            Line(Constants.LIGHT_GREY, new Location(x + 3, y + 3), new Location(x + w - 3, y + 3), panel, true, true);
            Line(Constants.LIGHT_GREY, new Location(x + 3, y + 3), new Location(x + 3, y + h - 3), panel, true, true);
            Line(Constants.DARK_GREY, new Location(x + 3, y + h - 3), new Location(x + w - 3, y + h - 3), panel, true);
            Line(Constants.DARK_GREY, new Location(x + w - 3, y + 3), new Location(x + w - 3, y + h - 3), panel, true);
        }

        public bool GetInteractionState()
        {
            return InteractionMenuOpen;
        }

        public void BringGameForward()
        {
            gManager.BringGameForward();
        }

        public void SetInteractionState(bool isOpen)
        {
            InteractionMenuOpen = isOpen;
        }

        //Draws lines based for UI Pieces
        public void Line(Color color, Location loc1, Location loc2, Panel panel = null, bool dashed = false, bool top = false)
        {
            Pen pen = new Pen(color);
            if (dashed)
            {
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom;
                if (top)
                    pen.DashPattern = new float[] { 5, 9, 1, 4, 4 };
                else
                    pen.DashPattern = new float[] { 6, 5, 5, 3, 5 };
            }
            if (panel == null)
                form.CreateGraphics().DrawLine(pen, loc1.X, loc1.Y, loc2.X, loc2.Y);
            else
                panel.CreateGraphics().DrawLine(pen, loc1.X, loc1.Y, loc2.X, loc2.Y);
        }

        //Creates a Filled Rectangle
        public void FillRect(Color color, Location loc1, int w, int h, Panel panel = null)
        {
            Brush brush = new SolidBrush(color);
            Rectangle rect = new Rectangle(loc1.X, loc1.Y, w, h);
            if (panel == null)
                form.CreateGraphics().FillRectangle(brush, rect);
            else
                panel.CreateGraphics().FillRectangle(brush, rect);
        }

        //Creates a Border of a Rectangle
        public void BorderRect(Color color, Location loc1, int w, int h, Panel panel = null)
        {
            Pen pen = new Pen(color);
            Rectangle rect = new Rectangle(loc1.X, loc1.Y, w, h);
            if (panel == null)
                form.CreateGraphics().DrawRectangle(pen, rect);
            else
                panel.CreateGraphics().DrawRectangle(pen, rect);
        }

        //Puts a picture into the middle of the screen
        public void PutPictureCentered(int y, Image image)
        {
            PutPicture(new Point((form.Width - image.Size.Width) / 2, y), image);
        }

        //Puts a picture at a given point
        public void PutPicture(Point point, Image image)
        {
            PictureBox picture = new PictureBox();
            picture.Size = image.Size;
            picture.Location = point;
            picture.Image = image;
            form.Controls.Add(picture);
            picture.BringToFront();
        }

        //Updates the icon of the player in the menu
        public void UpdateMenuPictureBox()
        {
            foreach (Control control in form.Controls)
            {
                if (control is PictureBox)
                {
                    ((PictureBox)control).Image = gManager.GetCharacterManager().Player.Icon;
                }
                if(control is TextBox)
                {
                    ((TextBox)control).Text = gManager.GetCharacterManager().Player.Name;
                }
            }
        }

        //Puts a string into the middle of the screen
        public void CenterText(int y, string message, Color color, Color background)
        {
            Size size = TextRenderer.MeasureText(message, new Font(Constants.FONT_STYLE, Constants.FONT_SIZE));
            Label text = new Label();
            text.Size = size;
            text.Font = new Font(Constants.FONT_STYLE, Constants.FONT_SIZE);
            text.Text = message;
            text.ForeColor = color;
            text.BackColor = background;
            text.Location = new Point((form.Width - text.Size.Width) / 2, y);
            form.Controls.Add(text);
            text.BringToFront();
        }

        //Sets the envent for when an icon is clicked
        public void SetIconClickEvent(bool on)
        {
            foreach (Control control in form.Controls)
            {
                if (control is PictureBox)
                {
                    if (on == true)
                        ((PictureBox)control).MouseDown += new MouseEventHandler(Icon_Click);
                    else
                        ((PictureBox)control).MouseDown -= new MouseEventHandler(Icon_Click);
                }
            }

        }

        //Mouse event from Main Menu Icon Click
        private void Icon_Click(object sender, MouseEventArgs e)
        {
            gManager.GetCharacterManager().Player.isFemale = !gManager.GetCharacterManager().Player.isFemale;
            gManager.GetCharacterManager().Update();
            UpdateMenuPictureBox();
        }

        //-----------------------------------------------------
        // Add Buttons
        //-----------------------------------------------------
        public void AddTransitionButton(int left, int top, int right, int bottom, string message, MenuState state)
        {
            TransitionButton button = new TransitionButton(new Location(left, top), new Location(right, bottom), state, message, gManager);
            buttons.Add(button);
            DrawButton(button);
        }

        public void AddEndTurnButton(int left, int top, int right, int bottom, String message)
        {
            EndTurnButton button = new EndTurnButton(new Location(left, top), new Location(right, bottom), message, gManager);
            buttons.Add(button);
            DrawButton(button);
        }

        //NotImplemented
        public void AddNextPCButton(int left, int top, int right, int bottom, String message)
        {
            //When a summon is summoned they get added to the pc list and you can swap between them
        }

        //NotImplemented
        public void AddPrevPCButton(int left, int top, int right, int bottom, String message)
        {
            //When a summon is summoned they get added to the pc list and you can swap between them
        }

        public void AddQuitButton(int left, int top, int right, int bottom, String message)
        {
            QuitButton button = new QuitButton(new Location(left, top), new Location(right, bottom), message, form);
            buttons.Add(button);
            DrawButton(button);
        }

        public void AddDropButton(int left, int top, int right, int bottom, String message)
        {
            DropButton button = new DropButton(new Location(left, top), new Location(right, bottom), message);
            buttons.Add(button);
            DrawButton(button);
        }

        public void AddOKButton(int left, int top, int right, int bottom, String message)
        {
            OKButton button = new OKButton(new Location(left, top), new Location(right, bottom), message, this);
            buttons.Add(button);
            DrawButton(button);
        }

        public void AddNextClassButton(int left, int top, int right, int bottom, String message)
        {
            NextClassButton button = new NextClassButton(new Location(left, top), new Location(right, bottom), message, gManager);
            buttons.Add(button);
            DrawButton(button);
        }

        public void AddPrevClassButton(int left, int top, int right, int bottom, String message)
        {
            PrevClassButton button = new PrevClassButton(new Location(left, top), new Location(right, bottom), message, gManager);
            buttons.Add(button);
            DrawButton(button);
        }

        public void AddPlusButton(int left, int top, int right, int bottom, Stats stat)
        {
            PlusButton button = new PlusButton(new Location(left, top), new Location(right, bottom), stat, gManager);
            buttons.Add(button);
            DrawButton(button);
        }

        public void AddMinusButton(int left, int top, int right, int bottom, Stats stat)
        {
            MinusButton button = new MinusButton(new Location(left, top), new Location(right, bottom), stat, gManager);
            buttons.Add(button);
            DrawButton(button);
        }

        public void AddHostileInterationButton(int left, int top, int right, int bottom, string message, Character pc, Character c, Panel panel = null)
        {
            HostileButton button = new HostileButton(new Location(left, top), new Location(right, bottom), message, this, pc, c);
            buttons.Add(button);
            DrawButton(button, panel);
        }

        public void AddNoButton(int left, int top, int right, int bottom, string message)
        {
            NoButton button = new NoButton(new Location(left, top), new Location(right, bottom), message, form);
            buttons.Add(button);
            DrawButton(button);
        }

        //-----------------------------------------------------
        // Visualize Buttons 
        //-----------------------------------------------------

        // Draws the button box on the form
        public void DrawButton(ButtonBase button, Panel panel = null)
        {
            int x = button.TopLeft.X;
            int y = button.TopLeft.Y;
            int w = button.BottomRight.X - x;
            int h = button.BottomRight.Y - y;
            Line(Constants.LIGHT_GREY, new Location(x + w + 1, y - 1), new Location(x + w + 1, y + h + 1), panel);
            Line(Constants.LIGHT_GREY, new Location(x + w + 1, y + h + 1), new Location(x, y + h + 1), panel);

            Line(Constants.DARK_GREY, new Location(x - 1, y + h + 1), new Location(x - 1, y - 1), panel);
            Line(Constants.DARK_GREY, new Location(x - 1, y - 1), new Location(x + w + 1, y - 1), panel);

            BorderRect(Color.Black, new Location(x, y), w, h, panel);

            FillRect(Constants.BUTTON_NORMAL, new Location(x + 1, y + 1), w - 1, h - 1, panel);

            BorderRect(Constants.BUTTON_LIGHT, new Location(x + 1, y + 1), w - 2, h - 2, panel);
            BorderRect(Constants.BUTTON_LIGHT, new Location(x + 2, y + 2), w - 3, h - 3, panel);

            Line(Constants.BUTTON_DARK, new Location(x + w - 1, y + 1), new Location(x + w - 1, y + h - 1), panel);
            Line(Constants.BUTTON_DARK, new Location(x + w - 1, y + h - 1), new Location(x + 2, y + h - 1), panel);
            Line(Constants.BUTTON_DARK, new Location(x + w - 2, y + 2), new Location(x + w - 2, y + h - 2), panel);
            Line(Constants.BUTTON_DARK, new Location(x + w - 2, y + h - 2), new Location(x + 3, y + h - 2), panel);

            int TextX = x + (w - 8 * button.Message.Length) / 2;
            int TextY = y - 4 + h / 2;

            ReliefTextXY(TextX, TextY, button.Message, Constants.BUTTON_LIGHT, Color.Black, Constants.BUTTON_DARK, Color.Red, panel);
        }

        // Frames buttons based on click state
        private void FrameButton(ButtonBase button, Panel panel = null, bool pressed = false)
        {
            int x = button.TopLeft.X;
            int y = button.TopLeft.Y;
            int w = button.BottomRight.X - x;
            int h = button.BottomRight.Y - y;

            int TextX = x + (w - 8 * button.Message.Length) / 2;
            int TextY = y - 4 + h / 2;

            Color color;
            if (pressed)
            {
                color = Constants.BUTTON_DARK;
                ReliefTextXY(TextX, TextY, button.Message, Constants.BUTTON_LIGHT, Constants.BUTTON_DARK, Constants.BUTTON_NORMAL, Color.Red, panel);
            }
            else
            {
                color = Constants.BUTTON_LIGHT;
                ReliefTextXY(TextX, TextY, button.Message, Constants.BUTTON_LIGHT, Color.Black, Constants.BUTTON_DARK, Color.Red, panel);
            }

            BorderRect(color, new Location(x + 1, y + 1), w - 2, h - 2, panel);
            BorderRect(color, new Location(x + 2, y + 2), w - 3, h - 3, panel);

            Line(Constants.BUTTON_DARK, new Location(x + w - 1, y + 1), new Location(x + w - 1, y + h - 1), panel);
            Line(Constants.BUTTON_DARK, new Location(x + w - 1, y + h - 1), new Location(x + 2, y + h - 1), panel);
            Line(Constants.BUTTON_DARK, new Location(x + w - 2, y + 2), new Location(x + w - 2, y + h - 2), panel);
            Line(Constants.BUTTON_DARK, new Location(x + w - 2, y + h - 2), new Location(x + 3, y + h - 2), panel);
        }

        // Sets the shaded text of the button
        private void ReliefTextXY(int x, int y, string message, Color light, Color dark, Color normal, Color background, Panel panel = null)
        {
            if (background != Color.Red)
            {
                FillRect(background, new Location(x, y), x + message.Length, y + 10, panel);
            }
            OutTextXY(x, y, message, dark, panel);
            OutTextXY(x, y + 2, message, light, panel);
            OutTextXY(x, y + 1, message, normal, panel);
        }

        // Draws text onto the form
        private void OutTextXY(int x, int y, string message, Color color, Panel panel = null)
        {
            Brush brush = new SolidBrush(color);
            Font font = new Font(Constants.FONT_STYLE, Constants.FONT_SIZE, FontStyle.Bold);
            if (panel == null)
                form.CreateGraphics().DrawString(message, font, brush, new Point(x, y));
            else
                panel.CreateGraphics().DrawString(message, font, brush, new Point(x, y));
        }

        // PLaces a frame around where textbox is placed
        private void TextEditFrame(int x, int y, int length, Panel panel = null)
        {
            Line(Constants.LIGHT_GREY, new Location(x - 3, y + 11), new Location(x + length * 8 + 3, y + 11), panel);
            Line(Constants.LIGHT_GREY, new Location(x + length * 8 + 3, y + 11), new Location(x + length * 8 + 3, y - 3), panel);
            Line(Constants.DARK_GREY, new Location(x - 3, y - 3), new Location(x + length * 8 + 3, y - 3), panel);
            Line(Constants.DARK_GREY, new Location(x - 3, y - 3), new Location(x - 3, y + 11), panel);
        }

        // Controls the Textbox to display name 
        private void TextEdit(int x, int y, int length)
        {
            TextBox characterName = new TextBox();
            characterName.Location = new Point(x - 2, y - 2);
            characterName.Width = length * 8 + 5;
            characterName.Height = y + 14;
            characterName.BackColor = Constants.GREY;
            characterName.BorderStyle = BorderStyle.None;
            characterName.MaxLength = Constants.MAX_CHARACTER_NAME_SIZE;
            characterName.Text = gManager.GetCharacterManager().Player.Name;
            form.Controls.Add(characterName);
        }

        // Hides the textbox from the character creation screen
        public void HideUI()
        {
            foreach (Control control in form.Controls)
            {
                if (control is TextBox)
                {
                    gManager.GetCharacterManager().Player.Name = control.Text;
                }
                control.Visible = false;
            }
            OnOffButtons(false);
        }

        // Sets the textbox for the character creation screen
        private void SetTextBox()
        {
            foreach (Control control in form.Controls)
            {
                if (control is TextBox)
                {
                    control.Text = gManager.GetCharacterManager().Player.Name;
                }
            }
        }

        //-----------------------------------------------------
        // Clear Buttons
        //-----------------------------------------------------

        public void ClearButtons()
        {
            buttons.Clear();
        }

        //-----------------------------------------------------
        // Button Click Check
        //-----------------------------------------------------

        // Event run on mouse click down
        public void ClickDownUI(Location loc)
        {
            foreach (ButtonBase button in buttons)
                if (!button.IsPressed && button.IsClicked(loc) && button.IsActive)
                {
                    FrameButton(button, InteractionPanel, true);
                    button.Run();
                    break;
                }
        }

        // Event run on mouse click up
        public void ClickUPUI(Location loc)
        {
            foreach (ButtonBase button in buttons)
                if (button.IsClicked(loc) && button.IsActive)
                {
                    button.IsPressed = false;
                    FrameButton(button, InteractionPanel);
                    break;
                }
        }

        // Sets all current buttons to true or false
        public void OnOffButtons(bool active)
        {
            foreach (ButtonBase button in buttons)
                button.IsActive = active;
        }

        //-----------------------------------------------------
        // UI Screens
        //-----------------------------------------------------

        //UI Components for loss screen
        private void MagusFace(Image image, string message, string buttonMessage)
        {
            DrawStoneBox(210, 100, 220, 120);
            PutPictureCentered(125, image);
            AddTransitionButton(270, 185, 370, 210, buttonMessage, MenuState.HallOfFame);
            CenterText(160, message, Color.Black, Constants.GREY);
        }

        //UI Components for splash screen
        private void PressKey()
        {
            CenterText(460, "Press any Key", Color.Gray, Color.Black);
        }

        //Draws all Status Pieces
        public void DrawStatusDisplay()
        {
            StatusPanel();
            Status();
            StatusBar();
        }

        // Game Screen UI Piece
        public void StatusPanel()
        {
            FillRect(Constants.GREY, new Location(10, 47), 130, 115);
            FillRect(Constants.GREY, new Location(10, 165), 70, 120);
            OutTextXY(10, 92, "XP", Color.Black);
            OutTextXY(10, 116, "Strength", Color.Black);
            OutTextXY(10, 128, "Agility", Color.Black);
            OutTextXY(10, 140, "Wisdom", Color.Black);
            OutTextXY(10, 152, "Load", Color.Black);
            OutTextXY(10, 170, "Using:", Color.Black);
            OutTextXY(10, 205, "Carrying:", Color.Black);
            OutTextXY(10, 265, "In Pack:", Color.Black);
        }

        // Game Screen UI Piece
        public void Status()
        {
            Character c = gManager.GetCharacterManager().Player;
            PutPicture(new Point(10, 57), c.Icon);
            OutTextXY(10, 46, c.Name, Color.Black);
            OutTextXY(10, 80, c.Level.ToString(), Color.Black);
            OutTextXY(34, 92, c.XP.ToString(), Color.Black);
            OutTextXY(10, 104, " " + c.Moves + " MP's", Color.Black);
            OutTextXY(90, 116, c.Strength.ToString(), Color.Black);
            OutTextXY(90, 128, c.Skill.ToString(), Color.Black);
            OutTextXY(90, 140, c.Wisdom.ToString(), Color.Black);
            if (OverLoaded(c))
                OutTextXY(90, 152, c.Load.ToString(), Color.DarkRed);
            else
                OutTextXY(90, 152, c.Load.ToString(), Color.Black);
        }

        // Health and Mana bars
        public void StatusBar()
        {
            Character c = gManager.GetCharacterManager().Player;
            int x;
            BorderRect(Constants.LIGHT_GREY, new Location(56, 59), 26, 7);
            Line(Constants.DARK_GREY, new Location(56, 59), new Location(82, 59));
            Line(Constants.DARK_GREY, new Location(56, 59), new Location(56, 66));

            if (c.ManaStat == 0)
                x = 0;
            else
                x = (25 * c.Mana / c.ManaStat);
            if (x > 0)
                FillRect(Color.Blue, new Location(57, 60), x - 1, 5);
            if (x < 25)
            {
                FillRect(Constants.GREY, new Location(57 + x, 60), 81 - (57 + x), 5);
            }
            OutTextXY(90, 58, Math.Max(0, c.Mana) + "/" + c.ManaStat, Color.Black);

            BorderRect(Constants.LIGHT_GREY, new Location(56, 69), 26, 7);
            Line(Constants.DARK_GREY, new Location(56, 69), new Location(82, 69));
            Line(Constants.DARK_GREY, new Location(56, 69), new Location(56, 76));

            if (c.HealthStat == 0)
                x = 0;
            else
                x = (25 * c.HP / c.HealthStat);
            if (x > 0)
                FillRect(Color.Green, new Location(57, 70), x - 1, 5);
            if (x < 25)
            {
                FillRect(Constants.GREY, new Location(57 + x, 70), 81 - (57 + x), 5);
            }
            OutTextXY(90, 68, Math.Max(0, c.HP) + "/" + c.HealthStat, Color.Black);
        }

        // Game Screen UI Piece
        public void GameButtons()
        {
            AddDropButton(58, 7, 102, 40, ">"); // FIX BUTTON TYPE
            AddDropButton(10, 7, 54, 40, "<"); // FIX BUTTON TYPE
            AddEndTurnButton(106, 7, 150, 40, "End");
            AddQuitButton(560, 445, 610, 470, "Quit");
            AddDropButton(100, 165, 150, 180, "Drop");
        }

        // Check if Load is larger than strength
        private bool OverLoaded(Character c)
        {
            return c.Load > c.Strength;
        }

        // Hall of Fame Screen
        public void HallOfFame()
        {
            DrawStoneBox(100, 0, 400, 480);
            AddTransitionButton(250, 440, 350, 465, "OK", MenuState.TryAgain);
            PutPicture(new Point(150, 10), Properties.Resources.BraveHeroes);

            //-----------------------------------------------------
            // NEEDS FILE TO SAVE AND READ SCOREBOARD FROM 
            //-----------------------------------------------------

        }

        //Character Selection Screen
        public void SelectionScreen()
        {
            DrawStoneBox(150, 100, 325, 120);
            AddPrevClassButton(175, 175, 225, 200, "<<");
            AddNextClassButton(250, 175, 300, 200, ">>");
            AddOKButton(325, 175, 375, 200, "OK");
            AddTransitionButton(400, 175, 450, 200, "Done", MenuState.Game);
            TextEditFrame(250, 120, Constants.MAX_CHARACTER_NAME_SIZE);
            TextEdit(250, 120, Constants.MAX_CHARACTER_NAME_SIZE);
            SetTextBox();
            PutPicture(new Point(200, 125), gManager.GetCharacterManager().Player.Icon);
            SetIconClickEvent(true);
        }

        //Magus Splash Screen
        public void MagusScreen()
        {
            FillRect(Color.Black, new Location(), form.Width, form.Width);
            MagusFace(Properties.Resources.DarkOneSmiles, "The Dark One smiles...", "He-he-he");
        }

        // Splash Screen
        public void SplashScreen()
        {
            PutPictureCentered(140, Properties.Resources.StartUp);
            CenterText(280, "Magus", Color.White, Color.Black);
            CenterText(300, "3rd edition", Color.LightGray, Color.Black);
            CenterText(310, "Code & Graphics by Ronny Wester, ronny@rat.se", Color.Gray, Color.Black);

            CenterText(330, "Thanx to", Color.Gray, Color.Black);
            CenterText(340, "Anders T”rlind", Color.Gray, Color.Black);
            CenterText(350, "Jay Stelly", Color.Gray, Color.Black);
            CenterText(360, "Christian Wagner", Color.Gray, Color.Black);
            CenterText(370, "Gary Shaw", Color.Gray, Color.Black);
            CenterText(380, "Mikael Arle", Color.Gray, Color.Black);

            PressKey();
        }

        // Opens stat screen
        public void ModifyCharacter()
        {
            DrawStoneBox(200, 50, 200, 315);
            PutPicture(new Point(250, 75), gManager.GetCharacterManager().Player.Icon);
            OutTextXY(225, 105, gManager.GetCharacterManager().Player.Name, Color.Black);

            AddMinusButton(320, 125, 345, 150, Stats.Mana);
            AddMinusButton(320, 160, 345, 185, Stats.Health);
            AddMinusButton(320, 195, 345, 220, Stats.Strength);
            AddMinusButton(320, 230, 345, 255, Stats.Agility);
            AddMinusButton(320, 265, 345, 290, Stats.Wisdom);
            AddPlusButton(350, 125, 375, 150, Stats.Mana);
            AddPlusButton(350, 160, 375, 185, Stats.Health);
            AddPlusButton(350, 195, 375, 220, Stats.Strength);
            AddPlusButton(350, 230, 375, 255, Stats.Agility);
            AddPlusButton(350, 265, 375, 290, Stats.Wisdom);

            AddTransitionButton(225, 315, 375, 340, "OK", MenuState.MainMenu);

            OutTextXY(210, 130, "Mana     " + gManager.GetCharacterManager().Player.ManaStat, Color.Black);
            OutTextXY(210, 165, "Health   " + gManager.GetCharacterManager().Player.HealthStat, Color.Black);
            OutTextXY(210, 200, "Strength " + gManager.GetCharacterManager().Player.Strength, Color.Black);
            OutTextXY(210, 235, "Agility  " + gManager.GetCharacterManager().Player.Agility, Color.Black);
            OutTextXY(210, 270, "Wisdom   " + gManager.GetCharacterManager().Player.Wisdom, Color.Black);
            OutTextXY(210, 300, SkillPoints.ToString() + " points left", Color.Black);
        }

        // Hostile interaction ui when left clicking a character
        public void HostileInteration(Character character)
        {
            Panel panel = gManager.CreateInteractionPanel(200, 50, 200, 275);
            InteractionPanel = panel;
            DrawStoneBox(0, 0, 200, 275, panel);
            EnableDisableButtons(false);
            AddHostileInterationButton(25, 75, 175, 100, "Push", gManager.GetCharacterManager().Player, character, panel);
            AddHostileInterationButton(25, 125, 175, 150, "Booh!", gManager.GetCharacterManager().Player, character, panel);
            AddHostileInterationButton(25, 175, 175, 200, "!#$%&?*!!", gManager.GetCharacterManager().Player, character, panel);
            AddHostileInterationButton(25, 225, 175, 250, "Cancel", gManager.GetCharacterManager().Player, character, panel);
        }

        // Removes a given number of buttons from the last created to the newest
        public void RemoveButtonFromInteraction(int amount)
        {
            buttons.RemoveRange(buttons.Count - amount, amount);
            gManager.MovePanelForward();
        }

        public void ResetInteractionPanel()
        {
            InteractionPanel = null;
        }

        //Prints a message in the bottom right of the game screen
        public void Message(string message)
        {
            FillRect(Constants.GREY, new Location(10, 465), 389, 10);
            OutTextXY(10, 465, message, Color.Black);
        }

        //Enables or disables the buttons to prevent them from being used when overlaying screens are shown
        public void EnableDisableButtons(bool enabled, int amount = -1)
        {
            int count = 0;
            foreach (ButtonBase button in buttons)
            {
                if (count == amount)
                    return;
                button.IsActive = enabled;
                count++;
            }
        }

        //Displays the try again screen
        public void TryAgainScreen()
        {
            string message = "Play Again?";
            FillRect(Color.Black, new Location(), form.Width, form.Width);
            DrawStoneBox(150, 100, 300, 120);
            AddTransitionButton(240, 175, 290, 200, "Yes", MenuState.Splash);
            AddNoButton(310, 175, 360, 200, "No");
            OutTextXY(300 - message.Length * 4, 140, message, Color.Black);
        }

    }
}
