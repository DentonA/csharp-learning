namespace csharp_learning
{
    class LifeGame
    {
        static void Main(string[] arguments)
        {
            GameController gameController = new GameController(arguments);
            ConsoleView view = new ConsoleView(gameController);
            gameController.Model = new LifeGameModel(view);
            gameController.Run();
        }
    }
}