using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using static MatchThreeGameForest.ResourceManager.Constants;

namespace MatchThreeGameForest.Gui
{
    class Clickable : DrawableGameComponent
    {
        protected Rectangle Rectangle { get; set; }

        protected bool IsHighlighted { get; private set; }
        public bool IsClicked;

        protected new MatchGame Game { get { return (MatchGame)base.Game; } }

        protected Clickable() : base(MatchGame.instance) { }

        public virtual void HandleInput()
        {
            IsHighlighted = false;
            IsClicked = false;
            var mouseState = Mouse.GetState();
            if (!Rectangle.Contains(new Point(mouseState.X, mouseState.Y))) return;
            IsHighlighted = true;
            IsClicked = mouseState.LeftButton == ButtonState.Pressed;
        }
    }

    class Button : Clickable
    {
        protected readonly Texture2D texture;
        public event EventHandler<PlayerIndexEventArgs> Clicked;

        public Button(Texture2D texture, Point position)
        {
            this.texture = texture;
            Rectangle = new Rectangle(position, new Point(this.texture.Width, this.texture.Height));
        }

        public override void HandleInput()
        {
            base.HandleInput();
            if (IsClicked && Clicked != null)
                Clicked(this, new PlayerIndexEventArgs(PlayerIndex.One));
        }

        public override void Update(GameTime gameTime)
        {
            HandleInput();
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            var color = (IsHighlighted) ? ButtonColorHighlighted :
                        (IsClicked) ? ButtonColorClicked :
                                      ButtonColor;
            Game.spriteBatch.Begin();
            Game.spriteBatch.Draw(texture, Rectangle, color);
            Game.spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}