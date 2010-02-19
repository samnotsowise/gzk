#region Using Statements

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

#endregion

namespace GZK {

    /// <summary>
    /// Class for Agent objects in the game
    /// Extends Target
    /// </summary>

    public class Agent: Target {

        #region Variable Declarations

        private bool exiting;
        private int appearTime;

        #endregion

        #region Class Constructor

        /// <summary>
        /// Create a new Agent object
        /// </summary>
        /// <param name="t">Target that Agent should be based on</param>

        public Agent(Target t) {

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
        /// Update Agent
        /// </summary>

        public void Update() {

            //Is this agent active?
            if(this.enabled == true) {

                //Is this agent alive?
                if(this.dead == false) {

                    //Check if we have just killed this agent
                    if(this.CheckKill() == false) {

                        //Has this agent just escaped?
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
                                this.gameState.difficulty++;
                                this.sounds[2].Play();
                            }
                        }

                    //If we just killed this agent
                    } else {
                        this.Reset();
                        this.gameState.score += 1 * this.gameState.difficulty;
                    }

                //Fade out dead agent until gone, then reset the agent
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
                if(rnum >= (int)((this.currentPosition.Y + this.currentPosition.X) / 10) && rnum <= (int)(((this.currentPosition.Y + this.currentPosition.X) / 10) + (this.gameState.difficulty))) {
                    this.enabled = true;
                    this.appearTime = this.gameState.clock;
                }
            }
        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Draw Agent
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to use when drawing</param>

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(this.texture, this.rectangle, null, this.colour, 0, new Vector2(0, 0), this.spriteEffects, this.layer);
        }

        #endregion

        #region Reset Method

        /// <summary>
        /// Reset Agent
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
