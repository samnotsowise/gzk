using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace GZK {

    /// <summary>
    /// Creates a target object, used to create more specific game targets
    /// </summary>

    public class Target: GameObject {

        #region Variable Declarations

        public string type;
        public int side;
        public double currentTexture;
        public Color colour;
        public float layer;
        public bool enabled, dead;
        public GameState gameState;
        public SpriteEffects spriteEffects;
        public Texture2D[] textures;
        public SoundEffect death1, death2;
        public SoundEffect[] sounds;

        #endregion

        #region Class Constructor

        /// <summary>
        /// Creates a new target object
        /// </summary>
        /// <param name="w">Target width</param>
        /// <param name="h">Target height</param>
        /// <param name="iPx">X co-ordinate of target</param>
        /// <param name="iPy">Y co-ordinate of target</param>
        /// <param name="gs">GameState</param>
        /// <param name="txs">Textures for this target</param>
        /// <param name="sfx">Sound effects for this target</param>
        /// <param name="t">Type of target</param>
        /// <param name="l">Layer for drawing target</param>

        public Target(int w, int h, int iPx, int iPy, GameState gs, Texture2D[] txs, SoundEffect[] sfx, string t, float l) {
            this.width = w;
            this.height = h;
            this.initialPosition.X = iPx;
            this.initialPosition.Y = iPy;
            this.textures = txs;
            this.type = t;
            this.layer = l;
            this.enabled = false;
            this.dead = false;
            this.currentTexture = 1;
            this.colour = new Color(255, 255, 255, 255);
            this.alpha = 255;
            this.spriteEffects = SpriteEffects.None;
            this.sounds = sfx;
            this.death1 = this.sounds[0];
            this.death2 = this.sounds[1];
            this.gameState = gs;
        }

        /// <summary>
        /// Only for extending this object 
        /// </summary>

        public Target() {
        }

        #endregion

        #region CheckKill Method

        /// <summary>
        /// Check to see wether this target has just been killed
        /// </summary>
        /// <returns>If target has been killed or not</returns>
        
        public bool CheckKill() {
            if(this.rectangle.Intersects(this.gameState.mouse.rectangle) && this.gameState.mouse.mouseUp == true && this.gameState.obstruction > this.layer) {

                //Stop dead zombies from taking one for the team
                this.gameState.mouse.mouseUp = false;

                //Increase the score according to difficulty, and kill the zombie, with sound effects
                if(this.gameState.random.Next(1, 3) == 1) {
                    this.death1.Play();
                } else {
                    this.death2.Play();
                }
                this.dead = true;
                this.texture = textures[0];
                return true;
            } else {
                return false;
            }
        }

        #endregion

        #region CalculateSide Method

        /// <summary>
        /// Calculate the side of the screen that the target is initially closest to (1 = left, -1 = right)
        /// and sets side accordingly
        /// </summary>
        
        public void CalculateSide() {
            if(this.initialPosition.X < 400) {
                this.side = 1;
                this.spriteEffects = SpriteEffects.FlipHorizontally;
            } else {
                this.side = -1;
                this.spriteEffects = SpriteEffects.None;
            }
        }

        #endregion

    }
}
