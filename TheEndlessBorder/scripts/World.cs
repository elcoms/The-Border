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

                Queue<Object> missingObjects = new Queue<Object>();
                int count = random.Next(0, newRoom.GetRoomSize().x);
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
                            // Store object to put elsewhere later if cannot place object
                            if (newWorld[x, y].GetSprite() == Constants.SPACE)
                            {
                                // place missing objects if there are any
                                if (missingObjects.Count > 0 && count <= 0)
                                {
                                    char sprite = missingObjects.Peek().GetSprite();
                                    // if original object is supposed to be a wall, check if missing object is a door
                                    if (newRoomObjects[x - startPosX, y - startPosY].GetSprite() == Constants.WALL)
                                    {
                                        bool placeObject = false;
                                        // place object according to direction
                                        if (sprite == Constants.DOOR_HORIZONTAL)
                                        {
                                            if (y - startPosY + 1 == newRoomObjects.GetLength(1) || y - startPosY - 1 == -1)
                                                placeObject = true;
                                            else if (newRoomObjects[x - startPosX, y - startPosY + 1].GetSprite() == Constants.SPACE ||
                                            newRoomObjects[x - startPosX, y - startPosY - 1].GetSprite() == Constants.SPACE)
                                                placeObject = true;
                                        }
                                        else if (sprite == Constants.DOOR_VERTICAL)
                                        {
                                            if (x - startPosX + 1 == newRoomObjects.GetLength(0) || x - startPosX - 1 == -1)
                                                placeObject = true;
                                            else if (newRoomObjects[x - startPosX, y - startPosY + 1].GetSprite() == Constants.SPACE ||
                                            newRoomObjects[x - startPosX, y - startPosY - 1].GetSprite() == Constants.SPACE)
                                                placeObject = true;
                                        }

                                        if (placeObject)
                                        {
                                            count = random.Next(0, newRoom.GetRoomSize().x / missingObjects.Count);
                                            newWorld[x, y] = missingObjects.Dequeue();
                                        }
                                        else
                                            newWorld[x, y] = newRoomObjects[x - startPosX, y - startPosY];
                                    }
                                    else if ((sprite != Constants.DOOR_VERTICAL || sprite != Constants.DOOR_HORIZONTAL) && 
                                            newRoomObjects[x - startPosX, y - startPosY].GetSprite() == Constants.FLOOR)
                                    {
                                        count = random.Next(0, newRoom.GetRoomSize().x / missingObjects.Count);
                                        newWorld[x, y] = missingObjects.Dequeue();
                                    }
                                    else
                                        newWorld[x, y] = newRoomObjects[x - startPosX, y - startPosY];

                                    newWorld[x, y].SetPositionDirectly(x, y);
                                }
                                else
                                {
                                    newWorld[x, y] = newRoomObjects[x - startPosX, y - startPosY];
                                    newWorld[x, y].SetPositionDirectly(x, y);
                                    count--;
                                }
                            }
                            else
                            {
                                char sprite = newRoomObjects[x - startPosX, y - startPosY].GetSprite();
                                if (!(sprite == Constants.SPACE || sprite == Constants.FLOOR || sprite == Constants.WALL))
                                    missingObjects.Enqueue(newRoomObjects[x - startPosX, y - startPosY]);
                                count--;
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
