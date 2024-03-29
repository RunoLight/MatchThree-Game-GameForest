﻿using MatchThreeGameForest.GameStateManagement;
using MatchThreeGameForest.ResourceManager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using static MatchThreeGameForest.ResourceManager.Constants;

namespace MatchThreeGameForest.Gui.Screens
{
    class LoadingScreen : GameScreen
    {
        bool otherScreensAreGone;

        GameScreen[] screensToLoad;

        public static void Load(ScreenManager screenManager, params GameScreen[] screensToLoad)
        {
            foreach (GameScreen screen in screenManager.GetScreens())
                screen.ExitScreen();
            LoadingScreen loadingScreen = new LoadingScreen(screensToLoad);
            screenManager.AddScreen(loadingScreen, PlayerIndex.One);
        }

        private LoadingScreen(GameScreen[] screensToLoad)
        {
            this.screensToLoad = screensToLoad;
            TransitionOnTime = TimeSpan.FromSeconds(MenuTransitionTime);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (otherScreensAreGone)
            {
                ScreenManager.RemoveScreen(this);

                foreach (GameScreen screen in screensToLoad)
                {
                    if (screen != null)
                    {
                        ScreenManager.AddScreen(screen, ControllingPlayer);
                    }
                }
                ScreenManager.Game.ResetElapsedTime();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if ((ScreenState == ScreenState.Active) &&
                (ScreenManager.GetScreens().Length == 1))
            {
                otherScreensAreGone = true;
            }
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
            Vector2 textSize = Resources.Font.MeasureString(LoadingScreenMessage);
            Vector2 textPosition = (viewportSize - textSize) / 2;
            spriteBatch.Begin();
            spriteBatch.DrawString(Resources.Font, LoadingScreenMessage, textPosition, TextColor * TransitionAlpha);
            spriteBatch.End();
        }
    }
}