using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ThreadWar.Helpers;
using Timer = System.Threading.Timer;
using System.Windows.Forms;

namespace ThreadWar
{
    class EnemyFactory
    {
        private List<Enemy> Enemies { get; set; } = new List<Enemy>();
        private object threadLock = new object();
        private int randVal = 10;
        private object randValLocker = new object();
        public void start()
        {
            /* create timer to generate enemy and move it
            * create timer for enemy creating
            * creat timer for enemy moving. this timer should change its elapsed timer
            */
            // create first enemy without timer
            
            Enemies.Add(new Enemy());
            
            // разные таймеры используют один и тот же ресурс Enemies нужно его синхронизировать
            Timer creater = new Timer((obj) =>
            {
                lock (randValLocker)
                {
                    if (!areaBusy())
                    {
                        Random r = new Random();
                        int i = r.Next(randVal);
                        if (i == 0)
                            lock (threadLock)
                            {
                                Enemies.Add(new Enemy());
                            }
                    }
                }
                /* Увеличить вероятность появления противника через функцию r.Next(n) чем больше n, тем вероятность меньше и наоборот
                * Сделать таймер, который через каждый промежуток времени, например 15 сек, будет уменьшать значение n, тем самым увеличивать шанс появления нового противника.
                * в целевой делегат (в этот) будет входить каждый раз новый параметр (int)obj, который будет давать новое значение n;
                */
            }, false, 0, 1000);

            // this timer changes n value for r.Next(n) function in creator callback. Increase appearance probability every 10 seconds
            Timer changer = new Timer(obj =>
            {
                if (randVal - 1 > 0)
                {
                    lock (randValLocker)
                    {
                        randVal--;
                        //MessageBox.Show($"{randVal}");
                    }
                }
            }, false, 0, 10000);

            Timer mover = new Timer((obj) =>
            {
                lock (threadLock)
                {
                    if(!Enemies.First().canMove())
                    {
                        Enemies.RemoveAt(0);
                    }
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

    }
}
