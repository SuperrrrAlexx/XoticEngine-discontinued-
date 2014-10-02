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
        private readonly int cellWidth, cellHeight, gridWidth, gridHeight;
        private GameObject[,] grid;

        public GameStateGrid(string name, int cellWidth, int cellHeight, int gridWidth, int gridHeight)
            : base(name)
        {
            this.cellWidth = cellWidth;
            this.cellHeight = cellHeight;
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
                    value.Parent = new GameObject("grid[" + x + ", " + y + "]", new Vector2(x * cellWidth, y * cellHeight));
                //Remove the parent for the old grid object
                if (grid[x, y] != null)
                    grid[x, y].Parent = null;
                //Save the new grid object
                grid[x, y] = value;
            }
        }
        public int CellWidth
        { get { return cellWidth; } }
        public int CellHeight
        { get { return cellHeight; } }
        public int GridWidth
        { get { return gridWidth; } }
        public int GridHeight
        { get { return gridHeight; } }
    }
}
