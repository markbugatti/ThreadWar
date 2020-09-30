using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ThreadWar.Helpers;
using ThreadWar.Interfaces;

namespace ThreadWar
{
    class Gun : IPositioned, IMovable, IDrawable
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; } = 6;
        public int Height { get; set; } = 9;
        // how fast they move
        public int Speed { get; set; } = 4;

        [Description("Method to fire a new bullet")]
        public void fire()
        {
        
        }      
        public Gun(int x, int y)
        {
            X = x-Width;
            Y = y-Height;
            draw();
        }
        public void move(Direction direction)
        {
            clear();
            switch (direction)
            {
                case Direction.Left:
                    if (X - Speed > 0)
                        X -= Speed;
                    else
                        X -= X;
                    break;
                case Direction.Right:
                    if (X + Speed + Width < Console.WindowWidth - 1)
                        X += Speed;
                    else
                        X += Console.WindowWidth - X - Width;
                    break;
                default:
                    break;
            }
            draw();
        }
        
        public void draw()
        {
            ConsoleHelper.drawLines(
                new string[] {
                "+----+",
                "|----|",
                "|----|",
                "|----|",
                "|----|",
                "|----|",
                "|----|",
                "|----|",
                "|----|",
                }, X, Y, Width, Height
            );
        }

        public void clear()
        {
            ConsoleHelper.clearArea(X, Y, Width, Height);
        }
    }
}
