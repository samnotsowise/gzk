#region Using Statements

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

#endregion

namespace GZK {

    /// <summary>
    /// Class for Bonus objects in the game
    /// Bonus zombies represent an unfinished game feature
    /// they were meant to give the user cake which could then
    /// be clicked at the users discretion, triggering an 
    /// explosion which would clear all enemies and destroy scenery.
    /// Abandonned due to time constraints
    /// Extends Target
    /// </summary>

    public class Bonus: Target {

        #region Variable Declarations

        private bool exiting;
        private int appearTime;

        #endregion

        #region Class Constructor

        /// <summary>
        /// Create a new Bonus object
        /// </summary>
        /// <param name="t">Target that Bonus should be based on</param>

        public Bonus(Target t) {

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
            this.currentTexture = 1.5;
            this.death1 = t.death1;
            this.death2 = t.death2;
            this.exiting = false;
            this.sounds = t.sounds;

            this.CalculateSide();
            this.UpdatePosition(this.initialPosition);
            this.colour = new Color(255, 255, 255, 255);
            this.alpha = 255;
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Update Bonus
        /// </summary>

        public void Update() {

            //Is this bonus zombie active?
            if(this.enabled == true) {

                //Is this bonus zombie alive?
                if(this.dead == false) {

                    //Check if we have just killed this bonus zombie
                    if(this.CheckKill() == false) {

                        //Has this bonus zombie just escaped?
                        if(this.currentTexture == 1) {
                            this.Reset();
                        } else {

                            //Animate
                            if(this.exiting == false) {
                                this.currentTexture += 0.5;
                            } else {
                                this.currentTexture -= 0.5;
                            }
                            if(this.currentTexture > 9) {
                                this.currentTexture = 9;
                            }
                            this.texture = this.textures[(int)this.currentTexture];

                            //If we've been on screen for 2 seconds, time is up, lets leave
                            if(((this.gameState.clock - this.appearTime) > (60 * 2)) && this.exiting == false) {
                                this.exiting = true;
                            }
                        }

                    //If we just killed this bonus zombie
                    } else {
                        this.Reset();
                        this.gameState.gotCake = true;
                        this.gameState.score += 20;
                    }

                //Fade out dead bonus zombie until gone, then reset it
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
                if((rnum == (int)this.currentPosition.Y || rnum == (int)this.currentPosition.X) && this.gameState.gotCake == false) {
                    this.enabled = true;
                    this.appearTime = this.gameState.clock;
                    this.sounds[3].Play();
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

        #region Reset Method

        /// <summary>
        /// Reset Bonus
        /// </summary>

        public void Reset() {
            this.alpha = 255;
            this.dead = false;
            this.enabled = false;
            this.exiting = false;
            this.appearTime = 0;
            this.currentTexture = 1.5;
        }

        #endregion

    }
}
