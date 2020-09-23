using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Timers;
using System.Threading;
using Timer = System.Timers.Timer;
using ThreadWar.Interfaces;
using System.IO;
using System.Windows.Controls.Primitives;

namespace ThreadWar
{
    class Program
    {
        public delegate void StartOp();

        public delegate bool BeginGameEventHandler();
        //public delegate bool ConsoleEventHandler();
        private static bool started = false;
        static async Task Main(string[] args)
        {
            //Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            ConsoleFacade.ShowWindow(WindowMode.MAXIMIZE);
            Thread.CurrentThread.Name = "Main thread";
            /*
             * создать поток таймера и поток нажиманий клавиши.
             * ждать ответ о завершениии одного из них. Как только ответ получен, прекратить исполнение потоков и вызвать в главном потоке игру
             * Попробовать реализовать ожидание перед игрой не через while и IAsyncResult, а через AutoResetEvent ст. 706
            */

            // thread for timer
            BeginGameEventHandler timerEventHandler = new BeginGameEventHandler(startOnTimer);
            IAsyncResult asyncResultTimer = timerEventHandler.BeginInvoke(null, null);
            
            // thread for keyboard
            BeginGameEventHandler keyEventHandler = new BeginGameEventHandler(startOnKey);
            IAsyncResult asyncResultKey = keyEventHandler.BeginInvoke(null, null);

            // wait until key is pressed or timer is expired;
            while (!asyncResultKey.IsCompleted && !asyncResultTimer.IsCompleted);

            // execute game in main thread
            StartGame();
            

            // indicate the end of game
            Console.WriteLine("The End");
            Console.ReadKey();
        }


        public static bool startOnTimer()
        {
            // настраиваем таймер и ждем 15 секунд
            Thread.CurrentThread.Name = "Timer thread";
            Thread.Sleep(3000);
            return true;
        }

        public static bool startOnKey()
        {
            Thread.CurrentThread.Name = "Key thread";
            while (true)
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.RightArrow:
                        return true;
                    default:
                        break;
                }
            }
        }


        private static void StartGame()
        {
            Gun gun = new Gun(Console.WindowWidth / 2, Console.WindowHeight);
            /* create enemy control thread
             * create instance of EnemyFactory
             * Create new thread and configure it
             * start main mathod of EnemyFactory with new thread
             */
            EnemyFactory enemyFactory = new EnemyFactory();
            Thread enemyThread = new Thread(new ThreadStart(enemyFactory.start));
            enemyThread.Name = "Enemy thread";
            enemyThread.Start();
            

            while (true)
            {
                Direction direction = Direction.SelfDefined;
                bool changed = false;
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.LeftArrow:
                        direction = Direction.Left;
                        changed = true;
                        break;
                    case ConsoleKey.RightArrow:
                        direction = Direction.Right;
                        changed = true;
                        break;
                    default:
                        break;
                }

                if (changed)
                {
                    gun.move(direction);
                }
            }
        }
    }
}
