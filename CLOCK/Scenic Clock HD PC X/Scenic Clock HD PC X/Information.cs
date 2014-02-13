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
    class Information
    {

        #region Screen
        public Vector2 screenSizePixels;
        public Vector2 startingPoint;
        public enum ScreenSize
        {
            i480,
            i576,
            p480,
            p720,
            i720,
            p1080
        }
        public ScreenSize currentScreenSize;
        #endregion

        #region Gamepad
        public GamePadState previousGamePadStatePlayerOne; // the previous gamepad state for player one's gamepad, from the time the game last ran the update function
        public GamePadState previousGamePadStatePlayerTwo; // the previous gamepad state for player two's gamepad, from the time the game last ran the update function
        public GamePadState previousGamePadStatePlayerThree; // the previous gamepad state for player three's gamepad, from the time the game last ran the update function
        public GamePadState previousGamePadStatePlayerFour; // the previous gamepad state for player four's gamepad, from the time the game last ran the update function

        public bool playerOneGamepadActive = false;
        public bool playerTwoGamepadActive = false;
        public bool playerThreeGamepadActive = false;
        public bool playerFourGamepadActive = false;

        public bool playerOneGamepadActiveThisCycle = false;
        public bool playerTwoGamepadActiveThisCycle = false;
        public bool playerThreeGamepadActiveThisCycle = false;
        public bool playerFourGamepadActiveThisCycle = false;



        public enum PlayerIDs
        {
            One,
            Two,
            Three,
            Four,
            All
        }
        #endregion

        #region Controls Configuration
        public Buttons manualChangeToNextWallpaperButton = Buttons.RightShoulder;
        public Keys manualChangeToNextWallpaperKey = Keys.D3;

        public Buttons manualChangeToPrevWallpaperButton = Buttons.LeftShoulder;
        public Keys manualChangeToPrevWallpaperKey = Keys.D2;

        public Buttons displayControlsButton = Buttons.Y;
        public Keys displayControlsKey = Keys.Y;

        public Buttons toggleSlideshowButton = Buttons.A;
        public Keys toggleSlideshowKey = Keys.A;
        #endregion

        #region App
        public ScenicClockApp app;
        #endregion

        #region Wallpaper
        public Wallpaper wallpaper;
        public List<Texture2D> wallpaperList;
        public Texture2D slideshowOnTex;
        public Texture2D slideshowOffTex;
        #endregion

        #region Clock
        public Clock clock;
        public DigitalClock digitalClock;
        public FlipClock flipClock;
        public AnalogClock analogClock;

        public List<Texture2D> flipClockMinutesTexturesList;
        public List<Texture2D> flipClockHoursTexturesList;

        public Texture2D analogClockShortHandTex;
        public Texture2D analogClockLongHandTex;
        public Texture2D analogClockSecondHandTex;
        public Texture2D analogClockFace;

        #endregion

        #region Options
        public Options options;
        #endregion

        #region ButtonDisplay
        public ButtonDisplay buttonDisplay;
        public Texture2D buttonDisplayBG;
        public Texture2D controlsBG;
        public Texture2D buttonDisplayHoldY;
        #endregion

        #region KeyboardControls
        public KeyboardState previousKeyboardState;
        public bool keyboardControlsBeingUsed = true;
        public bool keyboardActiveThisCycle = false;
        #endregion

        public Information(ScenicClockApp app) {
            this.app = app;
        }

        public bool IsControllerLive(GamePadState State)
        {
            return (State.IsConnected &&
                ((State.Buttons.A == ButtonState.Pressed) ||
                (State.Buttons.B == ButtonState.Pressed) ||
                (State.ThumbSticks.Left != Vector2.Zero) ||
                (State.ThumbSticks.Right != Vector2.Zero) ||
                (State.DPad.Up == ButtonState.Pressed) ||
                (State.DPad.Down == ButtonState.Pressed) ||
                (State.DPad.Left == ButtonState.Pressed) ||
                (State.DPad.Right == ButtonState.Pressed) ||
                (State.Buttons.X == ButtonState.Pressed) ||
                (State.Buttons.Y == ButtonState.Pressed) ||
                (State.Buttons.Start == ButtonState.Pressed) ||
                (State.Buttons.Back == ButtonState.Pressed) ||
                (State.Triggers.Left > 0f) ||
                (State.Triggers.Right > 0f) ||
                (State.Buttons.LeftShoulder == ButtonState.Pressed) ||
                (State.Buttons.RightShoulder == ButtonState.Pressed) ||
                (State.Buttons.LeftStick == ButtonState.Pressed) ||
                (State.Buttons.RightStick == ButtonState.Pressed)));
        }

        public bool AnyControllerActiveThisCycle()
        {
            return (playerOneGamepadActiveThisCycle ||
                    playerTwoGamepadActiveThisCycle ||
                    playerThreeGamepadActiveThisCycle ||
                    playerFourGamepadActiveThisCycle);
        }

        public void UpdateKeyboardActiveState()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.X) ||
              Keyboard.GetState().IsKeyDown(Keys.Y) ||
              Keyboard.GetState().IsKeyDown(Keys.A) ||
              Keyboard.GetState().IsKeyDown(Keys.B) ||
              Keyboard.GetState().IsKeyDown(Keys.D1) ||
              Keyboard.GetState().IsKeyDown(Keys.D2) ||
              Keyboard.GetState().IsKeyDown(Keys.D3) ||
              Keyboard.GetState().IsKeyDown(Keys.D4))
            {
                keyboardActiveThisCycle = true;
            }
            else
            {
                keyboardActiveThisCycle = false;
            }
        }

        public void UpdateControllerActiveState()
        {
            if (!playerOneGamepadActive)
            {
                playerOneGamepadActive = IsControllerLive(GamePad.GetState(PlayerIndex.One));
            }

            if (!playerTwoGamepadActive)
            {
                playerTwoGamepadActive = IsControllerLive(GamePad.GetState(PlayerIndex.Two));
            }

            if (!playerThreeGamepadActive)
            {
                playerThreeGamepadActive = IsControllerLive(GamePad.GetState(PlayerIndex.Three));
            }

            if (!playerFourGamepadActive)
            {
                playerFourGamepadActive = IsControllerLive(GamePad.GetState(PlayerIndex.Four));
            }

            playerOneGamepadActiveThisCycle = IsControllerLive(GamePad.GetState(PlayerIndex.One));
            playerTwoGamepadActiveThisCycle = IsControllerLive(GamePad.GetState(PlayerIndex.Two));
            playerThreeGamepadActiveThisCycle = IsControllerLive(GamePad.GetState(PlayerIndex.Three));
            playerFourGamepadActiveThisCycle = IsControllerLive(GamePad.GetState(PlayerIndex.Four));
        }

        public bool CheckIfButtonBeingPressed(Buttons button, PlayerIDs players)
        {
            bool buttonActivated = false;
            GamePadState pOneGamepadState = GamePad.GetState(PlayerIndex.One);
            GamePadState pTwoGamepadState = GamePad.GetState(PlayerIndex.Two);
            GamePadState pThreeGamepadState = GamePad.GetState(PlayerIndex.Three);
            GamePadState pFourGamepadState = GamePad.GetState(PlayerIndex.Four);

            switch (players)
            {
                #region All
                case PlayerIDs.All:
                    switch (button)
                    {
                        case Buttons.A:
                            if (pOneGamepadState.Buttons.A == ButtonState.Pressed ||
                                pTwoGamepadState.Buttons.A == ButtonState.Pressed ||
                                pThreeGamepadState.Buttons.A == ButtonState.Pressed ||
                                pFourGamepadState.Buttons.A == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }

                            break;
                        case Buttons.B:
                            if (pOneGamepadState.Buttons.B == ButtonState.Pressed ||
                                pTwoGamepadState.Buttons.B == ButtonState.Pressed ||
                                pThreeGamepadState.Buttons.B == ButtonState.Pressed ||
                                pFourGamepadState.Buttons.B == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.X:
                            if (pOneGamepadState.Buttons.X == ButtonState.Pressed ||
                                pTwoGamepadState.Buttons.X == ButtonState.Pressed ||
                                pThreeGamepadState.Buttons.X == ButtonState.Pressed ||
                                pFourGamepadState.Buttons.X == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.Y:
                            if (pOneGamepadState.Buttons.Y == ButtonState.Pressed ||
                                pTwoGamepadState.Buttons.Y == ButtonState.Pressed ||
                                pThreeGamepadState.Buttons.Y == ButtonState.Pressed ||
                                pFourGamepadState.Buttons.Y == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.LeftShoulder:
                            if (pOneGamepadState.Buttons.LeftShoulder == ButtonState.Pressed ||
                                pTwoGamepadState.Buttons.LeftShoulder == ButtonState.Pressed ||
                                pThreeGamepadState.Buttons.LeftShoulder == ButtonState.Pressed ||
                                pFourGamepadState.Buttons.LeftShoulder == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.RightShoulder:
                            if (pOneGamepadState.Buttons.RightShoulder == ButtonState.Pressed ||
                                pTwoGamepadState.Buttons.RightShoulder == ButtonState.Pressed ||
                                pThreeGamepadState.Buttons.RightShoulder == ButtonState.Pressed ||
                                pFourGamepadState.Buttons.RightShoulder == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.DPadDown:
                            if (pOneGamepadState.DPad.Down == ButtonState.Pressed ||
                                pTwoGamepadState.DPad.Down == ButtonState.Pressed ||
                                pThreeGamepadState.DPad.Down == ButtonState.Pressed ||
                                pFourGamepadState.DPad.Down == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.DPadLeft:
                            if (pOneGamepadState.DPad.Left == ButtonState.Pressed ||
                                pTwoGamepadState.DPad.Left == ButtonState.Pressed ||
                                pThreeGamepadState.DPad.Left == ButtonState.Pressed ||
                                pFourGamepadState.DPad.Left == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.DPadRight:
                            if (pOneGamepadState.DPad.Right == ButtonState.Pressed ||
                                pTwoGamepadState.DPad.Right == ButtonState.Pressed ||
                                pThreeGamepadState.DPad.Right == ButtonState.Pressed ||
                                pFourGamepadState.DPad.Right == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.DPadUp:
                            if (pOneGamepadState.DPad.Up == ButtonState.Pressed ||
                                pTwoGamepadState.DPad.Up == ButtonState.Pressed ||
                                pThreeGamepadState.DPad.Up == ButtonState.Pressed ||
                                pFourGamepadState.DPad.Up == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;

                    }
                    break;
                #endregion

                #region Player One
                case PlayerIDs.One:
                    switch (button)
                    {
                        case Buttons.A:
                            if (pOneGamepadState.Buttons.A == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }

                            break;
                        case Buttons.B:
                            if (pOneGamepadState.Buttons.B == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.X:
                            if (pOneGamepadState.Buttons.X == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.Y:
                            if (pOneGamepadState.Buttons.Y == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.LeftShoulder:
                            if (pOneGamepadState.Buttons.LeftShoulder == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.RightShoulder:
                            if (pOneGamepadState.Buttons.RightShoulder == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.DPadDown:
                            if (pOneGamepadState.DPad.Down == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.DPadLeft:
                            if (pOneGamepadState.DPad.Left == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.DPadRight:
                            if (pOneGamepadState.DPad.Right == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.DPadUp:
                            if (pOneGamepadState.DPad.Up == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;

                    }
                    break;
                #endregion

                #region Player Two
                case PlayerIDs.Two:
                    switch (button)
                    {
                        case Buttons.A:
                            if (pTwoGamepadState.Buttons.A == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }

                            break;
                        case Buttons.B:
                            if (pTwoGamepadState.Buttons.B == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.X:
                            if (pTwoGamepadState.Buttons.X == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.Y:
                            if (pTwoGamepadState.Buttons.Y == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.LeftShoulder:
                            if (pTwoGamepadState.Buttons.LeftShoulder == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.RightShoulder:
                            if (pTwoGamepadState.Buttons.RightShoulder == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.DPadDown:
                            if (pTwoGamepadState.DPad.Down == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.DPadLeft:
                            if (pTwoGamepadState.DPad.Left == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.DPadRight:
                            if (pTwoGamepadState.DPad.Right == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.DPadUp:
                            if (pTwoGamepadState.DPad.Up == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;

                    }
                    break;
                #endregion

                #region Player Three
                case PlayerIDs.Three:
                    switch (button)
                    {
                        case Buttons.A:
                            if (pThreeGamepadState.Buttons.A == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }

                            break;
                        case Buttons.B:
                            if (pThreeGamepadState.Buttons.B == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.X:
                            if (pThreeGamepadState.Buttons.X == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.Y:
                            if (pThreeGamepadState.Buttons.Y == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.LeftShoulder:
                            if (pThreeGamepadState.Buttons.LeftShoulder == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.RightShoulder:
                            if (pThreeGamepadState.Buttons.RightShoulder == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.DPadDown:
                            if (pThreeGamepadState.DPad.Down == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.DPadLeft:
                            if (pThreeGamepadState.DPad.Left == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.DPadRight:
                            if (pThreeGamepadState.DPad.Right == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.DPadUp:
                            if (pThreeGamepadState.DPad.Up == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;

                    }
                    break;
                #endregion

                #region Player Four
                case PlayerIDs.Four:
                    switch (button)
                    {
                        case Buttons.A:
                            if (pFourGamepadState.Buttons.A == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }

                            break;
                        case Buttons.B:
                            if (pFourGamepadState.Buttons.B == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.X:
                            if (pFourGamepadState.Buttons.X == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.Y:
                            if (pFourGamepadState.Buttons.Y == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.LeftShoulder:
                            if (pFourGamepadState.Buttons.LeftShoulder == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.RightShoulder:
                            if (pFourGamepadState.Buttons.RightShoulder == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.DPadDown:
                            if (pFourGamepadState.DPad.Down == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.DPadLeft:
                            if (pFourGamepadState.DPad.Left == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.DPadRight:
                            if (pFourGamepadState.DPad.Right == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.DPadUp:
                            if (pFourGamepadState.DPad.Up == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;

                    }
                    break;
                #endregion
            }

            return buttonActivated;
        }

        public void UpdateControllerPrevGamepadState()
        {
            if (playerOneGamepadActive)
            {
                previousGamePadStatePlayerOne = GamePad.GetState(PlayerIndex.One);
            }
            if (playerTwoGamepadActive)
            {
                previousGamePadStatePlayerTwo = GamePad.GetState(PlayerIndex.Two);
            }
            if (playerThreeGamepadActive)
            {
                previousGamePadStatePlayerThree = GamePad.GetState(PlayerIndex.Three);
            }
            if (playerFourGamepadActive)
            {
                previousGamePadStatePlayerFour = GamePad.GetState(PlayerIndex.Four);
            }
        }

        public bool CheckIfControlActivated(Buttons button, PlayerIDs players)
        {
            bool buttonActivated = false;
            GamePadState pOneGamepadState = GamePad.GetState(PlayerIndex.One);
            GamePadState pTwoGamepadState = GamePad.GetState(PlayerIndex.Two);
            GamePadState pThreeGamepadState = GamePad.GetState(PlayerIndex.Three);
            GamePadState pFourGamepadState = GamePad.GetState(PlayerIndex.Four);


            switch (players)
            {
                #region All
                case PlayerIDs.All:
                    switch (button)
                    {
                        case Buttons.A:
                            if (pOneGamepadState.Buttons.A == ButtonState.Released && previousGamePadStatePlayerOne.Buttons.A == ButtonState.Pressed ||
                                pTwoGamepadState.Buttons.A == ButtonState.Released && previousGamePadStatePlayerTwo.Buttons.A == ButtonState.Pressed ||
                                pThreeGamepadState.Buttons.A == ButtonState.Released && previousGamePadStatePlayerThree.Buttons.A == ButtonState.Pressed ||
                                pFourGamepadState.Buttons.A == ButtonState.Released && previousGamePadStatePlayerFour.Buttons.A == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            
                            break;
                        case Buttons.B:
                            if (pOneGamepadState.Buttons.B == ButtonState.Released && previousGamePadStatePlayerOne.Buttons.B == ButtonState.Pressed ||
                                pTwoGamepadState.Buttons.B == ButtonState.Released && previousGamePadStatePlayerTwo.Buttons.B == ButtonState.Pressed ||
                                pThreeGamepadState.Buttons.B == ButtonState.Released && previousGamePadStatePlayerThree.Buttons.B == ButtonState.Pressed ||
                                pFourGamepadState.Buttons.B == ButtonState.Released && previousGamePadStatePlayerFour.Buttons.B == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.X:
                            if (pOneGamepadState.Buttons.X == ButtonState.Released && previousGamePadStatePlayerOne.Buttons.X == ButtonState.Pressed ||
                                pTwoGamepadState.Buttons.X == ButtonState.Released && previousGamePadStatePlayerTwo.Buttons.X == ButtonState.Pressed ||
                                pThreeGamepadState.Buttons.X == ButtonState.Released && previousGamePadStatePlayerThree.Buttons.X == ButtonState.Pressed ||
                                pFourGamepadState.Buttons.X == ButtonState.Released && previousGamePadStatePlayerFour.Buttons.X == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.Y:
                            if (pOneGamepadState.Buttons.Y == ButtonState.Released && previousGamePadStatePlayerOne.Buttons.Y == ButtonState.Pressed ||
                                pTwoGamepadState.Buttons.Y == ButtonState.Released && previousGamePadStatePlayerTwo.Buttons.Y == ButtonState.Pressed ||
                                pThreeGamepadState.Buttons.Y == ButtonState.Released && previousGamePadStatePlayerThree.Buttons.Y == ButtonState.Pressed ||
                                pFourGamepadState.Buttons.Y == ButtonState.Released && previousGamePadStatePlayerFour.Buttons.Y == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.LeftShoulder:
                            if (pOneGamepadState.Buttons.LeftShoulder == ButtonState.Released && previousGamePadStatePlayerOne.Buttons.LeftShoulder == ButtonState.Pressed ||
                                pTwoGamepadState.Buttons.LeftShoulder == ButtonState.Released && previousGamePadStatePlayerTwo.Buttons.LeftShoulder == ButtonState.Pressed ||
                                pThreeGamepadState.Buttons.LeftShoulder == ButtonState.Released && previousGamePadStatePlayerThree.Buttons.LeftShoulder == ButtonState.Pressed ||
                                pFourGamepadState.Buttons.LeftShoulder == ButtonState.Released && previousGamePadStatePlayerFour.Buttons.LeftShoulder == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.RightShoulder:
                            if (pOneGamepadState.Buttons.RightShoulder == ButtonState.Released && previousGamePadStatePlayerOne.Buttons.RightShoulder == ButtonState.Pressed ||
                                pTwoGamepadState.Buttons.RightShoulder == ButtonState.Released && previousGamePadStatePlayerTwo.Buttons.RightShoulder == ButtonState.Pressed ||
                                pThreeGamepadState.Buttons.RightShoulder == ButtonState.Released && previousGamePadStatePlayerThree.Buttons.RightShoulder == ButtonState.Pressed ||
                                pFourGamepadState.Buttons.RightShoulder == ButtonState.Released && previousGamePadStatePlayerFour.Buttons.RightShoulder == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.DPadDown:
                            if (pOneGamepadState.DPad.Down == ButtonState.Released && previousGamePadStatePlayerOne.DPad.Down == ButtonState.Pressed ||
                                pTwoGamepadState.DPad.Down == ButtonState.Released && previousGamePadStatePlayerTwo.DPad.Down == ButtonState.Pressed ||
                                pThreeGamepadState.DPad.Down == ButtonState.Released && previousGamePadStatePlayerThree.DPad.Down == ButtonState.Pressed ||
                                pFourGamepadState.DPad.Down == ButtonState.Released && previousGamePadStatePlayerFour.DPad.Down == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.DPadLeft:
                            if (pOneGamepadState.DPad.Left == ButtonState.Released && previousGamePadStatePlayerOne.DPad.Left == ButtonState.Pressed ||
                                pTwoGamepadState.DPad.Left == ButtonState.Released && previousGamePadStatePlayerTwo.DPad.Left == ButtonState.Pressed ||
                                pThreeGamepadState.DPad.Left == ButtonState.Released && previousGamePadStatePlayerThree.DPad.Left == ButtonState.Pressed ||
                                pFourGamepadState.DPad.Left == ButtonState.Released && previousGamePadStatePlayerFour.DPad.Left == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.DPadRight:
                            if (pOneGamepadState.DPad.Right == ButtonState.Released && previousGamePadStatePlayerOne.DPad.Right == ButtonState.Pressed ||
                                pTwoGamepadState.DPad.Right == ButtonState.Released && previousGamePadStatePlayerTwo.DPad.Right == ButtonState.Pressed ||
                                pThreeGamepadState.DPad.Right == ButtonState.Released && previousGamePadStatePlayerThree.DPad.Right == ButtonState.Pressed ||
                                pFourGamepadState.DPad.Right == ButtonState.Released && previousGamePadStatePlayerFour.DPad.Right == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.DPadUp:
                            if (pOneGamepadState.DPad.Up == ButtonState.Released && previousGamePadStatePlayerOne.DPad.Up == ButtonState.Pressed ||
                                pTwoGamepadState.DPad.Up == ButtonState.Released && previousGamePadStatePlayerTwo.DPad.Up == ButtonState.Pressed ||
                                pThreeGamepadState.DPad.Up == ButtonState.Released && previousGamePadStatePlayerThree.DPad.Up == ButtonState.Pressed ||
                                pFourGamepadState.DPad.Up == ButtonState.Released && previousGamePadStatePlayerFour.DPad.Up == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;

                    }
                    break;
                #endregion

                #region Player One
                case PlayerIDs.One:
                    switch (button)
                    {
                        case Buttons.A:
                            if (pOneGamepadState.Buttons.A == ButtonState.Released && previousGamePadStatePlayerOne.Buttons.A == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            
                            break;
                        case Buttons.B:
                            if (pOneGamepadState.Buttons.B == ButtonState.Released && previousGamePadStatePlayerOne.Buttons.B == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.X:
                            if (pOneGamepadState.Buttons.X == ButtonState.Released && previousGamePadStatePlayerOne.Buttons.X == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.Y:
                            if (pOneGamepadState.Buttons.Y == ButtonState.Released && previousGamePadStatePlayerOne.Buttons.Y == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.LeftShoulder:
                            if (pOneGamepadState.Buttons.LeftShoulder == ButtonState.Released && previousGamePadStatePlayerOne.Buttons.LeftShoulder == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.RightShoulder:
                            if (pOneGamepadState.Buttons.RightShoulder == ButtonState.Released && previousGamePadStatePlayerOne.Buttons.RightShoulder == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.DPadDown:
                            if (pOneGamepadState.DPad.Down == ButtonState.Released && previousGamePadStatePlayerOne.DPad.Down == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.DPadLeft:
                            if (pOneGamepadState.DPad.Left == ButtonState.Released && previousGamePadStatePlayerOne.DPad.Left == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.DPadRight:
                            if (pOneGamepadState.DPad.Right == ButtonState.Released && previousGamePadStatePlayerOne.DPad.Right == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.DPadUp:
                            if (pOneGamepadState.DPad.Up == ButtonState.Released && previousGamePadStatePlayerOne.DPad.Up == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;

                    }
                    break;
                #endregion

                #region Player Two
                case PlayerIDs.Two:
                    switch (button)
                    {
                        case Buttons.A:
                            if (pTwoGamepadState.Buttons.A == ButtonState.Released && previousGamePadStatePlayerTwo.Buttons.A == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            
                            break;
                        case Buttons.B:
                            if (pTwoGamepadState.Buttons.B == ButtonState.Released && previousGamePadStatePlayerTwo.Buttons.B == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.X:
                            if (pTwoGamepadState.Buttons.X == ButtonState.Released && previousGamePadStatePlayerTwo.Buttons.X == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.Y:
                            if (pTwoGamepadState.Buttons.Y == ButtonState.Released && previousGamePadStatePlayerTwo.Buttons.Y == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.LeftShoulder:
                            if (pTwoGamepadState.Buttons.LeftShoulder == ButtonState.Released && previousGamePadStatePlayerTwo.Buttons.LeftShoulder == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.RightShoulder:
                            if (pTwoGamepadState.Buttons.RightShoulder == ButtonState.Released && previousGamePadStatePlayerTwo.Buttons.RightShoulder == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.DPadDown:
                            if (pTwoGamepadState.DPad.Down == ButtonState.Released && previousGamePadStatePlayerTwo.DPad.Down == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.DPadLeft:
                            if (pTwoGamepadState.DPad.Left == ButtonState.Released && previousGamePadStatePlayerTwo.DPad.Left == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.DPadRight:
                            if (pTwoGamepadState.DPad.Right == ButtonState.Released && previousGamePadStatePlayerTwo.DPad.Right == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.DPadUp:
                            if (pTwoGamepadState.DPad.Up == ButtonState.Released && previousGamePadStatePlayerTwo.DPad.Up == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;

                    }
                    break;
                #endregion

                #region Player Three
                case PlayerIDs.Three:
                    switch (button)
                    {
                        case Buttons.A:
                            if (pThreeGamepadState.Buttons.A == ButtonState.Released && previousGamePadStatePlayerThree.Buttons.A == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }

                            break;
                        case Buttons.B:
                            if (pThreeGamepadState.Buttons.B == ButtonState.Released && previousGamePadStatePlayerThree.Buttons.B == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.X:
                            if (pThreeGamepadState.Buttons.X == ButtonState.Released && previousGamePadStatePlayerThree.Buttons.X == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.Y:
                            if (pThreeGamepadState.Buttons.Y == ButtonState.Released && previousGamePadStatePlayerThree.Buttons.Y == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.LeftShoulder:
                            if (pThreeGamepadState.Buttons.LeftShoulder == ButtonState.Released && previousGamePadStatePlayerThree.Buttons.LeftShoulder == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.RightShoulder:
                            if (pThreeGamepadState.Buttons.RightShoulder == ButtonState.Released && previousGamePadStatePlayerThree.Buttons.RightShoulder == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.DPadDown:
                            if (pThreeGamepadState.DPad.Down == ButtonState.Released && previousGamePadStatePlayerThree.DPad.Down == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.DPadLeft:
                            if (pThreeGamepadState.DPad.Left == ButtonState.Released && previousGamePadStatePlayerThree.DPad.Left == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.DPadRight:
                            if (pThreeGamepadState.DPad.Right == ButtonState.Released && previousGamePadStatePlayerThree.DPad.Right == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.DPadUp:
                            if (pThreeGamepadState.DPad.Up == ButtonState.Released && previousGamePadStatePlayerThree.DPad.Up == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;

                    }
                    break;
                #endregion

                #region Player Four
                case PlayerIDs.Four:
                    switch (button)
                    {
                        case Buttons.A:
                            if (pFourGamepadState.Buttons.A == ButtonState.Released && previousGamePadStatePlayerFour.Buttons.A == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }

                            break;
                        case Buttons.B:
                            if (pFourGamepadState.Buttons.B == ButtonState.Released && previousGamePadStatePlayerFour.Buttons.B == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.X:
                            if (pFourGamepadState.Buttons.X == ButtonState.Released && previousGamePadStatePlayerFour.Buttons.X == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.Y:
                            if (pFourGamepadState.Buttons.Y == ButtonState.Released && previousGamePadStatePlayerFour.Buttons.Y == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.LeftShoulder:
                            if (pFourGamepadState.Buttons.LeftShoulder == ButtonState.Released && previousGamePadStatePlayerFour.Buttons.LeftShoulder == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.RightShoulder:
                            if (pFourGamepadState.Buttons.RightShoulder == ButtonState.Released && previousGamePadStatePlayerFour.Buttons.RightShoulder == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.DPadDown:
                            if (pFourGamepadState.DPad.Down == ButtonState.Released && previousGamePadStatePlayerFour.DPad.Down == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.DPadLeft:
                            if (pFourGamepadState.DPad.Left == ButtonState.Released && previousGamePadStatePlayerFour.DPad.Left == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.DPadRight:
                            if (pFourGamepadState.DPad.Right == ButtonState.Released && previousGamePadStatePlayerFour.DPad.Right == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;
                        case Buttons.DPadUp:
                            if (pFourGamepadState.DPad.Up == ButtonState.Released && previousGamePadStatePlayerFour.DPad.Up == ButtonState.Pressed)
                            {
                                buttonActivated = true;
                            }
                            break;

                    }
                    break;
                #endregion
            }

            return buttonActivated;
        }

        public bool CheckIfControlActivated(Keys key)
        {
            return (previousKeyboardState.IsKeyDown(key) && Keyboard.GetState().IsKeyUp(key));
            
        }

        public void UpdatePrevKeyboardState()
        {
            previousKeyboardState = Keyboard.GetState();
        }

        public bool CheckIfKeyBeingPressed(Keys key)
        {
            return (Keyboard.GetState().IsKeyDown(key));
            
        }

    }
}
