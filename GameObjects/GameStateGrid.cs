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
        private Vector2 cellSize;
        private readonly int gridWidth, gridHeight;
        private GameObject[,] grid;

        public GameStateGrid(string name, int gridWidth, int gridHeight)
            : base(name)
        {
            this.cellSize = Vector2.Zero;
            this.gridWidth = gridWidth;
            this.gridHeight = gridHeight;
            grid = new GameObject[gridWidth, gridHeight];
        }
        public GameStateGrid(string name, Vector2 cellSize, int gridWidth, int gridHeight)
            : base(name)
        {
            this.cellSize = cellSize;
            this.gridWidth = gridWidth;
            this.gridHeight = gridHeight;
            grid = new GameObject[gridWidth, gridHeight];
        }

        public override void Update()
        {
            //Update each GameObject in the grid
            foreach (GameObject g in grid)
                if (g != null)
                    g.Update();

            base.Update();
        }
        public override void Draw(SpriteBatchHolder spriteBatches)
        {
            //Draw each GameObject in the grid
            foreach (GameObject g in grid)
                if (g != null)
                    g.Draw(spriteBatches);

            base.Draw(spriteBatches);
        }

        public GameObject this[int x, int y]
        {
            get { return grid[x, y]; }
            set
            {
                //Set the parent for the new grid object
                if (value != null)
                    value.Parent = new GameObject("grid[" + x + ", " + y + "]", new Vector2(x * CellWidth, y * CellHeight));
                //Remove the parent for the old grid object
                if (grid[x, y] != null)
                    grid[x, y].Parent = null;
                //Save the new grid object
                grid[x, y] = value;
            }
        }
        public Vector2 CellSize
        { get { return cellSize; } set { cellSize = value; } }
        public float CellWidth
        { get { return cellSize.X; } set { cellSize.X = value; } }
        public float CellHeight
        { get { return cellSize.Y; } set { cellSize.Y = value; } }
        public int GridWidth
        { get { return gridWidth; } }
        public int GridHeight
        { get { return gridHeight; } }
    }
}
