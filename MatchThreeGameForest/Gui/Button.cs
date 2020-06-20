#region File Description
//-----------------------------------------------------------------------------
// Button.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MatchThreeGameForest.Gui
{
    /// <summary>
    /// A special button that handles toggling between "On" and "Off"
    /// </summary>
    class Clickable : DrawableGameComponent
    {
        protected Rectangle Rectangle { get; set; }

        public Point Position
        {
            get { return new Point(Rectangle.X, Rectangle.Y); }
            set { Rectangle = new Rectangle(Position, value); }
        }

        protected bool IsHighlighted { get; private set; }
        public bool IsClicked;

        protected new Game1 Game { get { return (Game1)base.Game; } }

        private ButtonState oldClickState = ButtonState.Released;

        protected Clickable(Rectangle targetRectangle) : base(Game1.instance)
        {
            Rectangle = targetRectangle;
        }

        protected Clickable() : base(Game1.instance)
        {
        }

        protected void HandleInput()
        {
            IsHighlighted = false;
            IsClicked = false;
            var mouseState = Mouse.GetState();
            if (!Rectangle.Contains(new Point(mouseState.X, mouseState.Y))) return;
            IsHighlighted = true;
            IsClicked = mouseState.LeftButton == ButtonState.Pressed;

            //dlt
            oldClickState = mouseState.LeftButton;
        }
    }

    class Button : Clickable
    {
        protected readonly Texture2D texture;
        public Button(Texture2D texture, Point position)
        {
            this.texture = texture;
            Rectangle = new Rectangle(position, new Point(this.texture.Width, this.texture.Height));
        }
        public override void Update(GameTime gameTime)
        {
            HandleInput();
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            var color = Color.White;
            if (IsHighlighted)
                color = Color.Wheat;
            if (IsClicked)
                color = Color.Orange;
            Game.spriteBatch.Begin();
            Game.spriteBatch.Draw(texture, Rectangle, color);
            Game.spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}