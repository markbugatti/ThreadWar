using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ThreadWar.Helpers;
using Timer = System.Threading.Timer;

namespace ThreadWar
{
    class EnemyFactory
    {
        private List<Enemy> Enemies { get; set; } = new List<Enemy>();
        private object threadLock = new object(); 
        public void start()
        {
            /* create timer to generate enemy and move it
            * create timer for enemy creating
            * creat timer for enemy moving. this timer should change its elapsed timer
            */
            // create first enemy without timer
            
            Enemies.Add(new Enemy());
            Random r = new Random();
            // разные таймеры используют один и тот же ресурс Enemies нужно его синхронизировать
            Timer creater = new Timer((obj) =>
                {
                    // check if console clear for creating;
                    if (!areaBusy()) {
                        int i = r.Next(3);
                        if (i == 0)
                        lock (threadLock)
                        {
                            Enemies.Add(new Enemy());
                        }
                    }
                }, null, 0, 1000);

            Timer mover = new Timer((obj) =>
            {
                lock (threadLock)
                {
                    foreach (var enemy in Enemies)
                    {
                            enemy.move(Direction.SelfDefined);
                    }
                }
            }, null, 0, 500);
            
            
            //enemy.move(Direction.SelfDefined);
        }

        private bool areaBusy()
        {
            return ConsoleHelper.areaBusy(typeof(Enemy));
        }

        // public void control
    }
}
