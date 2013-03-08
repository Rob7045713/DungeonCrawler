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
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
#endregion

namespace LabRatEscape
{
    /// <summary>
    /// Singleton which holds global values pertinant for drawing double vision
    /// </summary>
    public static class DoubleVisionHelper
    {
        #region Variables
        public static float opacity;
        public static Vector2 offset;
        #endregion

        #region Getters/Setters
        /// <summary>
        /// grabs the opacity
        /// </summary>
        public static float getOpacity()
        {
            return opacity;
        }

        /// <summary>
        /// grabs the offset variable
        /// </summary>
        public static Vector2 getOffset()
        {
            return offset;
        }

        /// <summary>
        /// sets the opacity to a new float
        /// </summary>
        /// <param name="newOpacity">float to be set</param>
        public static void setOpacity(float newOpacity)
        {
            opacity = newOpacity;
        }

        /// <summary>
        /// grabs the opacity
        /// </summary>
        /// <param name="newOffset">Vector2 to be set</param>
        public static void setOffset(Vector2 newOffset)
        {
            offset = newOffset;
        }
        #endregion
    }
}
