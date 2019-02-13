using System;
using System.Collections.Generic;
using System.Linq;

namespace csharp_learning
{
    class LifeGameModel
    {
        private ConsoleView view;

        private const int NeighbourVisibilityDistance = 1;

        public int Width { get; set; } = 40;
        public int Height { get; set; } = 10;
        public int SleepTimeMs { get; set; } = 300;
        public int GenerationCounter { get; set; } = 0;

        private bool[,] field;
        private List<bool[,]> history = new List<bool[,]>();
        private bool running;

        public LifeGameModel(ConsoleView view)
        {
            this.view = view;
        }

        public void Run()
        {
            Setup();
            view.Draw();

            while (running)
            {
                System.Threading.Thread.Sleep(SleepTimeMs);
                Update();
                view.Draw();
            }

            view.GameOver();
        }

        private void Setup()
        {
            field = new bool[Width, Height];
            view.Setup();
            GenerationCounter = 1;
            history.Add(field);
            running = IsFieldAlive();
        }

        private void Update()
        {
            GenerationCounter++;

            bool[,] fieldCopy = new bool[Width, Height];
            Array.Clear(fieldCopy, 0, Width * Height);

            int aliveNeighboursCount;
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    aliveNeighboursCount = CalculateNeighbours(x, y, NeighbourVisibilityDistance);

                    if (!field[x, y] && aliveNeighboursCount == 3 || 
                        (field[x, y] && (aliveNeighboursCount == 2 || aliveNeighboursCount == 3)))
                    {
                        fieldCopy[x, y] = true;
                    }
                }
            }

            field = fieldCopy;
            running = IsFieldAlive();

            if (running)
            {
                for (int i = history.Count - 1; i >= 0; i--)
                {
                    if (AreEqual(fieldCopy, history.ElementAt(i)))
                    {
                        running = false;
                        break;
                    }
                }
            }

            history.Add(fieldCopy);
        }

        public void InverseFieldCellValue(int x, int y)
        {
            field[x, y] = !field[x, y];
        }

        public void ParseArguments(string[] args)
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
                            if (widthParam > -1)
                            {
                                FinishWithError("Width was specified multiple times.");
                            }
                            widthParam = parsedValue;
                            Width = parsedValue;
                        }
                        break;
                    case 'h':
                        if (int.TryParse(parameter.Substring(1), out parsedValue) && parsedValue > 0)
                        {
                            if (heightParam != -1)
                            {
                                FinishWithError("Height was specified multiple times.");
                            }
                            heightParam = parsedValue;
                            Height = heightParam;
                        }
                        break;
                    case 's':
                        if (int.TryParse(parameter.Substring(1), out parsedValue) && parsedValue > 0)
                        {
                            SleepTimeMs = parsedValue;
                        }
                        break;
                    default:
                        break;
                }
            }
            if (widthParam != -1 && heightParam == -1)
            {
                FinishWithError("Height of the Universe was not specified.");
            }
            else if (widthParam == -1 && heightParam != -1)
            {
                FinishWithError("Width of the Universe was not specified.");
            }
        }

        private void FinishWithError(string errorMessage)
        {
            view.FinishWithError(errorMessage);
            Environment.Exit(1);
        }

        private int CalculateNeighbours(int x, int y, int distance)
        {
            int result = 0;
            for (int xShift = -distance; xShift <= distance; xShift++)
            {
                for (int yShift = -distance; yShift <= distance; yShift++)
                {
                    if ((xShift == 0 && yShift == 0) || 
                        (x + xShift < 0 || x + xShift >= Width) || 
                        (y + yShift < 0 || y + yShift >= Height))
                    {
                        continue;
                    }
                    if (field[x + xShift, y + yShift])
                    {
                        result++;
                    }
                }
            }
            return result;
        }

        private bool IsFieldAlive()
        {
            bool[,] blankField = new bool[Width, Height];
            return !AreEqual(blankField, field);
        }

        private bool AreEqual(bool[,] a, bool[,] b)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (a[x, y] != b[x, y])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool[,] GetField()
        {
            return field;
        }
    }
}