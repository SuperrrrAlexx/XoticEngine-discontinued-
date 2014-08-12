using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XoticEngine.GameObjects
{
    public class SpriteSheet
    {
        private readonly Texture2D sheet;
        private Texture2D[,] sprites;
        private int cols, rows;

        public SpriteSheet(string sheetName)
        {
            //Load the sheet
            this.sheet = Assets.Get<Texture2D>(sheetName);

            //Default is 1 column and row
            cols = 1;
            rows = 1;

            //Extract all sprites from the sheet
            ExtractSprites(sheetName);
        }
        public SpriteSheet(string sheetName, Rectangle[] sourceRectangles)
        {
            //Load the sheet
            this.sheet = Assets.Get<Texture2D>(sheetName);

            //Same amount of columns as rectangles, 1 row
            cols = sourceRectangles.Length;
            rows = 1;

            //Extract all sprites from the sheet
            ExtractSprites(sourceRectangles);
        }
        public SpriteSheet(Texture2D sheet, Rectangle[] sourceRectangles)
        {
            //Save the sheet
            this.sheet = sheet;

            //Same amount of columns as rectangles, 1 row
            cols = sourceRectangles.Length;
            rows = 1;

            //Extract all sprites from the sheet
            ExtractSprites(sourceRectangles);
        }
        public SpriteSheet(Texture2D sheet, int cols, int rows)
        {
            //Save the sheet
            this.sheet = sheet;

            //Default is 1 column and row
            cols = 1;
            rows = 1;

            //Extract all sprites from the sheet
            ExtractSprites("SpriteSheet@" + cols + "x" + rows);
        }
        public SpriteSheet(Texture2D[] sprites)
        {
            //Same amount of columns as sprites, 1 row
            cols = sprites.Length;
            rows = 1;

            //Save the sprites
            this.sprites = new Texture2D[cols, rows];
            for (int i = 0; i < cols; i++)
                this.sprites[i, 0] = sprites[i];
        }
        public SpriteSheet(Texture2D[,] sprites)
        {
            //Same amount of columns as sprites, 1 row
            cols = sprites.GetLength(0);
            rows = sprites.GetLength(1);

            //Save the sprites
            this.sprites = sprites;
        }

        private void ExtractSprites(string sheetName)
        {
            //Get the cols and rows
            string[] name = sheetName.Split('@');

            //Check for column/row data
            if (name.Length > 1)
            {
                string[] colrow = name[name.Length - 1].Split('x');
                cols = int.Parse(colrow[0]);
                if (colrow.Length == 2)
                    rows = int.Parse(colrow[1]);
            }

            //Get the sprite width and height
            int spriteWidth = sheet.Width / cols;
            int spriteHeight = sheet.Height / rows;

            //Set all the sprites
            sprites = new Texture2D[cols, rows];
            for (int c = 0; c < cols; c++)
                for (int r = 0; r < rows; r++)
                {
                    //The sprite source rectangle and texture
                    Rectangle sourceRect = new Rectangle(c * spriteWidth, r * spriteHeight, spriteWidth, spriteHeight);
                    Texture2D sprite = new Texture2D(Graphics.Device, spriteWidth, spriteHeight);

                    //Get the data from the sheet
                    Color[] data = new Color[spriteWidth * spriteHeight];
                    sheet.GetData(0, sourceRect, data, 0, data.Length);
                    sprite.SetData(data);

                    //Insert the sprite into the array of sprites
                    sprites[c, r] = sprite;
                }
        }
        private void ExtractSprites(Rectangle[] sourceRects)
        {
            //Create the array
            sprites = new Texture2D[sourceRects.Length, 1];

            //Extract each sprite based on the source rectangle
            for (int i = 0; i < sourceRects.Length; i++)
            {
                //Create a texture
                Texture2D sprite = new Texture2D(Graphics.Device, sourceRects[i].Width, sourceRects[i].Height);

                //Get the data from the sheet
                Color[] data = new Color[sourceRects[i].Width * sourceRects[i].Height];
                sheet.GetData(0, sourceRects[i], data, 0, data.Length);
                sprite.SetData(data);

                //Insert the sprite into the array of sprites
                sprites[i, 0] = sprite;
            }
        }

        public override string ToString()
        {
            return "SpriteSheet@" + cols + "x" + rows;
        }

        public Texture2D this[int item]
        {
            get
            {
                //If the item is larger than the length, throw an exception
                if (item >= Length)
                    throw new IndexOutOfRangeException("The spritesheet does not contain item number " + item);

                //Get the column and row
                int col = item % cols;
                int row = item / cols;

                //Return the sprite
                return sprites[col, row];
            }
        }
        public Texture2D this[int col, int row]
        {
            get
            {
                //If col or row is not part of the spritesheet, throw an exception
                if (col >= cols || col < 0 || row >= rows || row < 0)
                    throw new IndexOutOfRangeException("The spritesheet does not contain the coordinates (" + col + ", " + row + ")");

                //Return the sprite
                return sprites[col, row];
            }
        }

        public int Length
        { get { return cols * rows; } }
    }
}
