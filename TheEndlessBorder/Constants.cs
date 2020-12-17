using System;
using System.Collections.Generic;

namespace TheEndlessBorder
{
    class Constants
    {
        // Text files
        // ======================================
        public const string COLLISION_DATA_FILE = "..\\..\\..\\data\\collision_data.txt";
        public const string GRAPHICAL_INTERFACE_FILE = "..\\..\\..\\data\\interface.txt";
        public const string MENU_FILE = "..\\..\\..\\data\\main_menu.txt";
        public const string GAMEOVER_FILE = "..\\..\\..\\data\\gameover.txt";
        public const string FOREGROUND_FILE = "..\\..\\..\\data\\cutscene_foreground.txt";
        public const string BACKGROUND_FILE = "..\\..\\..\\data\\cutscene_background.txt";


        // Char
        // ======================================
        // characters
        public const char PLAYER                = 'O';
        public const char ENEMY                 = 'E';
        public const char ENEMY_PATROL          = 'e';

        // effects
        public const char HORIZONTAL_ATTACK     = '-';
        public const char VERTICAL_ATTACK       = '|';
        public const char DEAD                  = 'X';

        // items
        public const char KEY                   = 't';
        public const char APPLE                 = 'b';
        public const char TORCH                 = 'i';

        // collision types
        public const char WALL                  = '█';
        public const char FLOOR                 = '.';
        public const char SPACE                 = ' ';
        public const char DOOR_COLLISION        = '▌';
        public const char WIN_TRIGGER           = 'w';

        // graphic types
        public const char DOOR_VERTICAL         = '│';
        public const char DOOR_HORIZONTAL       = '─';
        public const char FENCE                 = '▒';
        public const char FENCE_WEAK            = '░';
        public const char UNKNOWN               = '?';

        // Int
        // ======================================
        // sizes
        public const int CAM_WIDTH              = 48;
        public const int CAM_HEIGHT             = 24;
        public const int WINDOW_WIDTH           = 95;
        public const int WINDOW_HEIGHT          = 39;

        // positions
        public const int CAM_START_X            = 5;
        public const int CAM_START_Y            = 3;
        public const int STATS_X                = 61;
        public const int STATS_Y                = 4;   
        public const int INVENTORY_X            = 60;
        public const int INVENTORY_Y            = 19;
        public const int LOG_X                  = 3;
        public const int LOG_Y                  = 30;

        // time (in milliseconds)
        public const int ATTACK_ANIM_TIME       = 200;
        public const int GAMEOVER_PAUSE_TIME    = 4000;

        // limits
        public const int NUM_OF_DOORS           = 4;

        // patterns
        public readonly static List<uint> WallPatterns = new List<uint>(new uint[] {              
            27,     // Corner
            432,    // Corner
            436,    // Double Corners
            502,    // Sides of corner
            502,    // Space on the right only
            504,    // Sides
            510, }); // Corner
        
        public readonly static List<uint> DoorPatterns = new List<uint>(new uint[] {
            504 });

        // Color
        //======================================
        public const ConsoleColor FOREGROUND_COLOR              = ConsoleColor.White;
        public const ConsoleColor UNLIT_COLOR                   = ConsoleColor.DarkGray;
        public const ConsoleColor BACKGROUND_COLOR              = ConsoleColor.Black;
        public static readonly ConsoleColor[] KEY_DOOR_COLORS       = { ConsoleColor.Cyan, ConsoleColor.Green, ConsoleColor.Magenta };
        public static readonly ConsoleColor[] KEY_DOOR_COLORS_DARK  = { ConsoleColor.DarkCyan, ConsoleColor.DarkGreen, ConsoleColor.DarkMagenta };
    }
}
