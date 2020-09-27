using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThreadWar.Interfaces;
using ThreadWar.Helpers;
using System.Threading;

namespace ThreadWar
{
    class Bullet : IPositioned, IMovable, IDrawable
    {
        public int X { get; set; }
        public int Y{ get; set; }
        public static int Width { get; set; } = 4;
        public static int Height { get; set; } = 3;
        public int Speed { get; set; } = 4;
        public int Speed1 { get; set; } = 100;
        //public Direction Direction { get; set; }
        private static Semaphore sem = new Semaphore(3, 3);
        [Description("Create and initialize bullet with coordinates")]
        public bool InMove { get; private set; } = false;
        public Bullet(int X, int Y)
        {
            this.X = X;
            this.Y = Y-4;
        }
        // this IMovable method isn't used because becouse of semephore which is out of global application logic
        // use move() instead.
        // этот move контролирует сам себя, и заканчиваеться как только обьект врезался, или попал за границу консоли
        public void move(Direction direction)
        {
            InMove = true;
            while (canMove()) {
                clear();
                Y--;
                draw();
                Thread.Sleep(Speed1);
            }
            clear();
            InMove = false;
        }
        /* поток обьекта захватывет семафор в методе , сразу могут захватить 3 обекта
         * остальные потоки ждут, тем временем их уничтажает BulletFactory
         * Как только семафор освобождаеться, и создаеться новый поток с обьектом, он тут же захватывает его
         * После захвата семафора, обект в потоке просто продолжает двигаться по консоли, сохроняя при этом безопасность потоков к консоли с помощью класса ConsoleHelper.
         * Освобождение семафора:
         * Как только обьект достиг пункта назначения (вышел за границы или врезался во врага), выстреливает событие, в BulletFactory
         * BulletFactory уничтожает поток вместе с обьектом 
         */
        public void move()
        {
            sem.WaitOne();
            
            move(Direction.SelfDefined);
            // 
            sem.Release();
        }

        public bool canMove()
        {
            if (Y - Height < 0)
                return false;
            return true;
        }

        public bool isCollision()
        {
            return false;
        }
        public void draw()
        {
            ConsoleHelper.drawLines(new string[]
            {
                "+--+",
                "|  |",
                "+--+"
            }, X, Y, Width, Height);
        }

        public void clear()
        {
            ConsoleHelper.clearArea(X, Y, Width, Height);
        }
    }
}
