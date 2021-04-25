using MatchThreeGameForest.ResourceManager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using static MatchThreeGameForest.ResourceManager.Constants;

namespace MatchThreeGameForest.GameLogic
{
    class Grid
    {
        public static Grid instance;

        public Cell[,] cells = new Cell[GridSize, GridSize];
        public Cell currentCell;
        public Cell selectedCell;

        public List<Destroyer> destroyerList = new List<Destroyer>();

        private Random random = new Random();
        private Array shapes = Enum.GetValues(typeof(ShapeType));

        public static Point Location => GridOffset;

        public static Point CellSize { get; private set; }
        public bool IsAnimating { get; private set; }

        public Grid()
        {
            CellSize = new Point(Resources.Cell.Width / 2, Resources.Cell.Height / 2);
            IsAnimating = false;
            instance = this;
        }

        internal void LoadContent(ContentManager content)
        {
            for (int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    cells[i, j] = new Cell(i, j);
                }
            }
        }

        internal void Update(GameTime gameTime)
        {
            IsAnimating = false;

            foreach (var cell in cells)
            {
                if (cell.Animation != AnimationType.None)
                {
                    IsAnimating = true;
                }
                cell.Update(gameTime);
            }

            UpdateDestroyers(gameTime);
        }

        private void UpdateDestroyers(GameTime gameTime)
        {
            List<Point> toDestroy = new List<Point>(destroyerList.Count);
            foreach (var destroyer in destroyerList)
            {
                IsAnimating = true;
                if (destroyer.Update(gameTime))
                {
                    toDestroy.Add(new Point(destroyer.Position.X, destroyer.Position.Y));
                }
            }
            var detonated = destroyerList.FindAll(d => d.toRemove && d.Direction == Direction.Detonate);
            FarDestroyers(detonated);
            destroyerList.RemoveAll(d => d.toRemove);
            int scoreFromDestroyers = toDestroy.Count(c => cells[c.X, c.Y].Animation == AnimationType.None);
            scoreFromDestroyers = scoreFromDestroyers * 10;
            GameScore.Add(scoreFromDestroyers);
            toDestroy.ForEach(point => cells[point.X, point.Y].Destroy());
        }

        private void FarDestroyers(List<Destroyer> blown)
        {
            foreach (var item in blown)
            {
                for (int i = item.Position.X - 1; i <= item.Position.X + 1; i++)
                {
                    for (int j = item.Position.Y - 1; j <= item.Position.Y + 1; j++)
                    {
                        if (i == item.Position.X && j == item.Position.Y)
                        {
                            continue;
                        }
                        destroyerList.Add(new Destroyer(new Vector2(j * CellSize.X + Location.X, i * CellSize.Y + Location.Y),
                            Direction.SecondDetonate));
                    }
                }
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            // Grid background
            var texture = Resources.Cell;
            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    var location = new Point((i * Grid.CellSize.X) + GridOffset.X, (j * Grid.CellSize.Y) + GridOffset.Y);
                    Rectangle rectangle = new Rectangle(location, Grid.CellSize);
                    spriteBatch.Draw(texture, rectangle, CellColor);
                }
            }
            spriteBatch.End();
            foreach (var cell in cells)
            {
                cell.Draw(spriteBatch);
            }
            foreach (var destroyer in destroyerList)
            {
                destroyer.Draw(spriteBatch);
            }
        }

        internal bool FillGrid()
        {
            bool gridChanged = false;

            for (int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    if (cells[i, j].Shape == ShapeType.Empty)
                    {
                        gridChanged = true;
                        ShapeType shape = (ShapeType)shapes.GetValue(random.Next(shapes.Length - 1) + 1);
                        cells[i, j].Spawn(shape);
                        cells[i, j].Bonus = Bonus.None;
                    }
                }
            }
            return gridChanged;
        }

        internal int MatchAndGetPoints()
        {
            int score = 0;
            List<Cell> toDestroy = new List<Cell>(GridSize * GridSize);
            List<Cell> newBonuses = new List<Cell>();

            for (int i = 0; i < cells.GetLength(0); i++)
            {
                List<Cell> match = new List<Cell>(GridSize) { cells[i, 0] };
                for (int j = 1; j < cells.GetLength(1); j++)
                {
                    bool stop = false;
                    if (cells[i, j].Shape == match[0].Shape)
                    {
                        match.Add(cells[i, j]);
                    }
                    else
                    {
                        stop = true;
                    }
                    if (stop || j == cells.GetLength(1) - 1)
                    {
                        Debug.Assert(true);
                        //Debug.WriteLine("Match clear and add");
                        if (match.Count >= 3)
                        {
                            if (selectedCell != null)
                            {
                                newBonuses.Add(SpawnBonus(match, Bonus.LineHorizontal));
                            }
                            toDestroy.AddRange(match);
                            score += match.Count * 10;
                        }
                        match.Clear();
                        match.Add(cells[i, j]);
                    }
                }
            }
            for (int j = 0; j < cells.GetLength(1); j++)
            {
                List<Cell> match = new List<Cell>(GridSize) { cells[0, j] };
                for (int i = 1; i < cells.GetLength(0); i++)
                {
                    bool stop = false;
                    if (cells[i, j].Shape == match[0].Shape)
                    {
                        match.Add(cells[i, j]);
                    }
                    else
                    {
                        stop = true;
                    }
                    if (stop || i == cells.GetLength(0) - 1)
                    {
                        if (match.Count >= 3)
                        {
                            if (selectedCell != null)
                            {
                                newBonuses.Add(SpawnBonus(match, Bonus.LineVertical));
                            }
                            var intersect = toDestroy.Intersect(match);
                            List<Cell> intersectList = intersect.ToList();
                            foreach (var bonusCell in intersectList)
                            {
                                match.Remove(bonusCell);
                                toDestroy.Remove(bonusCell);
                                bonusCell.Bonus = Bonus.Bomb;
                            }
                            toDestroy.AddRange(match);
                            score += match.Count * 10;
                        }
                        match.Clear();
                        match.Add(cells[i, j]);
                    }
                }
            }
            newBonuses.ForEach(c => toDestroy.Remove(c));
            toDestroy.ForEach(cell => cell.Destroy());
            if (selectedCell != null && score > 0)
            {
                selectedCell = null;
            }
            return score;
        }

        private Cell SpawnBonus(List<Cell> matchedCells, Bonus lineType)
        {
            Cell targetCell = matchedCells.Find(cell => (cell == selectedCell || cell == currentCell));
            if (targetCell.Bonus != Bonus.None)
                SpawnDestroyer(targetCell.Row, targetCell.Column, targetCell.Bonus);

            switch (matchedCells.Count)
            {
                case 4:
                    targetCell.Bonus = lineType;
                    break;
                case 5:
                    targetCell.Bonus = Bonus.Bomb;
                    break;
            }
            return targetCell;
        }

        internal void SpawnDestroyer(int row, int column, Bonus bonus)
        {
            switch (bonus)
            {
                case Bonus.LineVertical:
                    destroyerList.Add(new Destroyer(new Vector2(column * CellSize.X + Location.X, (row - 0.5f) * CellSize.Y + Location.Y),
                        Direction.Up));
                    destroyerList.Add(new Destroyer(new Vector2(column * CellSize.X + Location.X, (row + 0.5f) * CellSize.Y + Location.Y),
                        Direction.Down));
                    break;
                case Bonus.LineHorizontal:
                    destroyerList.Add(new Destroyer(new Vector2((column - 0.5f) * CellSize.X + Location.X, row * CellSize.Y + Location.Y),
                        Direction.Left));
                    destroyerList.Add(new Destroyer(new Vector2((column + 0.5f) * CellSize.X + Location.X, row * CellSize.Y + Location.Y),
                        Direction.Right));
                    break;
                case Bonus.Bomb:
                    destroyerList.Add(new Destroyer(new Vector2(column * CellSize.X + Location.X, row * CellSize.Y + Location.Y),
                        Direction.Detonate));
                    break;
            }
        }

        internal void DropCells()
        {
            for (int j = 0; j < cells.GetLength(1); j++)
            {
                for (int i = cells.GetLength(0) - 1; i > 0; i--)
                {
                    if (cells[i, j].Shape == ShapeType.Empty)
                    {
                        int k = i - 1;
                        while (k >= 0 && cells[k, j].Shape == ShapeType.Empty)
                        {
                            k--;
                        }
                        if (k < 0) break;
                        cells[k, j].FallInto(cells[i, j]);
                    }
                }
            }
        }

        internal bool UserInput()
        {
            MouseState mouseState = Mouse.GetState();

            Rectangle fieldRect = new Rectangle(Location.X, Location.Y, CellSize.X * GridSize, CellSize.Y * GridSize);

            if (fieldRect.Contains(mouseState.Position))
            {
                int i = (mouseState.Position.Y - fieldRect.Y) / CellSize.Y;
                int j = (mouseState.Position.X - fieldRect.X) / CellSize.X;

                if (currentCell != null && cells[i, j] != currentCell)
                {
                    currentCell.State = CellState.Normal;
                }
                currentCell = cells[i, j];

                if (mouseState.LeftButton == ButtonState.Released && currentCell.State == CellState.Pressed)
                {
                    currentCell.State = CellState.Hover;
                    if (currentCell.IsSelected)
                        ClearSelection();
                    else if (selectedCell != null && currentCell.IsCloseTo(selectedCell))
                        return true;
                    else
                        SelectCurrentCell();
                }
                else if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    currentCell.State = CellState.Pressed;
                }
                else
                {
                    currentCell.State = CellState.Hover;
                }
            }
            else
            {
                if (currentCell != null)
                {
                    currentCell.State = CellState.Normal;
                }
            }
            return false;
        }

        private void SelectCurrentCell()
        {
            currentCell.SwitchSelection();
            selectedCell?.SwitchSelection();
            selectedCell = currentCell;
        }

        private void ClearSelection()
        {
            currentCell.SwitchSelection();
            selectedCell = null;
        }

        internal void Swap()
        {
            selectedCell.SwapWith(currentCell, false);
            selectedCell.SwitchSelection();
        }

        internal void SwapBack()
        {
            currentCell.SwapWith(selectedCell, true);
            selectedCell = null;
        }
    }

}
