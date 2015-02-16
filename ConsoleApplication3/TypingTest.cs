using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asciigame
{
    class TypingTest : GameMode
    {
        private string text;

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
            loadText();
            displayText();
        }

        public override void Update()
        {
            base.Update();
        }

        private void loadText()
        {
            text = "A typewriter is a mechanical or electromechanical machine for writing in characters similar to those produced by printer's movable type by means of keyboard-operated types striking a ribbon to transfer ink or carbon impressions onto the paper. Typically one character is printed per keypress. The machine prints characters by making ink impressions of type elements similar to the sorts used in movable type letterpress printing.";
        }

        private void displayText()
        {
            Console.WriteLine(text.ToCharArray(), 0, game.bufferSize.x);
        }
    }
}
