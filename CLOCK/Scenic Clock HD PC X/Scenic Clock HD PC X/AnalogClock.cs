using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Scenic_Clock_HD_PC_X
{
    class AnalogClock : Clock
    {
        // texture rectangles
        Rectangle shortHandTexRect;
        Rectangle longHandTexRect;
        Rectangle secondHandTexRect;
        Rectangle faceTexRect;

        // texture rotations
        float shortHandRotation = 0f;
        float longHandRotation = 0f;
        float secondHandRotation = 0f;

        // textures origins
        Vector2 shortHandTexOrigin;
        Vector2 longHandTexOrigin;
        Vector2 secondHandTexOrigin;

        // proportion percentages (in decimal form)
        double faceTexProportion = 0.3; // scales against height of screen
        double shortHandTexProportion = 0.19; // scales against the height of the clock face
        double longHandTexProportion = 0.26; // scales against the height of the clock face
        double secondHandTexProportion = 0.3; // scales against the height of the clock face


        public AnalogClock(Information newInformation)
        {
            information = newInformation;
        }

        public override void Update(GameTime gameTime)
        {
            updateDigits();
            UpdateTextureInformation();
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //spriteBatch.Draw(currentWallpaperTex, dest, curSrc, currentWallColor, 0, new Vector2(), SpriteEffects.None, 0);
            Rectangle faceSrcRect = new Rectangle(0, 0, information.analogClockFace.Width, information.analogClockFace.Height);
            Rectangle shortHandSrcRect = new Rectangle(0, 0, information.analogClockShortHandTex.Width, information.analogClockShortHandTex.Height);
            Rectangle longHandSrcRect = new Rectangle(0, 0, information.analogClockLongHandTex.Width, information.analogClockLongHandTex.Height);
            Rectangle secondHandSrcRect = new Rectangle(0, 0, information.analogClockSecondHandTex.Width, information.analogClockSecondHandTex.Height);

            spriteBatch.Draw(information.analogClockFace, faceTexRect, faceSrcRect, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            spriteBatch.Draw(information.analogClockShortHandTex, shortHandTexRect, shortHandSrcRect, Color.White, shortHandRotation, shortHandTexOrigin, SpriteEffects.None, 1);
            spriteBatch.Draw(information.analogClockLongHandTex, longHandTexRect, longHandSrcRect, Color.White, longHandRotation, longHandTexOrigin, SpriteEffects.None, 1);
            spriteBatch.Draw(information.analogClockSecondHandTex, secondHandTexRect, secondHandSrcRect, Color.White, secondHandRotation, secondHandTexOrigin, SpriteEffects.None, 1);
        }

        /// <summary>
        /// Updates the size, position, origin, etc information for the textures making up the Analog Clock.
        /// </summary>
        public void UpdateTextureInformation()
        {
            // Updates the origins for the textures
            shortHandTexOrigin = new Vector2(((int)information.analogClockShortHandTex.Width / 2),
                                      ((int)information.analogClockShortHandTex.Height - (information.analogClockShortHandTex.Width / 2)));
            longHandTexOrigin = new Vector2(((int)information.analogClockLongHandTex.Width / 2),
                                      ((int)information.analogClockLongHandTex.Height - (information.analogClockLongHandTex.Width / 2)));
            secondHandTexOrigin = new Vector2(((int)information.analogClockSecondHandTex.Width / 2),
                                      ((int)information.analogClockSecondHandTex.Height - (information.analogClockSecondHandTex.Width / 2)));

            // Updates the rectangles for the textures
            int newFaceTexHeight = (int)(information.screenSizePixels.Y * faceTexProportion);
            int newFaceTexWidth = (int)((newFaceTexHeight * information.analogClockFace.Width) / information.analogClockFace.Height);
            int newFaceTexX = (int)((information.screenSizePixels.X / 2) - (newFaceTexWidth / 2));
            int newFaceTexY = (int)((information.screenSizePixels.Y / 2) - (newFaceTexHeight / 2));
            faceTexRect = new Rectangle(newFaceTexX,
                                 newFaceTexY,
                                 newFaceTexWidth,
                                 newFaceTexHeight);

            int newShortHandTexHeight = (int)(newFaceTexHeight * shortHandTexProportion);
            int newShortHandTexWidth = (int)((newShortHandTexHeight * information.analogClockShortHandTex.Width) / information.analogClockShortHandTex.Height);
            int newShortHandX = (int)(newFaceTexX + (newFaceTexWidth / 2));
            int newShortHandY = (int)(newFaceTexY + (newFaceTexHeight / 2) - (newShortHandTexWidth / 2));
            shortHandTexRect = new Rectangle(newShortHandX,
                                     newShortHandY,
                                     newShortHandTexWidth,
                                     newShortHandTexHeight);

            int newLongHandTexHeight = (int)(newFaceTexHeight * longHandTexProportion);
            int newLongHandTexWidth = (int)((newLongHandTexHeight * information.analogClockLongHandTex.Width) / information.analogClockLongHandTex.Height);
            int newLongHandX = newShortHandX;
            int newLongHandY = newShortHandY;
            longHandTexRect = new Rectangle(newLongHandX,
                                    newLongHandY,
                                    newLongHandTexWidth,
                                    newLongHandTexHeight);

            int newSecondHandTexHeight = (int)(newFaceTexHeight * secondHandTexProportion);
            int newSecondHandTexWidth = (int)((newSecondHandTexHeight * information.analogClockSecondHandTex.Width) / information.analogClockSecondHandTex.Height);
            int newSecondHandX = newShortHandX;
            int newSecondHandY = newShortHandY;
            secondHandTexRect = new Rectangle(newSecondHandX,
                                       newSecondHandY,
                                       newSecondHandTexWidth,
                                       newSecondHandTexHeight);

            // Updates the rotations for the textures, based on what time it currently is
            double pt1 = (double)(digits.min * 2) / (60 * 12);
            pt1 *= (double)(Math.PI);
            double shortHandRotationDouble = (((digits.hour * 2 * Math.PI) / 12));
            shortHandRotationDouble += pt1;
            shortHandRotation = (float)(shortHandRotationDouble);

            double longHandRotationDouble = (double)digits.min * 2 * Math.PI / 60;
            longHandRotation = (float)longHandRotationDouble;

            double secondHandRotationDouble = (double)digits.sec * 2 * Math.PI / 60;
            secondHandRotation = (float)secondHandRotationDouble;
        }
    }
}
