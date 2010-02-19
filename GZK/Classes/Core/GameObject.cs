#region Using Statements

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace GZK {

    /// <summary>
    /// Base class for game objects.
    /// </summary>

    public class GameObject {

        #region Variable Declarations

        public int height, width, alpha;
        public Vector2 initialPosition, currentPosition;
        public Texture2D texture;
        public Rectangle rectangle;

        #endregion

        #region Class Constructors

        /// <summary>
        /// Create a new GameObject
        /// </summary>
        /// <param name="w">Object width.</param>
        /// <param name="h">Object height.</param>
        /// <param name="iPx">Initial x co-ordinate.</param>
        /// <param name="iPy">Initial y co-ordinate.</param>
        /// <param name="t">Object texture.</param>
    
        public GameObject(int w, int h, int iPx, int iPy, Texture2D t) {
            this.width = w;
            this.height = h;
            this.alpha = 255;
            this.initialPosition.X = iPx;
            this.initialPosition.Y = iPy;
            this.texture = t;
            this.rectangle = new Rectangle((int)this.initialPosition.X, (int)this.initialPosition.Y, this.width, this.height);
            this.UpdatePosition((int)this.initialPosition.X, (int)this.initialPosition.Y);
        }

        /// <summary>
        /// Create a new GameObject, with an initial position 0,0
        /// </summary>
        /// <param name="w">Object width.</param>
        /// <param name="h">Object height.</param>
        /// <param name="t">Object texture.</param>
        
        public GameObject(int w, int h, Texture2D t) {
            this.width = w;
            this.height = h;
            this.alpha = 255;
            this.initialPosition.X = 0;
            this.initialPosition.Y = 0;
            this.texture = t;
            this.rectangle = new Rectangle((int)this.initialPosition.X, (int)this.initialPosition.Y, this.width, this.height);
            this.UpdatePosition((int)this.initialPosition.X, (int)this.initialPosition.Y);
        }

        /// <summary>
        /// This constructor is used when extending the class, variables should be declared in the extension
        /// NOT for use with regular objects!
        /// </summary>

        public GameObject() {
            this.alpha = 1;
            this.rectangle = new Rectangle((int)this.initialPosition.X, (int)this.initialPosition.Y, this.width, this.height);
            this.currentPosition = this.initialPosition;
            this.UpdateRectPosition();
        }

        #endregion

        #region UpdatePosition Method

        /// <summary>
        /// Updates the position of the object and it's rectangle
        /// <param name="pX">New x co-ordinate.</param>
        /// <param name="pY">New y co-ordinate.</param>
        /// </summary>

        public void UpdatePosition(int pX, int pY) {
            this.currentPosition.X = pX;
            this.currentPosition.Y = pY;
            this.UpdateRectPosition();
        }

        /// <summary>
        /// Updates the position of the object and it's rectangle
        /// <param name="pos">New vector.</param>
        /// </summary>

        public void UpdatePosition(Vector2 pos) {
            this.currentPosition = pos;
            this.UpdateRectPosition();
        }

        #endregion

        #region UpdateRectPosition Method

        /// <summary>
        /// Updates the position of the object's rectangle
        /// </summary>

        public void UpdateRectPosition() {
            this.rectangle.Height = this.height;
            this.rectangle.Width = this.width;
            this.rectangle.X = (int)this.currentPosition.X;
            this.rectangle.Y = (int)this.currentPosition.Y;
        }

        #endregion

    }
}
