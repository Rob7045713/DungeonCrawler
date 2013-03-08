// Copyright 12.13.2010
// Rob Argue
// David Edge
// Leanna Helton
// Joe Kiernen

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
    /// The engine which handles collisions, and updating and drawing the game
    /// </summary>
    public class Engine
    {
        
        #region Variables
        private static Engine uniqueInstance;
        static Vector2 camera = new Vector2(16, 16);
        EngineState engineState;
        internal Menu mainMenu;
        internal Menu pauseMenu;
        internal Menu dieMenu;
        internal Menu winLevelMenu;
        internal Menu winGameMenu;
        internal Menu cheeseMenu;
        #endregion

        #region Constants
        public const int WOBBLE_CYCLE_TIME = 4000;
        public const int MAX_WOBBLE_DISTANCE = 64;
        public const float MAX_DOUBLE_VISION_OPACITY = .66f;
        public const double WOBBLE_A = 14;
        public const double WOBBLE_B = 9;
        public const double WOBBLE_C = 13;
        public const float VIEW_MARGIN_X = .35f;
        public const float VIEW_MARGIN_Y = .35f;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of engine for the game
        /// </summary>
        private Engine()
        {
            engineState = new PlayState();
            TimerManager.RegisterTimer("Can Act", .25f);
            GameAction invoke = new GameAction(
                this,
                 this.GetType().GetMethod("Select"),
                 new object[0]);
            InputManager.AddToKeyboardMap(Keys.Enter, invoke);
            InputManager.AddToKeyboardMap(Keys.LeftShift, invoke);

        }
        #endregion

        #region Getters/Setters
        /// <summary>
        /// gets the unique instance 
        /// </summary>
        public static Engine GetInstance()
        {
            if (uniqueInstance == null)
            {
                uniqueInstance = new Engine();
            }
            return uniqueInstance;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Selects a menu item
        /// </summary>
        public void Select()
        {
            if (TimerManager.timers["Can Act"].HasElapsed())
            {
                engineState.Invoke();
                TimerManager.timers["Can Act"].Reset();
            }
            
        }

        /// <summary>
        /// Resets the game
        /// </summary>
        public static void Reset()
        {
            LREGame.time = 0;
            PlayerManager.Reset();
            ItemManager.Reset();
            EnemyManager.Reset();
            Level.Reset();
            TimerManager.Reset();
            Menu(GetInstance().mainMenu);
        }

        /// <summary>
        /// Throw a break point in here to enter debug mode when this is called
        /// </summary>
        public void Debug()
        {
            Console.WriteLine("Engine.Debug()");
        }

        /// <summary>
        /// Checks collisions and updates each piece of the game
        /// </summary>
        public static void Update(float elapsedTime)
        {
            uniqueInstance.engineState.Update(elapsedTime);
        }

        /// <summary>
        /// Checks for relevant collisions between sprites and calls the appropriate methods to act on them
        /// </summary> 
        public static void CheckCollisions()
        {
            List<Collision> collisions = new List<Collision>();
            foreach (Item item in ItemManager.levelItems)
            {
                foreach (Player player in PlayerManager.players)
                {
                    Collision c = new Collision(item, player);
                    if (c.isColliding)
                    {
                        collisions.Add(c);
                    }
                }
            }

            foreach (Collision c in collisions)
            {
                ItemManager.Collect(c.prop, c.actor as Item);
            }

            foreach (Player player in PlayerManager.players)
            {
                int playerBlockX = (int)Math.Floor(player.position.X / Block.WIDTH);
                int playerBlockY = (int)Math.Floor(player.position.Y / Block.HEIGHT);
                foreach (Block block in Level.GetInstance().MapSection(playerBlockY, playerBlockY + 1, playerBlockX, playerBlockX + 1))
                //foreach (Block block in game.getLevel().getMapSection(0, 100, 0, 100))
                {
                    Collision c = new Collision(block, player);
                    if (c.isColliding)
                    {
                        block.Collide(c);
                    }
                }
            }

            foreach (Enemy enemy in EnemyManager.enemies)
            {
                int enemyBlockX = (int)Math.Floor(enemy.position.X / Block.WIDTH);
                int enemyBlockY = (int)Math.Floor(enemy.position.Y / Block.HEIGHT);
                foreach (Block block in Level.GetInstance().MapSection(enemyBlockY, enemyBlockY + 1, enemyBlockX, enemyBlockX + 1))
                //foreach (Block block in game.getLevel().getMapSection(0, 100, 0, 100))
                {
                    Collision c = new Collision(block, enemy);
                    if (c.isColliding)
                    {
                        block.Collide(c);
                    }
                }
            }
        }

        /// <summary>
        /// Draws each piece of the game with double vision
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to draw with</param>
        public static void Draw(SpriteBatch spriteBatch)
        {
            uniqueInstance.engineState.Draw(spriteBatch);
        }

        /// <summary>
        /// Moves the camera so that the player is within the margin
        /// </summary>
        /// <param name="viewport">The viewport the camera is looking at</param>
        private static void MoveCamera(Viewport viewport)
        {
            float marginWidth = viewport.Width * VIEW_MARGIN_X;
            float marginLeft = camera.X + marginWidth;
            float marginRight = camera.X + viewport.Width - marginWidth;
            if (PlayerManager.players[0].position.X < marginLeft)
            {
                camera.X += PlayerManager.players[0].position.X - marginLeft;
            }
            else if (PlayerManager.players[0].position.X > marginRight)
            {
                camera.X += PlayerManager.players[0].position.X - marginRight;
            }
            camera.X = MathHelper.Clamp(camera.X, -Block.WIDTH / 2, Block.WIDTH * (-.5f + Level.GetInstance().levelMap.GetLength(1)) - viewport.Width);

            float marginHeight = viewport.Height * VIEW_MARGIN_Y;
            float marginTop = camera.Y + marginHeight;
            float marginBottom = camera.Y + viewport.Height - marginHeight;
            if (PlayerManager.players[0].position.Y < marginTop)
            {
                camera.Y += PlayerManager.players[0].position.Y - marginTop;
            }
            else if (PlayerManager.players[0].position.Y > marginBottom)
            {
                camera.Y += PlayerManager.players[0].position.Y - marginBottom;
            }
            camera.Y = MathHelper.Clamp(camera.Y, -Block.HEIGHT / 2, Block.HEIGHT * (-.5f + Level.GetInstance().levelMap.GetLength(0)) - viewport.Height);
        }

        /// <summary>
        /// Switches the state to a menu
        /// </summary>
        /// <param name="menu">Name of the menu to switch to</param>
        public static void Menu(string menu)
        {
            GetInstance().engineState = new MenuState(GetInstance().GetType().GetField(menu).GetValue(GetInstance()) as Menu);
        }

        /// <summary>
        /// Switches the state to a menu
        /// </summary>
        /// <param name="menu">Menu to switch to</param>
        public static void Menu(Menu menu)
        {
            menu.Reset();
            GetInstance().engineState = new MenuState(menu);
            SoundManager.PlaySong(menu.title);
        }

        /// <summary>
        /// Wins the game, with cheese!
        /// </summary>
        public static void Cheese()
        {
            Menu(GetInstance().cheeseMenu);
        }

        /// <summary>
        /// Switches the state to play
        /// </summary>
        public static void Play()
        {
            GetInstance().engineState = new PlayState();
            SoundManager.PlaySong("game");
        }
        #endregion

        #region States

        public class PlayState : EngineState
        {
            public PlayState()
            {

            }

            public void Invoke()
            {
                Engine.Menu(Engine.GetInstance().pauseMenu);
            }

            public void Update(float elapsedTime)
            {
                TimerManager.Update(elapsedTime);
                // update each branch of the game in the appropriate order
                PlayerManager.Update(elapsedTime);
                Level.GetInstance().Update(elapsedTime);
                HUD.GetInstance().Update(elapsedTime);
                EnemyManager.Update(elapsedTime);
                //game.itemManager.Update();
                CheckCollisions();
                if (PlayerManager.AllPlayersDead())
                {
                    Level.RestartLevel();
                    SoundManager.PlayEffect("playerDied");
                    Engine.Menu(Engine.GetInstance().dieMenu);
                }
            }

            public void Draw(SpriteBatch spriteBatch)
            {
                spriteBatch.GraphicsDevice.Clear(Color.Black);

                MoveCamera(spriteBatch.GraphicsDevice.Viewport);
                Matrix cameraMatrix = Matrix.CreateTranslation(-camera.X, -camera.Y, 0);

                // parametric equations for a Lemniscate with a = WOBBLE DISTANCE
                float drugIntensity = (float)PlayerManager.players[0].drugIntensity.getValue();
                double t = (double)(LREGame.time % WOBBLE_CYCLE_TIME) / (double)WOBBLE_CYCLE_TIME * (2 * Math.PI);
                // double t = (double)(game.getTime() % TIME_TO_WOBBLE) / (double)TIME_TO_WOBBLE * (c * 2 * Math.PI);
                double xOffset = drugIntensity * MAX_WOBBLE_DISTANCE * Math.Cos(t) / (1 + Math.Pow(Math.Sin(t), 2));
                double yOffset = drugIntensity * MAX_WOBBLE_DISTANCE * Math.Sin(t) * Math.Cos(t) / (1 + Math.Pow(Math.Sin(t), 2));
                //double xOffset = drugIntensity * WOBBLE_DISTANCE * (Math.Sin((a / c) * t) * Math.Cos(t));
                //double yOffset = drugIntensity * WOBBLE_DISTANCE * (Math.Sin((b / c) * t) * Math.Sin(t));
                DoubleVisionHelper.setOffset(new Vector2((float)xOffset, (float)yOffset));
                DoubleVisionHelper.setOpacity(drugIntensity * MAX_DOUBLE_VISION_OPACITY);

                // draw level
                spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.BackToFront, SaveStateMode.None, cameraMatrix);
                Level.GetInstance().Draw(spriteBatch);
                spriteBatch.End();

                spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.BackToFront, SaveStateMode.None, cameraMatrix);
                ItemManager.Draw(spriteBatch);
                spriteBatch.End();

                spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.BackToFront, SaveStateMode.None, cameraMatrix);
                EnemyManager.Draw(spriteBatch);
                spriteBatch.End();

                spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.BackToFront, SaveStateMode.None, cameraMatrix);
                PlayerManager.Draw(spriteBatch);
                spriteBatch.End();

                spriteBatch.Begin();
                HUD.GetInstance().Draw(spriteBatch);
                spriteBatch.End();
            }
        }

        public class MenuState : EngineState
        {
            internal Menu menu;

            public MenuState(Menu menu)
            {
                this.menu = menu;
            }

            public void Invoke()
            {
                menu.Select();
                
            }

            public void Update(float elapsedTime)
            {
                TimerManager.timers["Can Act"].Update(elapsedTime);
                menu.Update(elapsedTime);
            }

            public void Draw(SpriteBatch spriteBatch)
            {
                spriteBatch.Begin();
                menu.Draw(spriteBatch);
                spriteBatch.End();
            }
        }
        #endregion
    }
}
