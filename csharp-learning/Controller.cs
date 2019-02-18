namespace csharp_learning
{
    class Controller
    {
        public WaterPipesModel Model { get; set; }

        public void Run()
        {
            Model.Run();
        }

        public CellType[,] GetField()
        {
            return Model.GetField();
        }

        public int GetFieldWidth()
        {
            return Model.GetWidth();
        }

        public int GetFieldHeight()
        {
            return Model.GetHeight();
        }

        public int GetStepNumber()
        {
            return Model.Step;
        }

        public void SetCell(Coordinates coordinates, CellType type)
        {
            switch (type)
            {
                case CellType.Blank:
                    Model.PutBlank(coordinates);
                    break;
                case CellType.EmptyPipe:
                    Model.PutPipe(coordinates);
                    break;
                case CellType.Source:
                    Model.PutSource(coordinates);
                    break;
            }
        }
    }
}