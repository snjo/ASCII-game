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
        public float difficultyMultiplier = 10f;
        public float progressInSeconds = 0f;
        public int multiplier = 1;
        public bool trainingActive = false;
        public int timeSlots = 1;

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
            drawStatBar(x, y, length, ConsoleColor.White);
        }

        public void drawStatBar(int x, int y, int length, ConsoleColor textColor)
        {
            drawStatBar(x, y, length, textColor, ConsoleColor.DarkGreen, ConsoleColor.Gray);
        }

        public void drawStatBar(int x, int y, int length, ConsoleColor textColor, ConsoleColor barColorActive, ConsoleColor barColorPassive)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = textColor;
            Console.SetCursorPosition(x, y);
            Console.Write(name.PadRight(16));
            int blocksToFill = (int)(progressNormalized * (length * 1.1f));  //adding a bit to length so that the last block get used.
            //string time = ((int)secondsToNextLevel).ToString() + "s";            
            string time = Tools.secToTimeString(secondsToNextLevel);
            time = time.PadLeft((length / 2) + time.Length/2, '█');
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
                        if (trainingActive)
                            Console.ForegroundColor = barColorActive;
                        else
                            Console.ForegroundColor = barColorPassive;
                    }
                    else
                    {
                        if (trainingActive)
                            Console.BackgroundColor = barColorActive;
                        else
                            Console.BackgroundColor = barColorPassive;
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

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(level.ToString().PadLeft(3));
            //Console.Write(time);
        }
    }

    
}
