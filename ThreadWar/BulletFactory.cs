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
        private object threadLocker = new object();
        private static Mutex mutex = new Mutex();
        public void start()
        {
            // timer for moving the bullets
            Timer timer = new Timer(obj =>
            {    
                //lock(threadLocker) {
                    mutex.WaitOne();
                    foreach (var bullet in bullets)
                    {   
                        bullet.move(Direction.SelfDefined);
                    }
                    mutex.ReleaseMutex();
                //}
            }, null, 0, 200);
        }

        public void addBullet(int x, int y)
        {
            lock (threadLocker)
            {
            //mutex.WaitOne();
                if (bullets.Count < 3)
                {
                    Bullet bullet = new Bullet(x, y);
                    bullet.OutOfField += bul => {

                        var copy = new List<Bullet>(bullets);
                        copy.Remove(bul);
                        bullets = copy;
                        
                    };
                    bullets.Add(bullet);
                }
            //mutex.ReleaseMutex();
            }
        }
    }
}
