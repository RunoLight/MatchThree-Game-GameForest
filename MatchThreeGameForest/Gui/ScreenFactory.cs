using MatchThreeGameForest.GameStateManagement;
using System;

namespace MatchThreeGameForest.Gui
{
    class ScreenFactory : IScreenFactory
    {
        public GameScreen CreateScreen(Type screenType)
        {
            return Activator.CreateInstance(screenType) as GameScreen;
        }
    }
}
