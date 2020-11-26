﻿using System;
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

        int wallCount;
        List<uint> wallPatterns = new List<uint>();
        List<uint> doorPatterns = new List<uint>();
        
        // Plans
        public bool[,] FloorPlan { get; private set; }
        public bool[,] WallPlan { get; private set; }
        public bool[,] DoorPlan { get; private set; }

        Vector2 size;
        public Vector2 GetRoomSize() { return size; }

        public Room()
        {
            wallPatterns = new List<uint>(Constants.WallPatterns);
            GeneratePatterns(wallPatterns, Constants.WallPatterns);

            doorPatterns = new List<uint>(Constants.DoorPatterns);
            GeneratePatterns(doorPatterns, Constants.DoorPatterns);
        }

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
                        uint n = (FloorPlan[x, y]) ? 1u : 0u;   // 0 is space, 1 is floor
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

        public void GeneratePatterns(List<uint> patternsList, List<uint> patternsRef)
        {
            foreach (var pattern in patternsRef)
            {
                uint dec = pattern;

                // mirror
                for (int m = 0; m < 2; m++)
                {
                    // rotate
                    for (int r = 0; r < 4; r++)
                    {
                        // loop through current patterns, if not found add into current patterns
                        bool bFound = false;
                        for (int i = 0; i < patternsList.Count; i++)
                        {
                            if (patternsList[i] == dec)
                            {
                                bFound = true;
                                break;
                            }
                        }

                        if (!bFound)
                        {
                            patternsList.Add(dec);
                        }

                        // Rotate
                        dec = RotateDec90(dec);
                    }

                    // Mirror
                    dec = MirrorDec(dec);
                }
            }
        }

        public Object[,] Generate(int seed)
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
            DoorPlan = new bool[size.x, size.y];

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

            // Generate Wall and Doors plan
            wallCount = 0;
            for (int y = 0; y < WallPlan.GetLength(1); y++)
            {
                for (int x = 0; x < WallPlan.GetLength(0); x++)
                {
                    // Check if is wall
                    uint currentPattern = ConvertToDec(x, y);
                    foreach (uint pattern in wallPatterns)
                    {
                        if (currentPattern == pattern)
                        {
                            WallPlan[x, y] = true;
                            wallCount++;

                            // check if it is part of door patterns
                            foreach (uint doorPattern in doorPatterns)
                            {
                                if (currentPattern == doorPattern)
                                {
                                    DoorPlan[x, y] = true;
                                    break;
                                }
                            }

                            break;
                        }
                    }
                }
            }

            return ConvertToObjects();
        }

        Object[,] ConvertToObjects()
        {
            Object[,] roomObjects = new Object[size.x, size.y];
            Random random = new Random(World.WorldSeed);

            Enemy firstEnemySpawned = null;
            int doorCount = 0;
            int randomDoorCount = random.Next(0, wallCount);
            ConsoleColor firstDoorColor = Constants.KEY_DOOR_COLORS[0];
            for (int y = 0; y < FloorPlan.GetLength(1); y++)
            {
                // roomObjects[0, y] = new Object(0, y, '|');
                for (int x = 0; x < FloorPlan.GetLength(0); x++)
                {
                    if (WallPlan[x, y])
                    {
                        // randomly generate door up to a limit but only if it can be a door
                        if (doorCount < Constants.NUM_OF_DOORS && DoorPlan[x, y] && randomDoorCount <= 0)
                        {
                            roomObjects[x, y] = new Door(x, y, IsDoorHorizontal(x, y),                          // horizontal if floor is to the left or right of the door
                                Constants.KEY_DOOR_COLORS[random.Next(0, Constants.KEY_DOOR_COLORS.Length)]);   // random color

                            doorCount++;
                            randomDoorCount = random.Next(0, wallCount);
                        }
                        else
                        {
                            roomObjects[x, y] = new Object(x, y, Constants.WALL);
                            randomDoorCount--;
                        }
                    }
                    else if (FloorPlan[x, y])
                    {
                        // 1% Chance of spawning an enemy in each tile for first enemy, 0.1% after
                        if (firstEnemySpawned == null && random.NextDouble() < 0.01)
                        {
                            // 70% chance for normal enemy to spawn
                            firstEnemySpawned = random.NextDouble() < 0.7 ?
                                new Enemy(x, y , 20, 10, Constants.ENEMY, new Key(x, y, Constants.KEY_DOOR_COLORS[0])) :
                                new PatrolEnemy(x, y, 15, 5, Constants.ENEMY_PATROL, new Key(x, y, Constants.KEY_DOOR_COLORS[0]));

                            roomObjects[x, y] = firstEnemySpawned;
                        }
                        else if (firstEnemySpawned != null && random.NextDouble() < 0.001)
                        {
                            Item randomItem = new Key(x, y, Constants.KEY_DOOR_COLORS[Program.random.Next(0, 3)]);
                            
                            // 10% chance for an apple to spawn instead
                            if (random.NextDouble() < 0.1)
                            {
                                randomItem = new HealItem(x, y, random.Next(15, 75), Constants.APPLE, ConsoleColor.Red, "The Apple", "An Apple a day, keeps The Grave at bay.");
                            }

                            roomObjects[x, y] = random.NextDouble() < 0.7 ?
                                new Enemy(x, y, 20, 10, Constants.ENEMY, randomItem) :
                                new PatrolEnemy(x, y, 15, 5, Constants.ENEMY_PATROL, randomItem);
                        }
                        else
                            roomObjects[x, y] = new Object(x, y, Constants.FLOOR);
                    }
                    else
                    {
                        roomObjects[x, y] = new Object(x, y, Constants.SPACE);
                    }
                }
                // roomObjects[FloorPlan.GetLength(0)-1, y] = new Object(FloorPlan.GetLength(0)-1, y, '|');
            }

            // prevent soft lock by giving the first enemy to the first door color
            firstEnemySpawned.SetItem(new Key(0, 0, firstDoorColor));
            return roomObjects;
        }

        bool IsDoorHorizontal(int x, int y)
        {
            // check if within top and btm borders
            if (y > 0 && y < WallPlan.GetLength(1) - 1)
            {
                // check if within side borders
                if (x > 0 && x < WallPlan.GetLength(0) - 1)
                {
                    return !(WallPlan[x, y + 1] || WallPlan[x, y - 1]);  // is horizontal if top or bottom is not a wall
                }
                // Vertical Only
                else
                {
                    return false;
                }
            }
            // Horizontal Only
            else
            {
                return true;
            }
        }
    }
}