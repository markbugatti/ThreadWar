using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadWar.Interfaces;

namespace ThreadWar
{
    class Bullet : IPositioned, IMovable
    {
        public int X { get; set; }
        public int Y{ get; set; }
        public int Speed { get; set; }

        [Description("Create and initialize bullet with coordinates")]
        public Bullet(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public void move(Direction direction)
        {
            throw new NotImplementedException();
        }
    }
}
