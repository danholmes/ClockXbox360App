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
    class FlipClock : Clock
    {
        Rectangle rightSideFlipClockTextureRectangle; // right side is minutes part
        Rectangle leftSideFlipClockTextureRectangle; // left side is hours part
        double flipClockScalingRatio = 0.15; // the percentage, in decimel, that the flip clock takes up. FlipClock is scaled against width of screen.

        Texture2D currentRightSideFlipClockTexture;
        Texture2D currentLeftSideFlipClockTexture;
        

        public FlipClock(Information newInfo)
        {
            information = newInfo;
        }

        public override void Update(GameTime gameTime)
        {
            updateDigits();
            UpdateFlipClockTimeTextures(gameTime);
            UpdateTextureRectangles();
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime t)
        {
            if (currentLeftSideFlipClockTexture != null && currentRightSideFlipClockTexture != null)
            {
                spriteBatch.Draw(currentLeftSideFlipClockTexture, leftSideFlipClockTextureRectangle, Color.White);
                spriteBatch.Draw(currentRightSideFlipClockTexture, rightSideFlipClockTextureRectangle, Color.White);
            }
            
        }

        public void UpdateTextureRectangles()
        {
            Texture2D flipTex = information.flipClockHoursTexturesList.ElementAt(0);
            int newLeftSideHeight = (int)(information.screenSizePixels.X * flipClockScalingRatio);
            int newLeftSideWidth = (int)((newLeftSideHeight * flipTex.Width) / flipTex.Height);           
            int newLeftSideX = (int)((information.screenSizePixels.X/2)) - newLeftSideWidth;

            int newSideY = (int)((information.screenSizePixels.Y - newLeftSideHeight) / 2);

            leftSideFlipClockTextureRectangle = new Rectangle(newLeftSideX, newSideY, newLeftSideWidth, newLeftSideHeight);

            int newRightSideX = (int)((information.screenSizePixels.X/2));
            int newRightSideHeight = newLeftSideHeight;
            int newRightSideWidth = (int)((newRightSideHeight * flipTex.Width) / flipTex.Height);

            rightSideFlipClockTextureRectangle = new Rectangle(newRightSideX, newSideY, newRightSideWidth, newRightSideHeight);

        }

        public void UpdateFlipClockTimeTextures(GameTime gameTime)
        {
            int currentHours = digits.hour;
            int currentMinutes = digits.min;

            int currentHoursTextureLocationInList = currentHours - 1;
            int currentMinsTextureLocationInList = currentMinutes;

            if (currentHours > 0 && currentHours < 13)
            {
                currentLeftSideFlipClockTexture = information.flipClockHoursTexturesList[currentHoursTextureLocationInList];
            }

            if (currentMinutes >= 0 && currentMinutes <= 59)
            {
                currentRightSideFlipClockTexture = information.flipClockMinutesTexturesList[currentMinsTextureLocationInList];
            }

        }
    }
}
