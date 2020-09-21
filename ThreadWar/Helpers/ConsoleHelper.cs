using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ThreadWar.Helpers
{
    class ConsoleHelper
    {
        public static void clearArea(int x, int y, int width, int height)
        {
            string filler = string.Join(" ", new char[width]);
            for (int i = 0; i <= height; i++, y--)
            {
                Console.SetCursorPosition(x, y);
                Console.Write(filler);
            }
        }
    }
}
