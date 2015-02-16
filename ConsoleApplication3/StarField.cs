using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Asciigame
{
    class StarField : GameMode
    {
        Star[] stars;
        int numberOfStars = 100;

        private static StarField _instance;
        public static StarField Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new StarField();
                }
                return _instance;
            }
        }

        public override void Start(Game _game)
        {
            base.Start(_game);
            game.inputOverride = false;
            if (stars == null)
                stars = new Star[numberOfStars];
        }

        public override void Update()
        {
            base.Update();
            if (game.GetKeyDown(System.Windows.Forms.Keys.Escape))
                RequestTermination();

            game.clearBuffer();
            game.currentFrameRate = game.getFrameRate();
            updateStarField();
            game.drawFrameRate();
            game.writeBufferToScreen();
            game.lastFrameTimeStamp = DateTime.Now;
            Thread.Sleep(Math.Max(0, game.desiredTimePerFrame - game.timeSinceLastFrame));
        }

        private void updateStarField()
        {
            for (int i = 0; i < stars.Length; i++)
            {
                updateStarPosition(i);
                drawStar(i);
            }
        }

        private void drawStar(int starNumber)
        {
            Vector2 adjustedPos = stars[starNumber].position;
            adjustedPos.x += game.bufferSize.x / 2;
            adjustedPos.y += game.bufferSize.y / 2;

            //don't draw outside the screen
            if ((int)adjustedPos.x < 0 || (int)adjustedPos.x >= game.bufferSize.x ||
                (int)adjustedPos.y < 0 || (int)adjustedPos.y >= game.bufferSize.y ||
                stars[starNumber].position.magnitude < 1f)
            {
                return;
            }

            // color mode
            //Console.SetCursorPosition((int)adjustedPos.x, (int)adjustedPos.y);
            //if (stars[starNumber].velocity.magnitude < 0.4f) Console.ForegroundColor = ConsoleColor.DarkGray;
            //else
            //    if (stars[starNumber].velocity.magnitude < 0.8f) Console.ForegroundColor = ConsoleColor.Gray;
            //    else
            //        Console.ForegroundColor = ConsoleColor.White;
            //Console.Write(".");

            game.buffer[(int)adjustedPos.y][(int)adjustedPos.x] = '.';
        }

        private void updateStarPosition(int starNumber)
        {
            if (stars[starNumber].position == Vector2.Zero)
            {
                stars[starNumber].velocity = new Vector2(-1f + ((float)game.rnd.NextDouble() * 2f), -1f + ((float)game.rnd.NextDouble() * 2));
                stars[starNumber].position = stars[starNumber].velocity.Normalized() * (float)game.rnd.NextDouble() * 20f;
            }
            stars[starNumber].position += stars[starNumber].velocity;

            if (stars[starNumber].position.x < -Console.WindowWidth / 2 || stars[starNumber].position.x > Console.WindowWidth / 2 ||
                stars[starNumber].position.y < -Console.WindowHeight / 2 || stars[starNumber].position.y > Console.WindowHeight / 2)
            {
                //resetting star to center
                stars[starNumber].position = Vector2.Zero;
            }
        }
    }
}
