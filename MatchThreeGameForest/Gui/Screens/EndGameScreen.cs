using MatchThreeGameForest.GameLogic;
using MatchThreeGameForest.ResourceManager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MatchThreeGameForest.Gui.Screens
{
    class EndGameScreen : MenuScreen
    {
        private SpriteBatch spriteBatch = MatchGame.instance.spriteBatch;
        private Viewport viewport = MatchGame.instance.GraphicsDevice.Viewport;

        private Texture2D gameOverScreen;

        public EndGameScreen()
        {
            var content = MatchGame.instance.Content;
            var menuButtonTexture = Resources.MenuButton;
            var menuButton = new Button(menuButtonTexture, new Point((viewport.Width - menuButtonTexture.Width) / 2, 300));
            menuButton.Clicked += OkButtonClicked;
            MenuButtons.Add(menuButton);
            gameOverScreen = Resources.GameOverScreen;
        }

        void OkButtonClicked(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, new MainMenuScreen());
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(gameOverScreen,
                             new Vector2((viewport.Width - gameOverScreen.Width) / 2, (viewport.Height - gameOverScreen.Height) / 2),
                             Color.White);
            spriteBatch.DrawString(Resources.Font,
                                   GameScore.ScoreString,
                                   new Vector2((viewport.Width - Resources.Font.MeasureString(GameScore.ScoreString).X) / 2, 170),
                                   Color.Black);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
