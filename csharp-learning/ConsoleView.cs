using System;

namespace csharp_learning
{
    class ConsoleView
    {
        private const char FrameChar = '+';
        private const char AliveChar = '0';
        private const char DeadChar = ' ';
        private const char CursorChar = 'X';

        private GameController controller;

        private int cursorX = 0;
        private int cursorY = 0;
        private bool setupMode = false;

        public ConsoleView(GameController controller)
        {
            this.controller = controller;
        }

        public void Setup()
        {
            setupMode = true;

            while (setupMode)
            {
                Draw();
                ConsoleKey key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        if (cursorY - 1 >= 0)
                        {
                            cursorY--;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (cursorY + 1 < controller.GetHeight())
                        {
                            cursorY++;
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (cursorX - 1 >= 0)
                        {
                            cursorX--;
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (cursorX + 1 < controller.GetWidth())
                        {
                            cursorX++;
                        }
                        break;
                    case ConsoleKey.Enter:
                        controller.InverseFieldCellValue(cursorX, cursorY);
                        break;
                    case ConsoleKey.Spacebar:
                        setupMode = false;
                        break;
                    default:
                        break;
                }
            }
        }

        public void Draw()
        {
            Console.Clear();

            Console.Write("Generation: ");
            WriteColoredText(controller.GetGenerationCounter(), ConsoleColor.DarkCyan);
            Console.WriteLine();

            DrawHorizontalFrame();

            for (int y = 0; y < controller.GetHeight(); y++)
            {
                Console.Write(FrameChar);
                for (int x = 0; x < controller.GetWidth(); x++)
                {
                    if (setupMode && x == cursorX && y == cursorY)
                    {
                        WriteColoredText(CursorChar, ConsoleColor.Red);
                    }
                    else if (controller.GetField()[x, y])
                    {
                        WriteColoredText(AliveChar, ConsoleColor.DarkGreen);
                    }
                    else
                    {
                        Console.Write(DeadChar);
                    }
                }
                Console.WriteLine(FrameChar);
            }

            DrawHorizontalFrame();
        }

        private void DrawHorizontalFrame()
        {
            for (int i = 0; i < controller.GetWidth() + 2; i++)
            {
                Console.Write(FrameChar);
            }
            Console.WriteLine();
        }

        private void WriteColoredText(object text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }

        public void FinishWithError(string errorMessage)
        {
            Console.Write("Invalid arguments: ");
            WriteColoredText(errorMessage, ConsoleColor.Red);
            Console.WriteLine();
            Console.ReadKey();
            Environment.Exit(1);
        }

        public void GameOver()
        {
            WriteColoredText("GAME OVER.\nPress any key to exit...", ConsoleColor.DarkCyan);
            Console.ReadKey();
        }
    }
}