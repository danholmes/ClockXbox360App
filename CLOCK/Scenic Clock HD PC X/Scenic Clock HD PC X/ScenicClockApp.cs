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
using System.IO;

namespace Scenic_Clock_HD_PC_X
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ScenicClockApp : Microsoft.Xna.Framework.Game
    {
        #region Library
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        #region Information
        Information information;
        #endregion

        #endregion


        #region Constructor
        public ScenicClockApp()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferMultiSampling = true;
        }
        #endregion

        #region Initialize
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            base.Initialize();
        }
        #endregion

        #region LoadContent
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            

            information = new Information(this);
            #region Screen Size
            information.screenSizePixels.X = graphics.GraphicsDevice.Viewport.Width;
            information.screenSizePixels.Y = graphics.GraphicsDevice.Viewport.Height;

            information.startingPoint.X = graphics.GraphicsDevice.Viewport.X;
            information.startingPoint.Y = graphics.GraphicsDevice.Viewport.Y;
            #endregion
            #region Options
            information.options = new Options(information);
            #endregion
            #region Clock
            DigitalClock.LoadContent(Content);
            #region Flip Clock Hours
            information.flipClockHoursTexturesList = new List<Texture2D>();
            for (int i = 1; i < 12; i++) //TODO: change from 0-23 for 24 hour time
            {
                String fname = "MEDIA/Clock/FlipClock/hours";
                if (i < 10) fname += "0";
                fname += i;
                information.flipClockHoursTexturesList.Add(Content.Load<Texture2D>(fname));
            }

            #endregion
            #region Flip Clock Minutes 
            information.flipClockMinutesTexturesList = new List<Texture2D>();
            for (int i = 0; i < 60; i++) {  //load all of the minute textures
                String fname = "MEDIA/Clock/FlipClock/Min";
                if (i < 10) fname += "0";
                fname += i;
                information.flipClockMinutesTexturesList.Add( Content.Load<Texture2D>(fname));
            }
            #endregion

            information.digitalClock = new DigitalClock(information);
            information.flipClock = new FlipClock(information);
            //information.clock = information.flipClock;
            

            information.analogClockFace = Content.Load<Texture2D>("MEDIA/Clock/AnalogueClock/AnalogueClockFace");
            information.analogClockLongHandTex = Content.Load<Texture2D>("MEDIA/Clock/AnalogueClock/MinutesHand");
            information.analogClockShortHandTex = Content.Load<Texture2D>("MEDIA/Clock/AnalogueClock/HoursHand");
            information.analogClockSecondHandTex = Content.Load<Texture2D>("MEDIA/Clock/AnalogueClock/SecondsHand");
            information.analogClock = new AnalogClock(information);
            information.clock = information.digitalClock;

            #endregion
            #region Wallpapers
            information.wallpaper = new Wallpaper(information);

            information.wallpaperList = new List<Texture2D>();
            loadAllWallPapers(Content.RootDirectory + "/MEDIA/Wallpapers/", information.wallpaperList);
            #endregion
            #region ButtonDisplay
            information.buttonDisplayBG = Content.Load<Texture2D>("buttondisplaybg");
            information.buttonDisplay = new ButtonDisplay(information);
            information.controlsBG = Content.Load<Texture2D>("controlConfig");
            information.buttonDisplayHoldY = Content.Load<Texture2D>("holdytoshowcontrols");
            #endregion



        }

        void loadAllWallPapers(String dir, List<Texture2D> list) {
            String[] files = Directory.GetFiles(dir);
            foreach (String file in files){
                //sub string is a bit hacky. It's to remove the 'Content/' string before the path. The split is to remove the fileType
                list.Add(Content.Load<Texture2D>(file.Split('.')[0].Substring(8)));
            }

            foreach (String subdir in Directory.GetDirectories(dir)) {
                loadAllWallPapers(subdir, list);
            }
        }
        #endregion

        #region UnloadContent
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        #endregion

        #region HandleInput
        /// <summary>
        /// Handles key and button input
        /// </summary>
        protected void HandleInput() {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Space))
                this.Exit();
        }
        #endregion

        #region Update
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            information.UpdateControllerActiveState();

            HandleInput();

            information.clock.Update(gameTime);
            information.options.Update(gameTime);
            information.wallpaper.Update(gameTime);
            information.buttonDisplay.Update(gameTime);

            information.UpdateControllerPrevGamepadState();

            if (information.keyboardControlsBeingUsed)
            {
                information.UpdatePrevKeyboardState();
                information.UpdateKeyboardActiveState();
            }

            base.Update(gameTime);
        }
        #endregion

        #region Draw
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
            information.wallpaper.Draw(spriteBatch, gameTime);
            information.clock.Draw(spriteBatch, gameTime);
            information.options.Draw(spriteBatch, gameTime);
            information.buttonDisplay.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
        #endregion

        #region Main
        /// <summary>
        /// Main method
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            using (ScenicClockApp app = new ScenicClockApp())
            {
                app.Run();
            }
        }
        #endregion
    }
}
