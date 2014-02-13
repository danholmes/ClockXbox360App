using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Scenic_Clock_HD_PC_X
{
    abstract class Clock
    {
        /// <summary>
        /// Digits class for holding the 'digits' of time
        /// using 10:35:24 in example
        /// </summary>
 
        protected struct Digits
        {
            public int hour1; //first digit of hour = 1
            public int hour2; //second digit of hour = 0
            public int hour; //full hour number. = 10
            public int min1; //first minute digit. = 3
            public int min2; //second minute digit = 5
            public int min; //full minute number = 35
            public int sec1; //first second digit = 2
            public int sec2; //second second digit = 4
            public int sec; //seconds full = 24... really, if you havent caught the pattern, look again
            public int mili1;
            public int mili2;
            public bool pm;
            public int mon;
            public int day1;
            public int day2;
            public int dow;
        }

        protected Information information;
        protected long time;
        protected Digits digits;

        #region Constructor
        public Clock() { }

        public Clock(Information newInformation)
        {
            information = newInformation;
            time = 0;
        }
        #endregion

        protected void updateDigits()
        {
            DateTime t = DateTime.Now;
            int hour;
            if (information.options.TWENTYFOUR_HOUR) //twenty four hour time
            {
                hour = t.Hour;
            }
            else //twelve hour time
            {
                hour = t.Hour % 12;
                hour = hour == 0 ? 12 : hour; // set to 12 o clock if time MOD 12 is zero
            }
            digits.hour1 = (int)(hour / 10);
            digits.hour2 = hour % 10; 
            digits.hour = hour;
            digits.pm = t.Hour > 12;
            digits.min1 = (int)(t.Minute / 10);
            digits.min2 = t.Minute % 10;
            digits.min = t.Minute; 
            digits.sec1 = (int)(t.Second / 10);
            digits.sec2 = t.Second % 10;
            digits.sec = t.Second;
            digits.mili1 = (int)(t.Millisecond / 10);
            digits.mili2 = t.Millisecond % 10;
            digits.mon = t.Month;
            digits.day1 = (int)(t.Day / 10);
            digits.day2 = (int)t.Day % 10;
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}
