// Copyright 12.13.2010
// Rob Argue
// David Edge
// Leanna Helton
// Joe Kiernen

#region Imports
using System;
using System.Collections;
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
    /// An individual item in a menu
    /// </summary>
    public class MenuItem
    {
        #region Variables
        internal string message;
        internal GameAction action;
        internal Message displayMessage;
        internal bool isSelected;
        internal Vector2 position;
        internal SpriteFont font;
        #endregion

        #region Constructors
        /// <summary>
        /// Makes a new MenuItem
        /// </summary>
        /// <param name="message">Message for the MenuItem to display</param>
        /// <param name="action">Action to perform when selected</param>
        public MenuItem(string message, GameAction action)
        {
            this.message = message;
            this.action = action;
            this.isSelected = false;

        }
        #endregion

        #region Methods
        /// <summary>
        /// Performs the MenuItem's action
        /// </summary>
        public void Select()
        {
            action.Invoke();
        }

        /// <summary>
        /// changes our bool isSelected to the opposite of what is was
        /// </summary>
        public void changeIsSelected()
        {
            if (isSelected == false)
            {
                isSelected = true;
            }
            else
            {
                isSelected = false;
            }
        }

        /// <summary>
        /// Sets the message
        /// </summary>
        /// <param name="position">position of the message</param>
        /// <param name="font">font of the message</param>
        /// <param name="selected">color of the selected item</param>
        public void setMessage(Vector2 position, SpriteFont font, Color selected)
        {
            displayMessage = new Message(message, position, font, selected);
        }

        /// <summary>
        /// updates the menu item
        /// checks to see if it is selected or not and changes its display message based on that
        /// </summary>
        public void Update()
        {

        }

        /// <summary>
        /// draws the display message to the menu
        /// </summary>
        /// <param name="batch">batch to draw the message</param>
        public void Draw(SpriteBatch batch)
        {
            displayMessage.DrawCenter(batch);
        }

        /// <summary>
        /// draws the display message to the menu with a specific color
        /// </summary>
        /// <param name="batch">batch to draw the message</param>
        /// <param name="color">color to draw with</param>
        public void Draw(SpriteBatch batch, Color color)
        {
            displayMessage.DrawCenter(batch, color);
        }

        #endregion
    }
}
