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
    /// AI the moves directly towards the player
    /// </summary>
    class BeelineAI : EnemyAI
    {
        #region Variables
        internal int speed;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new Beelinee AI
        /// </summary>
        /// <param name="speed">Speed at which the enemy travels (pixels per second)</param>
        public BeelineAI(int speed) : base()
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
            Vector2 direction = VectorHelper.Normalize(enemy.chasing.position - enemy.position);
            enemy.velocity = direction * speed;
        }
        #endregion
    }
}
