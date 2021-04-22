using Microsoft.Xna.Framework;
using System;

namespace MatchThreeGameForest.GameLogic
{
    static class Timer
    {
        private static float timeToWait;
        public static string FormattedTimeRemaining => "Time: " + Math.Round(timeToWait, 0);

        private static bool isExpired = false;
        private static Action callback;

        public static void Reset(float newTime = 60)
        {
            timeToWait = newTime;
            isExpired = false;
        }

        public static void AddListener(Action listener)
        {
            callback += listener;
        }

        public static void Tick(GameTime gameTime)
        {
            if (isExpired)
            {
                return;
            }

            timeToWait -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            timeToWait = MathF.Max(0F, timeToWait);

            if (timeToWait == 0F)
            {
                isExpired = true;
                callback?.Invoke();
            }
        }
    }
}
