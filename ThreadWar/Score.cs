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
        public static event Action<bool> GameEndHandle;

        public static int Hit {
            get => hit;
            set {
                
                if (value > hit)
                {
                    hit = value;
                    ScoreChangedHandle();
                    if (value == 30)
                    {             
                        GameEndHandle(true);
                    }
                }
                else
                    throw new ArgumentException("New \"hit\" value must be grater then previous");
            } 
        }
        public static int Miss {
            get => miss;
            set {
                if (value > miss)
                {
                    miss = value;
                    ScoreChangedHandle();
                    if(miss == 30)
                    {
                        GameEndHandle(false);
                    }
                }
                else
                    throw new ArgumentException("New \"miss\" value must be grater then previous");
            } 
        }
    }
}
