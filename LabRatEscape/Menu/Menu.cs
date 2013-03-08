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
    /// A class for creating a menu screen
    /// </summary>
    public class Menu : Sprite
    {
        #region Variables
        internal SpriteFont font;
        internal List<MenuItem> menuItems;
        internal string title;
        internal string message;
        internal Vector2 screenSize;
        internal Color selected;
        internal Color unselected;
        internal Message displayMessage;
        internal int currentItemMenu;
        float timeSinceLastAction = 0;
        internal Vector2 menuItemStartPosition;
        public const double TIME_BETWEEN_ACTIONS = .15;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor for a new menu
        /// </summary>
        /// <param name="background">Background image to use</param>
        /// <param name="screenSize">Size of the screen</param>
        /// <param name="font">SpriteFont to use</param>
        /// <param name="title">Title of the menu</param>
        /// <param name="menuItems">Items to populate the menu with</param>
        /// <param name="selected">Color of the selected item</param>
        /// <param name="unselected">Color of an unselected item</param>
        public Menu(Texture2D background, Vector2 screenSize, SpriteFont font, string title, string message, List<MenuItem> menuItems,
            Color selected, Color unselected)
            : base(background, screenSize / 2)
        {
            this.screenSize = screenSize;
            this.font = font;
            this.title = title;
            this.message = message;
            this.menuItems = menuItems;
            this.selected = selected;
            this.unselected = unselected;
            decimal scaleX = ((decimal)(screenSize.X / background.Width));
            decimal scaleY = ((decimal)(screenSize.Y / background.Height));
            scale.setValue(Math.Max(scaleX, scaleY));
            currentItemMenu = 0;
            menuItemStartPosition = new Vector2(screenSize.X/2, screenSize.Y/2);
            displayMessage = new Message(title, menuItemStartPosition,font,Color.White);
        }
        #endregion

        #region Methods

        /// <summary>
        /// Sets up the menu
        /// </summary>
        public void setUp()
        {
            Vector2 position = menuItemStartPosition;
            foreach (MenuItem menuItem in menuItems)
            {
                position.Y += font.MeasureString("M").Y;
                menuItem.setMessage(position, font, unselected);
            }
            menuItems[0].changeIsSelected();
            GameAction next = new GameAction(
                this,
                this.GetType().GetMethod("Next"),
                 new object[0]);
            InputManager.AddToKeyboardMap(Keys.Down, next);
            InputManager.AddToKeyboardMap(Keys.E, next);

            GameAction previous = new GameAction(
                 this,
                 this.GetType().GetMethod("Previous"),
                 new object[0]);
            InputManager.AddToKeyboardMap(Keys.Up, previous);
            InputManager.AddToKeyboardMap(Keys.Q, previous);
        }

        /// <summary>
        /// lets you specify a starting position for your menu items
        /// </summary>
        /// <param name="position"></param>
        public void setMenuItemStartingPosition(Vector2 position)
        {
            menuItemStartPosition = position;
        }

        /// <summary>
        /// Determines if enough time has passed since to the last keystroke to register another keystroke
        /// </summary>
        /// <returns>True if enough time has passed since the last keystroke</returns>
        public bool CanAct()
        {
            return timeSinceLastAction > TIME_BETWEEN_ACTIONS;
        }

        /// <summary>
        /// Resets the time since the last keystroke
        /// </summary>
        public void Acted()
        {
            timeSinceLastAction = 0;
        }

        /// <summary>
        /// go to the next menu item
        /// </summary>
        public void Next()
        {
            if (CanAct())
            {
                //change the current selected item to be unselected
                currentItemMenu++;
                currentItemMenu %= menuItems.Count();
                //change new item to be selected
                Acted();
            }

        }

        /// <summary>
        /// go to the previous menu item
        /// </summary>
        public void Previous()
        {
            if (CanAct())
            {
                //change the current selected item to be unselected
                currentItemMenu--;
                currentItemMenu += menuItems.Count();
                currentItemMenu %= menuItems.Count();
                //change new item to be selected
                Acted();
            }
        }

        /// <summary>
        /// Resets the menu
        /// </summary>
        public void Reset()
        {
            currentItemMenu = 0;
           // setUp();
        }

        /// <summary>
        /// Selects the current item
        /// </summary>
        public void Select()
        {
            menuItems[currentItemMenu].Select();
        }

        /// <summary>
        /// Update the Menu
        /// </summary>
        /// <param name="elapsedTime">current elapsed time</param>
        public void Update(float elapsedTime)
        {
            timeSinceLastAction += elapsedTime;
            base.Update(elapsedTime);
            foreach (MenuItem menuItem in menuItems)
            {
                menuItem.Update();
            }
        }

        /// <summary>
        /// Draws the menu and such
        /// </summary>
        /// <param name="batch">batch to draw to</param>
        public void Draw(SpriteBatch batch)
        {
            base.Draw(batch);
            //displayMessage.Draw(batch);
            for(int i = 0; i < menuItems.Count(); i++)
            {
                MenuItem menuItem = menuItems.ElementAt(i);
                if (i == currentItemMenu)
                {
                    menuItem.Draw(batch, selected);
                }
                else
                {
                    menuItem.Draw(batch, unselected);
                }
            }
        }
        #endregion
    }
}
