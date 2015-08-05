﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Asciigame
{
    class TypingTest : GameMode
    {
        public string text;
        public int selectedTextNumber = 0;
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
            userSelectText();

            resetCounters();
            base.Start(_game);
            Console.Clear();
            Console.SetCursorPosition(0, paddingTop);
            loadText(selectedTextNumber);
            formatText();
            displayText();
            //Console.ReadKey(false);
        }

        private void resetCounters()
        {
            currentCharPos = 0;
            currentLinePos = 0;
            secondsSinceStart = 0;
            clockRunning = false;
        }

        private void userSelectText()
        {
            Console.Clear();
            Console.WriteLine("Select Text");
            Console.ReadKey();
            Console.WriteLine();
            for (int i = 0; i < 100; i++)
            {
                string newText = loadText(i);
                if (newText == string.Empty)
                    break;
                Console.WriteLine(i + ": " + newText.Substring(0,30) + "...");
            }

            string userInput = Console.ReadLine();
            int selection = 0;
            if (int.TryParse(userInput, out selection))
            {
                selectedTextNumber = selection;
            }
        }

        public override void Update()
        {
            base.Update();

            if (clockRunning) updateClock();
            updateStatusText();
            if (exitGameMode) return;
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            char key = keyInfo.KeyChar;
            
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

        private string loadText(int textNumber) // make sure the text has a trailing space
        {
            switch (textNumber)
            {
                case 0:
                    text = "A typewriter is a mechanical or electromechanical machine for writing in characters similar to those produced by printer's movable type by means of keyboard-operated types striking a ribbon to transfer ink. ";
                    break;
                case 1:
                    text = "Fare for vådeskudd. Det at politiet tar ladegrep når de skal gå inn i en skarp situasjon og dermed har et skuddklart våpen, er jo helt greit. Men at politiet skal gå rundt med skuddklare våpen i Oslos og Bergens gater i påvente av at noe dramatisk skal skje, er å gå altfor langt. Til det er faren for vådeskudd og uheldige hendelser altfor høy, sier Berg-Knutsen. ";
                    break;
                case 2:
                    text = "Ber politiet endre praksis. Nå viser det seg i tillegg at en politimann i april i år avfyrte et vådeskudd inne på politistasjonen i Kristiansund. Det er VG som melder dette tirsdag kveld. Hendelsen er under etterforskning av spesialenheten for politisaker. ";
                    break;
                case 3:
                    text = "\"The Brink\" er en ny HBO-komiserie som fokuserer på en geopolitisk krise og dens effekt på tre uforenelige og desperate menn: Walter Larson, USAs utenriksminister, Alex Talbott, en diplomat stasjonert i Islamabad, og Zeke \"Z-Pak\" Tilson, en jagerpilot med en lukrativ sidegeskjeft hvor han selger reseptbelagte medisiner. Denne episke mørke komiserien begynner med at det er fare for kupp i Pakistan. En kjeltring av en general tar kontroll over landet og atomvåpnene der, noe som gjør at verden må stole på disse tre utypiske amerikanske heltene. Fra de turbulente gatene i Midtøsten til Det Hvite Hus og et krigsskip i Rødehavet: \"The Brink\" tar oss med på en vill reise gjennom mange tidssoner for å vise oss hvordan svakhetene, egoene og rivaliseringen til politiske ledere kan føre oss til randen av en ny verdenskrig. ";
                    break;
                default:
                    text = string.Empty;
                    break;
            }
            return text;
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
