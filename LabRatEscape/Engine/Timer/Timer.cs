// Copyright 12.13.2010
// Rob Argue

#region Imports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace LabRatEscape
{
    public class Timer
    {
        #region Variables
        internal float time;
        internal float currentTime;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new timer
        /// </summary>
        /// <param name="time">Time in seconds to start the timer at</param>
        public Timer(float time)
        {
            this.time = time;
            currentTime = time;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Updates the timer
        /// </summary>
        /// <param name="elapsedTime">Time in seconds since the last update</param>
        public void Update(float elapsedTime)
        {
            currentTime -= elapsedTime;
        }

        /// <summary>
        /// Checks if the time has elapsed
        /// </summary>
        /// <returns>True if the time has elapsed</returns>
        public bool HasElapsed()
        {
            return currentTime < 0;
        }

        /// <summary>
        /// Resets the timer
        /// </summary>
        public void Reset()
        {
            currentTime = time;
        }
        #endregion
    }
}
