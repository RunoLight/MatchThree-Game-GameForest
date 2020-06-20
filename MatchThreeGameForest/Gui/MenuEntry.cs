using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace MatchThreeGameForest.Gui
{
    class MenuEntry : Button
    {
        float selectionFade;
        Vector2 position;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public event EventHandler<PlayerIndexEventArgs> Selected;

        protected internal virtual void OnSelectEntry(PlayerIndex playerIndex)
        {
            if (Selected != null)
                Selected(this, new PlayerIndexEventArgs(playerIndex));
        }

        public MenuEntry(Texture2D texture, Point position) : base(texture, position)
        {
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
            base.Draw(gameTime);
        }

        public virtual int GetHeight(MenuScreen screen)
        {
            return screen.ScreenManager.Font.LineSpacing;
        }

        public virtual int GetWidth(MenuScreen screen)
        {
            return texture.Width;
        }
    }
}