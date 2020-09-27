using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ThreadWar.Interfaces;
using ThreadWar.Helpers;
using System.ComponentModel;

namespace ThreadWar
{
    enum Direction
    {
        Left,
        Right,
        Upward,
        Downward,
        SelfDefined,
    }
    class Enemy : IPositioned, IMovable, IDrawable
    {
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        public static int Width { get;} = 9;
        public static int Height { get; } = 8;
        //public Direction Direction { get; set; }
        public int Speed { get; set; } = 4;
        
        public void clear()
        {
            ConsoleHelper.clearArea(X, Y, Width, Height);
        }
        public void move(Direction direction)
        {
            clear();
            if (X + Speed + Width < Console.WindowWidth - 1)
                X += Speed;
            else
                X += Console.WindowWidth - X - Width;
            draw();
        }

        public bool canMove()
        {
            if (X + Width > Console.WindowWidth - 1)
                return false;
            return true;
        }

        public void draw()
        {
            ConsoleHelper.drawLines(
                new string[] {
                " +-----+",
                " | X X |",
                " +  _  +",
                "  +---+",
                "+-+   +-+",
                "+ |   | +",
                "  | + |",
                "  +-+-+",
                }, X, Y, Width, Height
            );
        }

    }
}
