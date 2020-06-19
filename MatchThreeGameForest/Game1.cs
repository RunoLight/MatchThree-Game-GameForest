using MatchThreeGameForest.Textures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MatchThreeGameForest
{
    public class Game1 : Game
    {
        public static Game1 instance;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont font;

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


            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("Fonts/Font");
            TextureManager.Init(this.Content);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.AntiqueWhite);

            spriteBatch.Begin();

            // TODO: Add your drawing code here

            spriteBatch.DrawString(font, "Score", new Vector2(100, 100), Color.Black);
            spriteBatch.Draw(TextureManager.Cell, new Rectangle(30, 30, 50, 50), Color.White);
            spriteBatch.Draw(TextureManager.Diamond, new Rectangle(30, 30, 50, 50), Color.White);

            spriteBatch.End();



            base.Draw(gameTime);
        }
    }
}
