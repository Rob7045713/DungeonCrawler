// Copyright 12.13.2010
// Rob Argue
// David Edge
// Leanna Helton
// Joe Kiernen

#region Imports
using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Reflection;
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
    /// <summary>
    /// A single player in the game
    /// </summary>
    public class Player : Sprite
    {
        #region Variables
        GameValue VertVel = new GameValue(0, -1, 1);
        GameValue HorizVel = new GameValue(0, -1, 1);
        public GameValue drugIntensity = new GameValue(0, 0, 1);
        float timeSinceLastAction = 0;
        public GameValue score = new GameValue(0, 0, Decimal.MaxValue);
        internal int playerNumber;
        internal Condition deathCondition;
        private decimal oldDrugIntensity = 0;
        private HUD hud = HUD.GetInstance();
        internal decimal savedDrugIntensity;
        internal decimal savedSpeed;
        internal decimal savedScale;
        #endregion

        #region Constants
        public const int WANDER_CYCLE_TIME = 5000;
        public const float MAX_PARALLEL_WANDER = .25f;
        public const float MAX_PERPENDICULAR_WANDER = .66f;
        public const double WANDER_A = 14;
        public const double WANDER_B = 9;
        public const double WANDER_C = 13;
        public const double TIME_BETWEEN_ACTIONS = .15;
        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for a new player
        /// </summary>
        /// <param name="spriteSheet">Sprite sheet to use for the player</param>
        /// <param name="frameWidth">Width of an individual frame on the sprite sheet</param>
        /// <param name="frameHeight">Height of an individual frame on the sprite sheet</param>
        /// <param name="position">Initial position of the player</param>
        /// <param name="playerNumber">Player's number</param>
        public Player(Texture2D spriteSheet, int frameWidth, int frameHeight, Vector2 position, int playerNumber)
            : base(spriteSheet, frameWidth, frameHeight, DEFAULT_FRAME_TIME, position)
        {
            this.playerNumber = playerNumber;
            deathCondition = new Condition(drugIntensity, drugIntensity.GetType().GetMethod("IsMax"), new object[0]);
            SetUpInput();
            TimerManager.RegisterTimer("Can Act", .25f);
            Save();
        }
        #endregion

        #region Methods

        /// <summary>
        /// Sets the HUD message for the player
        /// </summary>
        /// <param name="message">Message to display</param>
        public void setMessage(string message)
        {
            hud.setMessage(message);
        }

        /// <summary>
        /// Saves the state of the player
        /// </summary>
        public void Save()
        {
            savedDrugIntensity = drugIntensity.getValue();
            savedSpeed = speed.getValue();
            savedScale = scale.getValue();
        }

        /// <summary>
        /// Reverts to the saved state
        /// </summary>
        public void Revert()
        {
            drugIntensity.setValue(savedDrugIntensity);
            speed.setValue(savedSpeed);
            scale.setValue(savedScale);
        }

        /// <summary>
        /// Resets the player
        /// </summary>
        public void Reset()
        {
            drugIntensity.Reset();
            speed.Reset();
            scale.Reset();
        }

        /// <summary>
        /// Checks if the player is drugges
        /// </summary>
        /// <returns>True if the player is drugged</returns>
        public bool isDrugged()
        {
            return drugIntensity.getValue() > 0;
        }

        /// <summary>
        /// Calculates how to change the player's velocity based on their drug intensity
        /// </summary>
        public Vector2 CalculateWander()
        {
            // parametric equations for some sort of polar rose
            double t = (double)(LREGame.time % (int)(WANDER_CYCLE_TIME * WANDER_C)) / (double)(WANDER_CYCLE_TIME * WANDER_C) * (WANDER_C * 2 * Math.PI);
            float x = (float)(Math.Sin((WANDER_A / WANDER_C) * t) * Math.Cos(t));
            float y = (float)(Math.Sin((WANDER_B / WANDER_C) * t) * Math.Sin(t));
            return new Vector2(x, y);
        }

        /// <summary>
        /// Calculates the velocity of the player based on player imput and wander
        /// </summary>
        public virtual void CalculateVelocity()
        {
            // get player input
            Vector2 playerDirection = new Vector2((float)HorizVel.getValue(), (float)VertVel.getValue());
            // normalize player input
            playerDirection = VectorHelper.Normalize(playerDirection);
            // get the pull vector (direction and magnitude determined solely by the pull function)
            Vector2 pull = CalculateWander();
            // scale the pull
            // if the player is not moving scale to max perpendicular
            if (playerDirection == new Vector2(0, 0))
            {
                pull *= MAX_PARALLEL_WANDER;
            }
            // if the player is moving break the pull vector into components perpendicular and parallel to the player's direction
            else
            {
                Vector2 pullParallel = VectorHelper.Project(pull, playerDirection);
                Vector2 pullPerpendicular = pull - pullParallel;
                // scale the components of pull to their respective maxes
                pullParallel *= MAX_PARALLEL_WANDER;
                pullPerpendicular *= MAX_PERPENDICULAR_WANDER;
                // combine components to create a scaled pull vector
                pull = pullParallel + pullPerpendicular;
            }
            // scale pull to drug intensity
            pull *= (float)drugIntensity.getValue();
            // combine pull with player input
            velocity = pull + playerDirection;
            // scale to speed
            velocity *= (float)speed.getValue();

            if(velocity != new Vector2(0,0))
            {
                angle = (float)Math.Atan2(velocity.Y, velocity.X);
            }
    
            HorizVel.Reset();
            VertVel.Reset();
        }

        /// <summary>
        /// Drugs the player, currently being used for development
        /// </summary>
        public void Drug(decimal amount)
        {
            if (TimerManager.timers["Can Act"].HasElapsed())
            {
                drugIntensity.Add(amount);
               TimerManager.timers["Can Act"].Reset();
            }
        }

        /// <summary>
        /// Action for user input of up
        /// </summary>
        public void Up()
        {
            VertVel.Add(-1);
        }

        /// <summary>
        /// Action for user input of down
        /// </summary>
        public virtual void Down()
        {
            VertVel.Add(1);
        }

        /// <summary>
        /// Action for user input of left
        /// </summary>
        public virtual void Left()
        {
            HorizVel.Add(-1);
        }

        /// <summary>
        /// Action for user input of right
        /// </summary>
        public virtual void Right()
        {
            HorizVel.Add(1);
        }

        /// <summary>
        /// Action for user input of use item
        /// </summary>
        public void Use()
        {
            if (TimerManager.timers["Can Act"].HasElapsed())
            {
                ItemManager.inventories[playerNumber].Use();
                TimerManager.timers["Can Act"].Reset();
            }
        }

        /// <summary>
        /// Action for user input of use of a specific item
        /// </summary>
        /// <param name="index">Index of the item to use</param>
        public void Use(int index)
        {
            if (TimerManager.timers["Can Act"].HasElapsed())
            {
                ItemManager.inventories[playerNumber].Use(index);
                TimerManager.timers["Can Act"].Reset();
            }
        }

        /// <summary>
        /// Action for user input of switching to the next item in the inventory
        /// </summary>
        public void NextItem()
        {
            if (TimerManager.timers["Can Act"].HasElapsed())
            {
                ItemManager.inventories[playerNumber].Next();
                TimerManager.timers["Can Act"].Reset();
            }
        }

        /// <summary>
        /// Action for user input of switching to the previous item in the inventory
        /// </summary>
        public void PrevItem()
        {
            if (TimerManager.timers["Can Act"].HasElapsed())
            {
                ItemManager.inventories[playerNumber].Previous();
                TimerManager.timers["Can Act"].Reset();
            }
        }

        /// <summary>
        /// Sets up the input for the player
        /// </summary>
        public void SetUpInput()
        {
            GameAction up = new GameAction(
              this,
              this.GetType().GetMethod("Up"),
              new object[0]);
            InputManager.AddToKeyboardMap(Keys.Up, up);
            InputManager.AddToKeyboardMap(Keys.W, up);

            GameAction down = new GameAction(
              this,
              this.GetType().GetMethod("Down"),
              new object[0]);
            InputManager.AddToKeyboardMap(Keys.Down, down);
            InputManager.AddToKeyboardMap(Keys.S, down);

            GameAction left = new GameAction(
              this,
              this.GetType().GetMethod("Left"),
              new object[0]);
            InputManager.AddToKeyboardMap(Keys.Left, left);
            InputManager.AddToKeyboardMap(Keys.A, left);

            GameAction right = new GameAction(
              this,
              this.GetType().GetMethod("Right"),
              new object[0]);
            InputManager.AddToKeyboardMap(Keys.Right, right);
            InputManager.AddToKeyboardMap(Keys.D, right);

            object[] plusDrugParameters = { 0.1m };
            GameAction plusDrug = new GameAction(
              this,
              this.GetType().GetMethod("Drug"),
              plusDrugParameters);
            InputManager.AddToKeyboardMap(Keys.P, plusDrug);

            object[] minusDrugParameters = { -0.1m };
            GameAction minusDrug = new GameAction(
              this,
              this.GetType().GetMethod("Drug"),
              minusDrugParameters);
            InputManager.AddToKeyboardMap(Keys.O, minusDrug);

            GameAction use = new GameAction(
                this,
                this.GetType().GetMethod("Use", new Type[0]),
                new object[0]);
            InputManager.AddToKeyboardMap(Keys.Space, use);
            InputManager.AddToKeyboardMap(Keys.NumPad0, use);

            GameAction nextItem = new GameAction(
                this,
                this.GetType().GetMethod("NextItem"),
                new object[0]);
            InputManager.AddToKeyboardMap(Keys.E, nextItem);

            GameAction prevItem = new GameAction(
                this,
                this.GetType().GetMethod("PrevItem"),
                 new object[0]);
            InputManager.AddToKeyboardMap(Keys.Q, prevItem);

            // there has GOT to be a better way to do this
            // also, the number 1 doesnt work for some reason
            Keys[] numKeys = { Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6,
                                 Keys.D7, Keys.D8, Keys.D9 };
            Keys[] numPadKeys = { Keys.NumPad1, Keys.NumPad2, Keys.NumPad3, Keys.NumPad4,
                                    Keys.NumPad5, Keys.NumPad6, Keys.NumPad7, Keys.NumPad8,
                                    Keys.NumPad9 };
            for (int i = 0; i < Math.Min(9, Inventory.NUM_SLOTS); i++)
            {
                object[] parameters = { i };
                Type[] types = { i.GetType() };
                GameAction select = new GameAction(
                    this,
                    this.GetType().GetMethod("Use",types),
                    parameters);
                InputManager.AddToKeyboardMap(numKeys[i], select);
                InputManager.AddToKeyboardMap(numPadKeys[i], select);
            }

        }

        /// <summary>
        /// Updates the player
        /// </summary>
        /// <param name="elapsedTime">Time in seconds since the last update</param>
        public override void Update(float elapsedTime)
        {
            timeSinceLastAction += elapsedTime;
            CalculateVelocity();
            if (velocity == new Vector2(0, 0))
            {
                frameTimer -= elapsedTime;
            }

            if (oldDrugIntensity < drugIntensity.getValue())
            {
                drawColor = Color.Red;
                SoundManager.PlayEffect("playerHit");
            }
            else
            {
                drawColor = Color.White;
            }
            oldDrugIntensity = drugIntensity.getValue();
            base.Update(elapsedTime);
        }

        /// <summary>
        /// Draws the player with double vision
        /// </summary>
        /// <param name="spriteBatch">Sprite batch to draw on</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.DrawDoubleVision(spriteBatch);
            base.Draw(spriteBatch);
        }
        #endregion
    }
}
