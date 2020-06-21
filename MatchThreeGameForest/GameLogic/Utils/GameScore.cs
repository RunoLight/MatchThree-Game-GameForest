namespace MatchThreeGameForest.GameLogic
{
    static class GameScore
    {
        public static int Score = 0;

        public static void Add(int amount)
        {
            Score += amount;
        }

        public static void Reset()
        {
            Score = 0;
        }

        public static string ScoreString
        {
            get
            {
                return "Your score: " + Score.ToString();
            }
        }
    }
}
