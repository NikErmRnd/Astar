using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace TestSolutionGame
{
    class Second
    {
        // 3 Player
        // 2 Loot
        // 1 Block
        // 0 Free

        static void Main1(string[] args)
        {
            var lines = File.ReadLines(args[0]).ToList();
            StringBuilder outSrtBuilder = new StringBuilder();
            int n = lines.First().Count();
            int m = lines.Count;
            var board = new int[n, m];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    if (lines[i][j] == ' ')
                    {
                        board[i, j] = 1;
                    }
                }
            }

            int lootCount = int.Parse(args[2]);
            var rand = new Random();
            outSrtBuilder.Append("Lootboxes: ");
            int playerX = rand.Next(0, n);
            int playerY = rand.Next(0, m);
            board[playerX, playerY] = 3;
            for (int i = 0; i < lootCount; i++)
            {
                int x = rand.Next(0, n);
                int y = rand.Next(0, m);
                if (board[x, y] != 1)
                {
                    board[x, y] = 2;
                    outSrtBuilder.Append($"({x},{y}) ");
                }
                else
                {
                    i--;
                }
            }
            outSrtBuilder.Append($"\nPlayer: ({playerX},{playerY})");

            (int, int) lootPoint = (-1, -1);
            lootPoint = Scan(board, lootPoint, n, m);
            var startPlayer = new Cell() { X = playerX, Y = playerY, IsStartPoint = true };
            var newPoint = NexPoint(board, lootPoint, startPlayer, n, m);
            while (true)
            {
                newPoint = NexPoint(board, lootPoint, newPoint, n, m);
                if (newPoint.X == lootPoint.Item1 && newPoint.Y == lootPoint.Item2)
                {
                    board[lootPoint.Item1, lootPoint.Item2] = 0;
                    lootPoint = (-1, -1);
                    lootPoint = Scan(board, lootPoint, n, m);
                    if (lootPoint.Item1 == -1 && lootPoint.Item2 == -1)
                    {
                        break;
                    }
                }
            }
            List<Cell> path = new List<Cell>();
            path.Add(newPoint);
            while (newPoint.Prev != null)
            {
                newPoint = newPoint.Prev;
                path.Add(newPoint);
            }

            outSrtBuilder.Append("\nPath: ");
            path.Reverse();
            foreach (var point in path)
            {
                outSrtBuilder.Append($"({point.X},{point.Y})");
            }
            var file = File.Open(args[1], FileMode.Create);
            file.Close();
            using (StreamWriter writeFile = new System.IO.StreamWriter(args[1], true))
            {
                writeFile.WriteLine(outSrtBuilder.ToString());
            }
        }

        private static (int, int) Scan(int[,] board, (int, int) lootPoint, int n, int m)
        {
            //Scan
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    if (board[i, j] == 2)
                    {
                        lootPoint = (i, j);
                        break;
                    }
                }
                if (lootPoint.Item1 != -1 && lootPoint.Item2 != -1)
                {
                    break;
                }
            }

            return lootPoint;
        }

        private static Cell NexPoint(int[,] board, (int, int) lootPoint, Cell startPlayer, int n, int m)
        {
            Cell newPoint = startPlayer;
            if (startPlayer.IsStartPoint != true)
            {
                if (startPlayer.Neighbors.Count != 1 && startPlayer.Neighbors[0].Sum == startPlayer.Neighbors[1].Sum)
                {
                    newPoint = startPlayer.Neighbors.OrderBy(x => Guid.NewGuid()).Take(1).First().DeepClone();
                }
                else
                {
                    newPoint = startPlayer.Neighbors.OrderBy(item => item.Sum).First().DeepClone();
                }
                newPoint.Prev = startPlayer;
            }
            if (newPoint.X + 1 < n)
            {
                newPoint.Neighbors.Add(new Cell() { X = newPoint.X + 1, Y = newPoint.Y, IsBlock = board[newPoint.X + 1, newPoint.Y] == 1, DistanceFromStart = 10 + (newPoint.Prev == null ? 0 : newPoint.Prev.DistanceFromStart) });
            }
            if (newPoint.X - 1 != -1)
            {
                newPoint.Neighbors.Add(new Cell() { X = newPoint.X - 1, Y = newPoint.Y, IsBlock = board[newPoint.X - 1, newPoint.Y] == 1, DistanceFromStart = 10 + (newPoint.Prev == null ? 0 : newPoint.Prev.DistanceFromStart) });
            }
            if (newPoint.Y + 1 < m)
            {
                newPoint.Neighbors.Add(new Cell() { X = newPoint.X, Y = newPoint.Y + 1, IsBlock = board[newPoint.X, newPoint.Y + 1] == 1, DistanceFromStart = 10 + (newPoint.Prev == null ? 0 : newPoint.Prev.DistanceFromStart) });
            }
            if (newPoint.Y - 1 != -1)
            {
                newPoint.Neighbors.Add(new Cell() { X = newPoint.X, Y = newPoint.Y - 1, IsBlock = board[newPoint.X, newPoint.Y - 1] == 1, DistanceFromStart = 10 + (newPoint.Prev == null ? 0 : newPoint.Prev.DistanceFromStart) });
            }
            newPoint.Neighbors = newPoint.Neighbors.Where(item => item.IsBlock != true).ToList();
            foreach (var cell in newPoint.Neighbors)
            {
                cell.EvristDistance = (Math.Abs(lootPoint.Item1 - cell.X) + Math.Abs(lootPoint.Item2 - cell.Y)) * 10;
                cell.Sum = cell.EvristDistance + cell.DistanceFromStart;
            }
            if (startPlayer.IsStartPoint == true)
            {
                startPlayer.IsStartPoint = false;
            }
            return newPoint;
        }
    }

    public static class ExtensionMethods
    {
        // Deep clone
        public static T DeepClone<T>(this T a)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, a);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }
    }

    [Serializable]
    public class Cell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsStartPoint { get; set; }
        public bool IsBlock { get; set; }
        public List<Cell> Neighbors { get; set; } = new List<Cell>();
        public Cell Prev { get; set; }
        public int DistanceFromStart { get; set; } //10
        public int EvristDistance { get; set; } //Count to end * 10
        public int Sum { get; set; } // DistanceFromStart + EvristDistance
    }
}
