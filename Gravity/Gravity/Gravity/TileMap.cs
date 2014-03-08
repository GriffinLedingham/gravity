using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gravity
{
    class TileMap
    {
        private int[,] map;

        private int width, height;

        public int TileWidth { get { return 16; } }
        public int TileHeight { get { return 16; } }

        public TileMap(int width, int height)
        {
            map = new int[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    map[i, j] = Game1.Game1Random.Next() % 2;
                }
            }

            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch should already have called its "begin()" method</param>
        public void draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (map[i, j] != 0)
                    {
                        spriteBatch.Draw(Game1.Pixi, new Vector2(i * TileWidth, j * TileHeight) + offset, new Rectangle(0, 0, 1, 1), Color.DarkBlue, 0.0f, Vector2.Zero, new Vector2(TileWidth, TileHeight), SpriteEffects.None, 0.5f);
                    }
                }
            }
        }
    }
}
