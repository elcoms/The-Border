using System;

namespace The_Border
{
    class Constants
    {
        // Text files
        // ======================================
        public const string COLLISION_DATA_FILE = "..\\..\\data\\collision_data.txt";
        public const string GRAPHICAL_INTERFACE_FILE = "..\\..\\data\\interface.txt";
        public const string MENU_FILE = "..\\..\\data\\main_menu.txt";
        public const string GAMEOVER_FILE = "..\\..\\data\\gameover.txt";
        public const string FOREGROUND_FILE = "..\\..\\data\\cutscene_foreground.txt";
        public const string BACKGROUND_FILE = "..\\..\\data\\cutscene_background.txt";


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
        public const char LOOTBAG               = 'b';

        // collision types
        public const char WALL                  = '█';
        public const char SPACE                 = ' ';
        public const char DOOR_COLLISION        = '▌';
        public const char WIN_TRIGGER           = 'w';

        // graphic types
        public const char DOOR_VERTICAL         = '│';
        public const char DOOR_HORIZONTAL       = '─';
        public const char FENCE                 = '▒';
        public const char FENCE_WEAK            = '░';

        // Int
        // ======================================
        // sizes
        public const int WORLD_WIDTH            = 250;
        public const int WORLD_HEIGHT           = 250;
        public const int CAM_WIDTH              = 50;
        public const int CAM_HEIGHT             = 24;
        public const int WINDOW_WIDTH           = 95;
        public const int WINDOW_HEIGHT          = 37;

        // positions
        public const int PLAYER_X               = 53;
        public const int PLAYER_Y               = 13;
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

        // Color
        //======================================
        public const ConsoleColor FOREGROUND_COLOR              = ConsoleColor.White;
        public const ConsoleColor BACKGROUND_COLOR              = ConsoleColor.Black;
        public static readonly ConsoleColor[] KEY_DOOR_COLORS   = { ConsoleColor.Cyan, ConsoleColor.Green, ConsoleColor.Magenta };
    }
}
