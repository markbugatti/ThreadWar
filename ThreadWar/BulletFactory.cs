using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Timer = System.Threading.Timer;
using System.Threading.Tasks;

namespace ThreadWar
{
    class BulletFactory
    {
        /* Реализовать чтобы bulletFactory работала с Bullet используя семафоры
         * AddBullet каждый раз создает новый процес, который являет собой пулю. Пуля должна контролировать сама себя
         * 
         * 
         * 
         */
        List<Bullet> bullets = new List<Bullet>();
        
        List<Thread> bulletThreads = new List<Thread>();
        private int threadCount = 0;
        public void start()
        {
            // timer for moving the bullets
            Timer timer = new Timer(obj =>
            {
                //bullet is self-movable check and delete waitable threads instead.
                // проверить если обьект в соответствующем потоке не двигаеться, удалить этот поток
                for (int i = 0; i < bulletThreads.Count; i++)
                {
                    if (!bullets[i].InMove && bulletThreads[i].ThreadState == ThreadState.WaitSleepJoin)
                    {
                        bulletThreads[i].Interrupt();
                        bulletThreads.RemoveAt(i);
                        bullets.RemoveAt(i);
                        i--;
                    }
                }
            }, null, 0, 6000); 
        }
        //public BulletFactory(int gunX, int gunY)
        //{
        //    X = gunX+1;
        //    Y = gunY-4;
        //}
        public void addBullet(int x, int y)
        {
            bullets.Add(new Bullet(x, y));
            Thread newBulletThread = new Thread(new ThreadStart(bullets.Last().move));
            newBulletThread.Name = $"bulletThread #{threadCount++}";
            bulletThreads.Add(newBulletThread);
            newBulletThread.Start();
        }
    }
}
