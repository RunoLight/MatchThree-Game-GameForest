using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MatchThreeGameForest.Gui.Screens
{
    class EndGameScreen : MenuScreen
    {
        private Texture2D GameOverLabel;
        public EndGameScreen()
        {
            var content = Game1.instance.Content;
            var okButtonTexture = content.Load<Texture2D>("OK");
            var okButton = new Button(okButtonTexture, new Point(300, 400));
            okButton.Clicked += OkButtonClicked;
            MenuButtons.Add(okButton);
            GameOverLabel = content.Load<Texture2D>("GameOver");
        }

        void OkButtonClicked(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, new MainMenuScreen());
        }

        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = Game1.instance.spriteBatch;
            spriteBatch.Begin();
            spriteBatch.Draw(GameOverLabel, new Vector2(200, 100), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
