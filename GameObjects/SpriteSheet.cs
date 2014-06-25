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
        Texture2D sheet;
        Texture2D[,] sprites;
        int cols, rows;
        int spriteWidth, spriteHeight;
        int length;

        public SpriteSheet(string sheetName)
        {
            //Load the sheet
            this.sheet = Assets.Get<Texture2D>(sheetName);

            //Get the cols and rows
            string[] name = sheetName.Split(new char[] { '@', 'x' }, StringSplitOptions.RemoveEmptyEntries);
            switch (name.Count())
            {
                case 1:
                    //Just the name, 1 sprite
                    cols = 1;
                    rows = 1;
                    break;
                case 2:
                    //Only cols
                    cols = int.Parse(name[1]);
                    rows = 1;
                    break;
                case 3:
                    //Cols and rows
                    cols = int.Parse(name[1]);
                    rows = int.Parse(name[2]);
                    break;
                default:
                    //Exception
                    throw new FormatException("The spritesheet name must be in this format: SpriteSheet_NAME@COLSxROWS");
            }

            //Get the sprite width and height
            spriteWidth = sheet.Width / cols;
            spriteHeight = sheet.Height / rows;

            //Set all the sprites
            sprites = new Texture2D[cols, rows];
            for (int c = 0; c < cols; c++)
            {
                for (int r = 0; r < rows; r++)
                {
                    //The sprite texture
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

            //Get the amount of sprites
            length = cols * rows;
        }

        public Texture2D this[int item]
        {
            get
            {
                //If the item is larger than the length, throw an exception
                if (item >= length)
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
        { get { return length; } }
    }
}
