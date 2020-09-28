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
using System.Diagnostics.Eventing.Reader;

namespace ThreadWar
{
    class Bullet : IPositioned, IMovable, IDrawable
    {
        public event Action<Bullet> OutOfField;
        public int X { get; set; }
        public int Y{ get; set; }
        public static int Width { get; set; } = 4;
        public static int Height { get; set; } = 3;
        public int Speed { get; set; } = 6;
        [Description("Create and initialize bullet with coordinates")]
        public bool InMove { get; private set; } = false;
        public Bullet(int X, int Y)
        {
            this.X = X;
            this.Y = Y-4;
        }

        public void move(Direction direction)
        {
            // коллизия просчитываеться наперед, нужно использовать параметр высоты и скорости
            bool collision = isCollision();
            if (canMove() && !collision)
            {
                clear();
                Y -= Speed;
                draw();
            }
            else
            {
                clear();
                if (collision)
                    Score.Hit++;
                else
                    Score.Hit--;
                    // отнять бал
                    OutOfField(this);
            }
        }

        public bool canMove()
        {
            if (Y - Height - Speed < 0)
                return false;
            return true;
        }

        public bool isCollision()
        {
            return ConsoleHelper.areaBusy(this);
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
