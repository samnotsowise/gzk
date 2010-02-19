#region Using Statements

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace GZK {

    /// <summary>
    /// This class creates interface button objects
    /// Extends GameObject
    /// </summary>

    public class Button: GameObject {

        #region Variable Declarations

        public Texture2D buttonTexture, buttonOverTexture, buttonDownTexture;
        public bool clicked, enabled, clickable;
        public string name;
        private ContentManager content;
        private CustomMouse myMouse;

        #endregion

        #region Class Constructor

        /// <summary>
        /// Button contructor - Initialises a new Button object.
        /// </summary>
        /// <param name="pX">Button x co-ordinate.</param>
        /// <param name="pY">Button y co-ordinate.</param>
        /// <param name="n">Button name.</param>
        /// <param name="m">Mouse object.</param>
        /// <param name="c">Content manager for storing textures and sounds.</param>

        public Button(int pX, int pY, string n, CustomMouse m, ContentManager c) {
            this.clicked = false;
            this.enabled = false;
            this.clickable = true;
            this.width = 208;
            this.height = 65;
            this.UpdatePosition(pX, pY);
            this.content = c;
            this.myMouse = m;
            this.name = n;

            //Each button has 3 textures which need to be loaded
            this.buttonTexture = this.content.Load<Texture2D>("Textures\\Buttons\\" + this.name + "\\" + this.name);
            this.buttonDownTexture = this.content.Load<Texture2D>("Textures\\Buttons\\" + this.name + "\\" + this.name + "Down");
            this.buttonOverTexture = this.content.Load<Texture2D>("Textures\\Buttons\\" + this.name + "\\" + this.name + "Over");

            //If it's a small button the the size needs to be set differently
            if(this.name == "Volume" || this.name == "Close" || this.name == "Pause") {
                this.width = 40;
                this.height = 40;
                this.UpdateRectPosition();
            }
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Updates the for rollover/click effects
        /// </summary>

        public void Update() {

            //Reset clicked 
            this.clicked = false;

            //If the mouse is over the button, set mouseOver
            if(this.rectangle.Intersects(this.myMouse.rectangle) && this.clickable == true) {

                //Set texture according to whether or not the mouse button is pressed
                if(this.myMouse.mouseDown == true) {
                    this.texture = this.buttonDownTexture;
                } else {
                    this.texture = this.buttonOverTexture;
                }

                //If the mouse has just been released, then this button has been clicked
                if(this.myMouse.mouseUp == true) {
                    this.clicked = true;
                }
            } else {

                //Reset mouseOver & texture
                this.texture = this.buttonTexture;
            }
        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Draws the button
        /// </summary>
        /// <param name="spriteBatch">Specifies SpriteBatch to be used when drawing.</param>

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(this.texture, this.currentPosition, Color.White);
        }

        #endregion

    }
}