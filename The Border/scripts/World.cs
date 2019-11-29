using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace The_Border.scripts
{
    class World
    {
        static char[,] worldData;
        string worldString;

        public World() {
            worldData = new char[Constants.WORLD_WIDTH, Constants.WORLD_HEIGHT];
        }

        // Load the data for the collision map
        public void Initialize()
        {
            // get world map
            if (File.Exists(Constants.DUNGEON_FILE))
            {
                worldString = File.ReadAllText(Constants.DUNGEON_FILE);
            }

            // Check if world file exists
            if(File.Exists(Constants.COLLISION_DATA_FILE))
            {
                StreamReader reader = new StreamReader(Constants.COLLISION_DATA_FILE);
                string line = reader.ReadLine();

                int x = 0, y = 0;
                // loop as long as there's another line
                while (line != null)
                {
                    foreach (char c in line)
                    {
                        // create the world based on the character read in the file
                        // update world data, camera data and program data arrays
                        switch (c)
                        {
                            case Constants.SPACE:
                                worldData[x, y] = Constants.SPACE;
                                Camera.UpdateVisibleMap(new Object(x, y, Constants.SPACE));
                                break;

                            case Constants.WALL:
                                worldData[x, y] = Constants.WALL;
                                Camera.UpdateVisibleMap(new Object(x, y, Constants.WALL));
                                break;

                            case Constants.ENEMY:
                                Enemy tempEnemy = new Enemy(x, y, 20, 1, Constants.ENEMY, new Key(x, y, Constants.KEY_DOOR_COLORS[new Random().Next(0, 3)], "The Key"));

                                worldData[x, y] = Constants.ENEMY;
                                Program.enemies.Add(tempEnemy);
                                Camera.UpdateVisibleMap(tempEnemy);
                                break;

                            case Constants.KEY:
                                Key tempKey = new Key(x, y, ConsoleColor.DarkRed, "The Key");

                                worldData[x, y] = Constants.KEY;
                                Program.items.Add(tempKey);
                                Camera.UpdateVisibleMap(tempKey);
                                break;

                            case Constants.DOOR_VERTICAL:
                            case Constants.DOOR_HORIZONTAL:
                                Door tempDoor = new Door(x, y, c == Constants.DOOR_HORIZONTAL);
                                worldData[x, y] = Constants.DOOR_COLLISION;
                                Program.doors.Add(tempDoor);
                                Camera.UpdateVisibleMap(tempDoor);
                                break;

                            default:
                                break;
                        }

                        x++;
                    }

                    x = 0;
                    y++;
                    line = reader.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("World file not found.");
            }
        }

        // Render everything in the game world
        public void Render()
        {
            Console.SetCursorPosition(0, 0);
            Console.Write(worldString);

            foreach (Item item in Program.items)
            {
                item.Render();
            }

            foreach (Door door in Program.doors)
            {
                door.Render();
            }

            foreach (Enemy enemy in Program.enemies)
            {
                enemy.Render();
            }
        }

        public bool CollidedWithWall(int x, int y)
        {
            // Check if going out of bounds
            if (x < 0 || y < 0)
                return false;
            else
                return worldData[x, y] == Constants.WALL;
        }

        // Check if there is collision in the position specified and call for any necessary action
        public bool CheckCollision(int x, int y, Player player)
        {

            // Program.LogNewLine(worldData[x, y].ToString());
            switch (worldData[x, y])
            {
                case Constants.ENEMY:
                    foreach (Enemy enemy in Program.enemies)
                    {
                        if (enemy.X == x && enemy.Y == y)
                        {
                            // player attack enemy
                            enemy.Damaged(player.GetDamage(), player);
                            return true;
                        }
                    }
                    break;

                case Constants.KEY:
 
                    for (int i = 0; i < Program.items.Count; i++)
                    {
                        if (Program.items[i].X == x && Program.items[i].Y == y)
                        {
                            // put item into inventory if possible
                            player.GetInventory().AddItem(Program.items[i]);

                            return true;
                        }
                    }

                    player.SetPosition(x, y);
                    break;

                case Constants.DOOR_COLLISION:
                    foreach (Door door in Program.doors)
                    {
                        if (door.X == x && door.Y == y)
                        {
                            door.OnCollision(player);
                            return true;
                        }
                    }
                    break;

                case Constants.SPACE:
                    player.SetPosition(x, y);
                    return false;

                default: return true;
            }

            return false;
        }

        // For testing purposes
        public void RenderWorldData()
        {
            for (int y = 0; y < worldData.GetLength(1); y++)
            {
                for (int x = 0; x < worldData.GetLength(0); x++)
                {
                    Console.Write(worldData[x, y]);
                }
                Console.WriteLine();
            }
        }

        public static void UpdateWorldData(int x, int y, char data)
        {
            worldData[x, y] = data;
        }

        public static char[,] GetData()
        {
            return worldData;
        }
    }
}
