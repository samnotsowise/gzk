#region Using Statements

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

#endregion

namespace GZK {

    /// <summary>
    /// This class creates a custom mouse cursor and controls mouse position and clicking.
    /// Extends GameObject.
    /// </summary>
 
    public class CustomMouse: GameObject {

        #region Variable Declarations

        public bool mouseDown, mouseUp;
        private MouseState mouseState;
        private Texture2D mouseTexture, mouseClickTexture;
        private SoundEffect soundClick1, soundClick2;
        private ContentManager content;
        private Vector2 cursorMiddle;

        #endregion

        #region Class Constructor

        /// <summary>
        /// CustomMouse contructor - initialises a new instance of CustomMouse.
        /// </summary>
        /// <param name="c">Content manager for storing textures and sounds.</param>
        
        public CustomMouse(ContentManager c) {

            //Initialise the variables
            this.content = c;

            //Load required textures
            mouseTexture = content.Load<Texture2D>("Textures\\Global\\Mouse");
            mouseClickTexture = content.Load<Texture2D>("Textures\\Global\\MouseClick");
            soundClick1 = content.Load<SoundEffect>("Sounds\\Click1");
            soundClick2 = content.Load<SoundEffect>("Sounds\\Click2");

            //Get the current state of the mouse
            mouseState = Mouse.GetState();

            //Create the custom pointer
            this.width = 10;
            this.height = 10;
            this.texture = mouseTexture;
            this.rectangle.Height = 1;
            this.rectangle.Width = 1;

            //Calculate where the middle of the cursor should be
            cursorMiddle.X = this.width / 2;
            cursorMiddle.Y = this.height / 2;
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Updates the CustomMouse.
        /// </summary>
        
        public void Update() {

            //Find where the mouse is pointing
            this.mouseState = Mouse.GetState();

            //Update the mouse properties accordingly
            this.UpdatePosition((int)(this.mouseState.X - this.cursorMiddle.X), (int)(this.mouseState.Y - this.cursorMiddle.Y));
            this.rectangle.Height = 1;
            this.rectangle.Width = 1;
            this.rectangle.X = (int)(this.currentPosition.X + this.cursorMiddle.X);
            this.rectangle.Y = (int)(this.currentPosition.Y + this.cursorMiddle.Y);
            
            //Reset mouseUp
            this.mouseUp = false;

            //Check if the mouse button is down, set mouseDown
            if(this.mouseState.LeftButton == ButtonState.Pressed) {
                this.mouseDown = true;
                this.texture = this.mouseClickTexture;
            }

            //If the mouse has just been released, set mouseUp, update the texture and unset mouseDown
            if(this.mouseState.LeftButton == ButtonState.Released && this.mouseDown == true) {
                this.mouseUp = true;
                this.texture = this.mouseTexture;
                this.mouseDown = false;
                
                //Play one of the 2 possible mouse click sounds to give a bit of variety
                Random random = new Random();
                if(random.Next(1, 3) == 1) {
                    this.soundClick1.Play();
                } else {
                    this.soundClick2.Play();
                }
            }
        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Draws the CustomMouse.
        /// </summary>
        /// <param name="spriteBatch">Specifies SpriteBatch to be used when drawing.</param>
     
        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(this.texture, this.currentPosition, Color.White);
        }

        #endregion

    }
}