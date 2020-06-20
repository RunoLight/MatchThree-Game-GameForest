using MatchThreeGameForest.GameStateManagement;
using MatchThreeGameForest.Gui;
using MatchThreeGameForest.Gui.Screens;
using MatchThreeGameForest.TextureRenderer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MatchThreeGameForest
{
    public class Game1 : Game
    {
        public static Game1 instance;

        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        public SpriteFont font;

        ScreenFactory screenFactory;
        ScreenManager screenManager;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            instance = this;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            screenFactory = new ScreenFactory();
            screenManager = new ScreenManager(this);
            Components.Add(screenManager);
            //screenManager.TraceEnabled = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            TextureManager.Init(this.Content);

            //screenManager.AddScreen(new BackgroundScreen(), null);
            screenManager.AddScreen(new MainMenuScreen(), null);
            //screenManager.AddScreen(new MessageBoxScreen("message box here!", true), null);
            //screenManager.AddScreen(new PauseMenuScreen(), null);

            font = Content.Load<SpriteFont>("Fonts/Font");


            // TODO: use this.Content to load your game content here
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.AntiqueWhite);
            base.Draw(gameTime);
        }

        //LoadingScreen.Load(ScreenManager, new EndGameScreen());
    }
}
