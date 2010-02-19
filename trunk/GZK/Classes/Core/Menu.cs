#region Using Statements

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace GZK {

    /// <summary>
    /// The menu class controls the in game menu for user navigation
    /// </summary>

    public class Menu {

        #region Variable Declarations

        public GameState gameState;
        public ContentManager content;
        private GameObject logo, background;
        private Panel about, difficulty;
        private SpriteFont font;

        #endregion

        #region Class Constructor

        /// <summary>
        /// Menu constructor, creates a new instance of the Menu class
        /// </summary>
        /// <param name="c">ContentManager for storing content from the menu</param>
        /// <param name="g">GameState with global variables</param>

        public Menu(ContentManager c, GameState g) {

            this.content = c;
            this.gameState = g;

            //Create core menu components
            this.background = new GameObject(800, 600, 0, 0, this.content.Load<Texture2D>("Textures\\Menu\\Background"));
            this.logo = new GameObject(655, 342, 20, 20, this.content.Load<Texture2D>("Textures\\Menu\\Logo"));

            //Create Panels
            this.about = new Panel(526, 321, 0, 100, "About", this.gameState.buttons[4], content);
            this.difficulty = new Panel(555, 259, 245, 50, "Difficulty", this.gameState.buttons[5], content);
            this.UpdateDifficultyButtons();

            //Font for drawing
            this.font = this.content.Load<SpriteFont>("Ninja Naruto");
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Update the menu
        /// </summary>

        public void Update() {

            //loop to enable the buttons required for the menu
            for(int i = 0; i < 9; i++) {
                this.gameState.buttons[i].enabled = true;
            }

            //Open about panel
            if(this.gameState.buttons[2].clicked == true) {
                this.about.action = 1;
                this.gameState.buttons[2].clickable = false;
                this.gameState.buttons[3].clickable = false;

            //Close about panel
            } else if(this.gameState.buttons[4].clicked == true) {
                this.about.action = 0;
                this.gameState.buttons[2].clickable = true;
                this.gameState.buttons[3].clickable = true;
            }

            //Open difficulty panel
            if(this.gameState.buttons[3].clicked == true) {
                this.difficulty.action = 1;
                this.gameState.buttons[2].clickable = false;
                this.gameState.buttons[3].clickable = false;
                this.gameState.buttons[6].clickable = true;
                this.gameState.buttons[7].clickable = true;
                this.gameState.buttons[8].clickable = true;

            //Close difficulty panel
            } else if(this.gameState.buttons[5].clicked == true || this.gameState.buttons[6].clicked == true || this.gameState.buttons[7].clicked == true || this.gameState.buttons[8].clicked == true) {
                this.difficulty.action = 0;
                this.gameState.buttons[2].clickable = true;
                this.gameState.buttons[3].clickable = true;
                this.gameState.buttons[6].clickable = false;
                this.gameState.buttons[7].clickable = false;
                this.gameState.buttons[8].clickable = false;

                //Update the difficulty level if a button was pressed
                if(this.gameState.buttons[6].clicked == true) {
                    this.gameState.difficulty = 1;
                } else if(this.gameState.buttons[7].clicked == true) {
                    this.gameState.difficulty = 2;
                } else if(this.gameState.buttons[8].clicked == true) {
                    this.gameState.difficulty = 3;
                }
            }

            //Handle the start button
            if(this.gameState.buttons[1].clicked == true) {
                this.gameState.ResetButtons();
                this.gameState.score = 0;
                this.gameState.level++;
            }

            //Update the about panel if it is opening/closing
            if(this.about.action > -1) {
                this.about.Update();
            }

            //Update the difficulty panel and associated buttons if it's opening/closing
            if(this.difficulty.action > -1) {
                this.difficulty.Update();
                this.UpdateDifficultyButtons();
            }
        }

        #endregion

        #region Draw Method

        /// <summary>
        /// method which draws the menu
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to use when drawing</param>

        public void Draw(SpriteBatch spriteBatch) {

            //Core menu components
            spriteBatch.Draw(this.background.texture, background.currentPosition, Color.White);
            spriteBatch.Draw(this.logo.texture, this.logo.currentPosition, Color.White);

            //Panels
            this.about.Draw(spriteBatch);
            this.difficulty.Draw(spriteBatch);

            //If we have scored, draw it using the spritefont
            if(this.gameState.score > 0) {
                spriteBatch.DrawString(this.font, "Score: " + this.gameState.score, new Vector2(15, 520), Color.White);
            }
        }

        #endregion

        #region UpdateDifficultyButtons Method

        /// <summary>
        /// Moves the difficulty buttons to a position relative to that of the difficulty panel
        /// </summary>

        private void UpdateDifficultyButtons() {
            this.gameState.buttons[6].UpdatePosition((int)(this.difficulty.currentPosition.X + 35), (int)(this.difficulty.currentPosition.Y + 100));
            this.gameState.buttons[7].UpdatePosition((int)(this.difficulty.currentPosition.X + 35), (int)(this.difficulty.currentPosition.Y + 185));
            this.gameState.buttons[8].UpdatePosition((int)(this.difficulty.currentPosition.X + 35), (int)(this.difficulty.currentPosition.Y + 270));
        }

        #endregion

    }
}
