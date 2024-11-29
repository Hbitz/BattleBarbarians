namespace BattleBarbarians
{
    internal class Program
    {
        // Todo
        // Some mental notes to not forget
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            GameController gameController = new GameController();
            gameController.StartGame();
        }
    }
}
