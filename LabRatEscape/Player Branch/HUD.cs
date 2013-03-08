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
    /// HUD display for the screen - basic UI with timerDisplay, scoreDisplay, and inventory display
    /// </summary>
    public class HUD
    {
        #region Variables
        // the timer display and the current score display messages
        Message timerDisplay;
        Message scoreDisplay;
        Message message;
        Inventory inventory = new Inventory(null, null, null, 0, new Vector2(0,0));
        SpriteFont font;
        SpriteFont smallFont;
        Vector2 screenSize;
        internal Sprite overlay;
        private static HUD uniqueInstance = new HUD();
        #endregion

        #region Constructors
        /// <summary>
        /// main constructor
        /// </summary>
        /// <param name="font">SpriteFont for the Messages to use</param>
        /// <param name="inventory">inventory to be drawn</param>
        /// <param name="scoreXY">scoreDisplay position vector</param>
        /// <param name="screenSize">current screen size vector</param>
        /// <param name="timerXY">timerDisplay position vector</param>
        private HUD(Vector2 timerXY, Vector2 scoreXY, SpriteFont font, Inventory inventory, Vector2 screenSize)
        {
            this.inventory = inventory;
            this.font = font;
            this.screenSize = screenSize;
            timerDisplay = new Message("Current Time: ", timerXY, font, Color.White);
            scoreDisplay = new Message("Score: ", scoreXY, font, Color.White);
        }

        /// <summary>
        /// empty constructor
        /// </summary>
        private HUD()
        {
        }
        #endregion

        #region Getters/Setters
        public void setTimerData(Vector2 position, SpriteFont font)
        {
            timerDisplay = new Message("Current Time: ", position, font, Color.White);
        }

        public void setScoreData(Vector2 position, SpriteFont font)
        {
            scoreDisplay = new Message("Score: ", position, font, Color.White);
        }

        public void setMessage(String message)
        {
            this.message = new Message(message,
                new Vector2(screenSize.X / 2 - font.MeasureString(message).X/2, 0), font, Color.White);
        }

        public void setScreenSize(Vector2 screenSize)
        {
            this.screenSize = screenSize;
        }

        public void setInventory(Inventory inventory)
        {
            this.inventory = inventory;
        }

        public void setFont(SpriteFont font)
        {
            this.font = font;
        }

        public void setSmallFont(SpriteFont font)
        {
            this.smallFont = font;
        }

        public static HUD GetInstance()
        {
            if (uniqueInstance == null)
            {
                uniqueInstance = new HUD();
            }
            return uniqueInstance;
        }
        #endregion

        #region Methods

        /// <summary>
        /// updates the messages
        /// </summary>
        /// <param name="elapsedTime">current elapsed game time</param>
        public void Update(float elapsedTime)
        {
            timerDisplay.setMessage("Current Time: " + LREGame.time);
            //int updatedScore = 0;
            decimal updatedScore = PlayerManager.players[0].score.getValue();
            scoreDisplay.setMessage("Score: " + updatedScore);
            HUD.GetInstance().setInventory(ItemManager.inventories[0]);
        }

        /// <summary>
        /// draws the Messages and Inventory to the screen
        /// </summary>
        /// <param name="batch">SpriteBatch to be drawn to</param>
        public void Draw(SpriteBatch batch)
        {
            overlay.opacity = (float)PlayerManager.players[0].drugIntensity.getValue();
            overlay.Draw(batch);
            //timerDisplay.Draw(batch);
            //scoreDisplay.Draw(batch);
            if (message != null)
            {
                message.Draw(batch);
            }
            inventory.Draw(batch, smallFont);
        }
        #endregion

    }
}
