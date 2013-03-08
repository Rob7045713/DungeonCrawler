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
    /// Holds information about a collision
    /// </summary>
    public class Collision
    {
        #region Variables
        internal Sprite actor;
        internal Sprite prop;
        internal Rectangle actorRectangle;
        internal Rectangle propRectangle;
        internal bool isColliding;
        internal bool isOverlapping;
        internal int[] edges = new int[4];
        #endregion

        #region Constants
        public const int NO_COLLISION = 0;
        public const int INTERNAL_COLLISION = -1;
        public const int EXTERNAL_COLLISION = 1;
        public const int RIGHT = 0;
        public const int BOTTOM = 1;
        public const int LEFT = 2;
        public const int TOP = 3;
        public const int BUFFER = 12;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a collision between two sprites
        /// </summary>
        /// <param name="actor">The sprite that cares about the collision</param>
        /// <param name="actedOn">The sprite the actor is interacting with</param>
        public Collision(Sprite actor, Sprite prop)
        {
            this.actor = actor;
            this.prop = prop;
            float actorWidth = (float)(actor.frameWidth * actor.scale.getValue());
            float actorHeight = (float)(actor.frameHeight * actor.scale.getValue());
            float propWidth = (float)(prop.frameWidth * prop.scale.getValue());
            float propHeight = (float)(prop.frameHeight * prop.scale.getValue());
            actorRectangle = new Rectangle((int)(actor.position.X - actorWidth / 2), (int)(actor.position.Y - actorHeight / 2), (int)actorWidth, (int)actorHeight);
            propRectangle = new Rectangle((int)(prop.position.X - propWidth / 2), (int)(prop.position.Y - propHeight / 2), (int)propWidth, (int)propHeight);
            isColliding = false;
            isOverlapping = false;
            DoCollision();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Constructs all of the important information about the collision
        /// </summary>
        private void DoCollision()
        {
            if (actorRectangle.Intersects(propRectangle))
            {
                isColliding = true;
            }
            if (isColliding)
            {
                Point propCenter = new Point((int)prop.position.X, (int)prop.position.Y);
                if (actorRectangle.Contains(propCenter))
                {
                    isOverlapping = true;
                }

                CheckEdges();

                PickOneExternal();
            }
        }

        /// <summary>
        /// Checks all the edges for the collision
        /// </summary>
        private void CheckEdges()
        {
            if (propRectangle.Right > actorRectangle.Right && propRectangle.Right < actorRectangle.Right + BUFFER && prop.velocity.X > 0)
            {
                edges[RIGHT] = INTERNAL_COLLISION;
            }
            else if (propRectangle.Left < actorRectangle.Right && propRectangle.Left > actorRectangle.Right - BUFFER && prop.velocity.X < 0)
            {
                edges[RIGHT] = EXTERNAL_COLLISION;
            }

            if (propRectangle.Left < actorRectangle.Left && propRectangle.Left > actorRectangle.Left - BUFFER && prop.velocity.X < 0)
            {
                edges[LEFT] = INTERNAL_COLLISION;
            }
            else if (propRectangle.Right > actorRectangle.Left && propRectangle.Right < actorRectangle.Left + BUFFER && prop.velocity.X > 0)
            {
                edges[LEFT] = EXTERNAL_COLLISION;
            }

            if (propRectangle.Top < actorRectangle.Top && propRectangle.Top > actorRectangle.Top - BUFFER && prop.velocity.Y < 0)
            {
                edges[TOP] = INTERNAL_COLLISION;
            }
            else if (propRectangle.Bottom > actorRectangle.Top && propRectangle.Bottom < actorRectangle.Top + BUFFER && prop.velocity.Y > 0)
            {
                edges[TOP] = EXTERNAL_COLLISION;
            }

            if (propRectangle.Bottom > actorRectangle.Bottom && propRectangle.Bottom < actorRectangle.Bottom + BUFFER && prop.velocity.Y > 0)
            {
                edges[BOTTOM] = INTERNAL_COLLISION;
            }
            else if (propRectangle.Top < actorRectangle.Bottom && propRectangle.Top > actorRectangle.Bottom - BUFFER && prop.velocity.Y < 0)
            {
                edges[BOTTOM] = EXTERNAL_COLLISION;
            }
        }

        /// <summary>
        /// Checks to see if the collision is internal or external
        /// </summary>
        private void PickOneExternal()
        {
            if(edges.Sum() > 1)
            {
                if (Math.Abs(actorRectangle.Center.X - propRectangle.Center.X)
                    > Math.Abs(actorRectangle.Center.Y - propRectangle.Center.Y))
                {
                    edges[TOP] = NO_COLLISION;
                    edges[BOTTOM] = NO_COLLISION;
                }
                else if (Math.Abs(actorRectangle.Center.X - propRectangle.Center.X)
                    > Math.Abs(actorRectangle.Center.Y - propRectangle.Center.Y))
                {
                    edges[LEFT] = NO_COLLISION;
                    edges[RIGHT] = NO_COLLISION;
                }
                else
                {
                    edges[LEFT] = NO_COLLISION;
                    edges[RIGHT] = NO_COLLISION;
                    edges[TOP] = NO_COLLISION;
                    edges[BOTTOM] = NO_COLLISION;
                }
            }
        }
        #endregion
    }
}