using System;

namespace The_Border
{
    class Constants
    {
        // Text files
        // ======================================
        public const string DUNGEON_FILE = "..\\..\\data\\dungeon.txt";


        // Char
        // ======================================
        // characters
        public const char PLAYER                = 'O';
        public const char ENEMY                 = 'E';

        // effects
        public const char HORIZONTAL_ATTACK     = '-';
        public const char VERTICAL_ATTACK       = '|';
        public const char DEAD                  = 'X';

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

        // time
        public const int ATTACK_ANIM_TIME       = 200;

        // Color
        //======================================
        public const ConsoleColor FOREGROUND_COLOR = ConsoleColor.White;
        public const ConsoleColor BACKGROUND_COLOR = ConsoleColor.Black;
    }
}
