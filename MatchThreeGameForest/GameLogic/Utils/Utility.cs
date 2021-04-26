using static MatchThreeGameForest.ResourceManager.Constants;

namespace MatchThreeGameForest.GameLogic.Utils
{
    class Utility
    {
        public static void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp;
            temp = lhs;
            lhs = rhs;
            rhs = temp;
        }

        public static bool InBounds(int x, int y)
        {
            return x >= 0 && x <= GridSize &&
                y >= 0 && y <= GridSize;
        }
    }
}
