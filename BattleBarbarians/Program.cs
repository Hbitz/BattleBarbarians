namespace BattleBarbarians
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            Console.WriteLine(Console.WindowWidth);
            Console.WriteLine(Console.WindowHeight);
            Console.SetWindowSize(120, 42);

            GameController gameController = new GameController();
            gameController.StartGame();
        }
    }
}
