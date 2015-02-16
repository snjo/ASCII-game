using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asciigame
{
    class TypingTest : GameMode
    {
        private string text;
        private List<string> textLines;

        private int currentCharPos = 0;
        private int currentLinePos = 0;

        private int paddingTop = 2;

        DateTime clock = new DateTime();
        bool clockRunning = false;
        double secondsSinceStart = 0;

        private static TypingTest _instance;
        public static TypingTest Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TypingTest();
                }
                return _instance;
            }
        }

        public override void Start(Game _game)
        {
            base.Start(_game);
            Console.Clear();
            Console.SetCursorPosition(0, paddingTop);
            loadText();
            formatText();
            displayText();
            Console.ReadKey(false);
        }

        public override void Update()
        {
            base.Update();
            if (clockRunning) updateClock();
            updateStatusText();
            if (exitGameMode) return;
            char key = Console.ReadKey(true).KeyChar;
            char expectedKey = getExpectedKey(currentCharPos);            
            if (key == expectedKey)
            {
                if (atEndOfText)
                {
                    stopClock();
                    return;
                }
                Console.SetCursorPosition(currentCharPos, currentLinePos + paddingTop);
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.Write(key);

                typeHeadForward();
                if (!clockRunning)
                {
                    Debug.Write("Starting Clock");
                    startClock();
                }
            }
            else if (key == (char)8)
            {
                typeHeadBack();
                Console.BackgroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(currentCharPos, currentLinePos + paddingTop);
                Console.Write(getExpectedKey(currentCharPos));                    
            }
            else if (key == (char)27)
            {
                stopClock();
                RequestTermination();
            }
            else
            {
                if (atEndOfText)
                {
                    stopClock();
                    return;
                }
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.SetCursorPosition(currentCharPos, currentLinePos + paddingTop);
                Console.Write(expectedKey);
                typeHeadForward();
            }
        }

        private bool atEndOfText
        {
            get
            {
                return (currentLinePos == textLines.Count - 1 && currentCharPos == textLines[currentLinePos].Length - 1);
            }
        }

        private void typeHeadForward()
        {
            currentCharPos++;
            if (currentCharPos > textLines[currentLinePos].Length - 1)
            {
                if (currentLinePos < textLines.Count - 1)
                {
                    currentCharPos = 0;
                    currentLinePos++;
                }
                else
                    currentCharPos--;
            }
        }

        private void typeHeadBack()
        {
            currentCharPos--;
            if (currentCharPos < 0)
            {
                if (currentLinePos > 0)
                {
                    currentLinePos--;
                    currentCharPos = textLines[currentLinePos].Length - 1;
                }
                else
                {
                    currentCharPos = 0;
                }
            }
        }

        private char getExpectedKey(int position)
        {
            char expectedKey = textLines[currentLinePos].Substring(position, 1).ToCharArray()[0];
            return expectedKey;
        }

        private void loadText() // make sure the text has a trailing space
        {
            //text = "A typewriter is a mechanical or electromechanical machine for writing in characters similar to those produced by printer's movable type by means of keyboard-operated types striking a ribbon to transfer ink or carbon impressions onto the paper. Typically one character is printed per keypress. The machine prints characters by making ink impressions of type elements similar to the sorts used in movable type letterpress printing. ";
            text = "A typewriter is a mechanical or electromechanical machine for writing in characters similar to those produced by printer's movable type by means of keyboard-operated types striking a ribbon to transfer ink. ";
        }

        private void formatText()
        {
            textLines = new List<string>();
            int bufferWidth = 62;//game.bufferSize.x;
            for (int i = 0; i < text.Length; )
            {
                string finalString = string.Empty;
                int adjustedWidth = bufferWidth;
                char[] line;
                if (text.Length > i + bufferWidth)
                    line = text.Substring(i, bufferWidth).ToCharArray();
                else
                    line = text.Substring(i).ToCharArray();

                for (int j = line.Length - 1; j >= 0; j--)
                {
                    Debug.WriteLine("i = " + i + ", j = " + j);
                    if (line[j] == ' ' || line[j] == ',' || line[j] == '.' || line[j] == '-')
                    {
                        adjustedWidth = j+1;
                        finalString = new string(line).Substring(0, adjustedWidth); ;
                        break;
                    }
                }
                if (adjustedWidth < 1) adjustedWidth = bufferWidth;
                i += adjustedWidth;
                //int width = bufferWidth;
                //if (i + adjustedWidth > text.Length)
                //    adjustedWidth = text.Length % adjustedWidth;
                textLines.Add(finalString);
            }
        }

        private void displayText()
        {
            for (int i = 0; i < textLines.Count; i++)
            {
                Console.WriteLine(textLines[i]);
            }
        }

        private void startClock()
        {
            clock = DateTime.Now;            
            clockRunning = true;
            secondsSinceStart = 0;
        }

        private void stopClock()
        {
            clockRunning = false;
        }

        private void updateClock()
        {
            secondsSinceStart = (DateTime.Now - clock).TotalSeconds;
        }

        private void updateStatusText()
        {
            Console.SetCursorPosition(0, 0);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("WPM: " + GetWordsPerMinute() + ", Chars written: " + GetCharactersWritten() + "               ");
        }

        private int GetWordsPerMinute()
        {
            Debug.WriteLine(clock + " / " + secondsSinceStart);
            if (secondsSinceStart > 1)
                return (int)((GetCharactersWritten() / 5) / (secondsSinceStart / 60d));
            else return 0;
        }

        private int GetCharactersWritten()
        {
            int result = 0;
            for (int i = 0; i < currentLinePos; i++)
            {
                result += textLines[i].Length;
            }
            result += currentCharPos;
            return result;
        }
    }
}
