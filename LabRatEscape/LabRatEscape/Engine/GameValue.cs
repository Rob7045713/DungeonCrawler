#region Imports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace LabRatEscape
{
    /// <summary>
    /// Maintains a decimal value with an initial value, a maximum value and a minimum value
    /// and allows for adding, multiplying, setting and resetting that value
    /// </summary>
    public class GameValue
    {
        #region Variables
        decimal minValue;
        decimal maxValue;
        decimal value;
        decimal initialValue;
        decimal step;
        #endregion
        #region Constants
        private static decimal NO_STEP = 0;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a GameValue
        /// </summary>
        /// <param name="initialValue">The base value which the value starts at and resets to</param>
        /// <param name="minValue">The smallest allowed value</param>
        /// <param name="maxValue">The largest allowed value</param>
        public GameValue(decimal initialValue, decimal minValue, decimal maxValue)
            : this(initialValue, minValue, maxValue, NO_STEP){}

        /// <summary>
        /// Creates a GameValue with a step
        /// </summary>
        /// <param name="initialValue">The base value which the value starts at and resets to</param>
        /// <param name="minValue">The smallest allowed value</param>
        /// <param name="maxValue">The largest allowed value</param>
        /// <param name="step">The step of allowed values (value is restriced to 0 +/- n * step)</param>
        public GameValue(decimal initialValue, decimal minValue, decimal maxValue, decimal step)
        {
            this.initialValue = initialValue;
            this.value = initialValue;
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.step = step;
        }
        #endregion

        #region Getters/Setters
        /// <summary>
        /// gets the current value
        /// </summary>
        public decimal getValue()
        {
            return value;
        }

        /// <summary>
        /// sets the value
        /// </summary>
        /// <param name="value">sets the new decimal value</param>
        public void setValue(decimal value)
        {
            this.value = value;
            ApplyRestrictions();
        }
        #endregion

        #region Methods

        /// <summary>
        /// Adds to the value
        /// </summary>
        /// <param name="value">The amount to add (or subtract if negative)</param>
        public void Add(decimal value)
        {
            this.value += value;
            ApplyRestrictions();
        }

        /// <summary>
        /// Multiplies the value
        /// </summary>
        /// <param name="value">The amount to multiply by (or by if < 1)</param>
        public void Multiply(decimal value)
        {
            this.value *= value;
            ApplyRestrictions();
        }

        /// <summary>
        /// Cuts off the value at the max and min, and rounds it to the nearest step if a step was specified
        /// </summary>
        private void ApplyRestrictions()
        {
            if (value < minValue)
            {
                value = minValue;
            }

            if (value > maxValue)
            {
                value = maxValue;
            }

            if (step != NO_STEP)
            {
                value = Math.Round(value / step) * step;
            }
        }

        /// <summary>
        /// Sets the value to its initial value
        /// </summary>
        public void Reset()
        {
            value = initialValue;
        }

        /// <summary>
        /// Checks if the value is at its maximum
        /// </summary>
        /// <returns>True if the value is at its maximum</returns>
        public bool IsMax()
        {
            return value == maxValue;
        }

        /// <summary>
        /// Checks if the value is at its minimum
        /// </summary>
        /// <returns>True if the value is at its minimum</returns>
        public bool IsMin()
        {
            return value == minValue;
        }
        #endregion
    }
}

