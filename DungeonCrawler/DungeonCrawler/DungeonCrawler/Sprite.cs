using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DungeonCrawler
{
    /// <summary>
    /// Sprite class which can handle various effects and flipbook animation
    /// </summary>
    public class Sprite
    {
        #region Variables
        internal Texture2D texture;
        
        internal Vector2 position;
        internal Vector2 velocity;
        internal Vector2 acceleration;
        
        internal float angle;

        internal SpriteEffects spriteEffect = SpriteEffects.None;

        internal float scale;
        
        internal float opacity = 1f;
        internal Color drawColor = Color.White;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a basic sprite
        /// </summary>
        /// <param name="texture">Texture to use for the sprite</param>
        /// <param name="position">Initial position of the sprite</param>
        public Sprite(Texture2D texture)
        {
            this.texture = texture;

            this.position = new Vector2(0, 0);
            this.velocity = new Vector2(0, 0);
            this.acceleration = new Vector2(0, 0);

            this.angle = 0;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Calculates the center of the sprite
        /// </summary>
        /// <returns>The center of the sprite</returns>
        public Vector2 Center()
        {
            return new Vector2(position.X + texture.Width / 2, position.Y + texture.Height / 2);
        }

        /// <summary>
        /// Updates the sprite's position and frame and tells the state to update
        /// </summary>
        public virtual void Update(float elapsedTime)
        {
            velocity += acceleration * elapsedTime;
            position += velocity * elapsedTime;
        }

        /// <summary>
        /// Draws the current frame of the current anumation
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to use to draw on</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
            Rectangle source = new Rectangle(0, 0, texture.Width, texture.Height);
            Color newColor = new Color(drawColor.R, drawColor.G, drawColor.B, opacity);
            spriteBatch.Draw(texture, position, source, newColor, angle, origin, scale, SpriteEffects.None, 0f);
        }
        #endregion
    }
}
