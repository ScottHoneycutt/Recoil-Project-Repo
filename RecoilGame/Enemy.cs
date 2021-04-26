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
        private bool shouldSwitchDirections;

        //Patrol movement stuff----
        private float moveSpeed;
        private float patrolTime;
        private float patrolTimer;
        private float patrolRandom;
        private float patrolRandomNum;
        private Random randomGenerator;
        private bool movesLeft;

        //Patrol floor detection rectangles----
        private Rectangle leftRect;
        private Rectangle rightRect;

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

        public float XVelocity
        {
            get { return velocity.X; }
            set { velocity.X = value; }
        }

        public float YVelocity
        {
            get { return velocity.Y; }
            set { velocity.Y = value; }
        }

        public int Damage
        {
            get { return damage; }
            set { damage = value; }
        }
        //public bool 

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
        /// <param name="attackPeriod">The amount of time between each attack----</param>
        public Enemy(int x, int y, int width, int height, Texture2D texture, bool isActive, Vector2 velocity, 
            float health, float attackPeriod, int damage)
            : base(x, y, width, height, texture, isActive)
        {
            this.health = health;
            this.velocity = velocity;
            this.attackPeriod = attackPeriod;
            this.damage = damage;
            attackTimer = 0;

            //Movement stuff----
            moveSpeed = 2;
            patrolTime = 2;
            patrolTimer = 0;
            patrolRandom = 1;
            patrolRandomNum = 0;
            randomGenerator = new Random();
            movesLeft = true;

            //Setting up the rectangles for floor detection for patrolling----
            leftRect = new Rectangle(objectRect.X - 1, objectRect.Y + objectRect.Height, 1, 10);
            rightRect = new Rectangle(objectRect.X + objectRect.Width, objectRect.Y + objectRect.Height, 1, 10);

            //Reporting this enemy's existence to the EnemyManager;
            Game1.enemyManager.ReportExists(this);
        }

        /// <summary>
        /// Handles the shooting/targeting of enemy objects----
        /// </summary>
        /// <param name="playerRef">Reference to the player object----</param>
        /// <param name="gameTime">GameTime object reference so that the duration between frames
        /// can be counted----</param>
        /// <param name="projectileSprite">The texture to use for the projectile the enemy shoots----</param>
        public void SimulateBehaviors(Player playerRef, GameTime gameTime, Texture2D projectileSprite)
        {
            //Movement stuff----
            //Updating the rectangles used to determine whether or not there is floor ahead----
            leftRect.X = objectRect.X -1;
            rightRect.X = objectRect.X + objectRect.Width;
            leftRect.Y = objectRect.Y + objectRect.Height;
            rightRect.Y = objectRect.Y + objectRect.Height;

            //Moving left----
            if (movesLeft)
            {
                position.X -= moveSpeed;

                //Incrementing timer----
                patrolTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                //Detecting floor ahead----
                bool floorAhead = false;
                foreach (MapTile hitbox in Game1.levelManager.ListOfMapTiles)
                {
                    if (leftRect.Intersects(hitbox.ObjectRect))
                    {
                        floorAhead = true;
                        //Breaking to save time---
                        break;
                    }
                }

                //Switching directions if the timer runs out for moving in one direction, or if there is no floor ahead----
                if (patrolTimer >= patrolTime + patrolRandomNum || !floorAhead)
                {
                    movesLeft = false;
                    //Resetting timer and getting a new random value----
                    patrolTimer = 0;
                    patrolRandomNum = (float)randomGenerator.NextDouble() * patrolRandom;
                }
            }

            //Moving right----
            else
            {
                position.X += moveSpeed;

                //Incrementing timer----
                patrolTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                //Detecting floor ahead----
                bool floorAhead = false;
                foreach (MapTile hitbox in Game1.levelManager.ListOfMapTiles)
                {
                    if (rightRect.Intersects(hitbox.ObjectRect))
                    {
                        floorAhead = true;
                        //Breaking to save time---
                        break;
                    }
                }

                //Switching directions if the timer runs out for moving in one direction, or if there is no floor ahead----
                if (patrolTimer >= patrolTime + patrolRandomNum || !floorAhead)
                {
                    movesLeft = true;
                    //Resetting timer and getting a new random value----
                    patrolTimer = 0;
                    patrolRandomNum = (float)randomGenerator.NextDouble() * patrolRandom;
                }
            }
            ConvertPosToRect();

            //Shooting----
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
                normalizedVector.X *= 5;
                normalizedVector.Y *= 5;

                //Creating the projectile----
                new Projectile((int)CenteredX, (int)CenteredY, 10, 10, projectileSprite, true, normalizedVector, Damage,
                    0, 2, false, false, true);
            }
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
