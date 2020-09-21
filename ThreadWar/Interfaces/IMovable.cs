using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadWar.Interfaces
{
    interface IMovable
    {
        int Speed { get; set; }
        void move(Direction direction);
    }
}
