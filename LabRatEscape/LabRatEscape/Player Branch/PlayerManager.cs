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
    /// Singleton which manages all of the players in the game
    /// </summary>
    class PlayerManager
    {
        #region Variables
        private static PlayerManager uniqueInstance;
        public static Player[] players = new Player[MAX_PLAYERS];
        public static int numPlayers = 0;
        #endregion

        #region Constants
        public const int MAX_PLAYERS = 1;
        #endregion

        #region Constructors

        /// <summary>
        /// Empty private constructor
        /// </summary>
        private PlayerManager()
        {
        }
        #endregion

        #region Getters/Setters

        /// <summary>
        /// Gets the unique instance of the player manager
        /// </summary>
        /// <returns>Unique instance of the player manager</returns>
        public static PlayerManager GetInstance()
        {
            if (uniqueInstance == null)
            {
                uniqueInstance = new PlayerManager();
            }
            return uniqueInstance;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Adds a new player to the game
        /// </summary>
        /// <param name="spriteSheet">Sprite sheet to use for the player</param>
        /// <param name="frameWidth">Width of an individual frame on the sprite sheet</param>
        /// <param name="frameHeight">Height of an individual frame on the sprite sheet</param>
        /// <param name="inventorySelected">Sprite to use for indicating the selected slot in the inverntory</param>
        /// <param name="inventoryUnselected">Sprite to use for indicating an unselected slot in the inverntory</param>
        public static void AddPlayer(Texture2D spriteSheet, int frameWidth, int frameHeight, Texture2D inventorySelected, Texture2D inventoryUnselected)
        {
            ItemManager.inventories[numPlayers] = new Inventory(inventorySelected, inventoryUnselected, numPlayers);
            players[numPlayers] = new Player(spriteSheet, frameWidth, frameHeight, new Vector2(0,0), numPlayers++);         
        }

        /// <summary>
        /// Updates all of the players
        /// </summary>
        /// <param name="elapsedTime">Time in seconds since the last update</param>
        public static void Update(float elapsedTime)
        {
            for (int i = 0; i < numPlayers; i++)
            {
                players[i].Update(elapsedTime);
            }
        }

        /// <summary>
        /// Draws the players
        /// </summary>
        /// <param name="spriteBatch">Sprite batch to draw the players on</param>
        public static void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < numPlayers; i++)
            {
                players[i].Draw(spriteBatch);
            }

        }

        public static bool AllPlayersDead()
        {
            bool b = true;
            foreach (Player player in players)
            {
                if (!player.deathCondition.CheckCondition())
                {
                    b = false;
                }
            }
            return b;
        }
        #endregion
    }
}
