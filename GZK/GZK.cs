#region Using Statements

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
using Microsoft.Xna.Framework.Storage;

#endregion

namespace GZK {

    /// <summary>
    /// This is the main class for the game
    /// </summary>
    
    public class GZK: Microsoft.Xna.Framework.Game {

        #region Variable Declarations

        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public bool gamePaused;
        public GameState gameState;
        public Menu menu;
        public Level level;

        #endregion

        #region Class Constructor Method

        public GZK() {
            this.graphics = new GraphicsDeviceManager(this);
            this.graphics.PreferredBackBufferWidth = 800;
            this.graphics.PreferredBackBufferHeight = 600;
            this.graphics.IsFullScreen = true;
            this.Content.RootDirectory = "Content";
        }

        #endregion

        #region Initialize Method

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        
        protected override void Initialize() {

            //Create the required game objects
            base.Initialize();
            this.gameState = new GameState(this.Content);
            this.gameState.level = 0;
            this.menu = new Menu(this.Content, this.gameState);
            this.level = new Level(this.gameState, this.Services);
            this.Window.Title = "Granny Zombie Killers";
        }

        #endregion

        #region LoadContent Method

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        
        protected override void LoadContent() {

            //Create spriteBatch for drawing to the screen
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

            //Set up the MediaPlayer object for playing the game music 
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.4f;
            MediaPlayer.Play(Content.Load<Song>("Sounds\\Music"));
        }

        #endregion
        
        #region UnloadContent Method

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent() {
            this.Content.Unload();
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        
        protected override void Update(GameTime gameTime) {
            KeyboardState newState = Keyboard.GetState();

            // Allows the game to exit
            if(newState.IsKeyDown(Keys.Escape)) {
                this.Exit();
            }

            //If not in game, update the menu
            if(this.gameState.level == 0) {
                this.menu.Update();
            } else {
                this.level.Update();
            }

            //Buttons are everywhere so we always wanna check if there have been any changes with them.
            this.gameState.UpdateButtons();

            //Check the volume button for clicks (it's always visible)
            if(this.gameState.buttons[0].clicked == true) {
                if(MediaPlayer.Volume == 0f) {
                    MediaPlayer.Volume = 0.4f;
                } else {
                    MediaPlayer.Volume = 0f;
                }
            }

            //Update the mouse
            this.gameState.mouse.Update();

            base.Update(gameTime);
        }

        #endregion

        #region Draw Method

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        
        protected override void Draw(GameTime gameTime) {
            this.spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

            //If not in game, update the menu
            if(this.gameState.level == 0) {
                this.menu.Draw(this.spriteBatch);
            } else {
                this.level.Draw(this.GraphicsDevice);
            }

            //Draw the buttons
            this.gameState.DrawButtons(this.spriteBatch);

            //Draw the mouse
            this.gameState.mouse.Draw(spriteBatch);

            this.spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion

    }
}
