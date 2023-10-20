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
        Texture2D _oldDoomGuy, _newDoomGuy, _oldCacoDemon, _newCacoDemon, _oldPinkyDemon, _newPinkyDemon, _oldLevel, _newLevel;
        Rectangle doomGuyRect, demonRect, levelRect;
        Screen screen = Screen.Intro;
        Random generator = new Random();
        SoundEffect scream;
        private int randDoomGuy, randDemon;
        private bool screamPlay = true;
        float seconds, visibility, spookTime;

        enum Screen
        {
            Intro,
            Main
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
            randDoomGuy = generator.Next(1, 3);
            randDemon = generator.Next(1, 3);
            doomGuyRect = new Rectangle(10, 150, 300, 400);
            demonRect = new Rectangle(690, 200, 300, 300);
            levelRect = new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            seconds = 0;
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
            scream = Content.Load<SoundEffect>("ScreamReverbEffect");
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            seconds = (float)gameTime.TotalGameTime.TotalSeconds;
            if (seconds >= spookTime)
            {
                if (screamPlay)
                {
                    scream.Play();
                    screamPlay = false;
                }
                visibility = 1 - ((seconds - spookTime) / 10 * 4);
            }
            if (seconds >= spookTime + 5)
                Exit();
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            if (randDoomGuy == 1)
            {
                _spriteBatch.Draw(_oldLevel, levelRect, Color.White);
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
                _spriteBatch.Draw(_newLevel, levelRect, Color.White);
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
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}