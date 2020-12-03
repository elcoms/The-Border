using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TheEndlessBorder.scripts;

namespace TheEndlessBorder.scripts
{
    class World
    {
        static Object[,] worldObjects;

        public static int WorldSeed { get; private set; }
        static Random random = new Random();
        Room spawnRoom = new Room();

        public Vector2 WorldSize { get { return new Vector2(worldObjects.GetLength(0), worldObjects.GetLength(1)); } }

        public World() {
            WorldSeed = random.Next();
            random = new Random(WorldSeed);
        }

        // Load the data for the collision map
        public void Initialize(Player player)
        {
            // Create Spawn Room
            worldObjects = spawnRoom.Generate(random.Next());

            int i = 0;
            bool spawned = false;
            Object spawnPosObject = worldObjects[0, 0];     // fix object being replaced by floor when player changes position
            while (!spawned)
            {
                // random position
                Vector2 playerPos = new Vector2(random.Next(0, WorldSize.x), random.Next(0, WorldSize.y));

                // Spawn only in floor objects
                if (worldObjects[playerPos.x, playerPos.y].GetSprite() == Constants.FLOOR)
                {

                    player.SetPosition(playerPos.x, playerPos.y);
                    spawned = true;
                }
                else if (i > 100) // if random takes too long
                {
                    foreach (bool isFloor in spawnRoom.FloorPlan)
                    {
                        if (isFloor)
                        {
                            player.SetPosition(playerPos.x, playerPos.y);
                            spawned = true;
                            break;
                        }
                    }
                }
            }
            worldObjects[0, 0] = spawnPosObject;

            // Spawn objects
            /*for (int y = 0; y < worldObjects.GetLength(1); y++)
            {
                for (int x = 0; x < worldObjects.GetLength(0); x++)
                {
                    switch (worldObjects[x, y])
                    {
                        case Constants.SPACE:
                        case Constants.WIN_TRIGGER:
                            UpdateWorldObjects(new Object(x, y, Constants.SPACE));
                            break;

                        case Constants.WALL:
                            UpdateWorldObjects(new Object(x, y, Constants.WALL));
                            break;

                        case Constants.FENCE:
                            UpdateWorldObjects(new Object(x, y, Constants.FENCE));
                            break;

                        case Constants.ENEMY:
                            Enemy tempEnemy = new Enemy(x, y, 20, 10, Constants.ENEMY, new Key(x, y, Constants.KEY_DOOR_COLORS[Program.random.Next(0, 3)]));

                            Program.enemies.Add(tempEnemy);
                            UpdateWorldObjects(tempEnemy);
                            break;

                        case Constants.ENEMY_PATROL:
                            PatrolEnemy tempPatrolEnemy = new PatrolEnemy(x, y, 15, 5, Constants.ENEMY_PATROL, new Key(x, y, Constants.KEY_DOOR_COLORS[Program.random.Next(0, 3)]));

                            Program.enemies.Add(tempPatrolEnemy);
                            UpdateWorldObjects(tempPatrolEnemy);
                            break;

                        case Constants.APPLE:
                            HealItem tempApple = new HealItem(x, y, Program.random.Next(5, 21), Constants.APPLE, ConsoleColor.Red, "The Apple",
                                "An Apple a day, keeps The Grave at bay.");

                            Program.items.Add(tempApple);
                            UpdateWorldObjects(tempApple);
                            break;

                        case Constants.KEY:
                            Key tempKey = new Key(x, y, ConsoleColor.DarkRed);

                            Program.items.Add(tempKey);
                            UpdateWorldObjects(tempKey);
                            break;

                        case Constants.DOOR_VERTICAL:
                        case Constants.DOOR_HORIZONTAL:
                            Door tempDoor = new Door(x, y, worldObjects[x, y] == Constants.DOOR_HORIZONTAL);

                            Program.doors.Add(tempDoor);
                            UpdateWorldObjects(tempDoor);
                            break;

                        case Constants.FENCE_WEAK:
                            Door tempFence = new Door(x, y, "The Fragile Fence", ConsoleColor.Gray, true);

                            Program.doors.Add(tempFence);
                            UpdateWorldObjects(new Object(x, y, Constants.FENCE_WEAK));
                            break;

                        default:
                            break;
                    }
                }
            }*/
        }

        public static void CreateNewRoom(Vector2 doorPosition)
        {
            // Generate new Room
            Room newRoom = new Room();
            Object[,] newRoomObjects = newRoom.Generate(random.Next());

            // Insert new room to world and adjust the world accordingly
            // ==============================================================

            // Check Door exit direction to spawn in
            Direction doorDirection = Direction.NULL;

            if (doorPosition.x - 1 >= 0)
                if (worldObjects[doorPosition.x - 1, doorPosition.y]?.GetSprite() == Constants.PLAYER) doorDirection = Direction.LEFT;
            if (doorPosition.x + 1 < worldObjects.GetLength(0))
                if (worldObjects[doorPosition.x + 1, doorPosition.y]?.GetSprite() == Constants.PLAYER) doorDirection = Direction.RIGHT;
            if (doorPosition.y - 1 >= 0)
                if (worldObjects[doorPosition.x, doorPosition.y - 1]?.GetSprite() == Constants.PLAYER) doorDirection = Direction.UP;
            if (doorPosition.y + 1 < worldObjects.GetLength(1))
                if (worldObjects[doorPosition.x, doorPosition.y + 1]?.GetSprite() == Constants.PLAYER) doorDirection = Direction.DOWN;

            // the rect at the side of the room closest to the door
            Rect roomRect = new Rect();
            switch (doorDirection)  
            {
                case Direction.UP: 
                    roomRect = newRoom.TopRect;
                    break;
                case Direction.DOWN:
                    roomRect = newRoom.BottomRect;
                    break;
                case Direction.LEFT:
                    roomRect = newRoom.LeftRect;
                    break;
                case Direction.RIGHT:
                    roomRect = newRoom.RightRect;
                    break;
                default:
                    break;
            }
            
            // merge room and world if direction is not null
            if (doorDirection != Direction.NULL)
            {
                // center position of side rect = half length of side rect + length of rect to room bounds
                Vector2 rectCenter = new Vector2((roomRect.max.x - roomRect.min.x) / 2 + (roomRect.min.x - newRoom.Bounds.min.x),
                                                 (roomRect.max.y - roomRect.min.y) / 2 + (roomRect.min.y - newRoom.Bounds.min.y));

                // Padding amount based on the difference between the new room and world
                Vector2 padding = doorPosition - rectCenter;

                // Expand world based on new room size and padding
                int lengthX = worldObjects.GetLength(0);
                int lengthY = worldObjects.GetLength(1);
                int startPosX = 0;
                int startPosY = 0;

                // World Length X
                // Room is placed after 0
                if (padding.x >= 0)
                {
                    // use the length that is longer: padding + newRoom size or current length
                    lengthX = (padding.x + newRoom.GetRoomSize().x) >= worldObjects.GetLength(0) ? (padding.x + newRoom.GetRoomSize().x) : worldObjects.GetLength(0);
                    startPosX = 0;
                }
                // Room is placed before 0
                else
                {
                    // use the length that is longer: new room length or absolute padding + current length
                    lengthX = (padding.x + newRoom.GetRoomSize().x) >= worldObjects.GetLength(0) ? newRoom.GetRoomSize().x : (worldObjects.GetLength(0) - padding.x);
                    startPosX = -padding.x;
                }

                // World Length Y
                if (padding.y >= 0)
                {
                    lengthY = (padding.y + newRoom.GetRoomSize().y) >= worldObjects.GetLength(1) ? (padding.y + newRoom.GetRoomSize().y) : worldObjects.GetLength(1);
                    startPosY = 0;
                }
                else
                {
                    lengthY = (padding.y + newRoom.GetRoomSize().y) >= worldObjects.GetLength(1) ? newRoom.GetRoomSize().y : (worldObjects.GetLength(1) - padding.y);
                    startPosY = -padding.y;
                }

                Object[,] newWorld = new Object[lengthX, lengthY];

                // Put world into new world
                for (int y = 0; y < lengthY; y++)
                {
                    // Transfer from startpos to world length, else insert space object
                    if (y >= startPosY && y < (startPosY + worldObjects.GetLength(1)))
                    {
                        for (int x = 0; x < lengthX; x++)
                        {
                            if (x >= startPosX && x < (startPosX + worldObjects.GetLength(0)))
                            {
                                newWorld[x, y] = worldObjects[x - startPosX, y - startPosY];
                                newWorld[x, y].SetPositionDirectly(x, y);
                            }
                            else
                                newWorld[x, y] = new Object(x, y, Constants.SPACE);
                        }
                    }
                    else
                    {
                        for (int x = 0; x < lengthX; x++)
                        {
                            newWorld[x, y] = new Object(x, y, Constants.SPACE);
                        }
                    }
                }

                newWorld.GetLength(1);
                worldObjects.GetLength(0);
                startPosX = padding.x >= 0 ? padding.x : 0;
                startPosY = padding.y >= 0 ? padding.y : 0;

                // Put room into new world but only create in empty spaces
                for (int y = startPosY; y < lengthY; y++)
                {
                    // if index is more than room length, break out of the loop
                    if (y - startPosY >= newRoom.GetRoomSize().y)
                        break;

                    for (int x = startPosX; x < lengthX; x++)
                    {
                        // if index is more than room length, break out of the loop
                        if (x - startPosX < newRoom.GetRoomSize().x)
                        {
                            if (newWorld[x, y].GetSprite() == Constants.SPACE)
                            {
                                newWorld[x, y] = newRoomObjects[x - startPosX, y - startPosY];
                                newWorld[x, y].SetPositionDirectly(x, y);
                            }
                        }
                        else
                            break;
                    }
                }

                worldObjects = newWorld;
            }
            // Block the path if room is invalid
            else
                worldObjects[doorPosition.x, doorPosition.y] = new Object(doorPosition.x, doorPosition.y, Constants.WALL);
        }

        // Getter/Setters
        // Return data specified by position
        public static Object GetObjectFromPosition(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < worldObjects.GetLength(0) && y < worldObjects.GetLength(1))
                return worldObjects[x, y];
            else
                return new Object(0, 0, Constants.UNKNOWN);
        }

        public static void UpdateWorldObjects(Object data)
        {
            worldObjects[data.X, data.Y] = data;
        }

        public static Object[,] GetWorldObjects()
        {
            return worldObjects;
        }
    }
}
