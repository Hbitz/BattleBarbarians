namespace BattleBarbarians
{
    internal class Program
    {
        // Todo
        // Some mental notes to not forget
        // Generic: Either Convert Character, or expand rewards and introduce an inventory system.
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            Console.WriteLine(Console.WindowWidth);
            Console.WriteLine(Console.WindowHeight);
            Console.SetWindowSize(120, 40);

            GameController gameController = new GameController();
            gameController.StartGame();
        }
    }
}
