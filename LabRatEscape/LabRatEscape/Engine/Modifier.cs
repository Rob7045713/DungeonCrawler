#region Imports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace LabRatEscape
{
    /// <summary>
    /// Utility for flexibly interacting with GameValues
    /// </summary>
    public class Modifier
    {
        #region Variables
        private double change;
        private int addSubtract;
        private int rangeChange;
        private double multiplier;
        private int multiplyDivide;
        private int rangeMultiplier;
        private double resetChance;
        private decimal setTo;
        private bool set = false;
        private Random random;
        #endregion
        #region Constants
        public static int ADD = 1;
        public static int SUBTRACT = -1;
        public static int ADD_OR_SUBTRACT = 0;
        public static int MULTIPLY = 1;
        public static int DIVIDE = -1;
        public static int MULTIPLY_OR_DIVIDE = 0;
        public static int DONT_RANGE = 0;
        public static int RANGE = 1;
        #endregion

        #region Constructors
        
        /// <summary>
        /// Creates a Modifier which sets a GameValue to a value
        /// </summary>
        /// <param name="setTo">Value to set a GameValue to</param>
        public Modifier(decimal setTo): this(0, 1)
        {
            this.setTo = setTo;
            this.set = true;
        }
        
        /// <summary>
        /// Creates a Modifier which can add or subtract and/or multiply or divide
        /// </summary>
        /// <param name="change">Value to add to a GameValue. Default is 0</param>
        /// <param name="multiplier">Value to multiply a GameValue by. Default is 1</param>
        public Modifier(double change, double multiplier) : this (null, change, 1, 0, multiplier, 1, 0, 0)
        {
        }

        public Modifier(string op, string value)
            : this(null, 0, 1, 0, 1, 1, 0, 0)
        {
            if(op.Equals("+"))
            {
                addSubtract = ADD;
                change = Double.Parse(value);
            }
            if (op.Equals("-"))
            {
                addSubtract = SUBTRACT;
                change = Double.Parse(value);
            }
            if (op.Equals("*"))
            {
                multiplyDivide = MULTIPLY;
                multiplier = Double.Parse(value);
            }
            if (op.Equals("*"))
            {
                multiplyDivide = MULTIPLY;
                multiplier = Double.Parse(value);
            }
        }
        
        /// <summary>
        /// Creates a Modifier capable of random effects
        /// </summary>
        /// <param name="random">Random number generator used for random effects</param>
        /// <param name="change">Value to add to a GameValue. Default is 0</param>
        /// <param name="addSubtract">Determines whether to add the change (ADD), subtract the change (SUBTRACT),
        /// or randomly pick (ADD_OR_SUBTRACT)</param>
        /// <param name="rangeChange">Determines whether or not to add/subtract the change (DONT_RANGE),
        /// or to add/subtract a random value between 0 and change (RANGE)</param>
        /// <param name="multiplier">Value to multiply a GameValue by. Default is 1</param>
        /// <param name="multiplyDivide">Determines whether to multiply by the multiplier (MULTIPLY),
        /// divide by the multiplier (DIVIDE), or randomly pick (MULTIPLY_OR_DIVIDE)</param>
        /// <param name="rangeMultiplier">Determines whether or not to multiply/divide by the multiplier (DONT_RANGE),
        /// or to multiply/divide by a random value between 1 and multiplier (RANGE)</param>
        public Modifier(Random random, double change, int addSubtract, int rangeChange, double multiplier, int multiplyDivide, int rangeMultiplier, double resetChance)
        {
            this.random = random;
            this.change = change;
            this.addSubtract = addSubtract;
            this.rangeChange = rangeChange;
            this.multiplier = multiplier;
            this.multiplyDivide = multiplyDivide;
            this.rangeMultiplier = rangeMultiplier;
            this.resetChance = resetChance;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Applies the modifier to a GameValue
        /// </summary>
        /// <param name="GameValue">GameValue to modify</param>
        public void Modify(GameValue gameValue)
        {
            if (set)
            {
                gameValue.setValue(setTo);
            }
            else
            {
                if (random != null)
                {
                    // applies the change with AddSubtract and RangeChange
                    gameValue.Add((decimal)(change * Math.Pow(random.NextDouble(), rangeChange) * Math.Sign(random.NextDouble() - .5 + addSubtract)));
                    // applies the multiplier with MultiplyDivide and RangeMultiplier
                    gameValue.Multiply((decimal)Math.Pow((multiplier - 1) * Math.Pow(random.NextDouble(), rangeMultiplier) + 1, Math.Sign(random.NextDouble() - .5 + multiplyDivide)));
                    if (random.NextDouble() < resetChance)
                    {
                        gameValue.Reset();
                    }
                }
                else
                {
                    // applies the change
                    gameValue.Add((decimal)(change * addSubtract));
                    //applies the multiplier
                    gameValue.Multiply((decimal)Math.Pow(multiplier, multiplyDivide));
                }
            }
        }
        #endregion
    }
}
