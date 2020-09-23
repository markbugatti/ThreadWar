using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadWar.Helpers
{
    class ConsoleHelper
    {

        #region Kernel32
        
        // this region use unmanaged Kernel32 library for reading console buffer
        
        [DllImport("kernel32", SetLastError = true)]
        static extern IntPtr GetStdHandle(int num);

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)] //   ̲┌───────────────────^
        static extern bool ReadConsoleOutputCharacterW(
            IntPtr hStdout,   // result of 'GetStdHandle(-11)'
            out Char ch,      // U̲n̲i̲c̲o̲d̲e̲ character result
            uint c_in,        // (set to '1')
            uint coord_XY,    // screen location to read, X:loword, Y:hiword
            out uint c_out);  // (unwanted, discard)
        
        #endregion


        private static object threadLock = new object();
        // безопасный к потокам clearArea
        public static void clearArea(int x, int y, int width, int height)
        {
            lock (threadLock)
            {
                string filler = string.Join(" ", new char[width]);
                for (int i = 0; i <= height; i++, y++)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(filler);
                }
            }
        }
        public static void drawLines(string[] lines, int x, int y, int width, int height)
        {
            lock (threadLock)
            {
                foreach (var line in lines)
                {
                    Console.SetCursorPosition(x, y++);
                    Console.Write(line);


                }
            }
        }

        internal static bool areaBusy(Type type)
        {
            lock(threadLock)
            {            
                int height = 0, width = 0;
                bool typeExist = false;
                if(type == typeof(Enemy))
                {
                    height = Enemy.Height;
                    width = Enemy.Width;
                    typeExist = true;
                }

                if (typeExist) {
                    var stdout = GetStdHandle(-11);
                    uint coord;
                    for (uint i = 0; i < height; i++)
                    {   
                        for (uint j = 0; j < width; j++)
                        {
                            coord = j;
                            coord |= i << 16;
                            if (!ReadConsoleOutputCharacterW(
                               stdout,
                               out char ch,    // result: single ANSI char
                               1,                  // # of chars to read
                               coord,              // (X,Y) screen location to read (see above)
                               out _))             // result: actual # of chars (unwanted)
                                throw new Win32Exception();
                            
                            if (ch != ' ') 
                                return true;
                        }
                    }
                }
                return false;
            }
        }

    }
}
