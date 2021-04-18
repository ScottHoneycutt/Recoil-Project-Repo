using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RecoilGame
{
    // Name: Jack Walsh
    // Date: 3/19/2021
    // Purpose: Create enemy and adjust its health
    public class Enemy : GameObject, IDamageable
    {
        // Variables
        private float health;
        private Vector2 velocity;
        private float attackPeriod;
        private float attackTimer;
        private int damage;

        // Properties
        public float Health
        {
            get { return health; }
            set { health = value; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public int Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        /// <summary>
        /// Constructor for Enemy class - inherits from GameObject
        /// </summary>
        /// <param name="x"> X position of topleft corner of playerRect </param>
        /// <param name="y"> Y position of topleft corner of playerRect </param>
        /// <param name="width"> Width of playerRect </param>
        /// <param name="height"> Height of playerRect </param>
        /// <param name="texture"> Player texture </param>
        /// <param name="isActive"> If player should be drawn to screen </param>
        /// <param name="velocity"> Velocity of player </param>
        /// <param name="health"> Health of player </param>
        /// /// <param name="attackPeriod">The amount of time between each attack----</param>
        public Enemy(int x, int y, int width, int height, Texture2D texture, bool isActive, Vector2 velocity, 
            float health, float attackPeriod, int damage)
            : base(x, y, width, height, texture, isActive)
        {
            this.health = health;
            this.velocity = velocity;
            this.attackPeriod = attackPeriod;
            this.damage = damage;
            attackTimer = 0;

            //Reporting this enemy's existence to the EnemyManager;
            Game1.enemyManager.ReportExists(this);
        }

        /// <summary>
        /// Handles the shooting/targeting of enemy objects----
        /// </summary>
        /// <param name="playerRef">Reference to the player object----</param>
        /// <param name="gameTime">GameTime object reference so that the duration between frames
        /// can be counted----</param>
        /// <param name="projectileSprite">The texture to use for the projectile th enemy shoots----</param>
        public void SimulateBehaviors(Player playerRef, GameTime gameTime, Texture2D projectileSprite)
        {
            //Shoot at the player once every attack period----
            attackTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (attackTimer > attackPeriod)
            {
                //Restting timer every attack----
                attackTimer = 0;

                //Calculating the velocity vector for the projectile----
                Vector2 normalizedVector = new Vector2(playerRef.CenteredX - CenteredX, 
                    playerRef.CenteredY - CenteredY);
                normalizedVector.Normalize();
                normalizedVector.X *= 15;
                normalizedVector.Y *= 15;

                //Creating the projectile----
                new Projectile((int)CenteredX, (int)CenteredY, 10, 10, projectileSprite, true, normalizedVector, Damage,
                    0, 2, false, false, true);
            }
        }

        /// <summary>
        /// Move enemy according to velocity 
        /// </summary>
        public void Move()
        {
            position = new Vector2(
                position.X + velocity.X,
                position.Y + velocity.Y);

            ConvertPosToRect();
        }

        /// <summary>
        /// Take damage and potentially deactivate if health < 0
        /// </summary>
        /// <param name="damage"> Damage to take </param>
        public void TakeDamage(int damage)
        {
            health -= damage;
            if (health <= 0)
            {
                isActive = false;
            }
        }
    }
}
