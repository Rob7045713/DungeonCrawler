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
    /// Item class handles the effects of a created item and knows how to apply those effects
    /// and store into an Inventory for later use
    /// </summary>
    public class Item : Sprite
    {
        #region Variables
        internal String id;
        internal List<Effect> effects;
        internal bool isCollectable;
        internal bool isUsable;
        internal bool isKept;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates an item with given params
        /// </summary>
        /// <param name="id">the String value corresponding to id</param>
        /// <param name="eList">the list of effects for the item</param>
        /// <param name="height">the height of the item sprite</param>
        /// <param name="isCollectable">bool representing if the item is collectable and able to add to an inventory</param>
        /// <param name="position">the Vector2 position where the item should be placed</param>
        /// <param name="spriteSheet">spriteSheet to be drawn for given item</param>
        /// <param name="width">the width of the item sprite</param>
        public Item(String id, List<Effect> eList, bool isCollectable, bool isUsable, bool isKept,
                    Texture2D spriteSheet, int width, int height, Vector2 position)
            : base(spriteSheet, width, height, Sprite.DEFAULT_FRAME_TIME, position)
        {
            this.id = id;
            if (eList == null)
            {
                eList = new List<Effect>();
            }
            this.effects = eList;
            this.isCollectable = isCollectable;
            this.isUsable = isUsable;
            this.isKept = isKept;
        }
        #endregion

        #region Getters/Setters

        /// <summary>
        /// gets the isCollectable bool variable
        /// </summary>
        public bool getIsCollectable()
        {
            return isCollectable;
        }

        /// <summary>
        /// gets the id String variable
        /// </summary>
        public String getID()
        {
            return id;
        }

        /// <summary>
        /// gets the spriteSheet Texture2D
        /// </summary>
        public Texture2D getSpriteSheet()
        {
            return spriteSheet;
        }
        #endregion

        #region Methods

        /// <summary>
        /// if the item has effects then it loops through and applies those effects
        /// </summary>
        public virtual void OnUse(Sprite sprite)
        {
            // if the item has an effect
            if (effects != null)
            {
                //use all effects given
                foreach (Effect effect in effects)
                {
                    effect.Apply(sprite);
                }
            }
        }

        /// <summary>
        /// create a copy of the item at a new given position
        /// </summary>
        /// <param name="position">new position Vector2</param>
        /// <returns>Copy of the item at the given position</returns>
        public Item CopyItemAt(Vector2 position)
        {
            return new Item(id, effects, isCollectable, isUsable, isKept, spriteSheet, frameWidth, frameHeight, position);
        }

        /// <summary>
        /// Adds the item to the item manager's level items, and calls to the base add a default block to the level map
        /// </summary>
        /// <param name="row">Row of the level map</param>
        /// <param name="col">Column of the level map</param>
        public override void AddToLevel(int row, int col)
        {
            ItemManager.levelItems.Add(CopyItemAt(new Vector2(col * Block.WIDTH, row * Block.HEIGHT)));
            base.AddToLevel(row, col);
        }
        #endregion
    }
}