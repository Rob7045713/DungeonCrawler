// Copyright 12.13.2010
// Rob Argue

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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
#endregion

namespace LabRatEscape
{
    public class TimerManager
    {
        #region Variables
        public static Dictionary<string, Timer> timers = new Dictionary<string, Timer>();
        #endregion

        #region Methods
        /// <summary>
        /// Registers a new timer
        /// </summary>
        /// <param name="id">Id to register</param>
        public static void RegisterTimer(string id, float time)
        {
            if (!timers.ContainsKey(id))
            {
                timers.Add(id, new Timer(time));
            }
        }

        public static void Reset()
        {
            foreach (KeyValuePair<string, Timer> timer in timers)
            {
                timer.Value.Reset();
            }
        }

        /// <summary>
        /// Updates each timer
        /// </summary>
        /// <param name="elapsedTime">Time in seconds since last update</param>
        public static void Update(float elapsedTime)
        {
            foreach (KeyValuePair<string, Timer> timer in timers)
            {
                timer.Value.Update(elapsedTime);
            }
        }
        #endregion

    }
}
