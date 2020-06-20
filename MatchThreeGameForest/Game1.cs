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

        // LOAD UNLOAD UPDATE //

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            TextureManager.Init(this.Content);

            //TODO
            screenManager.AddScreen(new BackgroundScreen(), null);
            screenManager.AddScreen(new MainMenuScreen(), null);
            //screenManager.AddScreen(new MessageBoxScreen("message box here!", true), null);
            screenManager.AddScreen(new PauseMenuScreen(), null);

            //screenManager.Enabled = true;


            font = Content.Load<SpriteFont>("Fonts/Font");


            // TODO: use this.Content to load your game content here
        }

        //protected override void Update(GameTime gameTime)
        //{
        //    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        //        Exit();
        //
        //    // TODO: Add your update logic here
        //
        //    base.Update(gameTime);
        //}

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.AntiqueWhite);

            //spriteBatch.Begin();
            //
            //screenManager.Draw(gameTime);
            //// TODO: Add your drawing code here
            //
            //spriteBatch.DrawString(font, "text sample", new Vector2(100, 100), Color.Black);
            //spriteBatch.Draw(TextureManager.Cell, new Rectangle(30, 30, 50, 50), Color.White);
            //spriteBatch.Draw(TextureManager.Diamond, new Rectangle(30, 30, 50, 50), Color.White);
            //
            //spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
