using MatchThreeGameForest.ResourceManager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

    class Cell
    {
        public AnimationType Animation { get; private set; }
        public bool IsSelected { get; private set; }
        public int Row { get; private set; }
        public int Column { get; private set; }
        public ShapeType Shape { get; set; }
        public Bonus Bonus { get; set; }
        public CellState State { get; set; }

        private Vector2 location;
        private Point size;
        private Vector2 moveDestination;
        private readonly Texture2D texture;
        private float opacity;
        private int speed;

        public Cell(int row, int column)
        {
            Shape = ShapeType.Empty;
            texture = Resources.Cell;
            Row = row;
            Column = column;

            size = Grid.CellSize;
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
            float fadeDelta = CellFadeSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (type == FadeType.Out)
            {
                //fadeDelta = fadeDelta * -1;
                opacity -= fadeDelta;
            }
            else
            {
                opacity += fadeDelta;
            }

            opacity += fadeDelta;

            //opacity = MathHelper.Lerp(opacity, (type == FadeType.Out) ? 0 : 1, 0.7f);


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
            location.Y += CellFallSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (location.Y >= moveDestination.Y)
            {
                location.Y = moveDestination.Y;
                Animation = AnimationType.None;
            }
        }

        private void Swap(GameTime gameTime)
        {
            if (location.X == moveDestination.X)
            {
                if (location.Y == moveDestination.Y)
                {
                    Animation = AnimationType.None;
                    opacity = 1f;
                }
                else
                {
                    MoveHorizontal(gameTime);
                }
            }
            else
            {
                MoveVertical(gameTime);
            }
        }

        private void MoveHorizontal(GameTime gameTime)
        {
            if (location.Y < moveDestination.Y)
            {
                location.Y += (float)(speed * gameTime.ElapsedGameTime.TotalSeconds);
                if (location.Y >= moveDestination.Y)
                {
                    location.Y = moveDestination.Y;
                    Animation = AnimationType.None;
                    opacity = 1f;
                }
            }
            else
            {
                location.Y -= (float)(speed * gameTime.ElapsedGameTime.TotalSeconds);
                if (location.Y <= moveDestination.Y)
                {
                    location.Y = moveDestination.Y;
                    Animation = AnimationType.None;
                    opacity = 1f;
                }
            }
        }

        private void MoveVertical(GameTime gameTime)
        {
            if (location.X < moveDestination.X)
            {
                location.X += (float)(speed * gameTime.ElapsedGameTime.TotalSeconds);
                if (location.X >= moveDestination.X)
                {
                    location.X = moveDestination.X;
                    Animation = AnimationType.None;
                    opacity = 1f;
                }
            }
            else
            {
                location.X -= (float)(speed * gameTime.ElapsedGameTime.TotalSeconds);
                if (location.X <= moveDestination.X)
                {
                    location.X = moveDestination.X;
                    Animation = AnimationType.None;
                    opacity = 1f;
                }
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            Rectangle rectangle = new Rectangle((int)location.X, (int)location.Y, size.X, size.Y);
            if (texture != null)
            {
                switch (State)
                {
                    case CellState.Normal:
                        spriteBatch.Draw(texture, rectangle, Color.White);
                        break;
                    case CellState.Hover:
                        spriteBatch.Draw(texture, rectangle, Color.Chocolate);
                        break;
                    case CellState.Pressed:
                        spriteBatch.Draw(texture, rectangle, Color.White);
                        break;
                }
                if (IsSelected)
                {
                    spriteBatch.Draw(texture, rectangle, Color.Red);
                }
            }

            spriteBatch.Draw(Resources.GetTexture(Shape, Bonus),
                             location,
                             null,
                             new Color(Color.White, opacity),
                             0,
                             Vector2.Zero,
                             0.5f,
                             SpriteEffects.None,
                             0f);
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

        internal void FallInto(Cell cell)
        {
            cell.State = CellState.Normal;
            cell.Shape = Shape;
            Shape = ShapeType.Empty;
            cell.Bonus = Bonus;
            Bonus = Bonus.None;

            cell.moveDestination = cell.location;
            cell.location = location;
            cell.speed = (cell.Row * 35) + 150;

            cell.Animation = AnimationType.Falling;
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
            int swapSpeed = swapBack ? CellSwapBackSpeed : CellSwapSpeed;

            State = CellState.Normal;
            other.State = CellState.Normal;

            Vector2 tempLocation = location;
            location = other.location;
            other.location = tempLocation;

            other.moveDestination = location;
            moveDestination = other.location;

            ShapeType tempShape = Shape;
            Shape = other.Shape;
            other.Shape = tempShape;

            Bonus tempBonus = Bonus;
            Bonus = other.Bonus;
            other.Bonus = tempBonus;

            speed = other.speed = swapSpeed;

            opacity = other.opacity = 0.5f;

            Animation = AnimationType.Swapping;
            other.Animation = AnimationType.Swapping;
        }

    }
}
