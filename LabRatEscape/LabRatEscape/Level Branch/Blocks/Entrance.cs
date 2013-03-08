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
    /// Entrance is a special type of block
    /// </summary>
    class Entrance : Block
    {
        #region Constructors

        /// <summary>
        /// Constructor that takes a spriteSheet and a position for the block
        /// </summary>
        /// <param name="position">Vector2 of where to place the block</param>
        /// <param name="spriteSheet">Texture2D spriteSheet for the block to display</param>
        public Entrance(Texture2D spriteSheet, Vector2 position)
            : base("entrance", spriteSheet, position, null, false, Block.FULLY_PASSABLE)
        {
        }

        public Entrance(String id, Texture2D spriteSheet, Vector2 position, List<Effect> effects, bool onStep,
            int[,] activePassability, int[,] inactivePassability, Condition passabilityCondition,
            float activeTime, float inactiveTime, float frameTime, float effectDelay)
            : base("entrance", spriteSheet, position, null, false, Block.FULLY_PASSABLE)
        { }
        #endregion

        #region Methods

        /// <summary>
        /// Method to add the entrance block to the level
        /// </summary>
        /// <param name="col">int col position on map</param>
        /// <param name="row">int row position on map</param>
        public override void AddToLevel(int row, int col)
        {
            if (Level.GetInstance().hasEnterance)
            {
                Console.WriteLine("Entrance.AddToLevel() : Entrance already exists");
            }
            else
            {
                // this needs to be fixed to work for multiple players
                PlayerManager.players[0].position = new Vector2(col * WIDTH, row * HEIGHT);
                Level.GetInstance().hasEnterance = true;
            }
            base.AddToLevel(row, col);
        }
        #endregion
    }
}
