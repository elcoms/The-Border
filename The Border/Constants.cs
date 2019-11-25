using System;

namespace The_Border
{
    class Constants
    {
        // Text files
        // ======================================
        public const string DUNGEON_FILE = "..\\..\\data\\dungeon.txt";
        public const string COLLISION_DATA_FILE = "..\\..\\data\\collision_data.txt";


        // Char
        // ======================================
        // characters
        public const char PLAYER                = 'O';
        public const char ENEMY                 = 'E';

        // effects
        public const char HORIZONTAL_ATTACK     = '-';
        public const char VERTICAL_ATTACK       = '|';
        public const char DEAD                  = 'X';

        // items
        public const char KEY                   = 't';
        public const char LOOTBAG               = 'b';

        // others
        public const char WALL                  = '█';
        public const char SPACE                 = ' ';
        public const char DOOR                  = '▌';

        // Int
        // ======================================
        // world size
        public const int WORLD_WIDTH            = 100;
        public const int WORLD_HEIGHT           = 100;
        public const int CAM_WIDTH              = 100;
        public const int CAM_HEIGHT             = 100;

        // positions
        public const int PLAYER_X = 5;
        public const int PLAYER_Y = 5;
        public const int INVENTORY_X = 100;
        public const int INVENTORY_Y = 1;

        // time (in milliseconds)
        public const int ATTACK_ANIM_TIME       = 200;

        // Color
        //======================================
        public const ConsoleColor FOREGROUND_COLOR              = ConsoleColor.White;
        public const ConsoleColor BACKGROUND_COLOR              = ConsoleColor.Black;
        public static readonly ConsoleColor[] KEY_DOOR_COLORS   = { ConsoleColor.Cyan, ConsoleColor.Green, ConsoleColor.Yellow };
    }
}
