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
         List<Bullet> bullets = new List<Bullet>();
        private object bulletLocker = new object();
        public event Action<Bullet> killHandler;
        //private static Mutex mutex = new Mutex();
        public void start()
        {
            // timer for moving the bullets
            Timer timer = new Timer(obj =>
            {    
                lock(bulletLocker) {
                    //mutex.WaitOne();
                    foreach (var bullet in bullets)
                    {   
                        bullet.move(Direction.SelfDefined);
                    }
                    //mutex.ReleaseMutex();
                }
            }, null, 0, 200);
        }

        public void addBullet(int x, int y)
        {
            //lock (threadLocker)
            //{
            //mutex.WaitOne();
            lock (bulletLocker)
            {
                if (bullets.Count < 3)
                {
                    Bullet bullet = new Bullet(x, y);
                    bullet.OutOfField += bul => {
                        var copy1 = new List<Bullet>(bullets);
                        copy1.Remove(bul);
                        bullets = copy1;
                    };
                    bullet.killHandler += bul =>
                    {
                        killHandler(bul);
                    };

                    List<Bullet> copy = new List<Bullet>(bullets);
                    copy.Add(bullet);
                    bullets = copy;
                }
            }
            //mutex.ReleaseMutex();
            //}
        }
    }
}
