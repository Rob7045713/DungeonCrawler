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
        Menu mainMenu;
        Menu pauseMenu;
        Menu dieMenu;
        Menu winLevelMenu;
        Menu winGameMenu;
        #endregion
        #region Constants
        public const int WOBBLE_CYCLE_TIME = 4000;
        public const int MAX_WOBBLE_DISTANCE = 48;
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
        /// Switches the state to play
        /// </summary>
        public static void Play()
        {
            GetInstance().engineState = new PlayState();
        }
        
        #endregion

        #region States

        public class PlayState : EngineState
        {
            public PlayState()
            {

            }

            public void Update(float elapsedTime)
            {
                // update each branch of the game in the appropriate order
                PlayerManager.Update(elapsedTime);
                Level.GetInstance().Update(elapsedTime);
                HUD.GetInstance().Update(elapsedTime);
                //game.enemyManager.Update();
                //game.itemManager.Update();
                CheckCollisions();
                if (PlayerManager.AllPlayersDead())
                {
                    Level.RestartLevel();
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

            public void Update(float elapsedTime)
            {
                menu.Update(elapsedTime);
            }

            public void Draw(SpriteBatch spriteBatch)
            {
                menu.Draw(spriteBatch);
            }
        }
        #endregion
    }
}
