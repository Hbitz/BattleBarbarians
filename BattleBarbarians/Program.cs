namespace BattleBarbarians
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(120, 42); // default is 120, 30. We increase height to show combat history

            GameController gameController = new GameController();
            gameController.StartGame();
        }
    }
}
