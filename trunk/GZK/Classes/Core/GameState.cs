#region Using Statements

using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;

#endregion

namespace GZK {

    /// <summary>
    /// Class for storing information about the current game environment.
    /// </summary>

    public class GameState {

        #region Variable Declarations

        public bool paused, gotCake, useCake;
        public int level, difficulty, score, countButtons, infection, clock, levelTime;
        public CustomMouse mouse;
        public Button[] buttons;
        private ContentManager content;
        public Random random;
        public float obstruction;

        #endregion

        #region Class Constructor

        /// <summary>
        /// GameState contructor - initialises a new instance of GameState.
        /// </summary>
        /// <param name="c">Content manager for storing textures and sounds.</param>
        
        public GameState(ContentManager c) {

            content = c;

            //Initial level is the menu (0)
            this.level = 0;

            //Default difficulty
            this.difficulty = 2;

            this.paused = false;
            this.gotCake = false;
            this.useCake = false;

            //Approximate number of seconds to stay in a level
            this.levelTime = 40;

            //A Random object for generating random effects etc
            random = new Random();

            this.obstruction = 1;

            //Create mouse
            this.mouse = new CustomMouse(content);

            //Load the XML button data file
            XmlDocument xmlButtons = new XmlDocument();
            xmlButtons.Load("Content\\Data\\Buttons.xml");

            //Put each button and it's attributes into a list
            XmlNodeList buttonNodes = xmlButtons.DocumentElement.SelectNodes("Button");
            this.countButtons = buttonNodes.Count;

            //Create an array for the buttons
            this.buttons = new Button[countButtons];

            //Loop through all buttons in the list
            for(int i = 0; i < this.countButtons; i++) {
                XmlNode buttonNode = buttonNodes[i];

                //Create a button in the array
                this.buttons[i] = new Button(int.Parse(buttonNode.SelectSingleNode("pX").InnerText), int.Parse(buttonNode.SelectSingleNode("pY").InnerText), buttonNode.SelectSingleNode("name").InnerText, this.mouse, this.content);
            }

        }

        #endregion

        #region Reset Method

        /// <summary>
        /// Resets the necessary elements of the game state, allowing for a new game etc
        /// </summary>

        public void Reset() {
            this.level = 0;
            this.infection = 0;
            this.ResetButtons();
            this.difficulty = 2;
        }

        #endregion

        #region UpdateButtons Method

        /// <summary>
        /// Updates the interface buttons.
        /// </summary>

        public void UpdateButtons() {

            //Loop through the buttons array, and update all active buttons
            for(int i = 0; i < this.countButtons; i++) {
                if(this.buttons[i].enabled == true) {
                    this.buttons[i].Update();
                    if(this.difficulty < 4) {
                        this.buttons[this.difficulty + 5].texture = this.buttons[this.difficulty + 5].buttonDownTexture;
                    }
                } else {
                    this.buttons[i].clicked = false;
                }
            }
        }

        #endregion
        
        #region DrawButtons Method

        /// <summary>
        /// Draws the interface buttons.
        /// </summary>
        /// <param name="spriteBatch">Specifies SpriteBatch to be used when drawing.</param>

        public void DrawButtons(SpriteBatch spriteBatch) {

            //Loop through the buttons array, and draw all active buttons
            for(int i = 0; i < this.countButtons; i++) {
                if(this.buttons[i].enabled == true) {
                    this.buttons[i].Draw(spriteBatch);
                }
            }
        }

        #endregion

        #region ResetButtons Method

        /// <summary>
        /// Loop through all buttons in the list, and reset their key values
        /// (doesn't reset the mute button as we always want it avaialable)
        /// </summary>

        public void ResetButtons() {
            for(int i = 1; i < this.countButtons; i++) {
                this.buttons[i].enabled = false;
                this.buttons[i].clicked = false;
            }
        }

        #endregion

    }
}
