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
        
        List<Door> doors;
        List<Enemy> enemies;

        public World() {
            worldData = new char[Constants.WORLD_WIDTH, Constants.WORLD_HEIGHT];
            doors = new List<Door>();
            enemies = new List<Enemy>();
        }

        // Load the data for the game world
        public void Initialize()
        {
            // Check if world file exists
            if(File.Exists(Constants.DUNGEON_FILE))
            {
                StreamReader reader = new StreamReader(Constants.DUNGEON_FILE);
                string line = reader.ReadLine();

                int x = 0, y = 0;
                // loop as long as there's another line
                while (line != null)
                {
                    worldString += line + Environment.NewLine;

                    foreach (char c in line)
                    {
                        // assign each character in the world to a 2D array according to x and y
                        switch (c)
                        {
                            case Constants.WALL:
                                worldData[x, y] = Constants.WALL;
                                break;

                            case Constants.DOOR:
                                worldData[x, y] = Constants.DOOR;
                                doors.Add(new Door(x, y));
                                break;

                            case Constants.ENEMY:
                                worldData[x, y] = Constants.ENEMY;
                                enemies.Add(new Enemy(x, y, 10, 1));
                                break;

                            case Constants.SPACE:
                                worldData[x, y] = Constants.SPACE;
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
            Console.Write(worldString);

            foreach (Door door in doors)
            {
                door.Render();
            }

            foreach (Enemy enemy in enemies)
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
            switch (worldData[x, y])
            {
                case Constants.DOOR:
                    foreach (Door door in doors)
                    {
                        if (door.X == x && door.Y == y)
                        {
                            door.OnCollision(player);
                            return true;
                        }
                    }
                    break;

                case Constants.ENEMY:
                    foreach (Enemy enemy in enemies)
                    {
                        if (enemy.X == x && enemy.Y == y)
                        {
                            // player attack enemy
                            enemy.ReduceHealth(player.Attack());
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
            for (int y = 0; y < worldData.GetLength(0); y++)
            {
                for (int x = 0; x < worldData.GetLength(1); x++)
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
    }
}
