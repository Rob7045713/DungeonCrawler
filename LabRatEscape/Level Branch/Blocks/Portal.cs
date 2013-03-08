// Copyright 12.13.2010
// Rob Argue
// David Edge
// Leanna Helton
// Joe Kiernen

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
    /// Block which teleports the player to the corresponding exit
    /// </summary>
    public class Portal : Block
    {
        #region Variables
        internal Portal portalExit;
        internal String matchID;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor that takes a spriteSheet and a position for the block
        /// </summary>
        /// <param name="position">Vector2 of where to place the block</param>
        /// <param name="spriteSheet">Texture2D spriteSheet for the block to display</param>
        public Portal(Texture2D spriteSheet, Vector2 position)
            : base("portalID",spriteSheet, position, null, false, Block.FULLY_PASSABLE)
        {
            portalExit = null;
        }

         /// <summary>
        /// Fully featured constructor for creating a new block
        /// </summary>
        /// <param name="id">Id to associate with this block</param>
        /// <param name="spriteSheet">Spritesheet to use for the block</param>
        /// <param name="position">Position of the block</param>
        /// <param name="effects">Effects to apply when the block is interacted with</param>
        /// <param name="onStep">If the block's effects are applied when it is stepped on, as opposed to when it is touched</param>
        /// <param name="passability">Passability of the block. Default is FULLY_PASSABLE</param>
        /// <param name="activeTime">How long the block is active for in seconds</param>
        /// <param name="inactiveTime">How long the block is inactive for in seconds</param>
        /// <param name="frameTime">How long each frame displays for in seconds</param>
        public Portal(String id, Texture2D spriteSheet, Vector2 position, List<Effect> effects, bool onStep, int[,] activePassability,
            int[,] inactivePassability, Condition passabilityCondition,
            float activeTime, float inactiveTime, float frameTime, float effectDelay)
            : base(id, spriteSheet, position, null, false, Block.FULLY_PASSABLE)
        { 
            portalExit = null;
            if (id.Contains("Entrance"))
            {
                state = new ActiveState(this);
            }
        }
        #endregion

        #region Getters/Setters
        public override void setMatchID(String matchID)
        {
            this.matchID = matchID;
        }

        public String getMatchID()
        {
            return matchID;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Links the portal to its exit
        /// </summary>
        /// <param name="portalExit">Exit to link to</param>
        public void Link(Portal portalExit)
        {
            this.portalExit = portalExit;
        }

        /// <summary>
        /// Applies the effects to a sprite
        /// </summary>
        /// <param name="sprite">Sprite to apply the effects to</param>
        public override void applyEffects(Sprite sprite)
        {
            if (portalExit != null)
            {
                PlayerManager.players[0].position = portalExit.position;
            }
        }

        /// <summary>
        /// Creates a copy of the block at the specified position
        /// </summary>
        /// <param name="position">Position to copy to</param>
        /// <returns>Copy of the block at the specified position</returns>
        public override Block copyBlockAt(Vector2 position)
        {
            Block portal = new Portal(id, spriteSheet, position, effects, onStep, activePassability, inactivePassability,
                     passabilityCondition, activeTime, inactiveTime, frameTime, TimerManager.timers[id].time);
            portal.setMatchID(this.matchID);
            return portal;
        }

        /// <summary>
        /// Clones the block
        /// </summary>
        /// <returns>Clone of the block</returns>
        public override Block Clone()
        {
            List<Effect> copyEffects = new List<Effect>(effects.Take<Effect>(effects.Count));
            return new Portal(id, spriteSheet, position, copyEffects, onStep, activePassability, inactivePassability,
                passabilityCondition, activeTime, inactiveTime, frameTime, TimerManager.timers[id].time);
        }

        /// <summary>
        /// Adds the block to the level map at the specified coordinates
        /// </summary>
        /// <param name="row">Row to add the block to</param>
        /// <param name="col">Column to ad the block to</param>
        public override void AddToLevel(int row, int col)
        {
            Block portal = this.copyBlockAt(new Vector2(col * WIDTH, row * HEIGHT));
            Level.GetInstance().levelMap[row, col] = portal;
            if (id.Contains("Entrance"))
            {
                Level.portalsEntrance.Add(portal as Portal);
            }
            if (id.Contains("Exit"))
            {
                Level.portalsExit.Add(portal as Portal);
            }
        }

        #endregion
    }
}
