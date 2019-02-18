using System;
using System.Collections.Generic;

namespace csharp_learning
{
    class WaterPipesModel
    {
        private Direction[] directions = { Direction.Left, Direction.Right, Direction.Up, Direction.Down };

        private ConsoleView view;

        private CellType[,] field;
        private const int width = 30;
        private const int height = 15;
        private const int delayMs = 400;
        private List<Coordinates> uttermostWaterSources;
        private bool running;

        public int Step { get; set; } = 0;

        public WaterPipesModel(ConsoleView view)
        {
            this.view = view;
            field = new CellType[width, height];
            uttermostWaterSources = new List<Coordinates>();
        }

        public void Run()
        {
            view.Setup();
            InitializeUttermostSources();
            Step = 1;
            running = true;

            while (running)
            {
                System.Threading.Thread.Sleep(delayMs);
                view.Draw();
                Update();
            }

            view.GameOver();
        }

        private void Update()
        {
            Step++;
            CellType[,] fieldCopy = new CellType[width, height];
            Array.Copy(field, fieldCopy, width * height);

            Coordinates source;
            Coordinates target;
            int cycles = uttermostWaterSources.Count;
            for (int i = 0; i < cycles; i++)
            {
                source = uttermostWaterSources[0];

                foreach (Direction direction in directions)
                {
                    target = GetNeighbourCoordinates(source, direction);
                    if (CanHaveNeighbour(source, direction) && field[target.X, target.Y] == CellType.EmptyPipe)
                    {
                        fieldCopy[target.X, target.Y] = CellType.FilledPipe;
                        uttermostWaterSources.Add(new Coordinates(target.X, target.Y));
                    }
                }
                uttermostWaterSources.RemoveAt(0);
            }
            Array.Copy(fieldCopy, field, width * height);

            running = uttermostWaterSources.Count > 0;
        }

        private void InitializeUttermostSources()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (field[x,y] == CellType.Source)
                    {
                        uttermostWaterSources.Add(new Coordinates(x, y));
                    }
                }
            }
        }

        public void PutBlank(Coordinates cell)
        {
            CheckCoordinates(cell.X, cell.Y);
            if (CanRemoveCell(cell))
            {
                field[cell.X, cell.Y] = CellType.Blank;
            }
        }

        private bool CanRemoveCell(Coordinates cell)
        {
            List<Coordinates> history = new List<Coordinates>();
            history.Add(cell);
            foreach (Direction direction in directions)
            {
                if (CanHaveNeighbour(cell, direction) && !IsSourceConnected(GetNeighbourCoordinates(cell, direction), history))
                {
                    return false;
                }
            }
            return true;
        }

        // not works as it should
        private bool IsSourceConnected(Coordinates cell, List<Coordinates> history)
        {
            if (field[cell.X, cell.Y] == CellType.Source)
            {
                return true;
            }
            history.Add(cell);
            foreach (Direction direction in directions)
            {
                if (CanHaveNeighbour(cell, direction))
                {
                    Coordinates neighbour = GetNeighbourCoordinates(cell, direction);
                    if (field[neighbour.X, neighbour.Y] == CellType.Blank)
                    {
                        continue;
                    }
                    if (!IsAvailableInList(neighbour, history))
                    {
                        if (IsSourceConnected(neighbour, history))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private bool IsAvailableInList(Coordinates coordinates, List<Coordinates> list)
        {
            foreach (Coordinates listItem in list)
            {
                if (listItem.X == coordinates.X && listItem.Y == coordinates.Y)
                {
                    return true;
                }
            }
            return false;
        }

        public void PutPipe(Coordinates cell)
        {
            CheckCoordinates(cell.X, cell.Y);
            if (HasPipedNeighbour(cell))
            {
                field[cell.X, cell.Y] = CellType.EmptyPipe;
            }
        }

        private bool HasPipedNeighbour(Coordinates cell)
        {
            Coordinates target;
            foreach (Direction direction in directions)
            {
                target = GetNeighbourCoordinates(cell, direction);
                if (CanHaveNeighbour(cell, direction) && (field[target.X, target.Y] == CellType.EmptyPipe || field[target.X, target.Y] == CellType.Source))
                {
                    return true;
                }
            }
            return false;
        }

        private Coordinates GetNeighbourCoordinates(Coordinates source, Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                default:
                    return new Coordinates(source.X - 1, source.Y);
                case Direction.Right:
                    return new Coordinates(source.X + 1, source.Y);
                case Direction.Up:
                    return new Coordinates(source.X, source.Y - 1);
                case Direction.Down:
                    return new Coordinates(source.X, source.Y + 1);
            }
        }

        private bool CanHaveNeighbour(Coordinates cell, Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    return cell.X - 1 >= 0;
                case Direction.Right:
                    return cell.X + 1 < width;
                case Direction.Up:
                    return cell.Y - 1 >= 0;
                case Direction.Down:
                    return cell.Y + 1 < height;
                default:
                    return false;
            }
        }

        public void PutSource(Coordinates cell)
        {
            CheckCoordinates(cell.X, cell.Y);
            field[cell.X, cell.Y] = CellType.Source;
        }

        private void CheckCoordinates(int x, int y)
        {
            if (x >= width || y >= height || x < 0 || y < 0)
            {
                throw new Exception($"Wrong coordinates: x = {x}, y = {y}");
            }
        }

        public int GetWidth()
        {
            return width;
        }

        public int GetHeight()
        {
            return height;
        }

        public CellType[,] GetField()
        {
            return field;
        }
    }
}