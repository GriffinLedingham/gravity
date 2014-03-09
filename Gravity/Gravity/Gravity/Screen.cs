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

        public void Load(ContentManager content)
        {
            start = new Btn(content.Load<Texture2D>("pixi"), new Rectangle(350, 250, 100, 40));
        }

        public void Update(TouchCollection tc)
        {
            foreach (TouchLocation tl in tc)
            {
                if (tl.State == TouchLocationState.Pressed && tl.Position.X > start.Rect.X && tl.Position.X < start.Rect.X + start.Rect.Width && tl.Position.Y > start.Rect.Y && tl.Position.Y < start.Rect.Y + start.Rect.Height)
                {
                    StartPress = true;
                }
            }
        }

        public void render(SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(start.tx, start.Rect, Color.White);
            sb.End();
        }

    }

}
