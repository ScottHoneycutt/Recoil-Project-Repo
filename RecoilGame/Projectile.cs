using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RecoilGame
{
    class Projectile
    {
        private Vector2 velocity;
        private int damage;
        private float gravity;
        private float lifetime;
        private bool isExplosive;
        private bool isFriendly;

        public Projectile(Vector2 velocity, int damage, float gravity, float lifetime, 
                          bool isExplosive, bool isFriendly)
        {
            this.velocity = velocity;
            this.damage = damage;
            this.gravity = gravity;
            this.lifetime = lifetime;
            this.isExplosive = isExplosive;
            this.isFriendly = isFriendly;
        }

        public Projectile(float speed, float angle, int damage, float gravity, float lifetime,
                          bool isExplosive, bool isFriendly)
        {

            velocity = new Vector2(speed * MathF.Cos(angle), speed * MathF.Sin(angle));
            this.damage = damage;
            this.gravity = gravity;
            this.lifetime = lifetime;
            this.isExplosive = isExplosive;
            this.isFriendly = isFriendly;
        }

    }
}
