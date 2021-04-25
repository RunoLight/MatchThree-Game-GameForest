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
                    Detonate(gameTime.ElapsedGameTime.TotalMilliseconds);
                    break;
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
            Position = new Point(i, j);
            return (Position.X != j || Position.Y != i);
        }

        private void MoveUp(float distance)
        {
            location.Y -= distance;
            float end = Grid.Location.Y - Grid.CellSize.Y;
            if (location.Y <= end)
            {
                location.Y = end;
            }
        }
        private void MoveDown(float dist)
        {
            location.Y += dist;
            float end = Grid.Location.Y + Grid.CellSize.Y * GridSize;
            if (location.Y >= end)
            {
                location.Y = end;
            }
        }
        private void MoveLeft(float dist)
        {
            location.X -= dist;
            float end = Grid.Location.X - Grid.CellSize.X;
            if (location.X <= end)
            {
                location.X = end;
            }
        }
        private void MoveRight(float dist)
        {
            location.X += dist;
            float end = Grid.Location.X + Grid.CellSize.X * GridSize;
            if (location.X >= end)
            {
                location.X = end;
            }
        }

        private void Move(MoveType type, float distance)
        {
            float end = 0; ;
            bool ended = false;

            ref float transform = ref (((type & MoveType.Horizontal) > 0) ? ref location.X : ref location.Y);

            transform += distance * (((type & MoveType.FarCorner) > 0) ? 1 : -1);



            switch (type)
            {
                case MoveType.Up:
                    end = Grid.Location.Y - Grid.CellSize.Y;
                    break;
                case MoveType.Down:
                    end = Grid.Location.Y + Grid.CellSize.Y * GridSize;
                    break;
                case MoveType.Left:
                    end = Grid.Location.X - Grid.CellSize.X;
                    break;
                case MoveType.Right:
                    end = Grid.Location.X + Grid.CellSize.X * GridSize;
                    break;
            }


            ended = (((type & MoveType.FarCorner) > 0) && transform >= end) ||
                    (((type & MoveType.FarCorner) == 0) && transform <= end);

            if (ended)
            {
                toRemove = true;

                transform = end;
            }



            // type == MoveType.Down || type == MoveType.Up
            // type == MoveType.Down || type == MoveType.Up
            // 
            // type == MoveType.Right || type == MoveType.Down
            // type == MoveType.Right || type == MoveType.Down
            // -----------------------------------
            //ref float transform = ref ((type == MoveType.Down || type == MoveType.Up) ? ref location.Y : ref location.X);
            //float deltaDistance = distance * ((type == MoveType.Right || type == MoveType.Down) ? 1 : -1);

            //transform += deltaDistance;

            //float gridOffset = (type == MoveType.Down || type == MoveType.Up) ? Grid.Location.Y : Grid.Location.X;
            //float cellSize = ((type == MoveType.Down || type == MoveType.Up) ? Grid.CellSize.Y : Grid.CellSize.X) * ((type == MoveType.Right || type == MoveType.Down) ? 1 : -1);
            //float cellAmount = (type == MoveType.Right || type == MoveType.Down) ? GridSize : 1;

            //float destination = gridOffset + cellSize * cellAmount;

            //if ((transform >= destination && type == MoveType.Right || type == MoveType.Down) ||
            //    (transform <= destination && type != MoveType.FarCorner))
            //{
            //    transform = destination;
            //    toRemove = true;
            //}
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
