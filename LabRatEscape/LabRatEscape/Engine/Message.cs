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
    /// Utility class used for writing a message to the screen
    /// </summary>
    public class Message
    {
        #region Variables
        String message;
        Vector2 position;
        SpriteFont font;
        #endregion

        #region Constructors
        /// <summary>
        /// main constructor that takes the message you want to be displayed, vector position, and font to be used
        /// </summary>
        public Message(String message, Vector2 position, SpriteFont font)
        {
            this.message = message;
            this.position = position;
            this.font = font;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Sets the message
        /// </summary>
        /// <param name="message">new message to be set to</param>
        public void setMessage(String message)
        {
            this.message = message;
        }

        /// <summary>
        /// Sets the position
        /// </summary>
        /// <param name="position">vector2 position to be set to</param>
        public void setPosition(Vector2 position)
        {
            this.position = position;
        }

        /// <summary>
        /// draws the message to the screen using the SpriteBatch.DrawString method
        /// </summary>
        /// <param name="batch">SpriteBatch to be drawn to</param>
        public void Draw(SpriteBatch batch)
        {
            batch.DrawString(font, message, position, Color.White);
        }
        #endregion
    }
}
