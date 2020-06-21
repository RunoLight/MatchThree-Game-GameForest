using MatchThreeGameForest.ResourceManager;
using Microsoft.Xna.Framework;

namespace MatchThreeGameForest.Gui.Screens
{
    class MainMenuScreen : MenuScreen
    {
        public MainMenuScreen()
        {
            var viewport = MatchGame.instance.spriteBatch.GraphicsDevice.Viewport;
            var width = (viewport.Width - Resources.PlayButton.Width) / 2;
            var height = (viewport.Height - Resources.PlayButton.Height) / 2;

            var playButton = new Button(Resources.PlayButton, new Point(width, height));
            playButton.Clicked += PlayGameMenuClicked;

            MenuButtons.Add(playButton);
        }

        void PlayGameMenuClicked(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, new GameplayScreen());
        }
    }
}