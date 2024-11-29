namespace BattleBarbarians
{
    internal class Program
    {
        // Todo
        // Some mental notes to not forget
        // Generic: Either Convert Character, or expand rewards and introduce an inventory system.
        // Implement logic for attack power modifier to allow a more diverse set of powerups
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
