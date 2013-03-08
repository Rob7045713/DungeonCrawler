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
    /// AI for chasing a player down a horizontal hall
    /// </summary>
    class LeftRightHallAI : EnemyAI
    {
        #region Variables
        internal int speed;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new LeftRightHallAI
        /// </summary>
        /// <param name="speed">Speed for the enemy to move at (pixels pre second)</param>
        public LeftRightHallAI(int speed) : base() 
        {
            this.speed = speed;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Updates the enemy using the AI
        /// </summary>
        /// <param name="ElapsedTime">Time in seconds since last update</param>
        /// <param name="enemy">Enemy to update</param>
        public override void Update(float ElapsedTime, Enemy enemy)
        {
            enemy.velocity = new Vector2(speed * Math.Sign(enemy.chasing.position.X - enemy.position.X), 0);
        }
        #endregion
    }
}
