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
    /// Entrance is a special type of block
    /// </summary>
    class Door : Block
    {
        #region Constructors

        /// <summary>
        /// Constructor that takes a spriteSheet and a position for the block
        /// </summary>
        /// <param name="position">Vector2 of where to place the block</param>
        /// <param name="spriteSheet">Texture2D spriteSheet for the block to display</param>
        public Door(Texture2D spriteSheet, Vector2 position)
            : base("door", spriteSheet, position, null, false, Block.FULLY_PASSABLE)
        {
        }

        public Door(String id, Texture2D spriteSheet, Vector2 position, List<Effect> effects, bool onStep,
            int[,] activePassability, int[,] inactivePassability, Condition passabilityCondition,
            float activeTime, float inactiveTime, float frameTime, float effectDelay)
            : base(id, spriteSheet, position, effects, onStep, activePassability, inactivePassability, 
            passabilityCondition, activeTime, inactiveTime, frameTime, effectDelay)
        { }
        #endregion

        #region Methods

        /// <summary>
        /// create a copy of the block at a new given position
        /// </summary>
        /// <param name="position">new position Vector2</param>
        /// <returns>Copy of the block at the given position</returns>
        public override Block copyBlockAt(Vector2 position)
        {
            Block block = new Door(id, spriteSheet, position, effects, onStep, activePassability, inactivePassability,
                passabilityCondition, activeTime, inactiveTime, frameTime, TimerManager.timers[id].time);
            return block;
        }

        /// <summary>
        /// Clones the block
        /// </summary>
        /// <returns>CLone of the block</returns>
        public override Block Clone()
        {
            List<Effect> copyEffects = new List<Effect>(effects.Take<Effect>(effects.Count));
            return new Door(id, spriteSheet, position, copyEffects, onStep, activePassability, inactivePassability,
                passabilityCondition, activeTime, inactiveTime, frameTime, TimerManager.timers[id].time);
        }

        /// <summary>
        /// Applies the effects of the block to a sprite
        /// </summary>
        /// <param name="sprite">Sprite to apply the effects to</param>
        public override void applyEffects(Sprite sprite)
        {
            if (sprite is Player)
            {
                if (state is InactiveState && ItemManager.inventories[(sprite as Player).playerNumber].TryUse(id + "Key"))
                {
                    SoundManager.PlayEffect("doorUnlocked");
                    Activate();
                }
            }
 	        base.applyEffects(sprite);
        }
        #endregion
    }
}
