using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Asciigame
{
    class RandomJunk : GameMode
    {   
        IntVector2 cursorPosition = new IntVector2();
        byte[] byteBuffer = new byte[4];
        

        public override void Start(Game _game)
        {            
            base.Start(_game);
            game.inputOverride = false;
        }

        public override void Update()
        {
            base.Update();
            if (game.GetKeyDown(System.Windows.Forms.Keys.Escape))
                RequestTermination();

            byte newColor = (byte)game.rnd.Next(0, 255);
            //byte nibble1 = (byte)(newColor & 0x0F);
            //byte nibble2 = (byte)((newColor & 0xF0) >> 4);

            cursorPosition.x = game.rnd.Next(0, Console.WindowWidth - 1);
            cursorPosition.y = game.rnd.Next(0, Console.WindowHeight - 1);
            Console.ForegroundColor = (ConsoleColor)(newColor & 0x0F);
            Console.BackgroundColor = (ConsoleColor)((newColor & 0xF0) >> 4);
            game.rnd.NextBytes(byteBuffer);
            Console.SetCursorPosition(cursorPosition.x, cursorPosition.y);
            Console.Write((char)(byteBuffer[0] + 64));
            //Thread.Sleep(1);
        }
    }
}
