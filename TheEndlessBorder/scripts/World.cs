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

        int worldSeed;
        Random random = new Random();
        Room spawnRoom = new Room();

        public Vector2 WorldSize { get { return new Vector2(worldObjects.GetLength(0), worldObjects.GetLength(1)); } }

        public World() {
            worldSeed = random.Next();
            random = new Random(worldSeed);
        }

        // Load the data for the collision map
        public void Initialize(Player player)
        {
            // Create Spawn Room
            worldObjects = spawnRoom.Generate(random.Next(), Constants.WallPatterns);

            /*int i = 0;
            bool spawned = false;
            while (!spawned)
            {
                // random position
                Vector2 playerPos = new Vector2(random.Next(0, WorldSize.x), random.Next(0, WorldSize.y));

                if (spawnRoom.FloorPlan[playerPos.x, playerPos.y])
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
            }*/
            player.SetPosition(WorldSize.x / 2, WorldSize.y / 2);
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

        /*public void CreateNewRoom()
        {
            var rooms = new List<Room>();
            Room room;
            room = new Room();
            room.Generate(random.Next());
            rooms.Add(room);

            Vector2 center = new Vector2();
        }*/

        // Getter/Setters
        // Return data specified by position
        public static Object GetObjectFromPosition(int x, int y)
        {
            return worldObjects[x, y];
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
