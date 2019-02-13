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
        private int cursorX = 0;
        private int cursorY = 0;
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
                Console.Read();
            }
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
                    if (setupMode && cursorX == x && cursorY == y)
                    {
                        Console.Write(Cursor);
                        continue;
                    }
                    switch (field[x, y])
                    {
                        case CellType.Blank:
                            Console.Write(Empty);
                            break;
                        case CellType.Source:
                            Console.Write(WaterSource);
                            break;
                        case CellType.EmptyPipe:
                            Console.Write(Pipe);
                            break;
                        case CellType.FilledPipe:
                            Console.Write(Pipe);
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
            Console.WriteLine($"Step: {controller.GetStepNumber()}");
        }

        private void DrawHorizontalFrame()
        {
            for (int i = 0; i < width + 2; i++)
            {
                Console.Write(Frame);
            }
            Console.WriteLine();
        }
    }
}