using MatchThreeGameForest.ResourceManager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static MatchThreeGameForest.ResourceManager.Constants;

namespace MatchThreeGameForest.GameLogic
{
    class Destroyer
    {
        private Vector2 location;
        private Texture2D texture = Resources.Destroyer;
        private double timer;
        public Direction Direction { get; private set; }
        public Point Position { get; private set; }
        public bool toRemove { get; private set; }

        public Destroyer(Vector2 location, Direction direction)
        {
            this.location = location;
            Direction = direction;
            toRemove = false;
            Position = new Point(-1, -1);
        }

        internal bool Update(GameTime gameTime)
        {
            bool isNewBlockReached = SetPosition() &&
                Position.Y < GridSize && Position.Y >= 0 && Position.X >= 0 && Position.X < GridSize;
            float speed = (float)(300f * gameTime.ElapsedGameTime.TotalSeconds);
            switch (Direction)
            {
                case Direction.Up:
                    MoveUp(speed);
                    break;
                case Direction.Down:
                    MoveDown(speed);
                    break;
                case Direction.Left:
                    MoveLeft(speed);
                    break;
                case Direction.Right:
                    MoveRight(speed);
                    break;
                case Direction.Detonate:
                    Detonate(gameTime.ElapsedGameTime.TotalMilliseconds);
                    break;
                case Direction.SecondDetonate:
                    Detonate(gameTime.ElapsedGameTime.TotalMilliseconds);
                    break;
            }
            return isNewBlockReached;
        }

        private bool SetPosition()
        {
            bool r = false;
            int i = (int)((location.Y - Grid.Location.Y) / Grid.CellSize.Y);
            int j = (int)((location.X - Grid.Location.X) / Grid.CellSize.X);

            if (Position.X != j || Position.Y != i)
            {
                r = true;
            }
            Position = new Point(i, j);
            return r;
        }

        private void MoveUp(float dist)
        {
            location.Y -= dist;
            float end = Grid.Location.Y - Grid.CellSize.Y;
            if (location.Y <= end)
            {
                location.Y = end;
                toRemove = true;
            }
        }

        private void MoveDown(float dist)
        {
            location.Y += dist;
            float end = Grid.Location.Y + Grid.CellSize.Y * GridSize;
            if (location.Y >= end)
            {
                location.Y = end;
                toRemove = true;
            }
        }

        private void MoveLeft(float dist)
        {
            location.X -= dist;
            float end = Grid.Location.X - Grid.CellSize.X;
            if (location.X <= end)
            {
                location.X = end;
                toRemove = true;
            }
        }

        private void MoveRight(float dist)
        {
            location.X += dist;
            float end = Grid.Location.X + Grid.CellSize.X * GridSize;
            if (location.X >= end)
            {
                location.X = end;
                toRemove = true;
            }
        }

        private void Detonate(double elapsedMilliseconds)
        {
            timer += elapsedMilliseconds;
            if (timer >= 250f)
            {
                toRemove = true;
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, location, null, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
            spriteBatch.End();
        }
    }
}
