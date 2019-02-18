using System;

namespace csharp_learning
{
    class ConsoleView
    {
        private const char Frame = '+';
        private const char Pipe = 'O';
        private const char Empty = ' ';
        private const char Cursor = 'X';
        private const char WaterSource = 'S';

        private Controller controller;

        private int width;
        private int height;
        private Coordinates cursor = new Coordinates(0, 0);
        private bool setupMode;

        public ConsoleView(Controller controller)
        {
            this.controller = controller;
        }

        public void Setup()
        {
            setupMode = true;
            width = controller.GetFieldWidth();
            height = controller.GetFieldHeight();


            while(setupMode)
            {
                Draw();
                ConsoleKey key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        if (cursor.Y > 0)
                        {
                            cursor.Y--;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (cursor.Y + 1 < height)
                        {
                            cursor.Y++;
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (cursor.X > 0)
                        {
                            cursor.X--;
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (cursor.X + 1 < width)
                        {
                            cursor.X++;
                        }
                        break;
                    case ConsoleKey.Enter:
                        controller.SetCell(cursor, CellType.EmptyPipe);
                        break;
                    case ConsoleKey.Delete:
                        controller.SetCell(cursor, CellType.Blank);
                        break;
                    case ConsoleKey.S:
                        controller.SetCell(cursor, CellType.Source);
                        break;
                    case ConsoleKey.Spacebar:
                        setupMode = false;
                        break;
                    default:
                        break;
                }
            }
        }

        public void GameOver()
        {
            WriteColored("GAME OVER.\nPress any key to exit...", ConsoleColor.DarkCyan);
            Console.ReadKey();
        }

        public void Draw()
        {
            var field = controller.GetField();
            Console.Clear();

            DrawSteps();
            DrawHorizontalFrame();

            for (int y = 0; y < height; y++)
            {
                Console.Write(Frame);
                for (int x = 0; x < width; x++)
                {
                    if (setupMode && cursor.X == x && cursor.Y == y)
                    {
                        WriteColored(Cursor, ConsoleColor.Red);
                        continue;
                    }
                    switch (field[x, y])
                    {
                        case CellType.Blank:
                            Console.Write(Empty);
                            break;
                        case CellType.Source:
                            WriteColored(WaterSource, ConsoleColor.Yellow);
                            break;
                        case CellType.EmptyPipe:
                            WriteColored(Pipe, ConsoleColor.White);
                            break;
                        case CellType.FilledPipe:
                            WriteColored(Pipe, ConsoleColor.Blue);
                            break;
                        default:
                            break;
                    }
                }
                Console.WriteLine(Frame);
            }

            DrawHorizontalFrame();
        }

        private void DrawSteps()
        {
            Console.Write("Step: ");
            WriteColored(controller.GetStepNumber(), ConsoleColor.DarkGreen);
            Console.WriteLine();
        }

        private void DrawHorizontalFrame()
        {
            for (int i = 0; i < width + 2; i++)
            {
                Console.Write(Frame);
            }
            Console.WriteLine();
        }

        private void WriteColored(object output, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(output);
            Console.ResetColor();
        }
    }
}