// Copyright 12.13.2010
// Rob Argue
// David Edge
// Leanna Helton
// Joe Kiernen

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
using System.Reflection;

namespace LabRatEscape
{
    /// <summary>
    /// Enemy class that represents enemies in the game that extend from the Sprite class
    /// </summary>
    public class Enemy : Sprite
    {
        #region Variables
        GameValue VertVel = new GameValue(0, -1, 1);
        GameValue HorizVel = new GameValue(0, -1, 1);
        float timeSinceLastAction = 0;
        internal GameValue score = new GameValue(0, 0, Decimal.MaxValue);
        internal int playerNumber;
        internal int sightRadius;
        internal Vector2 initialPosition;
        internal int wanderRadius;
        internal EnemyAI chaseAI;
        internal EnemyAI relaxAI;
        internal EnemyAI returnAI;
        internal Player chasing;
        internal EnemyState enemyState;
        internal List<Effect> effects;
        #endregion

        #region Constants
        public const float RELAX_TIME_ON_HOME = 0;
        public const float RELAX_TIME_ON_HIT = .5f;
        public const double TIME_BETWEEN_ACTIONS = .15;
        #endregion

        #region Contstructors
        /// <summary>
        /// Creates the default enemy character
        /// </summary>
        /// <param name="spriteSheet">textures used to be displayed</param>
        /// <param name="width">width of the textures</param>
        /// <param name="height">height of the textures</param>
        /// <param name="position">starting position for the enemy</param>
        public Enemy(Texture2D spriteSheet, int width, int height, Vector2 position, EnemyAI relaxAI, EnemyAI returnAI, EnemyAI chaseAI,
            List<Effect> effects, int sightRadius, int wanderRadius)
            : base(spriteSheet, width, height, DEFAULT_FRAME_TIME, position)
        {
            this.relaxAI = relaxAI;
            this.returnAI = returnAI;
            this.chaseAI = chaseAI;
            this.sightRadius = sightRadius;
            this.wanderRadius = wanderRadius;
            enemyState = new RelaxState(this, RELAX_TIME_ON_HOME);
            speed.setValue(0);
            this.effects = effects;
            this.initialPosition = position;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Adds the enemy to the level at the right grid position
        /// </summary>
        /// <param name="row">Row to add the enemy to</param>
        /// <param name="col">Column to add the enemy to</param>
        public override void AddToLevel(int row, int col)
        {
            EnemyManager.enemies.Add(CopyEnemyAt(new Vector2(col * Block.WIDTH, row * Block.HEIGHT)));
            base.AddToLevel(row, col);
        }

        /// <summary>
        /// Copys the enemy at a new position
        /// </summary>
        /// <param name="position">New position</param>
        /// <returns>Copy of the enemy at the position</returns>
        public Enemy CopyEnemyAt(Vector2 position)
        {
            return new Enemy(spriteSheet, frameWidth, frameHeight, position, relaxAI, returnAI, chaseAI, effects, sightRadius, wanderRadius);
        }

        /// <summary>
        /// updates the enemy
        /// </summary>
        /// <param name="elapsedTime">total time elapsed</param>
        public override void Update(float elapsedTime)
        {
            timeSinceLastAction += elapsedTime;
            if (Vector2.Distance(position, initialPosition) > wanderRadius)
            {
                enemyState = new ReturnState(this);
            }
            enemyState.Update(elapsedTime);
            if (velocity == new Vector2(0, 0))
            {
                frameTimer -= elapsedTime;
            }
            if (velocity != new Vector2(0, 0))
            {
                angle = (float)Math.Atan2(velocity.Y, velocity.X);
            }
            base.Update(elapsedTime);
        }

        /// <summary>
        /// draws the enemy to the spritebatch given
        /// </summary>
        /// <param name="spriteBatch">spritebatch to draw the enemy to</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.DrawDoubleVision(spriteBatch);
            base.Draw(spriteBatch);
        }
        #endregion

        #region Inner Class States
        public class RelaxState : EnemyState
        {
            internal Enemy enemy;
            internal float relaxTime;

            public RelaxState(Enemy enemy, float relaxTime)
            {
                this.enemy = enemy;
                this.relaxTime = relaxTime;
                enemy.velocity = new Vector2(0, 0);
            }

            public void PlayerSighted(Player player)
            {
                if (relaxTime <= 0)
                {
                    enemy.enemyState = new ChaseState(enemy, player);
                }
            }

            public void Update(float elapsedTime)
            {
                relaxTime -= elapsedTime;
                enemy.relaxAI.Update(elapsedTime, enemy);
            }
        }

        public class ChaseState : EnemyState
        {
            internal Enemy enemy;

            public ChaseState(Enemy enemy, Player player)
            {
                this.enemy = enemy;
                enemy.chasing = player;
                enemy.velocity = new Vector2(0, 0);
            }

            public void PlayerSighted(Player player)
            {
                // already chasing, do nothing
            }

            public void Update(float elapsedTime)
            {
                if (Vector2.Distance(enemy.initialPosition, enemy.chasing.position)
                    > enemy.wanderRadius 
                    && Vector2.Distance(enemy.position, enemy.chasing.position)
                    > enemy.sightRadius)
                {
                    enemy.enemyState = new RelaxState(enemy, 0);
                }
                Collision c = new Collision(enemy, enemy.chasing);
                if (c.isColliding)
                {
                    foreach (Effect effect in enemy.effects)
                    {
                        effect.Apply(enemy.chasing);
                    }
                    enemy.enemyState = new RelaxState(enemy, Enemy.RELAX_TIME_ON_HIT);
                }
                enemy.chaseAI.Update(elapsedTime, enemy);
            }
        }

        public class ReturnState : EnemyState
        {
            internal Enemy enemy;

            public ReturnState(Enemy enemy)
            {
                this.enemy = enemy;
                enemy.velocity = new Vector2(0, 0);
            }

            public void PlayerSighted(Player player)
            {
                enemy.enemyState = new ChaseState(enemy, player);
            }

            public void Update(float elapsedTime)
            {
                enemy.returnAI.Update(elapsedTime, enemy);
                if (enemy.position == enemy.initialPosition)
                {
                    enemy.enemyState = new RelaxState(enemy, Enemy.RELAX_TIME_ON_HOME);
                }
            }
        }
        #endregion
    }
}
