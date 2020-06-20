using Microsoft.Xna.Framework;
using System;

namespace MatchThreeGameForest.GameLogic
{
    static class Timer
    {
        private static double timeToWait;
        private static bool isExpired;
        private static Action callback;

        public static void Reset(float newTime = 60)
        {
            timeToWait = newTime;
            isExpired = false;
            //TODO reset the callbacks?
        }

        public static void AddListener(Action listener)
        {
            callback += listener;
        }

        public static void Tick(GameTime gameTime)
        {
            if (!isExpired)
            {
                timeToWait -= gameTime.TotalGameTime.TotalSeconds;
            }

            if (timeToWait > 0)
                return;

            callback?.Invoke();
        }

    }
}
