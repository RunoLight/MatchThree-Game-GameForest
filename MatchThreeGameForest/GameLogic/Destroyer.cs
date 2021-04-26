using MatchThreeGameForest.ResourceManager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using static MatchThreeGameForest.ResourceManager.Constants;

namespace MatchThreeGameForest.GameLogic
{
    class Destroyer
    {
        [Flags]
        enum MoveType
        {
            Up = 1,
            Down = 2,
            Left = 4,
            Right = 8,
            Vertical = Up | Down,
            Horizontal = Left | Right,
            FarCorner = Right | Down
        }

        private bool IsHorizontal(MoveType type)
        {
            return (type & MoveType.Horizontal) > 0;
        }

        private bool IsFarCorner(MoveType type)
        {
            return (type & MoveType.FarCorner) > 0;
        }

        private Vector2 location;
        private readonly Texture2D texture = Resources.Destroyer;
        private double timer;
        public Direction Direction { get; private set; }
        public Point Position { get; private set; }
        public bool toRemove { get; private set; }

        public Destroyer(Vector2 _location, Direction direction)
        {
            location = _location;
            Direction = direction;
            toRemove = false;
            Position = new Point(-1, -1);
        }

        internal bool Update(GameTime gameTime)
        {
            bool isNewBlockReached = UpdatePosition() &&
                Position.Y < GridSize && Position.Y >= 0 &&
                Position.X < GridSize && Position.X >= 0;
            float speed = DESTROYER_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
            switch (Direction)
            {
                case Direction.Up:
                    Move(MoveType.Up, speed);
                    break;
                case Direction.Down:
                    Move(MoveType.Down, speed);
                    break;
                case Direction.Left:
                    Move(MoveType.Left, speed);
                    break;
                case Direction.Right:
                    Move(MoveType.Right, speed);
                    break;
                case Direction.Detonate:
                case Direction.SecondDetonate:
                    Detonate(gameTime.ElapsedGameTime.TotalMilliseconds);
                    break;
            }
            return isNewBlockReached;
        }

        // Returns true if position is changed
        private bool UpdatePosition()
        {
            int i = (int)((location.Y - Grid.Location.Y) / Grid.CellSize.Y);
            int j = (int)((location.X - Grid.Location.X) / Grid.CellSize.X);
            bool positionChanged = Position.X != j || Position.Y != i;
            Position = new Point(i, j);
            return positionChanged;
        }

        private void Move(MoveType type, float distance)
        {
            ref float transform = ref ((IsHorizontal(type)) ? ref location.X : ref location.Y);
            transform += distance * (((type & MoveType.FarCorner) > 0) ? 1 : -1);

            float gridOffset = (IsHorizontal(type)) ? Grid.Location.X : Grid.Location.Y;
            float cellsToCorner = (IsFarCorner(type)) ? GridSize - 1 : 0;
            float cellSize = (IsHorizontal(type)) ? Grid.CellSize.X : Grid.CellSize.Y;

            float trasformDestination = gridOffset + cellSize * cellsToCorner;
            trasformDestination += cellSize * 0.15f * (IsFarCorner(type) ? 1 : -1);

            bool isComplete = (IsFarCorner(type) && transform >= trasformDestination) ||
                    (!IsFarCorner(type) && transform <= trasformDestination);

            if (isComplete)
            {
                toRemove = true;
                transform = trasformDestination;
            }
        }

        private void Detonate(double elapsedMilliseconds)
        {
            timer += elapsedMilliseconds;
            toRemove = timer >= 250f;
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, location, null, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
            spriteBatch.End();
        }
    }
}
