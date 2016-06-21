using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asciigame
{
    class ClickerGame : GameMode
    {
        private static ClickerGame _instance;
        public static ClickerGame Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ClickerGame();
                }
                return _instance;
            }
        }

        public float TickRate = 0.5f;

        private Clicker.Stat stat = new Clicker.Stat("Strength");


        public override void Update()
        {
            base.Update();

            //test toggling trainig
            if (game.GetKeyDown(System.Windows.Forms.Keys.Enter))
            {
                stat.trainingActive = !stat.trainingActive;
            }

            if (game.timeSinceLastFrame/1000f >= TickRate)
            {
                TickUpdate(game.timeSinceLastFrame / 1000f);
                game.lastFrameTimeStamp = DateTime.Now;
            }                        
        }

        private void TickUpdate(float deltaTime)
        {
            Console.Clear();
            stat.increase(deltaTime);
            stat.drawStatBar(0, 0, 10);
        }
    }
}
