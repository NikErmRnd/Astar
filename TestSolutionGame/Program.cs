using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace TestSolutionGame
{
    class Program
    {
        // 3 Player
        // 2 Loot
        // 1 Block
        // 0 Free

        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");

            var board = new Board();

            board.board = new int[6, 6];
            board.board[0, 0] = 0;
            board.board[0, 1] = 0;
            board.board[0, 2] = 0;
            board.board[0, 3] = 1;
            board.board[0, 4] = 1;
            board.board[0, 5] = 2;

            board.board[1, 0] = 0;
            board.board[1, 1] = 0;
            board.board[1, 2] = 2;
            board.board[1, 3] = 0;
            board.board[1, 4] = 0;
            board.board[1, 5] = 0;

            board.board[2, 0] = 0;
            board.board[2, 1] = 1;
            board.board[2, 2] = 1;
            board.board[2, 3] = 3;
            board.board[2, 4] = 0;
            board.board[2, 5] = 2;

            board.board[3, 0] = 0;
            board.board[3, 1] = 0;
            board.board[3, 2] = 0;
            board.board[3, 3] = 0;
            board.board[3, 4] = 0;
            board.board[3, 5] = 0;

            board.board[4, 0] = 2;
            board.board[4, 1] = 0;
            board.board[4, 2] = 0;
            board.board[4, 3] = 1;
            board.board[4, 4] = 1;
            board.board[4, 5] = 0;

            board.board[5, 0] = 0;
            board.board[5, 1] = 0;
            board.board[5, 2] = 2;
            board.board[5, 3] = 0;
            board.board[5, 4] = 0;
            board.board[5, 5] = 0;

            board.SHOW();

            //Console.WriteLine("+++  L");
            //Console.WriteLine("++L+++");
            //Console.WriteLine("+  P+L");
            //Console.WriteLine("++++++");
            //Console.WriteLine("L++  +");
            //Console.WriteLine("++L+++");

            int countLoot = board.Count();

            List<(int, int)> cords;
            cords = board.FindPath(6, 6);

            //Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }

    public class Board
    {
        public int[,] board;

        internal int Count()
        {
            return 5;
        }

        internal List<(int, int)> FindPath(int n, int m)
        {
            List<(int, int)> cords = new List<(int, int)>();
            cords.Add((2, 3));
            int score = 0;
            for (int b = 0; b < 5; b++)
            {
                //Move
                bool finish = false;
                var rand = new Random();
                while (finish != true)
                {
                    (int, int) currentPoint = cords.Last();
                    if (board[currentPoint.Item1, currentPoint.Item2] == 2)
                    {
                        score++;
                        board[currentPoint.Item1, currentPoint.Item2] = 0;
                        break;
                    }

                    switch (Console.ReadKey().KeyChar)
                    {
                        case 'w': { UP(ref currentPoint, cords); Console.Clear(); SHOW(); break; }
                        case 's': { DOWN(ref currentPoint, cords); Console.Clear(); SHOW(); break; }
                        case 'a': { LEFT(ref currentPoint, cords); Console.Clear(); SHOW(); break; }
                        case 'd': { RIGHT(ref currentPoint, cords); Console.Clear(); SHOW(); break; }
                    }
                    }
            }
            return cords;
        }

        private bool RIGHT(ref (int, int) currentPoint, List<(int, int)> cords)
        {
            if (currentPoint.Item2 + 1 < 6 && board[currentPoint.Item1, currentPoint.Item2 + 1] != 1)
            {
                board[currentPoint.Item1, currentPoint.Item2 + 1] = 3;
                board[currentPoint.Item1, currentPoint.Item2] = 0;
                currentPoint = (currentPoint.Item1, currentPoint.Item2 + 1);
                cords.Add(currentPoint);
                return true;
            }
            return false;
        }

        private bool LEFT(ref (int, int) currentPoint, List<(int, int)> cords)
        {
            if (currentPoint.Item2 - 1 != -1 && board[currentPoint.Item1, currentPoint.Item2 - 1] != 1)
            {
                board[currentPoint.Item1, currentPoint.Item2 - 1] = 3;
                board[currentPoint.Item1, currentPoint.Item2] = 0;
                currentPoint = (currentPoint.Item1, currentPoint.Item2 - 1);
                cords.Add(currentPoint);
                return true;
            }
            return false;
        }

        private bool UP(ref (int, int) currentPoint, List<(int, int)> cords)
        {
            if (currentPoint.Item1 - 1 != -1 && board[currentPoint.Item1 - 1, currentPoint.Item2] != 1)
            {
                board[currentPoint.Item1 - 1, currentPoint.Item2] = 3;
                board[currentPoint.Item1, currentPoint.Item2] = 0;
                currentPoint = (currentPoint.Item1 - 1, currentPoint.Item2);
                cords.Add(currentPoint);
                return true;
            }
            return false;
        }

        private bool DOWN(ref (int, int) currentPoint, List<(int, int)> cords)
        {
            if (currentPoint.Item1 + 1 < 6 && board[currentPoint.Item1 + 1, currentPoint.Item2] != 1)
            {
                board[currentPoint.Item1 + 1, currentPoint.Item2] = 3;
                board[currentPoint.Item1, currentPoint.Item2] = 0;
                currentPoint = (currentPoint.Item1 + 1, currentPoint.Item2);
                cords.Add(currentPoint);
                return true;
            }
            return false;
        }

        internal void SHOW()
        {
            for (int i = 0; i<6; i++)
            {
                for (int j=0; j<6; j++)
                {
                    if (board[i,j] == 3)
                    {
                        Console.Write("P");
                    }
                    if (board[i, j] == 2)
                    {
                        Console.Write("L");
                    }
                    if (board[i, j] == 1)
                    {
                        Console.Write(" ");
                    }
                    if (board[i, j] == 0)
                    {
                        Console.Write("+");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
