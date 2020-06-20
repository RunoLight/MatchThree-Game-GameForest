
using MatchThreeGameForest.GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace MatchThreeGameForest.Gui
{
    class MenuEntry : Button
    {
        string text;

        /// <summary>
        /// Tracks a fading selection effect on the entry.
        /// </summary>
        /// <remarks>
        /// The entries transition out of the selection effect when they are deselected.
        /// </remarks>
        float selectionFade;

        /// <summary>
        /// The position at which the entry is drawn. This is set by the MenuScreen
        /// each frame in Update.
        /// </summary>
        Vector2 position;


        /// <summary>
        /// Gets or sets the position at which to draw this menu entry.
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public event EventHandler<PlayerIndexEventArgs> Selected;


        /// <summary>
        /// Method for raising the Selected event.
        /// </summary>
        protected internal virtual void OnSelectEntry(PlayerIndex playerIndex)
        {
            if (Selected != null)
                Selected(this, new PlayerIndexEventArgs(playerIndex));
        }

        public MenuEntry(Texture2D texture, Point position, string text) : base(texture, position)
        {
            this.text = text;
        }

        public virtual void Update(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;

            if (IsClicked)
                selectionFade = Math.Min(selectionFade + fadeSpeed, 1);
            else
                selectionFade = Math.Max(selectionFade - fadeSpeed, 0);
            base.Update(gameTime);
        }


        /// <summary>
        /// Draws the menu entry. This can be overridden to customize the appearance.
        /// </summary>
        public virtual void Draw(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            // there is no such thing as a selected item on Windows Phone, so we always
            // force isSelected to be false

            // Draw the selected entry in yellow, otherwise white.
            Color color = IsClicked ? Color.Yellow : Color.White;

            // Pulsate the size of the selected menu entry.
            double time = gameTime.TotalGameTime.TotalSeconds;

            float pulsate = (float)Math.Sin(time * 6) + 1;

            float scale = 1 + pulsate * 0.05f * selectionFade;

            // Modify the alpha to fade text out during transitions.
            color *= screen.TransitionAlpha;

            // Draw text, centered on the middle of each line.
            ScreenManager screenManager = screen.ScreenManager;
            SpriteBatch spriteBatch = screenManager.SpriteBatch;
            SpriteFont font = screenManager.Font;

            Vector2 origin = new Vector2(0, font.LineSpacing / 2);

            if (font == null)
                throw new Exception();

            if (text == null)
                throw new Exception();

            if (position == null)
                throw new Exception();

            if (color == null)
                throw new Exception();

            if (origin == null)
                throw new Exception();

            if (scale == null)
                throw new Exception();

            spriteBatch.DrawString(font, text, position, color, 0,
                                   origin, scale, SpriteEffects.None, 0);

            base.Draw(gameTime);
        }


        /// <summary>
        /// Queries how much space this menu entry requires.
        /// </summary>
        public virtual int GetHeight(MenuScreen screen)
        {
            return screen.ScreenManager.Font.LineSpacing;
        }


        /// <summary>
        /// Queries how wide the entry is, used for centering on the screen.
        /// </summary>
        public virtual int GetWidth(MenuScreen screen)
        {
            return texture.Width;
        }
    }
}