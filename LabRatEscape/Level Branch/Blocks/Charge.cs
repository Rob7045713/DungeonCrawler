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
    class Charge : Block
    {
        #region Constructors

        public Charge(String id, Texture2D spriteSheet, Vector2 position, List<Effect> effects, bool onStep,
            int[,] activePassability, int[,] inactivePassability, Condition passabilityCondition,
            float activeTime, float inactiveTime, float frameTime, float effectDelay)
            : base(id, spriteSheet, position, effects, onStep, activePassability, inactivePassability, 
            passabilityCondition, activeTime, inactiveTime, inactiveTime / (spriteSheet.Width/Block.WIDTH), effectDelay)
        {
            state = new ActiveState(this);
        }
        #endregion

        #region Methods

        /// <summary>
        /// create a copy of the block at a new given position
        /// </summary>
        /// <param name="position">new position Vector2</param>
        /// <returns>Copy of the block at the given position</returns>
        public override Block copyBlockAt(Vector2 position)
        {
            Block block = new Charge(id, spriteSheet, position, effects, onStep, activePassability, inactivePassability,
                passabilityCondition, activeTime, inactiveTime, frameTime, TimerManager.timers[id].time);
            return block;
        }

        /// <summary>
        /// Clones the block
        /// </summary>
        /// <returns>A clone fo the block</returns>
        public override Block Clone()
        {
            List<Effect> copyEffects = new List<Effect>(effects.Take<Effect>(effects.Count));
            return new Charge(id, spriteSheet, position, copyEffects, onStep, activePassability, inactivePassability,
                passabilityCondition, activeTime, inactiveTime, frameTime, TimerManager.timers[id].time);
        }

        /// <summary>
        /// Applies the effects of the block to a sprite
        /// </summary>
        /// <param name="sprite">Sprite to apply the effects to</param>
        public override void applyEffects(Sprite sprite)
        {
 	        base.applyEffects(sprite);
            if (state is ActiveState)
            {
                state = new InactiveState(this);
            }
        }
        #endregion
    }
}
