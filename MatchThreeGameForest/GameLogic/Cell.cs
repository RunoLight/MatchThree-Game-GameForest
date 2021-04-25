using MatchThreeGameForest.GameLogic.Utils;
using MatchThreeGameForest.ResourceManager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using static MatchThreeGameForest.ResourceManager.Constants;

namespace MatchThreeGameForest.GameLogic
{
    enum CellState
    {
        Normal,
        Hover,
        Pressed
    }

    enum FadeType
    {
        In,
        Out
    }

    enum MoveType
    {
        Vertical,
        Horizontal
    }

    class Cell
    {
        public AnimationType Animation { get; private set; }
        public bool IsSelected { get; private set; }
        public int Row { get; private set; }
        public int Column { get; private set; }

        public ShapeType Shape;
        public Bonus Bonus;
        public CellState State { get; set; }

        private Vector2 location;
        private Point size = Grid.CellSize;
        private Vector2 moveDestination;
        private readonly Texture2D texture = Resources.Cell;
        private float opacity;
        private float speed = MOVING_LERP_SPEED;

        public Cell(int row, int column)
        {
            Shape = ShapeType.Empty;
            Row = row;
            Column = column;

            location = new Vector2((Column * size.X) + GridOffset.X, (Row * size.Y) + GridOffset.Y);

            Animation = AnimationType.Revealing;
            State = CellState.Normal;
            Bonus = Bonus.None;
            IsSelected = false;
        }

        /// <summary> </summary>
        /// <returns>Returns true if animation is not finished</returns>
        internal bool Update(GameTime gameTime)
        {
            if (Animation == AnimationType.None)
            {
                return false;
            }

            switch (Animation)
            {
                case AnimationType.Hiding:
                    Fade(FadeType.In, gameTime);
                    break;
                case AnimationType.Revealing:
                    Fade(FadeType.Out, gameTime);
                    break;
                case AnimationType.Falling:
                    Fall(gameTime);
                    break;
                case AnimationType.Swapping:
                    Swap(gameTime);
                    break;
            }
            return true;
        }

        private void Fade(FadeType type, GameTime gameTime)
        {
            opacity = MathHelper.Lerp(opacity, 1.1f, OPACITY_LERP_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds);


            if (opacity >= 1f || opacity <= 0f)
            {
                opacity = MathHelper.Clamp(opacity, 0f, 1f);
                Animation = AnimationType.None;

                if (type == FadeType.Out)
                {
                    Shape = ShapeType.Empty;
                }
            }
        }

        private void Fall(GameTime gameTime)
        {
            var lerpingY = MathF.Min(moveDestination.Y, location.Y + 20f);
            location.Y = MathHelper.Lerp(location.Y, lerpingY, speed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            var distance = MathF.Abs(location.Y - moveDestination.Y);
            if (distance <= DISTANCE_TOLERANCE)
            {
                location.Y = moveDestination.Y;
                Animation = AnimationType.None;
            }
        }

        private void Swap(GameTime gameTime)
        {
            var movingComplete = (location.X == moveDestination.X && location.Y == moveDestination.Y);

            if (!movingComplete)
            {
                var moveType = (location.Y != moveDestination.Y) ? MoveType.Vertical : MoveType.Horizontal;
                movingComplete = Move(ref (moveType == MoveType.Vertical ? ref location.Y : ref location.X),
                    (moveType == MoveType.Vertical) ? moveDestination.Y : moveDestination.X, gameTime);
            }

            if (movingComplete)
            {
                Animation = AnimationType.None;
                opacity = 1f;
            }
        }

        // Returns true if moving is complete, false otherwise
        private bool Move(ref float location, float destination, GameTime gameTime)
        {
            location = MathHelper.Lerp(location, destination, speed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            var distance = MathF.Abs(location - destination);
            if (distance <= DISTANCE_TOLERANCE)
            {
                location = destination;
                return true;
            }
            return false;
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            Rectangle rectangle = new Rectangle((int)location.X, (int)location.Y, size.X, size.Y);
            if (texture != null)
            {
                switch (State)
                {
                    case CellState.Hover:
                        spriteBatch.Draw(texture, rectangle, CellHoverColor);
                        break;
                    case CellState.Pressed:
                        spriteBatch.Draw(texture, rectangle, CellPressedColor);
                        break;
                }
                if (IsSelected)
                {
                    spriteBatch.Draw(texture, rectangle, CellSelectedColor);
                }
            }

            spriteBatch.Draw(Resources.GetTexture(Shape, Bonus), location, null, new Color(Color.White, opacity),
                             0, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
            spriteBatch.End();
        }

        internal void Destroy()
        {
            if (Shape != ShapeType.Empty && Animation != AnimationType.Revealing)
            {
                State = CellState.Normal;
                Animation = AnimationType.Revealing;
                Grid.instance.SpawnDestroyer(Row, Column, Bonus);
            }
        }

        internal void Spawn(ShapeType shape)
        {
            State = CellState.Normal;
            Bonus = Bonus.None;
            Shape = shape;
            opacity = 0f;
            Animation = AnimationType.Hiding;
        }

        internal void FallInto(Cell other)
        {
            other.State = CellState.Normal;
            other.Shape = Shape;
            Shape = ShapeType.Empty;
            other.Bonus = Bonus;
            Bonus = Bonus.None;

            other.moveDestination = other.location;
            other.location = location;
            other.speed = (other.Row * MOVING_ROW_MULT) * MOVING_LERP_SPEED;

            other.Animation = AnimationType.Falling;
        }

        public bool IsCloseTo(Cell cell)
        {
            return (Column == cell.Column && (Row == cell.Row - 1 || Row == cell.Row + 1)) ||
                (Row == cell.Row && (Column == cell.Column - 1 || Column == cell.Column + 1));
        }

        internal void SwitchSelection()
        {
            IsSelected = !IsSelected;
        }

        internal void SwapWith(Cell other, bool swapBack)
        {
            float swapSpeed = MOVING_LERP_SPEED * (swapBack ? MOVING_BACK_MULT : 1);

            Utility.Swap<Vector2>(ref location, ref other.location);
            other.moveDestination = location;
            moveDestination = other.location;

            Utility.Swap<Bonus>(ref Bonus, ref other.Bonus);
            Utility.Swap<ShapeType>(ref Shape, ref other.Shape);
            State = other.State = CellState.Normal;
            Animation = other.Animation = AnimationType.Swapping;
            speed = other.speed = swapSpeed;
            opacity = other.opacity = CELL_SWAP_OPACITY;
        }

    }
}
