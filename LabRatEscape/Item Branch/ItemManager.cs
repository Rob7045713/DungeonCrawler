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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
#endregion

namespace LabRatEscape
{
    /// <summary>
    /// ItemManager class that controls the level items and inventories given
    /// A singleton class with a private constructor
    /// </summary>
    public class ItemManager
    {
        #region Variables
        private static ItemManager uniqueInstance;
        public static Dictionary<String, Item> possibleItems = new Dictionary<string,Item>();
        //in case we wanted more than 1 player
        public static Inventory[] inventories = new Inventory[PlayerManager.MAX_PLAYERS];
        public static List<Item> levelItems = new List<Item>();
        #endregion

        #region Constructors
        /// <summary>
        /// private empty constructor
        /// </summary>
        private ItemManager()
        {

        }
        #endregion

        #region Getters/Setters
        /// <summary>
        /// grabs the unique instance of ItemManager
        /// </summary>
        public static ItemManager GetInstance()
        {
            if (uniqueInstance == null)
            {
                uniqueInstance = new ItemManager();
            }
            return uniqueInstance;
        }
        #endregion

        #region Methods

        /// <summary>
        /// resets all level items and inventories
        /// </summary>
        public static void Reset()
        {
            foreach (Inventory inventory in inventories)
            {
                inventory.Reset();
            }
            levelItems.Clear();
        }

        /// <summary>
        /// reverts all inventories to saved inventories
        /// </summary>
        public static void RevertInventories()
        {
            foreach (Inventory inventory in inventories)
            {
                inventory.Revert();
            }
            levelItems.Clear();
        }

        /// <summary>
        /// saves all inventories
        /// </summary>
        public static void SaveInventories()
        {
            foreach (Inventory inventory in inventories)
            {
                inventory.Save();
            }
        }

        /// <summary>
        /// updates the ItemManager
        /// </summary>
        public static void Update(float elapsedTime)
        {
        }

        /// <summary>
        /// Draws all the items in the level to the screen with double vision
        /// </summary>
        /// <param name="batch">SpriteBatch to draw items to</param>
        public static void Draw(SpriteBatch batch)
        {
            foreach (Item it in levelItems)
            {
                it.Draw(batch);
            }
            foreach (Item it in levelItems)
            {
                it.DrawDoubleVision(batch);
            }
        }

        /// <summary>
        /// Removes the item from level items and applies it to the sprite
        /// </summary>
        /// <param name="item">Item being collected</param>
        /// <param name="sprite">Sprite that is collecting the item</param>
        public static void Collect(Sprite sprite, Item item)
        {
            if (sprite is Player)
            {
                SoundManager.PlayEffect("itemCollected");
                if (item.getIsCollectable())
                {
                    inventories[(sprite as Player).playerNumber].Add(item);
                }
                else
                {
                    item.OnUse(sprite);
                }
            }
            levelItems.Remove(item);

        }

        /// <summary>
        /// Clears out all unkept items
        /// </summary>
        public static void Clean()
        {
            levelItems.Clear();
            foreach (Inventory inventory in inventories)
            {
                inventory.DiscardUnkept();
            }
        }
        #endregion
    }   
}
