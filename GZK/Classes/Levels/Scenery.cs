#region Using Statement

using System;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace GZK {

    /// <summary>
    /// Class for handling scenery in levels
    /// Extends GameObject
    /// </summary>

    public class Scenery: GameObject {

        #region Variable Declarations

        public bool enabled, destructible, obstructive;
        public float layer;

        #endregion

        #region Class Constructor

        /// <summary>
        /// Creates a Scenery object
        /// </summary>
        /// <param name="w">Scenery width</param>
        /// <param name="h">Scenery height</param>
        /// <param name="iPx">Scenery X co-ordinate</param>
        /// <param name="iPy">Scenery Y co-ordinate</param>
        /// <param name="tx">Scenery texture</param>
        /// <param name="o">Is this object obstructive</param>
        /// <param name="d">Is this object destructible</param>
        /// <param name="l">Layer in which to draw this object</param>

        public Scenery(int w, int h, int iPx, int iPy, Texture2D tx, bool o, bool d, float l) {
            this.width = w;
            this.height = h;
            this.initialPosition.X = iPx;
            this.initialPosition.Y = iPy;
            this.texture = tx;
            this.obstructive = o;
            this.destructible = d;
            this.layer = l;
            this.enabled = true;
            this.UpdatePosition(this.initialPosition);
        }

        #endregion

    }
}
