using MatchThreeGameForest.GameLogic;
using MatchThreeGameForest.GameStateManagement;
using MatchThreeGameForest.ResourceManager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using static MatchThreeGameForest.GameLogic.Timer;

namespace MatchThreeGameForest.Gui.Screens
{
    class GameplayScreen : GameScreen
    {
        private Grid grid = new Grid();

        ContentManager content;
        SpriteBatch spriteBatch;
        SpriteFont gameFont;

        GameState gameState;

        public GameplayScreen()
        {
            spriteBatch = MatchGame.instance.spriteBatch;

            gameState = GameState.GridFill;

            grid.LoadContent(content);
        }

        public override void Activate(bool instancePreserved)
        {
            if (!instancePreserved)
            {
                if (content == null)
                    content = MatchGame.instance.Content;

                // Timer
                AddListener(() => { ScreenManager.AddScreen(new EndGameScreen(), null); });
                ScreenManager.Game.ResetElapsedTime();
                Reset(60);

                // Score
                GameScore.Reset();

                gameFont = Resources.Font;

                ScreenManager.Game.ResetElapsedTime();
            }
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            if (!IsActive)
                return;

            Timer.Tick(gameTime);
            GameUpdate();
            grid.Update(gameTime);
        }

        private void GameUpdate()
        {
            if (grid.IsAnimating)
                return;

            var score = 0;

            switch (gameState)
            {
                case GameState.GridFill:
                    //true if blocks added
                    gameState = grid.FillGrid() ? GameState.MatchAfterFill : GameState.Input;
                    break;

                case GameState.MatchAfterFill:
                    score = grid.MatchAndGetPoints();
                    if (score > 0)
                    {
                        GameScore.Add(score);
                        gameState = GameState.CellFalling;
                    }
                    else
                    {
                        gameState = GameState.Input;
                    }
                    break;

                case GameState.CellFalling:
                    grid.DropCells();
                    gameState = GameState.GridFill;
                    break;

                case GameState.Input:
                    //true if swapped blocks
                    if (grid.UserInput())
                    {
                        gameState = GameState.Swap;
                    }
                    break;

                case GameState.Swap:
                    grid.Swap();
                    gameState = GameState.MatchAfterSwap;
                    break;

                case GameState.MatchAfterSwap:
                    score = grid.MatchAndGetPoints();
                    if (score > 0)
                    {
                        GameScore.Add(score);
                        gameState = GameState.CellFalling;
                    }
                    else
                    {
                        gameState = GameState.SwapBack;
                    }
                    break;

                case GameState.SwapBack:
                    grid.SwapBack();
                    gameState = GameState.Input;
                    break;


            }
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.AliceBlue, 0, 0);

            spriteBatch.Begin();
            spriteBatch.DrawString(gameFont, GameScore.ScoreString, new Vector2(500, 25), Color.Black);
            spriteBatch.DrawString(gameFont, Timer.TimeRemaining, new Vector2(500, 75), Color.Black);
            spriteBatch.End();

            grid.Draw(spriteBatch);
        }
    }
}