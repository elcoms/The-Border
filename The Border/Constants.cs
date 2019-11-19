using System;

namespace The_Border
{
    class Constants
    {
        // Text files
        public const string DUNGEON_FILE = "..\\..\\data\\dungeon.txt";

        // char
        public const char PLAYER        = 'O';
        public const char ENEMY         = 'E';
        public const char ENEMY_DEAD    = 'X';
        public const char WALL          = '█';
        public const char SPACE         = ' ';
        public const char DOOR          = '▌';

        // world size
        public const int WORLD_WIDTH    = 100;
        public const int WORLD_HEIGHT   = 100;
        public const int CAM_WIDTH      = 100;
        public const int CAM_HEIGHT     = 100;

        // color
        public const ConsoleColor FOREGROUND_COLOR = ConsoleColor.White;
        public const ConsoleColor BACKGROUND_COLOR = ConsoleColor.Black;
    }
}
