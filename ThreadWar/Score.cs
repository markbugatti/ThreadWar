using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadWar
{
    class Score
    {
        private int hit;
        private int miss;
        public int Hit { 
            get => hit;
            set {
                if (value > hit)
                    hit = value;
                else
                    throw new ArgumentException("New \"hit\" value must be grater then previous");
            } 
        }
        public int Miss { 
            get => miss;
            set {
                if (value > miss)
                    hit = value;
                else
                    throw new ArgumentException("New \"miss\" value must be grater then previous");
            } 
        }
    }
}
