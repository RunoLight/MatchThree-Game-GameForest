using System;

namespace MatchThreeGameForest
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new MatchGame())
                game.Run();
        }
    }
}
