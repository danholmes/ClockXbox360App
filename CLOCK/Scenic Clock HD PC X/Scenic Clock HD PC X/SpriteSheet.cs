using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Scenic_Clock_HD_PC_X
{
    class Spritesheet
    {
        Texture2D tex;

        int lenx;
        int leny;

        Vector2 spriteSize;
        Rectangle src;
        Rectangle dest;

        ///
        ///Constructor
        ///
        ///assumes that each 'sprite' is evenly distributed 
        ///
        ///x number of 'sprites' in the x axis
        ///y number of 'sprites' in the y axis
        ///
        public Spritesheet(Texture2D tex, int x, int y)
        {
            this.tex = tex;
            spriteSize = new Vector2(tex.Width / x, tex.Height / y);
            src = new Rectangle(0, 0, tex.Width / x, tex.Height / y);
            dest = new Rectangle(0, 0, (int)spriteSize.X, (int)spriteSize.Y);
            this.lenx = x;
            this.leny = y;
        }

        /// <summary>
        /// sets the sprite. Ex SetSprite(1,2) will set the current sprite to 1 to the left, and 2 down
        /// </summary>
        /// <param name="x">sprite in x axis</param>
        /// <param name="y">sprite in y axis</param>
        public void SetSprite(int x, int y)
        {
            src.X = (int)(x * spriteSize.X);
            src.Y = (int)(y * spriteSize.Y);
        }

        /// <summary>
        /// set the current sprite draw position. This 'should' be the center of the sprite
        /// </summary>
        /// <param name="x">x position to draw</param>
        /// <param name="y">y position to draw</param>
        public void setPosition(int x, int y)
        {
            dest.X = x;
            dest.Y = y;
        }

        public void Draw(SpriteBatch sb, GameTime g, int x, int y, int sx, int sy)
        {
            setPosition(x, y);
            SetSprite(sx, sy);
            sb.Draw(tex, dest, src, Color.White, 0, new Vector2(src.Width / 2, src.Height / 2), SpriteEffects.None, 0);
        }

        public void Draw(SpriteBatch sb, GameTime g)
        {
            sb.Draw(tex, dest, src, Color.White, 0, new Vector2(src.Width / 2, src.Height / 2), SpriteEffects.None, 0);
        }
    }
}
