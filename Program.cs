using System.Dynamic;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Tower_Of_Hanoi
{
    
    internal class Program
    {
        public class Game
        {
            public Game(int d)
            {
                _diskSize = d;
                _data = new int[3, _diskSize];
                for (int i = 1; i <= _diskSize; i++)
                {
                    _data[0, _diskSize - i] = i;
                }
            }

            private static int _diskSize;
            private static int[,]? _data;

            public static void DumpData()
            {

                for (int i = _diskSize-1; i >= 0; i--)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        for (int k = _diskSize; k > 0; k--)
                            Console.Write(_data![j, i] >= k ? '#' : ' ');
                        Console.Write('|');
                        for (int k = 0; k <_diskSize; k++)
                            Console.Write(_data![j, i] > k ? '#' : ' ');
                    }
                    Console.Write('\n');
                }

                for (int j = 0; j < 3; ++j)
                {
                    for (int i = 0; i < _diskSize; i++)
                        Console.Write(' ');
                    Console.Write(j+1);
                    for (int i = 0; i < _diskSize; i++)
                        Console.Write(' ');
                }
                Console.Write('\n');
            }

            public static bool MakeMove(int from, int to)
            {
                if (_data == null)
                {
                    Console.WriteLine("Internal Data error");
                    return false;
                }
                if (from is < 1 or > 3 || to is < 1 or > 3 || from == to)
                {
                    return false;
                }
                from--;
                to--;
                int fromRow = -1, toRow = -1;
                for (int i = 0; i < _diskSize; i++)
                {
                    if (i == _diskSize - 1 && _data[from, i] != 0)
                    {
                        fromRow = _diskSize - 1;
                        break;
                    }
                    if (_data[from, i] == 0)
                    {
                        fromRow = i - 1;
                        break;
                    }
                }

                for (int i = 0; i < _diskSize; i++)
                {
                    if (_data[to, i] != 0) continue;
                    toRow = i;
                    break;
                }

                if (fromRow is -1 || toRow is -1)
                    return false;
                if (toRow != 0 && _data[from, fromRow] > _data[to, toRow - 1])
                    return false;

                (_data[from, fromRow], _data[to, toRow]) = (_data[to, toRow], _data[from, fromRow]);

                return true;
            }

            public static bool CheckWin()
            {
                for (int i = 0; i < _diskSize; i++)
                {
                    if (_data == null)
                    {
                        Console.WriteLine("Internal Data error");
                        return false;
                    }
                    if (_data[2, i] == 0) return false;
                }
                return true;
            }
        }

        private static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Tower Of Hanoi! Choose disk amount between 3 and 6:");
            int diskSize;
            while (true)
            {
                if (!int.TryParse(Console.ReadLine(), out diskSize))
                {
                    Console.WriteLine("OOPS! I can't read this :C. Please try again!");
                    continue;
                }
                if (diskSize is < 3 or > 6)
                {
                    Console.WriteLine(
                        "OOPS! I can't make a game of this size :C. Please try again! (expecting a number between 3 and 6)");
                    continue;
                }
                break;
            }
            Game a = new Game(diskSize);
            int moves = 1;
            while (true)
            {
                Game.DumpData();
                Console.WriteLine($"Move: {moves}\nWrite out your move (from column to column)");
                if (!int.TryParse(Console.ReadLine(), out int x) || !int.TryParse(Console.ReadLine(), out int y))
                {
                    Console.WriteLine("OOPS! I can't read this :C. Please try again!");
                    continue;
                }
                if (!Game.MakeMove(x, y))
                {
                    Console.WriteLine("OOPS! Can't do such a move :C. Please try again! (expecting two numbers between 1 and 3)");
                    continue;
                }

                moves++;
                if (!Game.CheckWin()) continue;
                Console.ForegroundColor = ConsoleColor.Green;
                Game.DumpData();
                Console.WriteLine($"Congratulations, you won in {moves} moves!");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }
        }
    }
}
