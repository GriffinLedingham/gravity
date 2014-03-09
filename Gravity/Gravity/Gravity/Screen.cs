using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gravity
{

    class Btn
    {
        public Texture2D tx;
        public Rectangle Rect;
        public Btn(Texture2D t, Rectangle r)
        {
            tx = t;
            Rect = r;
        }
    }

    class Screen
    {

        Rectangle Btn = new Rectangle();

        float colourDelta = 0;

        public void init(Rectangle btn)
        {
            Btn = btn;
        }

        public void update(TouchCollection tc)
        {

            foreach (TouchLocation tl in tc)
            {
                if (tl.Position.X > Btn.X && tl.Position.X < Btn.X + Btn.Width && tl.Position.Y > Btn.Y && tl.Position.Y < Btn.Y + Btn.Height)
                {

                }
            }
        }

        public void render()
        {

        }

    }

    public class MainMenu
    {

        Btn start;
        public bool StartPress = false;

        float colourDelta = 0;

        int sloganIndex = 0;

        private string[] sloganTable =
        {
            "Hello, world!",
            "The Corgiest game around!",
            "(^_^)b",
            "A game for two!",
            "SPACESHIPS!",
            "Play me with a friend!",
            "Space and corgies and fun!",
            "Fun with sprite batching!",
            "So cute!"
        };

        public void Load(ContentManager content)
        {
            start = new Btn(Game1.menuSprites, new Rectangle(250, 360, 300, 55));

            sloganIndex = (Game1.Game1Random.Next()) % sloganTable.Length;
        }

        public void Update(TouchCollection tc, GameTime gameTime)
        {
            foreach (TouchLocation tl in tc)
            {
                if (tl.State == TouchLocationState.Pressed && tl.Position.X > start.Rect.X && tl.Position.X < start.Rect.X + start.Rect.Width && tl.Position.Y > start.Rect.Y && tl.Position.Y < start.Rect.Y + start.Rect.Height)
                {
                    StartPress = true;
                }
            }

            colourDelta += gameTime.ElapsedGameTime.Milliseconds;
        }

        public void render(SpriteBatch sb)
        {
            // OH OH YEAH DANIEL SAVAGE IN THE HOUSE WITH SOME MENU GRAPHICXXX

            sb.Begin();

            sb.Draw(Game1.backgroundImage, new Rectangle(-4000 - (int)((colourDelta / 10) % 2000), -3500, 8000, 7000), Color.White);

            sb.Draw(Game1.spaceCorgi, new Vector2(120, 350 + (float)(Math.Sin(colourDelta / 500f) * 50)), null, Color.White, (float)(Math.Sin(colourDelta / 1000f) * 0.075f), new Vector2(45, 37 ) / 2, 3.0f, SpriteEffects.None, 0.5f);

            sb.Draw(Game1.gameLogo, new Vector2((sb.GraphicsDevice.DisplayMode.Width - Game1.gameLogo.Width) / -2, 10), Color.White);

            sb.Draw(Game1.menuSprites, start.Rect, new Rectangle(0, 0, 1821, 673 / 2), Color.White);
            sb.Draw(Game1.menuSprites, start.Rect, new Rectangle(0, 673 / 2, 1821, 673 / 2), new Color((float)(Math.Sin(colourDelta / 100f) / 2 + 0.5f), (float)(Math.Sin(colourDelta / 100f + 2) / 2 + 0.5f), (float)(Math.Sin(colourDelta / 100f + 4) / 2 + 0.5f), 1.0f));

            sb.DrawString(Game1.gameFont, sloganTable[sloganIndex], new Vector2((sb.GraphicsDevice.DisplayMode.Width - Game1.gameLogo.Width) / -2 + 500, 300), Color.Orange, -0.1f, Game1.gameFont.MeasureString(sloganTable[sloganIndex]) / 2, (float)(Math.Sin(colourDelta / 500f) * 0.075f + 1.0f), SpriteEffects.None, 1.0f);
            sb.End();
        }

    }


    public class EndScreen
    {

        public bool YouWon = false;
        float colourDelta = 0;

        public void Update(TouchCollection tc, GameTime gameTime)
        {
            colourDelta += gameTime.ElapsedGameTime.Milliseconds;
        }

        public void render(SpriteBatch sb)
        {
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            if (YouWon)
            {
                sb.Draw(Game1.winner, new Rectangle(-4000 - (int)((colourDelta / 10) % 2000), -3500, 8000, 7000), Color.Green);

                sb.Draw(Game1.spaceCorgi, new Vector2(120, 400), null, Color.White, 0.0f, new Vector2(45, 37) / 2, 3.0f, SpriteEffects.None, 0.5f);
                sb.Draw(Game1.youWin, new Vector2(20), Color.White);
            }
            else
            {
                sb.Draw(Game1.loser, new Rectangle(-4000 - (int)((colourDelta / 10) % 2000), -3500, 8000, 7000), Color.Purple);

                sb.Draw(Game1.spaceCorgi, new Vector2(120, 400), null, Color.White, 0.0f, new Vector2(45, 37) / 2, 3.0f, SpriteEffects.FlipVertically, 0.5f);

                sb.Draw(Game1.youLose, new Vector2(20), Color.White);
            }

            sb.End();
        }

    }





}
