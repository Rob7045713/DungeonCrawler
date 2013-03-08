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
using System.Reflection;
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
    /// Block class for all the units that make up the map
    /// </summary>
    public class Block : Sprite
    {
        #region Variables
        internal int[,] passability;
        internal int[,] activePassability;
        internal int[,] inactivePassability;
        internal List<Effect> effects;
        internal bool onStep;
        internal String id;
        internal float activeTime;
        internal float inactiveTime;
        internal float timer = 0.0f;
        internal bool isTimed;
        internal bool isBeingSteppedOn;
        internal Condition passabilityCondition;
        #endregion

        #region Constants
        public static int WIDTH = 64;
        public static int HEIGHT = 64;
        public const float DEFAULT_EFFECT_DELAY = .1f;
        public const int PASSABLE = -1;
        public const int IMPASSABLE = 0;
        public const int OUTSIDE = 0;
        public const int INSIDE = 1;
        public const int RIGHT = 0;
        public const int BOTTOM = 1;
        public const int LEFT = 2;
        public const int TOP = 3;
        public const int ACTIVE = 1;
        public const int INACTIVE = 0;
        public static int[,] FULLY_PASSABLE = { { PASSABLE, PASSABLE, PASSABLE, PASSABLE }, { PASSABLE, PASSABLE, PASSABLE, PASSABLE } };
        public static int[,] FULLY_IMPASSABLE = { { IMPASSABLE, IMPASSABLE, IMPASSABLE, IMPASSABLE }, { PASSABLE, PASSABLE, PASSABLE, PASSABLE } };
        public static int[,] LEFT_RIGHT_TUNNEL = { { PASSABLE, IMPASSABLE, PASSABLE, IMPASSABLE }, { PASSABLE, IMPASSABLE, PASSABLE, IMPASSABLE } };
        public static int[,] UP_DOWN_TUNNEL = { { IMPASSABLE, PASSABLE, IMPASSABLE, PASSABLE }, { IMPASSABLE, PASSABLE, IMPASSABLE, PASSABLE } };
        #endregion

        #region Constructors

        /// <summary>
        /// Basic constructor for a new block
        /// </summary>
        /// <param name="id">Id to associate with this block</param>
        /// <param name="spriteSheet">Spritesheet to use for the block</param>
        /// <param name="position">Position of the block</param>
        public Block(String id, Texture2D spriteSheet, Vector2 position)
            : this(id, spriteSheet, position, null, false, FULLY_PASSABLE, 0, 0)
        {
        }

        /// <summary>
        /// Constructor for creating a new block with a non-default passability
        /// </summary>
        /// <param name="id">Id to associate with this block</param>
        /// <param name="spriteSheet">Spritesheet to use for the block</param>
        /// <param name="position">Position of the block</param>
        /// <param name="passability">Passability of the block. Default is FULLY_PASSABLE</param>
        public Block(String id, Texture2D spriteSheet, Vector2 position, int[,] passability)
            : this(id, spriteSheet, position, null, false, passability, 0, 0)
        {
        }

        /// <summary>
        /// Constructor for creating a block with effects
        /// </summary>
        /// <param name="id">Id to associate with this block</param>
        /// <param name="spriteSheet">Spritesheet to use for the block</param>
        /// <param name="position">Position of the block</param>
        /// <param name="effects">Effects to apply when the block is interacted with</param>
        /// <param name="onStep">If the block's effects are applied when it is stepped on, as opposed to when it is touched</param>
        /// <param name="passability">Passability of the block. Default is FULLY_PASSABLE</param>
        public Block(String id, Texture2D spriteSheet, Vector2 position, List<Effect> effects, bool onStep, int[,] passability)
            : this(id, spriteSheet, position, effects, onStep, passability, 0, 0)
        {         
        }

        /// <summary>
        /// Constructor for creating a block with a timer to switch between active and inactive
        /// </summary>
        /// <param name="id">Id to associate with this block</param>
        /// <param name="spriteSheet">Spritesheet to use for the block</param>
        /// <param name="position">Position of the block</param>
        /// <param name="effects">Effects to apply when the block is interacted with</param>
        /// <param name="onStep">If the block's effects are applied when it is stepped on, as opposed to when it is touched</param>
        /// <param name="passability">Passability of the block. Default is FULLY_PASSABLE</param>
        /// <param name="activeTime">How long the block is active for in seconds</param>
        /// <param name="inactiveTime">How long the block is inactive for in seconds</param>
        public Block(String id, Texture2D spriteSheet, Vector2 position, List<Effect> effects, bool onStep, int[,] passability, float activeTime, float inactiveTime)
            : this(id, spriteSheet, position, effects, onStep, passability, passability, null, 0, 0, Sprite.DEFAULT_FRAME_TIME, DEFAULT_EFFECT_DELAY)
        {
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
        public Block(String id, Texture2D spriteSheet, Vector2 position, List<Effect> effects, bool onStep, int[,] activePassability,
            int[,] inactivePassability, Condition passabilityCondition,
            float activeTime, float inactiveTime, float frameTime, float effectDelay)
            : base(spriteSheet, WIDTH, HEIGHT, frameTime, position)
        {
            if (effects == null)
            {
                effects = new List<Effect>();
            }
            this.effects = effects;
            this.onStep = onStep;
            this.id = id;
            this.activePassability = activePassability;
            this.inactivePassability = inactivePassability;
            this.passabilityCondition = passabilityCondition;
            isBeingSteppedOn = false;        
            this.isTimed = false;
            TimerManager.RegisterTimer(id, effectDelay);
            if (activeTime > 0 || inactiveTime > 0)
            {
                this.isTimed = true;
            }
            this.activeTime = activeTime;
            this.inactiveTime = inactiveTime;
            this.state = new InactiveState(this);
        }
        #endregion

        #region Getters/Setters
        //gets the block ID string
        public string getId()
        {
            return id;
        }
        #endregion

        #region Methods

        /// <summary>
        /// create a copy of the block at a new given position
        /// </summary>
        /// <param name="position">new position Vector2</param>
        /// <returns>Copy of the block at the given position</returns>
        public virtual Block copyBlockAt(Vector2 position)
        {
            return new Block(id, spriteSheet, position, effects, onStep, activePassability, inactivePassability,
                passabilityCondition, activeTime, inactiveTime, frameTime, TimerManager.timers[id].time);
        }

        /// <summary>
        /// Returns a clone of the block
        /// </summary>
        /// <returns>A clone of the block</returns>
        public virtual Block Clone()
        {
            List<Effect> copyEffects = new List<Effect>(effects.Take<Effect>(effects.Count));
            return new Block(id, spriteSheet, position, copyEffects, onStep, activePassability, inactivePassability,
                passabilityCondition, activeTime, inactiveTime, frameTime, TimerManager.timers[id].time);
        }

        /// <summary>
        /// Handles a collision with the block
        /// </summary>
        /// <param name="collision">Collision to handle</param>
        public void Collide(Collision collision)
        {
            if (collision.isColliding)
            {
                if (onStep)
                {
                    if (collision.isOverlapping)
                    {
                        applyEffects(collision.prop);
                        isBeingSteppedOn = true;
                    }
                    else
                    {
                        isBeingSteppedOn = false;
                    }

                }
                else
                {
                    applyEffects(collision.prop);
                }
                //determines passibility
                Sprite prop = collision.prop;
                // if the passability condition is met, dont check passability
                if (passabilityCondition == null || !passabilityCondition.CheckCondition(collision.prop))
                {
                    for (int edge = RIGHT; edge <= TOP; edge++)
                    {
                        if (passability[INSIDE, edge] != PASSABLE && collision.edges[edge] == Collision.INTERNAL_COLLISION)
                        {
                            if (edge == RIGHT || edge == LEFT)
                            {
                                prop.position.X = prop.oldPosition.X;
                            }

                            if (edge == BOTTOM || edge == TOP)
                            {
                                prop.position.Y = prop.oldPosition.Y;
                            }

                        }

                        if (passability[OUTSIDE, edge] != PASSABLE && collision.edges[edge] == Collision.EXTERNAL_COLLISION)
                        {
                            if (edge == RIGHT || edge == LEFT)
                            {
                                prop.position.X = prop.oldPosition.X;
                            }

                            if (edge == BOTTOM || edge == TOP)
                            {
                                prop.position.Y = prop.oldPosition.Y;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Applies the blocks effects
        /// </summary>
        public virtual void applyEffects(Sprite sprite)
        {
            if (sprite is Player)
            {
                (state as BlockState).ApplyEffects(this, sprite);
            }
        }

        /// <summary>
        /// Sets the state to active
        /// </summary>
        public void Activate()
        {
            state = new ActiveState(this);
        }

        /// <summary>
        /// Sets the state to inactive
        /// </summary>
        public void Deactivate()
        {
            state = new InactiveState(this);
        }

        /// <summary>
        /// Draws the block
        /// </summary>
        /// <param name="batch">Sprite batch to use to draw the block</param>
        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);
        }

        /// <summary>
        /// Updates the block
        /// </summary>
        /// <param name="elapsedTime">Elapsed time in seconds since last update</param>
        public override void Update(float elapsedTime)
        {
            timer += elapsedTime;
            base.Update(elapsedTime);
        }

        /// <summary>
        /// Adds the block to the level map
        /// </summary>
        /// <param name="row">Row of the level map</param>
        /// <param name="col">Column of the level map</param>
        public override void AddToLevel(int row, int col)
        {
            Level.GetInstance().levelMap[row, col] = copyBlockAt(new Vector2(col * WIDTH, row * HEIGHT));
        }
        #endregion

        /// <summary>
        /// Seys a match ID for matching
        /// </summary>
        /// <param name="matchID">ID to use for matching</param>
        public virtual void setMatchID(String matchID)
        {
            // do nothing by default
        }

        #region Inner State Classes

        // Block states
        public class InactiveState : BlockState
        {
            public InactiveState(Block block)
            {
                block.timer = 0;
                block.frameTimer = 0;
                block.currentFrame = 0;
                block.currentAnimation = Block.INACTIVE;
                block.passability = block.inactivePassability;
            }

            public void Update(Sprite sprite, float elapsedTime)
            {
                if ((sprite as Block).isTimed)
                {
                    if ((sprite as Block).timer > (sprite as Block).inactiveTime)
                    {
                        sprite.state = new ActiveState((sprite as Block));
                    }
                }
                else if ((sprite as Block).onStep)
                {
                    if ((sprite as Block).isBeingSteppedOn)
                    {
                        sprite.state = new ActiveState((sprite as Block));
                    }
                }
            }

            public void Draw(Sprite sprite, SpriteBatch batch)
            {
                //probably dont need state to draw.
            }

            public void ApplyEffects(Block block, Sprite sprite)
            {
                // dont apply effects
            }
        }

        public class ActiveState : BlockState
        {
            public ActiveState(Block block)
            {
                block.timer = 0;
                block.frameTimer = 0;
                block.currentFrame = 0;
                block.currentAnimation = Block.ACTIVE;
                block.passability = block.activePassability;
            }

            public void Update(Sprite sprite, float elapsedTime)
            {
                if ((sprite as Block).isTimed)
                {
                    if ((sprite as Block).timer > (sprite as Block).activeTime)
                    {
                        sprite.state = new InactiveState((sprite as Block));
                    }
                }
                else if ((sprite as Block).onStep)
                {
                    if ((sprite as Block).isBeingSteppedOn)
                    {
                        sprite.state = new InactiveState((sprite as Block));
                    }
                }
            }

            public void Draw(Sprite sprite, SpriteBatch batch)
            {
                //probably dont need state to draw.
            }

            public void ApplyEffects(Block block, Sprite sprite)
            {
                if (block.effects != null && TimerManager.timers[block.id].HasElapsed())
                {
                    TimerManager.timers[block.id].Reset();
                    foreach (Effect effect in block.effects)
                    {
                        effect.Apply(sprite);
                    }
                }
            }
        }
        #endregion
    }
}
