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
        internal ArrayList menuItems;
        internal string title;
        internal string message;
        internal Vector2 screenSize;
        internal Color selected;
        internal Color unselected;
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
        public Menu(Texture2D background, Vector2 screenSize, SpriteFont font, string title, string message, ArrayList menuItems,
            Color selected, Color unselected) : base(background, screenSize/2)
        {
            this.screenSize = screenSize;
            this.font = font;
            this.title = title;
            this.message = message;
            this.menuItems = menuItems;
            this.selected = selected;
            this.unselected = unselected;
        }
        #endregion



    }
}
