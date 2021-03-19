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
        public Enemy(int x, int y, int width, int height, Texture2D texture, bool isActive, Vector2 velocity, float health)
            : base(x, y, width, height, texture, isActive)
        {
            this.health = health;
            this.velocity = velocity;
        }

        public void Move()
        {
            position = new Vector2(
                CenteredX + velocity.X,
                CenteredY + velocity.Y);

            objectRect.X = (int)position.X;
            objectRect.Y = (int)position.Y;
        }

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
