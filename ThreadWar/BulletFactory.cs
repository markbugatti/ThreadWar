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
        public void start()
        {
            // timer for moving the bullets
            Timer timer = new Timer(obj =>
            {
                try
                {
                    foreach (var bullet in bullets)
                    {
                        bullet.move(Direction.SelfDefined);
                    }                    
                }
                catch
                {
                }
            }, null, 0, 200);
        }

        public void addBullet(int x, int y)
        {

            if (bullets.Count < 3)
            {
                Bullet bullet = new Bullet(x, y);
                bullets.Add(bullet);
                bullet.OutOfField += bul => bullets.Remove(bul);
            }
        }
    }
}
