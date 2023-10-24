using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Monogame_Summative___1_5_
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Texture2D _oldDoomGuy, _newDoomGuy, _oldCacoDemon, _newCacoDemon, _oldPinkyDemon, _newPinkyDemon, _oldLevel, _newLevel, _startScreenNew, _startScreenOld, _endScreen;
        Rectangle doomGuyRect, demonRect, screenRect;
        Screen screen;
        Random generator = new Random();
        SoundEffect scream, doomIntroNew, doomIntroOld, cheer;
        SoundEffectInstance screamInstance, oldIntroInstance, newIntroInstance, cheerInstance;
        SpriteFont boo;
        private int randDoomGuy, randDemon;
        private bool screamPlay = true, introPlay = true, booTrue = false;
        float time, introTime, visibility, spookTime, prevTime;
        MouseState mouseState, prevMouseState;

        enum Screen
        {
            Intro,
            Main,
            End
        }
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }
        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1000;
            _graphics.PreferredBackBufferHeight = 700;
            _graphics.ApplyChanges();
            screen = Screen.Intro;
            randDoomGuy = generator.Next(1, 3);
            randDemon = generator.Next(1, 3);
            doomGuyRect = new Rectangle(-450, 150, 300, 400);
            demonRect = new Rectangle(690, 200, 300, 300);
            screenRect = new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            time = 0;
            prevTime = 0;
            introTime = 0;
            visibility = 1;
            spookTime = 5;
            base.Initialize();
        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _newDoomGuy = Content.Load<Texture2D>("DoomGuyNew");
            _oldDoomGuy = Content.Load<Texture2D>("DoomGuyOld");
            _newCacoDemon = Content.Load<Texture2D>("DemonCacoNew");
            _oldCacoDemon = Content.Load<Texture2D>("DemonCacoOld");
            _newPinkyDemon = Content.Load<Texture2D>("DemonPinkyNew");
            _oldPinkyDemon = Content.Load<Texture2D>("DemonPinkyOld");
            _oldLevel = Content.Load<Texture2D>("DoomLevelOld");
            _newLevel = Content.Load<Texture2D>("DoomLevelNew");
            _startScreenNew = Content.Load<Texture2D>("DoomIntro");
            _startScreenOld = Content.Load<Texture2D>("OldDoomIntro");
            _endScreen = Content.Load<Texture2D>("TheEnd");
            scream = Content.Load<SoundEffect>("ScreamReverbEffect");
            doomIntroNew = Content.Load<SoundEffect>("57. Doom Eternal");
            doomIntroOld = Content.Load<SoundEffect>("03 - At Dooms Gate [Live]");
            cheer = Content.Load<SoundEffect>("short-crowd-cheer-6713");
            boo = Content.Load<SpriteFont>("Boo");
            screamInstance = scream.CreateInstance();
            oldIntroInstance = doomIntroOld.CreateInstance();
            newIntroInstance = doomIntroNew.CreateInstance();
            cheerInstance = cheer.CreateInstance();
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();           
            mouseState = Mouse.GetState();
            if (screen == Screen.Intro)
            {
                introTime = (float)gameTime.TotalGameTime.TotalSeconds - prevTime;
                if (randDoomGuy == 1)
                {
                    if (oldIntroInstance.State != SoundState.Playing)
                        oldIntroInstance.Play();
                }
                else
                {
                    if (newIntroInstance.State != SoundState.Playing)
                        newIntroInstance.Play();
                }
                if (mouseState.LeftButton == ButtonState.Pressed & prevMouseState.LeftButton != ButtonState.Pressed & mouseState.Position.X <= _graphics.PreferredBackBufferWidth & mouseState.Position.X >= 0 & mouseState.Position.Y >= 0 & mouseState.Position.Y <= _graphics.PreferredBackBufferHeight)
                    screen = Screen.Main;
            }
            else if (screen == Screen.Main)
            {
                oldIntroInstance.Stop();
                newIntroInstance.Stop();
                time = (float)gameTime.TotalGameTime.TotalSeconds - introTime - prevTime;
                if (doomGuyRect.X < 10)
                    doomGuyRect.X += 2;
                else
                {
                    doomGuyRect.X = 10;
                    booTrue = true;
                }
                if (time >= spookTime)
                {
                    if (screamPlay)
                    {
                        scream.Play();
                        screamPlay = false;
                    }
                    booTrue = false;
                    visibility = 1 - ((time - spookTime) / 10 * 4);
                }
                if (time >= spookTime + 6)
                    screen = Screen.End;
            }
            else if (screen == Screen.End)
            {
                if (cheerInstance.State != SoundState.Playing)
                    cheerInstance.Play();
                if (mouseState.LeftButton == ButtonState.Pressed & mouseState.Position.X <= _graphics.PreferredBackBufferWidth & mouseState.Position.X >= 0 & mouseState.Position.Y >= 0 & mouseState.Position.Y <= _graphics.PreferredBackBufferHeight)
                {
                    cheerInstance.Stop();
                    screen = Screen.Intro;
                    prevTime = (float)gameTime.TotalGameTime.TotalSeconds - time - introTime;
                    doomGuyRect.X = -450;
                    visibility = 1;
                    screamPlay = true;
                    randDoomGuy = generator.Next(1, 3);
                    randDemon = generator.Next(1, 3);
                }
            }
            prevMouseState = Mouse.GetState();
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            if (screen == Screen.Main)
            {
                if (randDoomGuy == 1)
                {
                    _spriteBatch.Draw(_oldLevel, screenRect, Color.White);
                    _spriteBatch.Draw(_oldDoomGuy, doomGuyRect, Color.White);
                    switch (randDemon)
                    {
                        case 1:
                            _spriteBatch.Draw(_oldCacoDemon, demonRect, Color.White * visibility);
                            break;
                        case 2:
                            _spriteBatch.Draw(_oldPinkyDemon, demonRect, Color.White * visibility);
                            break;
                    }

                }
                else if (randDoomGuy == 2)
                {
                    _spriteBatch.Draw(_newLevel, screenRect, Color.White);
                    _spriteBatch.Draw(_newDoomGuy, doomGuyRect, Color.White);
                    switch (randDemon)
                    {
                        case 1:
                            _spriteBatch.Draw(_newCacoDemon, demonRect, Color.White * visibility);
                            break;
                        case 2:
                            _spriteBatch.Draw(_newPinkyDemon, demonRect, Color.White * visibility);
                            break;
                    }

                }
            }
            else if (screen == Screen.Intro)
            {
                if (randDoomGuy == 1)
                    _spriteBatch.Draw(_startScreenOld, screenRect, Color.White);
                else
                    _spriteBatch.Draw(_startScreenNew, screenRect, Color.White);
            }
            else if (screen == Screen.End)
                _spriteBatch.Draw(_endScreen, screenRect, Color.White);
            if (screen == Screen.Main)
            {
                if (booTrue)
                    _spriteBatch.DrawString(boo, "Boo", new Vector2(doomGuyRect.Right, doomGuyRect.Top), Color.White);
            }
            else if (screen == Screen.End)
                _spriteBatch.DrawString(boo, "Click To Reset", new Vector2(335, 420), Color.White);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}