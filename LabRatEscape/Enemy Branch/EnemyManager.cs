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
    /// Singleton that manages all the enemies for a given map
    /// </summary>
    public class EnemyManager
    {
        #region Variables
        private static EnemyManager uniqueInstance;
        public static List<Enemy> enemies = new List<Enemy>();
        public static int numEnemies = 0;
        public static Dictionary<string, Enemy> possibleEnemies = new Dictionary<string, Enemy>();
        #endregion

        #region Constants
        public const int MAX_ENEMIES = 15;
        #endregion

        #region Methods
        /// <summary>
        /// empty singleton constructor
        /// </summary>
        private EnemyManager()
        {
            //empty singleton constructor
        }

        /// <summary>
        /// Gets the unique instance of this class
        /// </summary>
        /// <returns>EnemyManager uniqueInstance</returns>
        public static EnemyManager GetInstance()
        {
            if (uniqueInstance == null)
            {
                uniqueInstance = new EnemyManager();
            }
            return uniqueInstance;
        }

        public static void Reset()
        {
            enemies.Clear();
        }

        /// <summary>
        /// updates all the enemies that the manager is in charge of
        /// </summary>
        /// <param name="elapsedTime">total elapsed time</param>
        public static void Update(float elapsedTime)
        {
            foreach (Enemy enemy in enemies)
            {
                enemy.Update(elapsedTime);
                foreach (Player player in PlayerManager.players)
                {
                    if (Vector2.Distance(player.position, enemy.position) < enemy.sightRadius)
                    {
                        enemy.enemyState.PlayerSighted(player);
                    }
                }
            }
        }

        /// <summary>
        /// draws all the enemies that the manager is in charge of
        /// </summary>
        /// <param name="spriteBatch"></param>
        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(spriteBatch);
            }

        }
        #endregion
    }
}
