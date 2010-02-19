#region Using Statements

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

#endregion

namespace GZK {

    /// <summary>
    /// Class for Zombie objects in the game
    /// Extends Target
    /// </summary>

    public class Zombie: Target {

        #region Variable Declarations

        private bool flea;
        private Vector2 minVelocity, maxVelocity, currentVelocity;
        private Rectangle fearRect;

        #endregion

        #region Class Constructor

        /// <summary>
        /// Create a new Zombie object
        /// </summary>
        /// <param name="t">Target that Zombie should be based on</param>

        public Zombie(Target t) {

            //Get what is necessary from the target object
            this.gameState = t.gameState;
            this.width = t.width;
            this.height = t.height;
            this.textures = t.textures;
            this.texture = this.textures[1];
            this.initialPosition = t.initialPosition;
            this.currentPosition = this.initialPosition;
            this.layer = t.layer;
            this.enabled = t.enabled;
            this.dead = t.dead;
            this.currentTexture = t.currentTexture;
            this.death1 = t.death1;
            this.death2 = t.death2;
            this.CalculateSide();

            this.UpdateVelocity();
            this.UpdatePosition(this.initialPosition);
            this.fearRect = new Rectangle((int)(this.currentPosition.X - (this.width / 2)), (int)this.currentPosition.Y, this.width * 2, this.height);
            this.colour = new Color(255, 255, 255, 255);
            this.alpha = 255;
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Update Zombie
        /// </summary>

        public void Update() {
            
            //Is this zombie active?
            if(this.enabled == true) {

                //Is this zombie alive?
                if(this.dead == false) {

                    //Check if we have just killed this zombie
                    if(this.CheckKill() == false) {

                        //Has this zombie just escaped?
                        if((this.side == 1 && this.currentPosition.X > 800) || (this.side == -1 && this.currentPosition.X < (-1 * this.width))) {
                            this.Reset();
                            this.gameState.infection++;
                        } else {

                            //If this zombie is fleaing, or has just been panicked, accelerate towards maximum velocity
                            if(this.flea == true || (this.fearRect.Intersects(this.gameState.mouse.rectangle) && this.gameState.mouse.mouseUp == true)) {
                                this.flea = true;
                                if((this.side == 1 && this.currentVelocity.X < this.maxVelocity.X) || (this.side == -1 && this.currentVelocity.X > this.maxVelocity.X)) {
                                    this.currentVelocity.X *= (float)1.04;
                                } else {
                                    this.currentVelocity = this.maxVelocity;
                                }
                            }

                            //Animate
                            this.currentTexture += 0.5;
                            if(this.currentTexture > 20) {
                                this.currentTexture = 1;
                            }
                            this.texture = this.textures[(int)this.currentTexture];

                            //And now we move the zombie according to it's current velocity
                            this.currentPosition.X += this.currentVelocity.X;
                            this.UpdateRectPosition();
                            this.fearRect.X = (int)(this.currentPosition.X - (this.width / 2));
                        }

                    //Death action specific to Zombies
                    } else {
                        this.gameState.score += 1 * this.gameState.difficulty;
                    }

                //Fade out dead Zombie until gone, then reset the zombie
                } else {
                    if(this.alpha > 0) {
                        this.alpha -= 17;
                    } else {
                        this.Reset();
                    }
                    this.colour = new Color(255, 255, 255, (byte)this.alpha);
                }

            //Give it a chance to start moving
            } else {
                int rnum = this.gameState.random.Next(1, 1000);
                if(rnum >= (int)((this.currentPosition.Y + this.currentPosition.X) / 2) && rnum <= (int)(((this.currentPosition.Y + this.currentPosition.X) / 2) + (this.gameState.difficulty * 2))) {
                    this.UpdateVelocity();
                    this.enabled = true;
                }
            }
        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Draw Zombie
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to use when drawing</param>

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(this.texture, this.rectangle, null, this.colour, 0, new Vector2(0, 0), this.spriteEffects, this.layer);
        }

        #endregion

        #region UpdateFearRect Method

        /// <summary>
        /// Updates the location of the fear rectanlge for this zombie
        /// </summary>

        private void UpdateFearRect() {
            this.fearRect.X = (int)(this.currentPosition.X - this.width);
        }

        #endregion

        #region UpdateVelocity Method

        /// <summary>
        /// Recalculates velocity based on current difficulty level
        /// </summary>

        private void UpdateVelocity() {
            this.minVelocity.X = (int)(((this.gameState.random.Next(1, 4) + 2 + this.gameState.difficulty) * (this.width / 140.0)) * this.side);
            this.maxVelocity.X = minVelocity.X + (4 * this.side);
            this.currentVelocity = this.minVelocity;
        }

        #endregion

        #region Reset Method

        /// <summary>
        /// Reset Zombie
        /// </summary>

        private void Reset() {
            this.alpha = 255;
            this.flea = false;
            this.dead = false;
            this.enabled = false;
            this.UpdatePosition(this.initialPosition);
            this.currentVelocity = this.minVelocity;
        }

        #endregion

    }
}
