#region Using Statements

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace GZK {

    /// <summary>
    /// This class creates interface panel objects for the menu
    /// Extends GameObject
    /// </summary>

    public class Panel: GameObject {

        #region Variable Declarations

        public int action, offset, side;
        private ContentManager content;
        private string name;
        private Button closeButton;
        private Vector2 maxVelocity, minVelocity, currentVelocity;

        #endregion

        #region Class Constructor

        /// <summary>
        /// Class Constructor - Creates a new panel object for the game menu
        /// </summary>
        /// <param name="w">Panel width.</param>
        /// <param name="h">Panel height.</param>
        /// <param name="pX">Panel onscreen x co-ordinate.</param>
        /// <param name="pY">Panel onscreen y co-ordinate.</param>
        /// <param name="s">Side of the screen the panel is on.</param>
        /// <param name="n">Panel name for resource location.</param>
        /// <param name="cBtn">Close button accociated with this panel.</param>
        /// <param name="c">Content manager for storing textures and sounds.</param>

        public Panel(int w, int h, int pX, int pY, string n, Button cBtn, ContentManager c) {
            this.content = c;
            this.action = -1; 
            this.name = n;
            this.width = w;
            this.height = h;
            this.initialPosition.X = pX;
            this.initialPosition.Y = pY;
            this.currentPosition = this.initialPosition;
            this.texture = content.Load<Texture2D>("Textures\\Menu\\" + this.name);
            this.closeButton = cBtn;
            this.offset = this.width;

            //Figure out what side of the screen this panel is on ()
            if(this.initialPosition.X < 100) {
                this.side = 1;
                this.offset *= -1;
            } else {
                this.side = -1;
            }

            //Adjust velocity and location according to the side of the screen the panel is on
            this.maxVelocity.X = this.side * 15;
            this.minVelocity.X = this.side * 2;
            this.currentVelocity.X = this.maxVelocity.X;

            //Move the panel off screen, along with it's close button
            this.UpdatePosition((int)(this.currentPosition.X + this.offset), (int)this.currentPosition.Y);
            this.UpdateCloseButton();
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Updates the panels current position when opening/closing
        /// </summary>

        public void Update() {

            //If opening or closing, do the appropriate action
            if(this.action == 1) {
                this.Open();
            } else if(this.action == 0) {
                this.Close();
            }
        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Draws the panel
        /// </summary>
        /// <param name="spriteBatch">Specifies SpriteBatch to be used when drawing.</param>

        public void Draw(SpriteBatch spriteBatch) {
                spriteBatch.Draw(this.texture, this.currentPosition, Color.White);
        }

        #endregion

        #region UpdateCloseButton Method

        /// <summary>
        /// Moves the close button associated with this panel according to where this panel currently is
        /// </summary>

        public void UpdateCloseButton() {
            this.closeButton.UpdatePosition((int)(this.currentPosition.X + this.width - (closeButton.width + 10)), (int)(this.currentPosition.Y + 10));
        }

        #endregion

        #region Open Method

        /// <summary>
        /// Calculates and implements the position adjustments necessary to "open" the panel
        /// </summary>

        public void Open() {

            //Set the currentVelocity
            if((this.side == 1 && this.currentVelocity.X > this.minVelocity.X) || (this.side == -1 && this.currentVelocity.X < this.minVelocity.X)) {
                this.currentVelocity.X *= (float)0.985;
            } else {
                this.currentVelocity = this.minVelocity;
            }

            //Move according to the currentVelocity
            if((this.side == 1 && this.currentPosition.X < this.initialPosition.X) || (this.side == -1 && this.currentPosition.X > this.initialPosition.X)) {
                this.currentPosition.X += this.currentVelocity.X;
            } else {
                this.currentPosition = this.initialPosition;
                this.currentVelocity = this.minVelocity;
                this.action = -1; 
            }

            //Move the close button
            this.UpdateCloseButton();
        }

        #endregion

        #region Close Method

        /// <summary>
        /// Calculates and implements the position adjustments necessary to "close" the panel
        /// </summary>
        
        public void Close() {

            //Set the currentVelocity
            if((this.side == 1 && this.currentVelocity.X < this.maxVelocity.X) || (this.side == -1 && this.currentVelocity.X > this.maxVelocity.X)) {
                this.currentVelocity.X *= (float)1.07;
            } else {
                this.currentVelocity = this.maxVelocity;
            }

            //Move according to the currentVelocity
            if((this.side == 1 && this.currentPosition.X > (this.initialPosition.X + this.offset)) || (this.side == -1 && this.currentPosition.X < (this.initialPosition.X + this.offset))) {
                this.currentPosition.X -= this.currentVelocity.X;
            } else {
                this.currentPosition.X = (this.initialPosition.X + this.offset);
                this.currentVelocity = this.maxVelocity;
                this.action = -1;
            }

            //Move the close button
            this.UpdateCloseButton();
        }

        #endregion

    }
}
