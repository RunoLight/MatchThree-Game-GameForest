using MatchThreeGameForest.GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MatchThreeGameForest.Gui
{
    abstract class MenuScreen : GameScreen
    {
        List<Button> menuButtons = new List<Button>();

        protected IList<Button> MenuButtons
        {
            get { return menuButtons; }
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            foreach (var button in menuButtons)
            {
                button.HandleInput();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            for (int i = 0; i < menuButtons.Count; i++)
            {
                menuButtons[i].Draw(gameTime);
            }
            spriteBatch.End();
        }
    }
}