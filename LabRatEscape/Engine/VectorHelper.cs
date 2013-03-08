// Copyright 12.13.2010
// Rob Argue

#region Imports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
#endregion

namespace LabRatEscape
{
    /// <summary>
    /// Utility class with addition methods for working with vectors
    /// </summary>
    /// <param name="value">The amount to add (or subtract if negative)</param>
    public static class VectorHelper
    {
        #region Methods
        /// <summary>
        /// Calculates the normal of a vector
        /// </summary>
        /// <param name="vector">Vector to normalize</param>
        /// <returns>Unit vector in the direction of vector, or (0,0) if vector was (0,0)</returns>
        public static Vector2 Normalize(Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(Math.Pow(vector.X, 2) + Math.Pow(vector.Y, 2));
            if (magnitude > 0)
            {
                vector /= magnitude;
            }
            return vector;
        }

        /// <summary>
        /// Projects vector a onto vector b
        /// </summary>
        /// <param name="a">Vector to project</param>
        /// <param name="b">Vector to project onto</param>
        /// <returns>The projection of a onto b</returns>
        public static Vector2 Project(Vector2 a, Vector2 b)
        {
            Vector2 normB = VectorHelper.Normalize(b);
            return Vector2.Dot(a, normB) * normB;
        }
        #endregion
    }
}
