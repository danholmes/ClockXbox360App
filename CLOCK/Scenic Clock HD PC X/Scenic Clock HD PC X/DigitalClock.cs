using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Scenic_Clock_HD_PC_X
{
    class DigitalClock : Clock
    {
        static List<Texture2D> numbers;
        static List<Texture2D> months;
        static List<Texture2D> days;
        static List<Texture2D> ampm;
        static Texture2D background;
        static Texture2D colon;
        public DigitalClock(Information newInformation)
        {
            information = newInformation;
            digits = new Digits();
        }

        public static void LoadContent(ContentManager Content) {
            numbers = new List<Texture2D>();
            for (int i = 0; i < 10; i++) {
                numbers.Add(Content.Load<Texture2D>("MEDIA/Clock/DigitalClock/" + i));
            }
            months = new List<Texture2D>();
            for (int i = 1; i <= 12; i++) {
                months.Add(Content.Load<Texture2D>("MEDIA/Clock/DigitalClock/Month" + i));
            }
            days = new List<Texture2D>();
            for (int i = 0; i < 7; i++)
            {
                days.Add(Content.Load<Texture2D>("MEDIA/Clock/DigitalClock/Day" + i));
            }
            ampm = new List<Texture2D>();
            ampm.Add(Content.Load<Texture2D>("MEDIA/Clock/DigitalClock/AM"));
            ampm.Add(Content.Load<Texture2D>("MEDIA/Clock/DigitalClock/PM"));
            colon = Content.Load<Texture2D>("MEDIA/Clock/DigitalClock/Colon");
            background = Content.Load<Texture2D>("MEDIA/Clock/DigitalClock/BG");
        }

        public override void Update(GameTime g)
        {
            updateDigits();
        }

        public override void Draw(SpriteBatch sb, GameTime g)
        {
            Vector2 screen = information.screenSizePixels;
            int y = (int)(screen.Y / 2);
            int x = (int)(screen.X / 2);
            //calculate sprite size by scaling
            int bgh = (int) (screen.Y / 4);
            double scaleRatio = bgh / screen.Y;
            int bgw = (int) (scaleRatio * screen.X);
            int numw = bgw / (information.options.SHOW_SECONDS ? 10 : 8);
            scaleRatio = (double) numw / numbers.ElementAt(0).Width;
            int numh = (int) (scaleRatio * numbers.ElementAt(0).Height);
            double xoffset = information.options.SHOW_SECONDS ? -1.5  : 0.0;
            double yoffset = information.options.SHOW_DATE ? 0.5 : 0.0;

            sb.Draw(background, new Rectangle(x - bgw / 2, y - bgh / 2, bgw, bgh), Color.White);
            sb.Draw(numbers.ElementAt(digits.hour1), new Rectangle((int)(x + numw * (-2.5 + xoffset)), (int)(y - numh * (1 + yoffset) / 2), numw, numh), Color.White);
            sb.Draw(numbers.ElementAt(digits.hour2), new Rectangle((int)(x + numw * (-1.5 + xoffset)), (int)(y - numh * (1 + yoffset) / 2), numw, numh), Color.White);
            if (!information.options.BLINKING_COLON || digits.sec % 2 == 0)
            {
                sb.Draw(colon, new Rectangle((int)(x + numw * (-0.5 + xoffset)), y - numh / 2, numw, numh), Color.White);
            }
            sb.Draw(numbers.ElementAt(digits.min1), new Rectangle((int)(x + numw * (0.5 + xoffset)), (int)(y - numh * (1 + yoffset) / 2), numw, numh), Color.White);
            sb.Draw(numbers.ElementAt(digits.min2), new Rectangle((int)(x + numw * (1.5 + xoffset)), (int)(y - numh * (1 + yoffset) / 2), numw, numh), Color.White);
            if (information.options.SHOW_SECONDS)
            {
                sb.Draw(colon, new Rectangle((int)(x + numw * (2.5 + xoffset)), y - numh / 2, numw, numh), Color.White);
                sb.Draw(numbers.ElementAt(digits.sec1), new Rectangle((int)(x + numw * (3.5 + xoffset)), (int)(y - numh * (1 + yoffset) / 2), numw, numh), Color.White);
                sb.Draw(numbers.ElementAt(digits.sec2), new Rectangle((int)(x + numw * (4.5 + xoffset)), (int)(y - numh * (1 + yoffset) / 2), numw, numh), Color.White);
              
            }
            if (information.options.SHOW_DATE) {
                int monw = (int) (months.ElementAt(digits.mon - 1).Width * scaleRatio * 0.5); // 0.5 is hacky, because all the images have tons of white space -_-
                int monh = (int) (months.ElementAt(digits.mon - 1).Height * scaleRatio * 0.5);
                int dayw = (int)(numbers.ElementAt(digits.day1).Width * scaleRatio * 0.5);
                int dayh = (int)(numbers.ElementAt(digits.day1).Height * scaleRatio * 0.5);
                sb.Draw(months.ElementAt(digits.mon - 1), new Rectangle((int)(x + numw * (-3.5)), (int)(y - numh * (-1 + yoffset) / 2), monw, monh), Color.White);
                //sb.Draw(days.ElementAt(digits.day), new Rectangle((int)(x + numw * (3 + xoffset)), (int)(y - numh * (-1 + yoffset) / 2), dayw, dayh), Color.White);
                sb.Draw(numbers.ElementAt(digits.day1), new Rectangle((int)(x + numw * (2)), (int)(y - numh * (-2.5 + yoffset) / 3), dayw, dayh), Color.White);
                sb.Draw(numbers.ElementAt(digits.day2), new Rectangle((int)(x + numw * (2.5)), (int)(y - numh * (-2.5 + yoffset) / 3), dayw, dayh), Color.White);
            }
        }
    }
}
