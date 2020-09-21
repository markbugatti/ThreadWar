using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ThreadWar
{
    public enum WindowMode {
        HIDE = 0,
        MAXIMIZE = 3,
        MINIMIZE = 6,
        RESTORE = 9
    }
    class ConsoleFacade
    {
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        public static IntPtr ThisConsole = GetConsoleWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]

        private static extern bool ShowWindow(IntPtr hWnd, WindowMode nCmdShow);
        private const int HIDE = 0;
        private const int MAXIMIZE = 3;
        private const int MINIMIZE = 6;
        private const int RESTORE = 9;

        public static void ShowWindow(WindowMode nCmdShow)
        {
            ShowWindow(ThisConsole, nCmdShow);
        }
    }
}
