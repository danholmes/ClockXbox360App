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
    class Options
    {
        Information information;

        #region Wallpaper Options
        public int WallpaperDurationInMS = 3000;

        public enum WallpaperTransitionMode
        {
            AutoSlideshow,
            Static
        }
        public WallpaperTransitionMode currentWallpaperTransitionMode = WallpaperTransitionMode.AutoSlideshow; // TODO: move this to where it should be
        #endregion
        public bool TWENTYFOUR_HOUR; //note, usually there should be an accessor and setter method for constants. Im just sticking with your current design, but keep that in mind for the future
        public bool SHOW_SECONDS;
        public bool BLINKING_COLON;
        public bool SHOW_DATE;

        #region Constructor
        public Options(Information newInformation)
        {
            information = newInformation;
            TWENTYFOUR_HOUR = false;
            SHOW_SECONDS = false;
            BLINKING_COLON = false;
            SHOW_DATE = true;
        }
        #endregion

        #region Update
        public void Update(GameTime gameTime)
        {
        }
        #endregion

        #region Draw
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
        }
        #endregion''
    }
}
