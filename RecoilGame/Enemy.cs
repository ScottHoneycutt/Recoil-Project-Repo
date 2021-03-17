using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RecoilGame
{
    class Enemy : IDamageable
    {
        private int health;
        private Vector2 velocity;

        public Enemy(int health, Vector2 velocity)
        {
            this.health = health;
            this.velocity = velocity;
        }

        public void TakeDamage(int damage)
        {
            health -= damage;
            if (health <= 0)
            {
                //die
            }
        }
    }
}
