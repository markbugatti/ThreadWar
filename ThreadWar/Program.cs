﻿using System;
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
using ThreadWar.Helpers;

namespace ThreadWar
{
    class Program
    {
        private delegate void StartOp();
        private delegate bool BeginGameEventHandler();
        private static bool isPlaying = true;
        static void Main(string[] args)
        {
            //Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            ConsoleFacade.ShowWindow(WindowMode.MAXIMIZE);
            Thread.CurrentThread.Name = "Main thread";
            /*
             * создать поток таймера и поток нажиманий клавиши.
             * ждать ответ о завершениии одного из них. Как только ответ получен, прекратить исполнение потоков и вызвать в главном потоке игру
             * Попробовать реализовать ожидание перед игрой не через while и IAsyncResult, а через AutoResetEvent ст. 706
             */

            Console.Title = $"Thread Wars";

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
            enemyThread.Name = "Enemy main thread";
            enemyThread.Start();

            BulletFactory bulletFactory = new BulletFactory();
            bulletFactory.killHandler += bul =>
            {
                enemyFactory.killEnemy(bul.X, bul.Y);
            };
            Thread bulletThread = new Thread(new ThreadStart(bulletFactory.start));
            bulletThread.Name = "Bullet main thread";
            bulletThread.Start();

            Score.ScoreChangedHandle += () =>
            {
                // change application title to current score;
                Console.Title = $"hits: {Score.Hit}, misses: {Score.Miss}";
            };
            // add handle for score changes

            Score.GameEndHandle += (won)=>
            {
                enemyFactory.stop();
                //ConsoleHelper.endMessage(msg, Console.WindowWidth/2, Console.WindowHeight/2);
                int width = 30, height = 5;
                int x = Console.WindowWidth / 2 - width / 2;
                int y = Console.WindowHeight / 2 - height / 2;

                ConsoleHelper.drawLines(
                    new string[] {
                    "+----------------------------+", // width = 30
                    "+                            +",
              won ? "+         You won!!!         +" :
                    "+        You lose!!!         +",
                    "+                            +",
                   $"+         Your score         +",
                   $"+    hits: {Score.Hit.ToString("00")}, misses: {Score.Miss.ToString("00")}    +",
                    "+                            +",
                    "+----------------------------+", // width = 30
                     }, x, y, width, height
                );


                isPlaying = false;
            };

            while (isPlaying)
            {
                Direction direction = Direction.SelfDefined;
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.LeftArrow:
                        direction = Direction.Left;
                        gun.move(direction);
                        break;
                    case ConsoleKey.RightArrow:
                        direction = Direction.Right;
                        gun.move(direction);
                        break;
                    case ConsoleKey.Spacebar:
                        bulletFactory.addBullet(gun.X, gun.Y);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
