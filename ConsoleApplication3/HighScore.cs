using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Asciigame
{
    class HighScore
    {
        public Dictionary<string, int> scores = new Dictionary<string,int>();


        public bool updateScore(string textName, int newScore)
        {
            int oldScore = 0;

            if (scores.ContainsKey(textName))
            {
                oldScore = scores[textName];
                scores[textName] = Math.Max(newScore, oldScore);
            }
            else
            {
                scores.Add(textName, newScore);
            }

            return newScore > oldScore;
        }

        public int getScore(string textName)
        {
            if (scores.ContainsKey(textName))
            {
                return (scores[textName]);
            }
            else
            {
                return 0;
            }
        }

        public bool readScoresFromFile()
        {
            if (File.Exists(TypingTest.path + "\\scores.txt"))
            {
                Debug.WriteLine("scores file exists");
                return true;
            }
            else
            {
                Debug.WriteLine("No scores file");
                return false;
            }
        }
    }
}
