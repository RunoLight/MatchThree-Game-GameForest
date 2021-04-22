using MatchThreeGameForest.GameStateManagement;
using MatchThreeGameForest.Gui.Screens;
using MatchThreeGameForest.ResourceManager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MatchThreeGameForest
{
    public class MatchGame : Game
    {
        public static MatchGame instance;

        /**
         * Devices to draw on the screen
         */
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        private ScreenManager screenManager;

        public MatchGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = Constants.ResourcesRoot;

            IsMouseVisible = true;
            Window.AllowUserResizing = false;
            Window.Title = "GameForest Test task";


            instance = this;
        }

        protected override void Initialize()
        {
            screenManager = new ScreenManager(this);
            Components.Add(screenManager);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Resources.Init(this.Content);

            screenManager.AddScreen(new MainMenuScreen(), null);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.AliceBlue);
            base.Draw(gameTime);
        }
    }
}
