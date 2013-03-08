#region Imports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace LabRatEscape
{
    /// <summary>
    /// An effect which either applies a Modifier to a GameValue or invokes a GameAction
    /// </summary>
    public class Effect
    {
        #region Variables
        private GameValue gameValue;
        private Modifier modifier;
        private GameAction gameAction;
        private string gameValueKey;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates an Effect which applies a Modifier to a GameValue
        /// </summary>
        /// <param name="gameValue">The GameValue to modify</param>
        /// <param name="Modifier">The Modifier to apply</param>
        public Effect(GameValue gameValue, Modifier modifier)
        {
            this.gameValue = gameValue;
            this.modifier = modifier;
        }

        /// <summary>
        /// Creates an effect which applies a Modifier to a sprite's GameValue
        /// </summary>
        /// <param name="gameValueKey">Key of the GameValue to modify</param>
        /// <param name="modifier">Modifier to apply</param>
        public Effect(string gameValueKey, Modifier modifier)
        {
            this.gameValueKey = gameValueKey;
            this.modifier = modifier;
        }
        
        /// <summary>
        /// Creates an Effect which invokes a GameAction
        /// </summary>
        /// <param name="gameAction">The GameAction to invoke</param>
        public Effect(GameAction gameAction)
        {
            this.gameAction = gameAction;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Applies the appropriate effect (either applies a Modifier ot a GameValue or invokes a GameAction
        /// </summary>
        public void Apply()
        {
            if (gameValue != null)
            {
                modifier.Modify(gameValue);
            }
            if (gameAction != null)
            {
                gameAction.Invoke();
            }
        }

        /// <summary>
        /// Applies the effect to a sprite
        /// </summary>
        /// <param name="sprite">Sprite to apply the effect to</param>
        public void Apply(Sprite sprite)
        {
            if (gameValueKey != null && modifier != null)
            {
                if(sprite.gameValueDictionary.ContainsKey(gameValueKey))
                {
                    modifier.Modify(sprite.gameValueDictionary[gameValueKey]);
                }
            }
            if (gameValue != null)
            {
                modifier.Modify(gameValue);
            }
            if (gameAction != null)
            {
                gameAction.Invoke(sprite);
            }
        }
        #endregion
    }
}
