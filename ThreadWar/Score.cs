using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace ThreadWar
{
    class Score
    {
        private static int hit = 0;
        private static int miss = 0;
        public event Action<int> hitHandle;
        public event Action<int> missHandle;
        public static int Hit { 
            get => hit;
            set {
                if (value > hit)
                    hit = value;
                else
                    throw new ArgumentException("New \"hit\" value must be grater then previous");
            } 
        }
        public static int Miss { 
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
