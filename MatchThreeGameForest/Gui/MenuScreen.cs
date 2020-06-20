using MatchThreeGameForest.GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace MatchThreeGameForest.Gui
{
    abstract class MenuScreen : GameScreen
    {
        List<Button> menuButtons = new List<Button>();
        int selectedEntry = 0;

        InputAction menuUp;
        InputAction menuDown;
        InputAction menuSelect;
        InputAction menuCancel;

        protected IList<Button> MenuButtons
        {
            get { return menuButtons; }
        }

        public MenuScreen()
        {

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            menuUp = new InputAction(
                new Buttons[] { },
                new Keys[] { Keys.Up },
                true);
            menuDown = new InputAction(
                new Buttons[] { },
                new Keys[] { Keys.Down },
                true);
            menuSelect = new InputAction(
                new Buttons[] { },
                new Keys[] { Keys.Enter, Keys.Space },
                true);
            menuCancel = new InputAction(
                new Buttons[] { },
                new Keys[] { Keys.Escape },
                true);
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            foreach (var button in menuButtons)
            {
                button.HandleInput();
            }
        }

        protected virtual void OnCancel(PlayerIndex playerIndex)
        {
            ExitScreen();
        }

        protected void OnCancel(object sender, PlayerIndexEventArgs e)
        {
            OnCancel(e.PlayerIndex);
        }

        //public override void Update(GameTime gameTime, bool otherScreenHasFocus,
        //                                               bool coveredByOtherScreen)
        //{
        //    base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        //
        //    // Update each nested MenuEntry object.
        //    for (int i = 0; i < menuEntries.Count; i++)
        //    {
        //        bool isSelected = IsActive && (i == selectedEntry);
        //
        //        menuEntries[i].Update(this, isSelected, gameTime);
        //    }
        //}

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            spriteBatch.Begin();

            // Draw each menu entry in turn.
            for (int i = 0; i < menuButtons.Count; i++)
            {
                Button button = menuButtons[i];

                bool isSelected = IsActive && (i == selectedEntry);

                button.Draw(gameTime);
            }
            spriteBatch.End();
        }
    }
}