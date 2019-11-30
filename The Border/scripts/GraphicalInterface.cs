using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace The_Border.scripts
{
    class GraphicalInterface
    {
        // string text;
        struct Pixel
        {
            public int x;
            public int y;
            public char c;

            public Pixel(int xPos, int yPos, char sprite)
            {
                x = xPos;
                y = yPos;
                c = sprite;
            }
        }

        Pixel[] text;
        public GraphicalInterface()
        {
            text = new Pixel[1000];
        }

        // save the text from the graphical interace file
        public void Initialize()
        {
            if (File.Exists(Constants.GRAPHICAL_INTERFACE_FILE))
            {
                StreamReader reader = new StreamReader(Constants.GRAPHICAL_INTERFACE_FILE);

                int count = 0, x = 0, y = 0;
                string line = reader.ReadLine();
                while (line != null)
                {
                    foreach (char c in line)
                    {
                        if (c != Constants.SPACE)
                        {
                            text[count] = new Pixel(x, y, c);
                            count++;
                        }

                        x++;
                    }
                    x = 0;
                    y++;
                    line = reader.ReadLine();
                }

                text[count] = new Pixel(-1, -1, Constants.SPACE);
            }
        }

        public void Render()
        {
            foreach (Pixel pixel in text)
            {
                if (pixel.c == Constants.SPACE)
                    break;

                Console.SetCursorPosition(pixel.x, pixel.y);
                Console.WriteLine(pixel.c);
            }
        }
    }
}
