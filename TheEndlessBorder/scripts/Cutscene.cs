using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace The_Border.scripts
{
    class Cutscene
    {
        private string[] foregroundText;
        private string backgroundText;
        private int foregroundStartLine = 0;

        public Cutscene() 
        {
            foregroundText = new string[Constants.WINDOW_HEIGHT];
        }

        public void Initialize()
        {
            backgroundText = File.ReadAllText(Constants.BACKGROUND_FILE);

            StreamReader reader = new StreamReader(Constants.FOREGROUND_FILE);

            int count = 0;
            string line = reader.ReadLine();
            while(line != null && count < foregroundText.Length)
            {
                foregroundText[count] = line;
                line = reader.ReadLine();
                count++;
            }
        }

        public bool Play()
        {
            if (foregroundStartLine < 30)
            {
                Console.Clear();
                Console.Write(backgroundText);

                Console.SetCursorPosition(0, foregroundStartLine);
                for (int i = 0; i < foregroundText.Length - foregroundStartLine; i++)
                {
                    Console.WriteLine(foregroundText[i]);
                }

                foregroundStartLine++;
                Thread.Sleep(100);
                return true;
            }
            else
            {
                Thread.Sleep(2000);
                Console.Clear();
                // The Man is Free
                Console.SetCursorPosition(Constants.WINDOW_WIDTH / 3, Constants.WINDOW_HEIGHT / 3);
                Console.Write("   The Man is Free :)   ");

                Thread.Sleep(2000);
                Console.SetCursorPosition(Constants.WINDOW_WIDTH / 3, (Constants.WINDOW_HEIGHT / 3) + 1);
                Console.Write("   Thanks for playing.  ");
            }

            return false;
        }
    }
}
