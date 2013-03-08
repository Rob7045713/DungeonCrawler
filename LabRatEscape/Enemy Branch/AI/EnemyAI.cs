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
    /// Abstract class for building enemy AI
    /// </summary>
    public abstract class EnemyAI
    {
        #region Constructors
        /// <summary>
        /// Constructs a new EnemyAI
        /// </summary>
        /// <param name="enemy">Enemy to be using this AI</param>
        public EnemyAI()
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Updates the enemy using the AI
        /// </summary>
        /// <param name="ElapsedTime">Time in seconds since last update</param>
        /// <param name="enemy">Enemy to update</param>
        public abstract void Update(float ElapsedTime, Enemy enemy);
        #endregion

    }
}
