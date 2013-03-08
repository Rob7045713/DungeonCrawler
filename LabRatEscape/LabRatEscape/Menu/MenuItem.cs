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
        #endregion

        #region Constructors
        /// <summary>
        /// Makes a new MenuItem
        /// </summary>
        /// <param name="message">Message for the MenuItem to display</param>
        /// <param name="action">Action to perform when selected</param>
        public MenuItem(string message, GameAction action)
        {

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
        #endregion
    }
}
