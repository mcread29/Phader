using Phader;

namespace TestGame
{
    class Program
    {
        private static void Main(string[] args)
        {
            new Phader.Game(new GameOptions(800, 600, "Test Window", new GameScene()));
        }
    }
}