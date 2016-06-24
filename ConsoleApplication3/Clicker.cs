using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Asciigame.Clicker;

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

        public Dictionary<string, Clicker.Stat> stats = new Dictionary<string, Clicker.Stat>();
        private int selectedItem = 0;
        public int timeSlots = 3;
        public int timeSlotsAvailable = 3;

        //private Clicker.Stat stat = new Clicker.Stat("Fitness");        

        public enum StatNames
        {
            Fitness,
            Charm,
            Smarts,
            Perception,
            Luck
        }
        public int numberOfStats;

        public override void Start(Game _game)
        {
            base.Start(_game);
            numberOfStats = Enum.GetNames(typeof(StatNames)).Length;
            for (int i = 0; i < numberOfStats; i++)
            {
                addStat(((StatNames)i).ToString());
            }            

        }

        public override void Update()
        {
            base.Update();
            bool redrawNow = false;

            if (game.GetKeyDown(System.Windows.Forms.Keys.Up))
            {
                selectedItem--;
                if (selectedItem < 0)
                    selectedItem = numMenuItems-1;
                redrawNow = true;
            }
            if (game.GetKeyDown(System.Windows.Forms.Keys.Down))
            {
                selectedItem++;
                if (selectedItem >= numMenuItems)
                    selectedItem = 0;
                redrawNow = true;
            }

            //test toggling trainig
            if (game.GetKeyDown(System.Windows.Forms.Keys.Enter))
            {
                //toggle selected item
                Stat stat = stats[((StatNames)selectedItem).ToString()];
                toggleStatTraining(stat);                
                redrawNow = true;
            }

            if (game.timeSinceLastFrame/1000f >= TickRate || redrawNow)
            {
                TickUpdate(game.timeSinceLastFrame / 1000f);
                game.lastFrameTimeStamp = DateTime.Now;
            }                        
        }

        private void TickUpdate(float deltaTime)
        {
            Console.Clear();
            int i = 0;
            foreach (Stat stat in stats.Values)
            {
                ConsoleColor textColor = ConsoleColor.Gray;
                if (selectedItem == i)
                    textColor = ConsoleColor.White;
                stat.increase(deltaTime);
                stat.drawStatBar(0, i, 10, textColor);
                i++;
            }
            Console.WriteLine();
            Console.WriteLine("Time slots:" + timeSlotsAvailable);
        }

        public bool toggleStatTraining(Stat stat)
        {
            if (stat.trainingActive)
            {
                stat.trainingActive = false;
                timeSlotsAvailable += stat.timeSlots;
                //should recalculate time slots
                return false;
            }
            else
            {
                if (timeSlotsAvailable >= stat.timeSlots)
                {
                    stat.trainingActive = true;
                    timeSlotsAvailable -= stat.timeSlots;
                    //should recalculate time slots
                    return true;
                }                
                else
                {
                    stat.trainingActive = false;
                    //should recalculate time slots
                    return false;
                }
            }            
        }

        private void addStat(string name)
        {
            stats.Add(name, new Clicker.Stat(name));
        }

        private int numMenuItems
        {
            get
            {
                return stats.Count;
            }
        }
    }
}
