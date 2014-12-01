using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XoticEngine.GameObjects
{
    public class GameStateGrid : GameState
    {
        private Point cellSize;
        private readonly int gridWidth, gridHeight;
        private IGridObject[,] grid;

        public GameStateGrid(string name, int gridWidth, int gridHeight)
            : base(name)
        {
            this.cellSize = Point.Zero;
            this.gridWidth = gridWidth;
            this.gridHeight = gridHeight;
            grid = new IGridObject[gridWidth, gridHeight];
        }
        public GameStateGrid(string name, int gridWidth, int gridHeight, Point cellSize)
            : base(name)
        {
            this.cellSize = cellSize;
            this.gridWidth = gridWidth;
            this.gridHeight = gridHeight;
            grid = new IGridObject[gridWidth, gridHeight];
        }

        public override void Update()
        {
            //Update each GameObject in the grid
            foreach (IGridObject g in grid)
                if (g != null)
                    g.Update();

            base.Update();
        }
        public override void Draw(SpriteBatchHolder spriteBatches)
        {
            //Draw each GameObject in the grid
            for (int x = 0; x < grid.GetLength(0); x++)
                for (int y = 0; y < grid.GetLength(1); y++)
                    if (grid[x, y] != null)
                        grid[x, y].Draw(spriteBatches, new Point(x * CellWidth, y * CellHeight));

            base.Draw(spriteBatches);
        }

        public IGridObject this[int x, int y]
        {
            get { return grid[x, y]; }
            set
            {
                //Save the new grid object
                grid[x, y] = value;
            }
        }
        public Point CellSize
        { get { return cellSize; } set { cellSize = value; } }
        public int CellWidth
        { get { return cellSize.X; } set { cellSize.X = value; } }
        public int CellHeight
        { get { return cellSize.Y; } set { cellSize.Y = value; } }
        public int GridWidth
        { get { return gridWidth; } }
        public int GridHeight
        { get { return gridHeight; } }
    }
}
