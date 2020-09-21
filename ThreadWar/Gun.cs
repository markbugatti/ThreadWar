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
    class Gun : IPositioned, IMovable/*, IDrawable*/
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; } = 6;
        public int Height { get; set; } = 9;
        public int Speed { get; set; } = 4;
        //public ushort Speed { get; set; }
        private bool drawn = false;

        [Description("Method to fire a new bullet")]
        public void fire()
        {
        
        }
        public Gun() {}
        
        public Gun(int x, int y)
        {
            X = x;
            Y = y;
            draw(Direction.Neutral);
        }
        public void move(Direction direction)
        {
            throw new NotImplementedException();
        }
        
        public void draw(Direction direction)
        {
            if (!drawn) {
                Y -= 9;
                X -= 3;
            }
            else { 
                clear();
            }
            // here you can specify speed X += ...  X -= ...
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
            Console.SetCursorPosition(X, Y);
            Console.Write("+----+");
            Console.SetCursorPosition(X, ++Y);
            Console.Write("|----|");
            Console.SetCursorPosition(X, ++Y);
            Console.Write("|----|");
            Console.SetCursorPosition(X, ++Y);
            Console.Write("|----|");
            Console.SetCursorPosition(X, ++Y);
            Console.Write("|----|");
            Console.SetCursorPosition(X, ++Y);
            Console.Write("|----|" );
            Console.SetCursorPosition(X, ++Y);
            Console.Write("|----|");
            Console.SetCursorPosition(X, ++Y);
            Console.Write("|----|");
            Console.SetCursorPosition(X, ++Y);
            Console.Write("|----|");
            Console.SetCursorPosition(X + 3, Y);
            ++Y;
            drawn = true;
        }

        public void clear()
        {
            ConsoleHelper.clearArea(X, Y, Width, Height);
        }
    }
}
