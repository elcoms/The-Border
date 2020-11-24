using System;
using System.Collections.Generic;
using System.Text;
using TheEndlessBorder.scripts;

namespace TheEndlessBorder.scripts
{
    struct Vector2
    {
        public int x, y;

        public Vector2(int X, int Y)
        {
            x = X;
            y = Y;
        }
    }

    struct Rect
    {
        public Vector2 min, max;
    }

    class Room
    {
        enum Direction
        {
            UP,
            DOWN,
            LEFT,
            RIGHT,
            NULL
        }

        readonly int MAXSIZE_X = 40;
        readonly int MAXSIZE_Y = 10;

        public bool[,] FloorPlan { get; private set; }
        public bool[,] WallPlan { get; private set; }
        Vector2 size;
        public Vector2 GetRoomSize() { return size; }

        public uint ConvertToDec(int X, int Y)
        {
            uint Dec = 0;
            int i = 0;
            for (int y = Y - 1; y <= Y + 1; ++y)
                for (int x = X - 1; x <= X + 1; ++x)
                {
                    if (x >= 0
                     && y >= 0
                     && x < FloorPlan.GetLength(0)
                     && y < FloorPlan.GetLength(1))
                    {
                        uint n = (FloorPlan[x, y]) ? 1u : 0u;
                        n = n << i;
                        Dec = Dec | n;
                    }
                    i++;
                }

            return Dec;
        }

        public uint RotateDec90(in uint Dec)
        {
            uint newDec = 0;

            // move Dec bit position to newDec's bit position
            newDec |= ((Dec >> 0) & 1u) << 2;
            newDec |= ((Dec >> 1) & 1u) << 5;
            newDec |= ((Dec >> 2) & 1u) << 8;

            newDec |= ((Dec >> 3) & 1u) << 1;
            newDec |= ((Dec >> 4) & 1u) << 4;
            newDec |= ((Dec >> 5) & 1u) << 7;

            newDec |= ((Dec >> 6) & 1u) << 0;
            newDec |= ((Dec >> 7) & 1u) << 3;
            newDec |= ((Dec >> 8) & 1u) << 6;

            return newDec;
        }

        public uint MirrorDec(in uint Dec)
        {
            uint newDec = 0;

            // move Dec bit position to newDec's bit position
            newDec |= ((Dec >> 0) & 1u) << 2;
            newDec |= ((Dec >> 1) & 1u) << 1;
            newDec |= ((Dec >> 2) & 1u) << 0;

            newDec |= ((Dec >> 3) & 1u) << 5;
            newDec |= ((Dec >> 4) & 1u) << 4;
            newDec |= ((Dec >> 5) & 1u) << 3;

            newDec |= ((Dec >> 6) & 1u) << 8;
            newDec |= ((Dec >> 7) & 1u) << 7;
            newDec |= ((Dec >> 8) & 1u) << 6;

            return newDec;
        }

        public void SetPattern(int x, int y, List<uint> wallPatterns)
        {
            uint dec = ConvertToDec(x, y);

            bool bFound = false;
            for (int m = 0; m < 2; m++)
            {
                for (int r = 0; r < 4; r++)
                {
                    for (int i = 0; i < wallPatterns.Count; i++)
                    {
                        if (wallPatterns[i] == dec)
                        {
                            bFound = true;
                        }
                    }

                    if (!bFound)
                    {
                        wallPatterns.Add(dec);
                    }

                    // Rotate
                    dec = RotateDec90(dec);
                }

                dec = MirrorDec(dec);
            }
        }

        public Object[,] Generate(int seed, List<uint> wallPatterns)
        {
            var random = new Random(seed);
            var Rooms = new Rect[4 + random.Next() % 4];

            Rooms[0].max.x = 4 + random.Next() % MAXSIZE_X;
            Rooms[0].max.y = 4 + random.Next() % MAXSIZE_Y;
            Rooms[0].min.x = Rooms[0].min.y = 0;
            Rect Bounds = Rooms[0];

            int corners = 0;
            int direction = 0;
            Direction currentDirection = Direction.NULL;
            Direction prevDirectionMirrored = Direction.NULL;
            for (int i = 1; i < Rooms.Length; i++)
            {
                // corners
                // 0           1
                // +-----------+
                // |  Room[0]  |
                // |           |
                // +-----------+
                // 2           3

                // direction
                // 0 <- -> 1

                // prevent room from generating backwards
                do
                {
                    corners = 1 + random.Next() % 4;
                    direction = 1 + random.Next() % 2;

                    switch (corners)
                    {
                        case 1:
                            if (direction == 1)
                            {
                                // -X +Y
                                Rooms[i].min.x = Rooms[0].min.x - (4 + random.Next() % MAXSIZE_X);
                                Rooms[i].max.x = Rooms[0].min.x;

                                Rooms[i].min.y = Rooms[0].min.y;
                                Rooms[i].max.y = Rooms[0].min.y + (4 + random.Next() % MAXSIZE_Y);

                                currentDirection = Direction.LEFT;
                            }
                            else
                            {
                                // +X -Y
                                Rooms[i].min.x = Rooms[0].min.x;
                                Rooms[i].max.x = Rooms[0].min.x + (4 + random.Next() % MAXSIZE_X);

                                Rooms[i].min.y = Rooms[0].min.y - (4 + random.Next() % MAXSIZE_Y);
                                Rooms[i].max.y = Rooms[0].min.y;

                                currentDirection = Direction.UP;
                            }
                            break;

                        case 2:
                            if (direction == 1)
                            {
                                // -X -Y
                                Rooms[i].min.x = Rooms[0].max.x - (4 + random.Next() % MAXSIZE_X);
                                Rooms[i].max.x = Rooms[0].max.x;

                                Rooms[i].min.y = Rooms[0].min.y - (4 + random.Next() % MAXSIZE_Y);
                                Rooms[i].max.y = Rooms[0].min.y;

                                currentDirection = Direction.UP;
                            }
                            else
                            {
                                // +X +Y
                                Rooms[i].min.x = Rooms[0].max.x;
                                Rooms[i].max.x = Rooms[0].max.x + (4 + random.Next() % MAXSIZE_X);

                                Rooms[i].min.y = Rooms[0].min.y;
                                Rooms[i].max.y = Rooms[0].min.y + (4 + random.Next() % MAXSIZE_Y);

                                currentDirection = Direction.RIGHT;
                            }
                            break;

                        case 3:
                            if (direction == 1)
                            {
                                // -X -Y
                                Rooms[i].min.x = Rooms[0].min.x - (4 + random.Next() % MAXSIZE_X);
                                Rooms[i].max.x = Rooms[0].min.x;

                                Rooms[i].min.y = Rooms[0].max.y - (4 + random.Next() % MAXSIZE_Y);
                                Rooms[i].max.y = Rooms[0].max.y;

                                currentDirection = Direction.LEFT;
                            }
                            else
                            {
                                // +X +Y
                                Rooms[i].min.x = Rooms[0].min.x;
                                Rooms[i].max.x = Rooms[0].min.x + (4 + random.Next() % MAXSIZE_X);

                                Rooms[i].min.y = Rooms[0].max.y;
                                Rooms[i].max.y = Rooms[0].max.y + (4 + random.Next() % MAXSIZE_Y);

                                currentDirection = Direction.DOWN;
                            }
                            break;

                        case 4:
                            if (direction == 1)
                            {
                                // -X +Y
                                Rooms[i].min.x = Rooms[0].max.x - (4 + random.Next() % MAXSIZE_X);
                                Rooms[i].max.x = Rooms[0].max.x;

                                Rooms[i].min.y = Rooms[0].max.y;
                                Rooms[i].max.y = Rooms[0].max.y + (4 + random.Next() % MAXSIZE_Y);

                                currentDirection = Direction.DOWN;
                            }
                            else
                            {
                                // +X -Y
                                Rooms[i].min.x = Rooms[0].max.x;
                                Rooms[i].max.x = Rooms[0].max.x + (4 + random.Next() % MAXSIZE_X);

                                Rooms[i].min.y = Rooms[0].max.y - (4 + random.Next() % MAXSIZE_Y);
                                Rooms[i].max.y = Rooms[0].max.y;

                                currentDirection = Direction.RIGHT;
                            }
                            break;
                        default:
                            break;
                    }
                } while (currentDirection == prevDirectionMirrored);

                switch (currentDirection)
                {
                    case Direction.UP:
                        prevDirectionMirrored = Direction.DOWN;
                        break;
                    case Direction.DOWN:
                        prevDirectionMirrored = Direction.UP;
                        break;
                    case Direction.LEFT:
                        prevDirectionMirrored = Direction.RIGHT;
                        break;
                    case Direction.RIGHT:
                        prevDirectionMirrored = Direction.LEFT;
                        break;
                    case Direction.NULL:
                        break;
                    default:
                        break;
                }

                // Update bounds
                // bounds minimum x more than the room minimum, change bound minimum x to be room minimum
                if (Bounds.min.x > Rooms[i].min.x)
                    Bounds.min.x = Rooms[i].min.x;

                // max check
                if (Bounds.max.x < Rooms[i].max.x)
                    Bounds.max.x = Rooms[i].max.x;

                // bounds minimum y more than the room minimum, change bound minimum y to be room minimum
                if (Bounds.min.y > Rooms[i].min.y)
                    Bounds.min.y = Rooms[i].min.y;

                // max check
                if (Bounds.max.y < Rooms[i].max.y)
                    Bounds.max.y = Rooms[i].max.y;
            }

            // Render rect
            size.x = Bounds.max.x - Bounds.min.x;
            size.y = Bounds.max.y - Bounds.min.y;
            FloorPlan = new bool[size.x, size.y];
            WallPlan = new bool[size.x, size.y];

            // init array
            for (int y = 0; y < FloorPlan.GetLength(1); y++)
            {
                for (int x = 0; x < FloorPlan.GetLength(0); x++)
                {
                    WallPlan[x, y] = FloorPlan[x, y] = false;
                }
            }

            // From rect to bitmap
            foreach (var r in Rooms)
            {
                for (int y = r.min.y; y < r.max.y; y++)
                {
                    for (int x = r.min.x; x < r.max.x; x++)
                    {
                        // min bounds is either 0 or negative -> 5 - (-5) = 10 (x on the console)
                        FloorPlan[x - Bounds.min.x, y - Bounds.min.y] = true;
                    }
                }
            }

            // Generate Wall
            for (int y = 0; y < WallPlan.GetLength(1); y++)
            {
                for (int x = 0; x < WallPlan.GetLength(0); x++)
                {
                    uint currentPattern = ConvertToDec(x, y);
                    foreach (uint pattern in wallPatterns)
                    {
                        if (currentPattern == pattern)
                        {
                            WallPlan[x, y] = true;
                            break;
                        }
                    }
                }
            }

            return ConvertToObjects();
        }

        Object[,] ConvertToObjects()
        {
            Object[,] roomChar = new Object[size.x, size.y];

            for (int y = 0; y < FloorPlan.GetLength(1); y++)
            {
                roomChar[0, y] = new Object(0, y, '|');
                for (int x = 0; x < FloorPlan.GetLength(0); x++)
                {
                    if (WallPlan[x, y])
                    {
                        roomChar[x, y] = new Object(x, y, Constants.WALL);
                    }
                    else if (FloorPlan[x, y])
                    {
                        roomChar[x, y] = new Object(x, y, Constants.FLOOR);
                    }
                    else
                    {
                        roomChar[x, y] = new Object(x, y, Constants.SPACE);
                    }
                }
                roomChar[FloorPlan.GetLength(0)-1, y] = new Object(FloorPlan.GetLength(0)-1, y, '|');
            }

            return roomChar;
        }
    }
}
