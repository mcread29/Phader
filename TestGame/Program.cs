using Phader;

namespace TestGame
{
    class Program
    {
        private static void Main(string[] args)
        {
            new Phader.Game(new GameOptions(800, 600, "Phader Test", new GameScene()));
        }
    }
}