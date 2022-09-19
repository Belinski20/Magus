using System;
﻿using System.Drawing;

namespace Magus.Util
{
    class Constants
    {
        //location and size constants
        public static int FORM_SIZE_HEIGHT = 640;
        public static int FORM_SIZE_WIDTH = 519;

        public static int MAX_CHARACTER_NAME_SIZE = 15;

        public static int CAMERA_PANEL_WIDTH = 456;
        public static int CAMERA_PANEL_HEIGHT = 399;
        public static int CAMERA_OFFSET_X = 160;

        public static int GAMEBOARD_SIZE_X = 200;
        public static int GAMEBOARD_SIZE_Y = 320;

        public static int PLAYER_SPAWN_X = 112;
        public static int PLAYER_SPAWN_Y = 13;

        public static int CAMERA_VIEW_SIZE = 19;

        public static int DARK_ONE_SPAWN_X = 100;
        public static int DARK_ONE_SPAWN_Y = 316;

        public static int DEMON_PRINCE_SPAWN_X = 100;
        public static int DEMON_PRINCE_SPAWN_Y = 312;

        //rank constants
        public static int[] rankList = {-1,0,10,20,30,40,80,100,120,140,170,200,230,250,1200};

        public static int NUM_BAD_GUYS = 100;
        public static int ItemSpawnCount = 400;

        public static int cMaxStep = 12;
        public static int cAreaSize = 20;
        public static int FONT_SIZE = 8;
        public static string FONT_STYLE = "Lucida Console"; // Consolas

        public static Color DARK_GREY = Color.FromArgb(65, 65, 65);
        public static Color GREY = Color.FromArgb(97, 97, 97);
        public static Color LIGHT_GREY = Color.FromArgb(130, 130, 130);
        public static Color BUTTON_NORMAL = Color.FromArgb(130, 65, 32);
        public static Color BUTTON_LIGHT = Color.FromArgb(227, 97, 0);
        public static Color BUTTON_DARK = Color.FromArgb(97, 32, 32);
    }
}
