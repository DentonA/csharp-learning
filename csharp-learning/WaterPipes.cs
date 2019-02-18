using System;
using System.Collections.Generic;
using System.Linq;
namespace csharp_learning
{
    class WaterPipes
    {
        public static void Main(string[] arguments)
        {
            Controller controller = new Controller();
            ConsoleView view = new ConsoleView(controller);
            WaterPipesModel model = new WaterPipesModel(view);
            controller.Model = model;

            controller.Run();
        }
    }
}
