namespace csharp_learning
{
    class GameController
    {
        public LifeGameModel Model { get; set; }

        private string[] initArguments;

        public GameController(params string[] arguments)
        {
            initArguments = arguments;
        }

        public void Run()
        {
            if (initArguments != null && initArguments.Length > 0)
            {
                Model.ParseArguments(initArguments);
            }
            Model.Run();
        }

        public void InverseFieldCellValue(int x, int y)
        {
            Model.InverseFieldCellValue(x, y);
        }

        public int GetWidth()
        {
            return Model.Width;
        }

        public int GetHeight()
        {
            return Model.Height;
        }

        public bool[,] GetField()
        {
            return Model.GetField();
        }
        
        public int GetGenerationCounter()
        {
            return Model.GenerationCounter;
        }
    }
}