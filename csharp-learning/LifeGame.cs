using System;
using System.Collections.Generic;
using System.Linq;

namespace csharp_learning
{
    class LifeGame
    {
        private static readonly char FrameChar = '+';
        private static readonly char AliveChar = '0';
        private static readonly char DeadChar = ' ';
        private static readonly char CursorChar = 'X';
        private static readonly int NeighbourVisibilityDistance = 1;

        private static LifeGame game;
        private bool running = true;
        private bool setupMode = true;

        public int Width { get; set; } = 40;
        public int Height { get; set; } = 10;
        public int SleepTimeMs { get; set; } = 300;

        private int cursorX = 0;
        private int cursorY = 0;
        private bool[,] field;
        private List<bool[,]> history = new List<bool[,]>();
        private int generationCounter;

        static void Main(string[] args)
        {
            game = new LifeGame();

            if (args.Length > 0)
                game.ParseArguments(args);

            game.Setup();
            game.RunLoop();

            game.WriteColoredText("GAME OVER.\nPress any key to exit...", ConsoleColor.DarkCyan);
            Console.ReadKey();
        }

        private void ParseArguments(string[] args)
        {
            int widthParam = -1;
            int heightParam = -1;
            foreach (string parameter in args)
            {
                int parsedValue;
                switch (parameter[0])
                {
                    case 'w':
                        if (int.TryParse(parameter.Substring(1), out parsedValue) && parsedValue > 0)
                        {
                            if (widthParam != -1)
                                game.FinishWithError("Width was specified multiple times.");
                            widthParam = parsedValue;
                            game.Width = widthParam;
                        }
                        break;
                    case 'h':
                        if (int.TryParse(parameter.Substring(1), out parsedValue) && parsedValue > 0)
                        {
                            if (heightParam != -1)
                                game.FinishWithError("Height was specified multiple times.");
                            heightParam = parsedValue;
                            game.Height = heightParam;
                        }
                        break;
                    case 's':
                        if (int.TryParse(parameter.Substring(1), out parsedValue) && parsedValue > 0)
                            game.SleepTimeMs = parsedValue;
                        break;
                    default:
                        break;
                }
            }
            if (widthParam != -1 && heightParam == -1)
                game.FinishWithError("Height of the Universe was not specified.");
            if (widthParam == -1 && heightParam != -1)
                game.FinishWithError("Width of the Universe was not specified.");
        }

        public void Setup()
        {
            field = new bool[Width, Height];
            while (setupMode)
            {
                Draw();
                ConsoleKey key = Console.ReadKey().Key;
                switch (key) {
                    case ConsoleKey.UpArrow:
                        if (cursorY - 1 >= 0)
                            cursorY--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (cursorY + 1 < Height)
                            cursorY++;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (cursorX - 1 >= 0)
                            cursorX--;
                        break;
                    case ConsoleKey.RightArrow:
                        if (cursorX + 1 < Width)
                            cursorX++;
                        break;
                    case ConsoleKey.Enter:
                        field[cursorX, cursorY] = !field[cursorX, cursorY];
                        break;
                    case ConsoleKey.Spacebar:
                        setupMode = false;
                        break;
                    default:
                        break;
                }
            }

            Draw();
            history.Add(field);
        }

        public void RunLoop()
        {
            while (running)
            {
                System.Threading.Thread.Sleep(SleepTimeMs);
                Update();
                Draw();
            }
        }

        public void Update()
        {
            generationCounter++;

            bool[,] fieldCopy = new bool[Width, Height];
            Array.Clear(fieldCopy, 0, Width * Height);
            bool clearField = true;

            int aliveNeighboursCount;
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    aliveNeighboursCount = CalculateNeighbours(x, y, NeighbourVisibilityDistance);

                    if (!field[x, y] && aliveNeighboursCount == 3 || (field[x, y] && (aliveNeighboursCount == 2 || aliveNeighboursCount == 3)))
                    {
                        fieldCopy[x, y] = true;
                        if (clearField)
                            clearField = false;
                    }
                }
            }
            if (clearField)
                running = false;

            if (running)
            {
                for (int i = history.Count - 1; i >= 0; i--)
                {
                    if (CompareGameFields(fieldCopy, history.ElementAt(i)))
                    {
                        running = false;
                        break;
                    }
                }
            }

            field = fieldCopy;
            history.Add(fieldCopy);
        }

        private int CalculateNeighbours(int x, int y, int distance)
        {
            int result = 0;
            for (int i = -distance; i < distance + 1; i++)
            {
                for (int j = -distance; j < distance + 1; j++)
                {
                    if (i == 0 && j == 0) continue;
                    if (x + i < 0 || x + i >= Width) continue;
                    if (y + j < 0 || y + j >= Height) continue;
                    if (field[x + i, y + j])
                        result++;
                }
            }
            return result;
        }

        private bool CompareGameFields(bool[,] a, bool[,] b)
        {
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    if (a[x, y] != b[x, y])
                        return false;
            return true;
        }

        public void Draw()
        {
            Console.Clear();

            Console.Write("Generation: ");
            WriteColoredText(generationCounter, ConsoleColor.DarkCyan);
            Console.WriteLine();

            DrawHorizontalFrame();

            for (int y = 0; y < Height; y++)
            {
                Console.Write(FrameChar);
                for (int x = 0; x < Width; x++)
                {
                    if (setupMode && x == cursorX && y == cursorY)
                        WriteColoredText(CursorChar, ConsoleColor.Red);
                    else if (field[x, y])
                        WriteColoredText(AliveChar, ConsoleColor.DarkGreen);
                    else
                        Console.Write(DeadChar);
                }
                Console.WriteLine(FrameChar);
            }

            DrawHorizontalFrame();
        }

        private void DrawHorizontalFrame()
        {
            for (int i = 0; i < Width + 2; i++)
                Console.Write(FrameChar);
            Console.WriteLine();
        }

        private void WriteColoredText(object text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }

        private void FinishWithError(string errorMessage)
        {
            Console.Write("Invalid arguments: ");
            WriteColoredText(errorMessage, ConsoleColor.Red);
            Console.WriteLine();
            Console.ReadKey();
            Environment.Exit(1);
        }
    }
}
