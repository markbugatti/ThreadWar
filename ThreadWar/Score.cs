using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace ThreadWar
{
    static class Score
    {
        private static int hit = 0;
        private static int miss = 0;
        public static event Action ScoreChangedHandle;
        private static object hitLocker = new object();
        private static object missLocker = new object();

        public static int Hit {
            get
            {
                //lock (hitLocker)
                //{
                    return hit;
                //}
            }
            set {
                //lock (hitLocker)
                //{
                    if (value > hit)
                    {
                        hit = value;
                        ScoreChangedHandle();
                    }
                    else
                        throw new ArgumentException("New \"hit\" value must be grater then previous");
                //}
            } 
        }
        public static int Miss {
            get
            {
                //lock()
                return miss;
            }
            set {
                if (value > miss)
                {
                    miss = value;
                    ScoreChangedHandle();
                }
                else
                    throw new ArgumentException("New \"miss\" value must be grater then previous");
            } 
        }
    }
}
