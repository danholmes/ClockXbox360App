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
    class ButtonDisplay
    {
        Information information; // central information repository

        
        Rectangle bgTextureRectangle; // sizing and positioning rectangle for the bgTexture
        double bgTexHeightRatio = 0.1; // the percentage (in decimel) of the screen that the bgTexture's height takes up
        double bgTexWidthRatio = 1; // the percentage (in decimel) of the screen that the bgTexture's width takes up
        Color bgTexColor = new Color(255, 255, 255, 150); // color to draw the texture with, makes it semi-transparent

        Rectangle holdYRectangle; // sizing and positioning rectangle for the holdYTexture

        Rectangle controlsTextureRectangle; // sizing and positioning rectangle for the controlsTextureRectangle
        double controlsTexWidthRatio = 0.8; // the percentage (in decimel) of the screen that the bgTexture's width takes up. The height scales to be appropriate to the width.
        Color controlsTexColor = new Color(255, 255, 255, 200); // color to draw the texture with, makes it sem-transparent


        int timeSinceLastActive = 0;
        int maxTimeSinceLastActive = 3000;

        bool buttonDisplayActive = false; 

        bool initialDisplayCompleted = false;
        int currentInitialDisplayTime = 0;
        int maxInitialDisplayTime = 5000;

        bool displayingControlsScheme = false; 

        public ButtonDisplay(Information newInfo)
        {
            information = newInfo;
        }

        public void Update(GameTime gameTime)
        {
            UpdateTextureInfo(gameTime);
           
            if (information.CheckIfButtonBeingPressed(information.displayControlsButton, Information.PlayerIDs.All) ||
              (information.keyboardControlsBeingUsed && information.CheckIfKeyBeingPressed(information.displayControlsKey)))
            {
                displayingControlsScheme = true;
            }
            else
            {
                displayingControlsScheme = false;
            }
            
            if (initialDisplayCompleted)
            {
                if (information.AnyControllerActiveThisCycle() ||
                (information.keyboardControlsBeingUsed && information.keyboardActiveThisCycle))
                {
                    timeSinceLastActive = 0;
                    buttonDisplayActive = true;
                }
                else
                {
                    timeSinceLastActive += gameTime.ElapsedGameTime.Milliseconds;
                }

                if (buttonDisplayActive)
                {
                    if (buttonDisplayActive && (!information.AnyControllerActiveThisCycle() || (information.keyboardActiveThisCycle && !information.keyboardActiveThisCycle)) && timeSinceLastActive > maxTimeSinceLastActive)
                    {
                        buttonDisplayActive = false;
                        timeSinceLastActive = 0;
                    }
                }
            }
            else
            {
                buttonDisplayActive = true;
                currentInitialDisplayTime += gameTime.ElapsedGameTime.Milliseconds;

                if (currentInitialDisplayTime > maxInitialDisplayTime)
                {
                    buttonDisplayActive = false;
                    initialDisplayCompleted = true;
                }
                
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            

            if (displayingControlsScheme)
            {
                spriteBatch.Draw(information.controlsBG, controlsTextureRectangle, controlsTexColor);
            }

            if (buttonDisplayActive)
            {
                int x = information.buttonDisplayBG.Width;
                int y = information.buttonDisplayBG.Height;
                spriteBatch.Draw(information.buttonDisplayBG, bgTextureRectangle, new Rectangle(0, 0, x, y), Color.White, 0, new Vector2(), SpriteEffects.None, 0);
                
                x = information.buttonDisplayHoldY.Width;
                y = information.buttonDisplayHoldY.Height;
                spriteBatch.Draw(information.buttonDisplayHoldY, holdYRectangle, new Rectangle(0, 0, x, y), Color.White, 0, new Vector2(), SpriteEffects.None, 1);
            }
            
        }

        /// <summary>
        /// Updates the size and position information for the textures used by the ButtonDisplay object.
        /// </summary>
        /// <param name="gameTime"></param>
        public void UpdateTextureInfo(GameTime gameTime)
        {
            int newBgTexWidth = (int)(bgTexWidthRatio * information.screenSizePixels.X);
            int newBgTexHeight = (int)(bgTexHeightRatio * information.screenSizePixels.Y);
            // set the bars' location to be the bottom of the screen, like the windows taskbar
            int newBgTexX = (int)( (information.screenSizePixels.X/2) - (newBgTexWidth/2));
            int newBgTexY = (int)(information.screenSizePixels.Y - newBgTexHeight);
            bgTextureRectangle = new Rectangle(newBgTexX, newBgTexY, newBgTexWidth, newBgTexHeight);
            
            
            int newHoldYWidth = (int)((newBgTexHeight * information.buttonDisplayHoldY.Width)/information.buttonDisplayBG.Height);
            int newHoldYX = (int)((information.screenSizePixels.X - newHoldYWidth)/2);
            int newHoldYY = (int)((information.screenSizePixels.Y - newBgTexHeight));
            holdYRectangle = new Rectangle(newHoldYX, newHoldYY, newHoldYWidth, newBgTexHeight);
            
            int newControlsTexWidth = (int)(controlsTexWidthRatio * information.screenSizePixels.X);
            int newControlsTexHeight = (int)((newControlsTexWidth * information.controlsBG.Height)/ information.controlsBG.Width);
            int newControlsTexX = (int)((information.screenSizePixels.X - newControlsTexWidth) / 2);
            int newControlsTexY = (int)((information.screenSizePixels.Y - newControlsTexHeight) / 2);

            controlsTextureRectangle = new Rectangle(newControlsTexX, newControlsTexY, newControlsTexWidth, newControlsTexHeight);

        }
    }
}
