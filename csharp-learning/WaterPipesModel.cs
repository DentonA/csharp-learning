namespace csharp_learning
{
    class WaterPipesModel
    {
        private ConsoleView view;

        private CellType[,] field;
        private const int width = 30;
        private const int height = 15;
        private const int delayMs = 400;

        public int Step { get; set; } = 0;

        public WaterPipesModel(ConsoleView view)
        {
            this.view = view;
            field = new CellType[width, height];

        }

        public void Run()
        {
            view.Setup();
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