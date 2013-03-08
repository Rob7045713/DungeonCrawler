// Copyright 12.13.2010
// Rob Argue
// David Edge
// Leanna Helton
// Joe Kiernen

#region Imports
using System;
using System.Collections.Generic;
using System.Collections;
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
    /// Sprite class which can handle various effects and flipbook animation
    /// </summary>
    public class Sprite
    {
        #region Variables
        internal Texture2D spriteSheet;
        internal int frameHeight;
        internal int frameWidth;
        internal Rectangle[,] frames;
        internal Vector2 position;
        internal Vector2 oldPosition;
        internal Vector2 velocity = new Vector2(0, 0);
        internal State state;
        internal float angle = 0f;
        internal SpriteEffects spriteEffect = SpriteEffects.None;
        internal float opacity = 1f;
        public GameValue scale = new GameValue(1, .5m, 5);
        internal int numFrames = 1;
        internal int numAnimations = 1;
        internal int currentFrame = 0;
        internal int currentAnimation = 0;
        internal float frameTimer = 0f;
        internal float frameTime;
        internal Color drawColor = Color.White;
        public GameValue speed = new GameValue(400, 200, 800);
        #endregion
        #region Constants
        public const float DEFAULT_FRAME_TIME = .25f;
        #endregion
         
        #region Constructors
        /// <summary>
        /// Creates a basic sprite
        /// </summary>
        /// <param name="texture">Texture to use for the sprite</param>
        /// <param name="position">Initial position of the sprite</param>
        public Sprite(Texture2D texture, Vector2 position)
            : this(texture, texture.Width, texture.Height, DEFAULT_FRAME_TIME, position) { }

        /// <summary>
        /// Creates an animated sprite using a spriteSheet
        /// </summary>
        /// <param name="spriteSheet">Sprite sheet to use for the sprite</param>
        /// <param name="frameWidth">Width of an individual frame</param>
        /// <param name="frameHeight">Height of an individual frame</param>
        /// <param name="frameTime">Time in seconds each frame displays for. DEFAULT_FRAME_TIME = .25</param>
        /// <param name="position">Initial position of the sprite</param>
        public Sprite(Texture2D spriteSheet, int frameWidth, int frameHeight, float frameTime, Vector2 position)
        {
            this.frameTime = frameTime;
            this.spriteSheet = spriteSheet;
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            this.position = position;
            ConstructFrames();
        }
        #endregion

        #region Methods

        /// <summary>
        /// Checks if the player is at least at a scale of 1
        /// </summary>
        /// <returns>True if the player is big</returns>
        public bool isBig()
        {
            return scale.getValue() >= 1;
        }

        /// <summary>
        /// Checks if the player is at a scale of less than 1
        /// </summary>
        /// <returns>True if the player is small</returns>
        public bool isSmall()
        {
            return scale.getValue() < 1;
        }

        /// <summary>
        /// Creates the matrix of frames from the sprite sheet
        /// </summary>
        private void ConstructFrames()
        {
            numFrames = (int)Math.Floor((decimal)(spriteSheet.Width / frameWidth));
            numAnimations = (int)Math.Floor((decimal)(spriteSheet.Height / frameHeight));
            frames = new Rectangle[numAnimations, numFrames];
            for (int r = 0; r < frames.GetLength(0); r++)
            {
                for (int c = 0; c < frames.GetLength(1); c++)
                {
                    frames[r, c] = new Rectangle(frameWidth * r, frameHeight * c, frameWidth, frameHeight);
                }
            }
        }

        /// <summary>
        /// Calculates the center of the sprite
        /// </summary>
        /// <returns>The center of the sprite</returns>
        public Vector2 Center()
        {
            return new Vector2(position.X + frameWidth / 2, position.Y + frameHeight / 2);
        }

        /// <summary>
        /// Adds the sprite to the level map, by default adds a default block
        /// </summary>
        /// <param name="row">Row of the level map</param>
        /// <param name="col">Column of the level map</param>
        public virtual void AddToLevel(int row, int col)
        {
            Level.GetInstance().levelMap[row, col] = Level.defaultBlock.copyBlockAt(new Vector2(col * Block.WIDTH, row * Block.HEIGHT));
        }

        /// <summary>
        /// Updates the sprite's position and frame and tells the state to update
        /// </summary>
        public virtual void Update(float elapsedTime)
        {
            // keep track of the old position for dealing with collisions
            oldPosition = position;

            position += velocity * elapsedTime;

            // update frame
            frameTimer += elapsedTime;
            if (frameTimer > frameTime)
            {
                currentFrame = (currentFrame + 1) % numFrames;
                frameTimer = 0.0f;
            }

            if (state != null)
            {
                state.Update(this, elapsedTime);
            }
        }

        /// <summary>
        /// Draws the current frame of the current anumation
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to use to draw on</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
                Rectangle rec = new Rectangle(currentFrame * frameWidth, currentAnimation * frameHeight, frameWidth, frameHeight);
                Color newColor = new Color(drawColor.R, drawColor.G, drawColor.B, opacity);
                spriteBatch.Draw(spriteSheet, position, rec,
                    newColor, angle, new Vector2(frameWidth / 2, frameHeight / 2), (float)scale.getValue(), SpriteEffects.None, 0f);
        }

        /// <summary>
        /// Draws double vision of the current frame of the current anumation
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to use to draw on</param>
        public virtual void DrawDoubleVision(SpriteBatch spriteBatch)
        {
                Rectangle rec = new Rectangle(currentFrame * frameWidth, currentAnimation * frameHeight, frameWidth, frameHeight);
                Color newColor = new Color(drawColor.R, drawColor.G, drawColor.B, opacity * DoubleVisionHelper.getOpacity());
                spriteBatch.Draw(spriteSheet, position + DoubleVisionHelper.getOffset(), rec,
                    newColor, angle, new Vector2(frameWidth / 2, frameHeight / 2), (float)scale.getValue(), SpriteEffects.None, 0f);
        }
        #endregion
    }
}