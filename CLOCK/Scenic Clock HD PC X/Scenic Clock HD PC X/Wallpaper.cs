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
    class Wallpaper
    {
        // Library
        Information information;

        float wallscale;
        
        int currentWallpaperDurationInMS = 0; // how long the current wallpaper has been shown

        bool changingWallpaper = false; // if the wallpaper is currently going through a change animation
        int durationOfAnimationinMS = 1000; // the goal duration of the change animation (fade) in milliseconds
        int currentAnimationDuration = 0; // the actual duration of the change animation (fade) in milliseconds

        bool manualNextWallpaperChangeButtonPushed = false; // if the user has pressed the change-to-next-wallpaper button during the current transition animation
        bool manualPrevWallpaperChangeButtonPushed = false; // if the user has pressed the change-to-prev-wallpaper button during the current transition animation

        public Texture2D oldWallpaperTex;
        Color oldWallColor;

        public Texture2D currentWallpaperTex;
        Color currentWallColor = Color.White;

        public Texture2D newWallpaperTex;
        Color newWallColor;


        #region Wallpaper Display Mode
        WallDisplayMode defaultWallDisplayMode = WallDisplayMode.Stretch;
        WallDisplayMode wallDisplayMode;
        enum WallDisplayMode
        {
            Stretch,
            Fill
        }
        #endregion

        bool initialized = false;

        int timeSinceUserLastGaveInputMS = 50000;
        int minTimeBetweenUserInputAndSlideshowUpdate = 3000;

        int timeSinceSlideshowSettingChanged = 0;
        int durationOfSlideshowSettingChangedAlert = 2000;
        bool slideshowSettingChangedAlertDisplaying = false;
        double slideshowAlertXProportion = 0.7;
        double slideshowAlertYProportion = 0.2;
        double slideshowAlertProportion = 0.1; // height scaled relative to screen height, width scaled to maintain proportions

        // Methods


        public Wallpaper(Information newInformation)
        {
            information = newInformation;
            wallDisplayMode = defaultWallDisplayMode;
        }

        public void Update(GameTime gameTime)
        {
            if (information.AnyControllerActiveThisCycle() || (information.keyboardControlsBeingUsed && information.keyboardActiveThisCycle))
            {
                timeSinceUserLastGaveInputMS = 0;
            }
            else
            {
                timeSinceUserLastGaveInputMS += gameTime.ElapsedGameTime.Milliseconds;
            }

            if (!initialized)
            { // if the wallpaper obj has not run through initialization yet, do it
                Initialize();
            }
            SlideshowChangeAlertUpdate(gameTime);
            UpdateSlideshowStatus(gameTime);
            
            TransitionUpdate(gameTime); // update the transition effect, if its currently taking place at all
            
            if (timeSinceUserLastGaveInputMS > minTimeBetweenUserInputAndSlideshowUpdate)
            {
            SlideshowUpdate(gameTime); // update the slideshow effect, if its currently taking place at all
            }


            ManualWallpaperChangeUpdate(gameTime); // deal with a manual wallpaper change (if any)

        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Vector2 sp = information.startingPoint; // the starting point
            Vector2 pixels = information.screenSizePixels;
            Rectangle dest = new Rectangle((int)sp.X, (int)sp.Y, (int)pixels.X, (int)pixels.Y); //destination draw rectangle
            if (!changingWallpaper)
            {
                Rectangle curSrc = new Rectangle(0, 0, currentWallpaperTex.Width, currentWallpaperTex.Height); // sets the current source (wallpaper) rectangle
                spriteBatch.Draw(currentWallpaperTex, dest, curSrc, currentWallColor, 0, new Vector2(), SpriteEffects.None, 0);
            }
            else
            {
                Rectangle oldSrc = new Rectangle(0, 0, oldWallpaperTex.Width, newWallpaperTex.Height); //sets the old source (wallpaper) rectangle
                Rectangle newSrc = new Rectangle(0, 0, newWallpaperTex.Width, newWallpaperTex.Height); //sets the new source (wallpaper) rectangle
                spriteBatch.Draw(oldWallpaperTex, dest, oldSrc, oldWallColor, 0, new Vector2(), SpriteEffects.None, 0); //draw the old wallpaper 
                spriteBatch.Draw(newWallpaperTex, dest, newSrc, newWallColor, 0, new Vector2(), SpriteEffects.None, 0); //draw the new wallpaper
            }
            if (slideshowSettingChangedAlertDisplaying)
            {
                if (information.options.currentWallpaperTransitionMode == Options.WallpaperTransitionMode.AutoSlideshow)
                {
                    Rectangle slideshowOnSrcRect = new Rectangle(0, 0, information.slideshowOnTex.Width, information.slideshowOnTex.Height);
                    Rectangle slideshowOffDrawRect = new Rectangle( ((int) (information.screenSizePixels.X * slideshowAlertXProportion)),
                                                      ((int) (information.screenSizePixels.Y * slideshowAlertYProportion)),
                                                      (int)((information.screenSizePixels.Y * slideshowAlertProportion * information.slideshowOnTex.Width) / information.slideshowOnTex.Height) ,
                                                      ((int)(information.screenSizePixels.Y * slideshowAlertProportion)));
                    spriteBatch.Draw(information.slideshowOnTex, slideshowOffDrawRect, slideshowOnSrcRect, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                }
                else if (information.options.currentWallpaperTransitionMode == Options.WallpaperTransitionMode.Static)
                {
                    Rectangle slideshowOffSrcRect = new Rectangle(0, 0, information.slideshowOffTex.Width, information.slideshowOffTex.Height);
                    Rectangle slideshowOffDrawRect = new Rectangle(((int)(information.screenSizePixels.X * slideshowAlertXProportion)),
                                                      ((int)(information.screenSizePixels.Y * slideshowAlertYProportion)),
                                                      (int)((information.screenSizePixels.Y * slideshowAlertProportion * information.slideshowOffTex.Width) / information.slideshowOffTex.Height),
                                                      ((int)(information.screenSizePixels.Y * slideshowAlertProportion)));
                    spriteBatch.Draw(information.slideshowOffTex, slideshowOffDrawRect, slideshowOffSrcRect, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                }
            }
        }

        public void ChangeWallpaper(bool quickChange, Texture2D newWallTex)
        {
            if (quickChange && !changingWallpaper)
            {
                currentWallpaperTex = newWallTex;
            }
            else 
            {
                newWallpaperTex = newWallTex;
                oldWallpaperTex = currentWallpaperTex;
                changingWallpaper = true;
            }
        }

        /*
        public Rectangle CreateWallRectangle(Texture2D newWall)
        {
            Rectangle newWallRectangle = new Rectangle();
            switch (wallDisplayMode)
            {
                case WallDisplayMode.Fill:
                    if (newWall.Width > newWall.Height)
                    {
                        // if width is greater than height, then fit image to screen height
                        float heightFitRatio = (information.screenSizePixels.Y / newWall.Height);
                        int newWallWidthPx = (int)(newWall.Width * heightFitRatio);
                        int newWallHeightPx = (int)(newWall.Height * heightFitRatio);
                        newWallRectangle = new Rectangle((int)information.startingPoint.X,
                                                         (int)information.startingPoint.Y,
                                                         newWallWidthPx,
                                                         newWallHeightPx);
                        wallscale = heightFitRatio;

                    }
                    else
                    {
                        // otherwise, fit image to screen width
                        float widthFitRatio = (information.screenSizePixels.X / newWall.Width);
                        int newWallWidthPx = (int)(newWall.Width * widthFitRatio);
                        int newWallHeightPx = (int)(newWall.Height * widthFitRatio);
                        newWallRectangle = new Rectangle((int)information.startingPoint.X,
                                                         (int)information.startingPoint.Y,
                                                         newWallWidthPx,
                                                         newWallHeightPx);
                        wallscale = widthFitRatio;
                    }
                    break;

                case WallDisplayMode.Stretch:
                    newWallRectangle = new Rectangle((int)information.startingPoint.X,
                                                     (int)information.startingPoint.Y,
                                                     (int)information.screenSizePixels.X,
                                                     (int)information.screenSizePixels.Y);
                    break;
            }
            return newWallRectangle;
        }
        
        */

        public void TransitionUpdate(GameTime gameTime)
        { // updates the transition effect between wallpapers

            if (changingWallpaper)
            { // if the wallpaper is currently going through a transition, update it
                currentAnimationDuration += gameTime.ElapsedGameTime.Milliseconds; // update the current duration of the animation
                if (currentAnimationDuration < durationOfAnimationinMS)
                { // if the current animation duration has not exceeded the goal animation duration, update the animation
                    int newWallAlpha = (int)((currentAnimationDuration * 255) / durationOfAnimationinMS); // this creates the new transparency for the new wallpaper (fades from nothing to full opacity)
                    newWallColor = new Color(255, 255, 255, newWallAlpha);

                    int oldWallAlpha = 255 - newWallAlpha; // this creates the new transparency for the old wallpaper (fades to nothing)
                    oldWallColor = new Color(255, 255, 255, oldWallAlpha);
                }
                else
                { // else, since the animation has exceeded the goal duration, then reset the animation values and turn off the animation
                    currentAnimationDuration = 0;
                    changingWallpaper = false;
                    currentWallpaperTex = newWallpaperTex;
                    oldWallpaperTex = null;
                    newWallColor = new Color(0, 0, 0, 0);
                    oldWallColor = Color.White;
                    manualNextWallpaperChangeButtonPushed = false;

                }

            }
        }

        public Texture2D GetNextWallpaper(Texture2D wall)
        { // returns the next wallpaper in line to be displayed

            // find the index location of the given wallpaper in the central wallpaper list
            int wallLocation = information.wallpaperList.IndexOf(wall);
            // determine the index location of the next wallpaper
            int nextWallLocation = wallLocation + 1;
            
            if (nextWallLocation >= information.wallpaperList.Count)
            { // check if the index location determined for the next wallpaper is beyond the list's length
                // if it is, then the end of the list has been reached, reset to the first in the list
                nextWallLocation = 0;
            }

            // return the next wallpaper in the list
            return information.wallpaperList[nextWallLocation];
            
        } 

        public Texture2D GetPrevWallpaper(Texture2D wall)
        { // returns the next wallpaper in line to be displayed

            // find the index location of the given wallpaper in the central wallpaper list
            int wallLocation = information.wallpaperList.IndexOf(wall);

            // determine the index location of the previous wallpaper
            int prevWallLocation = wallLocation - 1;

            if (prevWallLocation < 0)
            { // check if the index location determined for the previous wallpaper is less than the first possible index location
                // if it is, then the beginning of the list has been reached, reset to the last in the list
                prevWallLocation = (information.wallpaperList.Count - 1);
            }

            // return the next wallpaper in the list
            return information.wallpaperList[prevWallLocation];

        }

        public void SlideshowChangeAlertUpdate(GameTime gameTime)
        {
            if (slideshowSettingChangedAlertDisplaying)
            {
                timeSinceSlideshowSettingChanged += gameTime.ElapsedGameTime.Milliseconds;

                if (timeSinceSlideshowSettingChanged > durationOfSlideshowSettingChangedAlert)
                {
                    slideshowSettingChangedAlertDisplaying = false;
                    
                }
            }
        }


        public void SlideshowUpdate(GameTime gameTime)
        { // updates the slideshow, if there is one occuring
            if (information.options.currentWallpaperTransitionMode == Options.WallpaperTransitionMode.AutoSlideshow)
            {
                currentWallpaperDurationInMS += gameTime.ElapsedGameTime.Milliseconds;

                if (currentWallpaperDurationInMS >= information.options.WallpaperDurationInMS && !changingWallpaper)
                {
                    currentWallpaperDurationInMS = 0;
                    ChangeWallpaper(false, GetNextWallpaper(currentWallpaperTex));

                }
            }

            
        }

        public void UpdateSlideshowStatus(GameTime gameTime)
        {
            if ( ((information.CheckIfControlActivated(information.toggleSlideshowButton, Information.PlayerIDs.All) ||
               (information.keyboardControlsBeingUsed && information.CheckIfKeyBeingPressed(information.toggleSlideshowKey)))) &&
                (slideshowSettingChangedAlertDisplaying == false))
            {
                if (information.options.currentWallpaperTransitionMode == Options.WallpaperTransitionMode.AutoSlideshow)
                    information.options.currentWallpaperTransitionMode = Options.WallpaperTransitionMode.Static;
                else
                    information.options.currentWallpaperTransitionMode = Options.WallpaperTransitionMode.AutoSlideshow;

                slideshowSettingChangedAlertDisplaying = true;
                timeSinceSlideshowSettingChanged = 0;
            }
        }

        public void ManualWallpaperChangeUpdate(GameTime gameTime)
        { // deals with if the user tries to manually change the wallpaper
            if (changingWallpaper && (information.CheckIfControlActivated(information.manualChangeToNextWallpaperButton, Information.PlayerIDs.All) ||
                (information.keyboardControlsBeingUsed && information.CheckIfControlActivated(information.manualChangeToNextWallpaperKey))))
            { // updates information - specifically that the user pressed the change-to-next-wallpaper-button while the wallpaper was changing
                manualNextWallpaperChangeButtonPushed = true;
            }
            

            if (!changingWallpaper && ((information.CheckIfControlActivated(information.manualChangeToNextWallpaperButton, Information.PlayerIDs.All)) || 
                               (information.keyboardControlsBeingUsed && information.CheckIfControlActivated(information.manualChangeToNextWallpaperKey))))
            { // if the wallpaper is not currently changing, then change to the next wallpaper with a fade effect
                ChangeWallpaper(false, GetNextWallpaper(currentWallpaperTex));
            }
            else if (changingWallpaper && manualNextWallpaperChangeButtonPushed && (information.CheckIfControlActivated(information.manualChangeToNextWallpaperButton, Information.PlayerIDs.All) ||
                  (information.keyboardControlsBeingUsed && information.CheckIfControlActivated(information.manualChangeToNextWallpaperKey))))
            { // skips the fading animation effect - makes it so the user can double-tap the manual-wallpaper-change button and it will instantly go to the next wallpaper
                currentAnimationDuration = 0;
                changingWallpaper = false;
                currentWallpaperTex = newWallpaperTex;
                oldWallpaperTex = null;
                newWallColor = new Color(0, 0, 0, 0);
                oldWallColor = Color.White;
                manualNextWallpaperChangeButtonPushed = false;
            }



            if (changingWallpaper && information.CheckIfControlActivated(information.manualChangeToPrevWallpaperButton, Information.PlayerIDs.All) ||
              (information.keyboardControlsBeingUsed && information.CheckIfControlActivated(information.manualChangeToPrevWallpaperKey)))
            {// updates information - specifically that the user pressed the change-to-prev-wallpaper button while the wallpaper was changing
                manualPrevWallpaperChangeButtonPushed = true;
            }

            if (!changingWallpaper && (information.CheckIfControlActivated(information.manualChangeToPrevWallpaperButton, Information.PlayerIDs.All) ||
              (information.keyboardControlsBeingUsed && information.CheckIfControlActivated(information.manualChangeToPrevWallpaperKey))))
            { // if the wallpaper is not currently changing, then change to the previous wallpaper with a fade effect
                ChangeWallpaper(false, GetPrevWallpaper(currentWallpaperTex));
            }
            else if (changingWallpaper && manualPrevWallpaperChangeButtonPushed && (information.CheckIfControlActivated(information.manualChangeToPrevWallpaperButton, Information.PlayerIDs.All) ||
                  (information.keyboardControlsBeingUsed && information.CheckIfControlActivated(information.manualChangeToPrevWallpaperKey))))
            {// skips the fading animation effect - makes it so the user can double-tap the manual-wallpaper-change button and it will instantly go to the previous wallpaper
                currentAnimationDuration = 0;
                changingWallpaper = false;
                currentWallpaperTex = newWallpaperTex;
                oldWallpaperTex = null;
                newWallColor = new Color(0, 0, 0, 0);
                oldWallColor = Color.White;
                manualPrevWallpaperChangeButtonPushed = false;
            }
        }

        public void Initialize()
        {
            // change the wallpaper to the default one (which is the first one in the wallpaper list)
            ChangeWallpaper(true, information.wallpaperList[0]);
            // initialization has run, so update to show as such
            initialized = true;
        }




    }
}
