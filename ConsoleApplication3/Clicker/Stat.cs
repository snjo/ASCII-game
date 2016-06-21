using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asciigame.Clicker
{
    class Stat
    {
        public string name = "Unnamed Stat";
        public int level = 0;
        public float baseTimeToFirstLevel = 5f;
        public float basetimeToNextLevel = 5f;
        public float difficultyMultiplier = 1.25f;
        public float progressInSeconds = 0f;
        public int multiplier = 1;
        public bool trainingActive = false;

        public Stat(string name)
        {
            this.name = name;
        }

        public void increase(float seconds)
        {
            if (trainingActive)
            {
                float secondsToProcess = seconds;
                //Console.WriteLine("sec to Process: " + secondsToProcess);
                while (secondsToProcess > 0f)
                {
                    if (secondsToProcess < secondsToNextLevel)
                    {
                        progressInSeconds += secondsToProcess;
                        //Console.WriteLine("adding: " + secondsToProcess);
                        secondsToProcess = 0f;
                    }
                    else
                    {
                        secondsToProcess -= secondsToNextLevel;
                        //Console.WriteLine("removing: " + secondsToProcess + " from " + secondsToProcess);
                        levelUp();
                    }
                }
            }
        }

        public float secondsToNextLevel
        {
            get
            {
                return basetimeToNextLevel - progressInSeconds;
            }
        }

        public void levelUp()
        {
            //Console.WriteLine("level up" + name);
            level += 1;
            progressInSeconds = 0f;
            basetimeToNextLevel *= difficultyMultiplier;
        }

        public float percentProgress
        {
            get
            {
                return (progressInSeconds / basetimeToNextLevel) * 100f;
            }
        }

        public float progressNormalized
        {
            get
            {
                return (progressInSeconds / basetimeToNextLevel);
            }
        }



        public void drawStatBar(int x, int y, int length)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(x, y);
            Console.Write(name.PadRight(16));
            int blocksToFill = (int)(progressNormalized * (length * 1.1f));  //adding a bit to length so that the last block get used.
            string time = ((int)secondsToNextLevel).ToString();            
            time = time.PadLeft(length / 2, '█');
            time = time.PadRight(length, '█');
            char[] timeChars = time.ToCharArray();

            int i = 0;
            foreach (char c in timeChars)
            {
                if (i < blocksToFill)
                {
                    if (c == '█')
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                else
                {
                    if (c == '█')
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                Console.Write(c);
                i++;
            }
            //Console.Write(time);
        }
    }

    
}
