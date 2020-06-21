using Microsoft.Xna.Framework;
using System;

namespace MatchThreeGameForest.GameLogic
{
    static class Timer
    {
        private static double timeToWait;
        public static string TimeRemaining
        {
            get
            {
                return "Time: " + Math.Round(timeToWait, 0);
            }
        }

        private static bool isExpired;
        private static Action callback;

        public static void Reset(float newTime = 60)
        {
            timeToWait = newTime;
            isExpired = false;
            //TODO reset the callbacks?
            //TODO check if timer continious when game paused
        }

        public static void AddListener(Action listener)
        {
            callback += listener;
        }

        public static void Tick(GameTime gameTime)
        {
            if (!isExpired)
            {
                timeToWait -= gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (timeToWait > 0)
                return;

            timeToWait = 0;
            callback?.Invoke();
        }
    }
}
