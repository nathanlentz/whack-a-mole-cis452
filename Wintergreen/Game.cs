using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Wintergreen.Graphics;
using Wintergreen.Interfaces;
using Wintergreen.Input;
using Wintergreen.Settings;
using Wintergreen.Sounds;

namespace Wintergreen
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public partial class Game : Microsoft.Xna.Framework.Game
    {
        private ContentManager _engineContent;
        private SpriteFont _engineFont;

        public IState PreviousState { get; set; }

        public IState CurrentState { get; set; }

        public Game()
        {
            GraphicsManager.Initialize(this);
            string engineContentPath = System.IO.Path.Combine(System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).FullName, "Wintergreen", "Content");
            _engineContent = new ContentManager(Services, engineContentPath);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            GraphicsManager.InitializeSpriteBatch();
            InputManager.Initialize();

            ApplySettings();
            base.Initialize();
        }

        /// <summary>
        /// Applies settings from the <see cref="EngineSettings"/> class.
        /// </summary>
        protected virtual void ApplySettings()
        {
            Window.AllowUserResizing = EngineSettings.AllowUserResizing;
            IsMouseVisible = EngineSettings.IsMouseVisible;
            GraphicsManager.GraphicsDeviceManager.PreferredBackBufferHeight = EngineSettings.DisplayHeight;
            GraphicsManager.GraphicsDeviceManager.PreferredBackBufferWidth = EngineSettings.DisplayWidth;
            SoundEffect.MasterVolume = EngineSettings.MasterVolume;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary> 
        protected override void LoadContent()
        {
            SoundManager.LoadSounds(this);
            _engineFont = Content.Load<SpriteFont>("fonts/gameplay_mini");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            GraphicsManager.SpriteBatch.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        { 
            InputManager.Update();

            IState state = CurrentState.UpdateState(gameTime);

            // TODO: add a better method for exiting the game that can do any necessary shutdown procedures
            if (state == null)
                Exit();

            // Update the state if the state changed.
            if (state != CurrentState)
            {
                PreviousState = CurrentState;
                CurrentState = state;
            }

            if (InputManager.IsNewKeyPress(Keys.F12))
            {
                EngineSettings.ShowFPS = !EngineSettings.ShowFPS;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Draw all the things
            GraphicsManager.SpriteBatch.Begin();
            CurrentState.DrawUI(GraphicsManager.SpriteBatch);
            if (EngineSettings.ShowFPS)
            {
                GraphicsManager.SpriteBatch.DrawString(_engineFont, "FPS: " + Math.Round((1 / (decimal)gameTime.ElapsedGameTime.TotalSeconds), 2).ToString(),
                    new Vector2(5, EngineSettings.DisplayHeight - 15), Color.White);
            }
            GraphicsManager.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
