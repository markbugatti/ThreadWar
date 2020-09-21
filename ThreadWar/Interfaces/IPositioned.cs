using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadWar.Interfaces
{
    interface IPositioned
    {
        [Description("Position X on the field")]
        int X { get; set; }
        [Description("Position Y on the field")]
        int Y { get; set; }
    }
}
