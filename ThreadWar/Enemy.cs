using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ThreadWar.Interfaces;

namespace ThreadWar
{
    enum Direction
    {
        Left,
        Right,
        Upward,
        Downward,
        Neutral
    }
    class Enemy : IPositioned, IMovable
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Direction Direction { get; set; }
        public int Speed { get; set; }

        public void move()
        {

        }
        public void move(Direction direction)
        {
            throw new NotImplementedException();
        }
    }
}
