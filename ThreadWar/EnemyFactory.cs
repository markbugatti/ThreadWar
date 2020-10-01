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
using System.Runtime.CompilerServices;

namespace ThreadWar
{
    class EnemyFactory
    {
        private List<Enemy> Enemies { get; set; } = new List<Enemy>();
        private int randVal = 10;
        private object enemiesLocker = new object();
        private object randValLocker = new object();
        Timer creater;
        Timer changer;
        Timer mover;
        public void start()
        {
            /* create timer to generate enemy and move it
            * create timer for enemy creating
            * creat timer for enemy moving. this timer should change its elapsed timer
            */
            // create first enemy without timer
            Enemy enemy = new Enemy();
            enemy.OutOfField += removeEnemy;
            Enemies.Add(enemy);
            

            // разные таймеры используют один и тот же ресурс Enemies нужно его синхронизировать
            creater = new Timer((obj) =>
            {
                lock (randValLocker)
                {
                        if (!areaBusy())
                        {
                            Random r = new Random();
                            int i = r.Next(randVal);
                            if (i == 0)
                            lock (enemiesLocker)
                            {
                                enemy = new Enemy();
                                enemy.OutOfField += removeEnemy;
                                Enemies.Add(enemy);
                            }
                        }
                }
                /* Увеличить вероятность появления противника через функцию r.Next(n) чем больше n, тем вероятность меньше и наоборот
                * Сделать таймер, который через каждый промежуток времени, например 15 сек, будет уменьшать значение n, тем самым увеличивать шанс появления нового противника.
                * в целевой делегат (в этот) будет входить каждый раз новый параметр (int)obj, который будет давать новое значение n;
                */
            }, false, 100, 1000);


            
            // this timer changes n value for r.Next(n) function in creator callback. Increase appearance probability every 10 seconds
            changer = new Timer(obj =>
            {
                if (randVal - 1 > 0)
                {
                    lock (randValLocker)
                    {
                        randVal--;
                    }
                }
            }, false, 0, 10000);

            mover = new Timer((obj) =>
            {
                lock (enemiesLocker)
                {
                    foreach (var en in Enemies)
                    {
                        en.move(Direction.SelfDefined);
                    }
                }
            }, null, 0, 500);



        }

        public void stop()
        {
            lock(enemiesLocker)
            {
            creater.Change(Timeout.Infinite, Timeout.Infinite);
            changer.Change(Timeout.Infinite, Timeout.Infinite);
            mover.Change(Timeout.Infinite, Timeout.Infinite);

            changer.Dispose();
            mover.Dispose();
            creater.Dispose();
                foreach (var en in Enemies)
                {
                    removeEnemy(en);
                }
                Enemies = null;
            }
        }

        // x and y represent bullet position
        public void killEnemy(int bulletX, int bulletY)
        {
            lock(enemiesLocker)
            {
                foreach (var en in Enemies)
                {
                    if(bulletX+Bullet.Width > en.X)
                    {
                        removeEnemy(en);
                        break;
                    }
                }
            }
        }
        private void removeEnemy(Enemy enemy)
        {
            try
            {
                enemy.clear();
                var copy = new List<Enemy>(Enemies);
                copy.Remove(enemy);
                Enemies = copy;
            }
            catch { }
        }

        private bool areaBusy()
        {
            return ConsoleHelper.areaBusy(typeof(Enemy));
        }

    }
}
