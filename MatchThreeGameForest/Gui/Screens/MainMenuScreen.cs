using MatchThreeGameForest.GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MatchThreeGameForest.Gui.Screens
{
    class MainMenuScreen : MenuScreen
    {
        public MainMenuScreen()
        {
            var content = Game1.instance.Content;
            var playButtonTexture = content.Load<Texture2D>("Sprites/PlayButton");

            var viewport = Game1.instance.spriteBatch.GraphicsDevice.Viewport;
            var width = (viewport.Width - playButtonTexture.Width) / 2;
            var height = (viewport.Height - playButtonTexture.Height) / 2;

            var playButton = new Button(playButtonTexture, new Point(width, height));
            playButton.Clicked += PlayGameMenuClicked;
            MenuButtons.Add(playButton);
        }

        void PlayGameMenuClicked(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, new GameplayScreen());
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            const string message = "Are you sure you want to exit game?";
            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);
            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;
            ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }

        void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }
    }
}