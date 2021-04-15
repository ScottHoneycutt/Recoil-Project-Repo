using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RecoilGame
{
    /// <summary>
    /// Scott Honeycutt----
    /// 3/29/2021----
    /// Explosion class to be used by the rocket launcher's projectiles----
    /// </summary>
    public class Explosion : GameObject
    {
        private int damage;
        private float radius;
        private float playerKnockback;
        private float lifeTime;
        private bool isFriendly;

        /// <summary>
        /// Creates an explosion with a specified radius, damage, and player knockback----
        /// </summary>
        /// <param name="xPosition">The X coordinate of the object's centered position----</param>
        /// <param name="yPosition">The Y coordinate of the object's centered position----</param>
        /// <param name="width">The width of the rectangle----</param>
        /// <param name="height">The heigh of the rectangle----</param>
        /// <param name="texture">The texture to be displayed in the rectangle----</param>
        /// <param name="active">Whether or not the object is currently active----</param>
        /// <param name="damage">The damage dealt by the explosion to targets within its radius----</param>
        /// <param name="radius">The radius of the explosion's damage and knockback----</param>
        /// <param name="playerKnockback">The strength of the knockback the explosion deals
        /// to the player----</param>
        /// <param name="lifeTime">How long the explosion sprite lingers----</param>
        /// <param name="isFriendly">Whether or not the explosiond deals damage to enemies or
        /// the player----</param>
        public Explosion(int xPosition, int yPosition, int width, int height, Texture2D texture, bool active,
            int damage, float radius, float playerKnockback, float lifeTime, bool isFriendly)
            : base(xPosition, yPosition, width, height, texture, active)
        {
            this.damage = damage;
            this.radius = radius;
            this.playerKnockback = playerKnockback;
            this.lifeTime = lifeTime;
            this.isFriendly = isFriendly;

            //Setting the position of the explosion to be centered on the given coordinates
            //(which were the centered coordinates of the projectile upon collision)----
            CenteredX = xPosition;
            CenteredY = yPosition;
            //Updating the projectile's rectangle to match the position----
            ConvertPosToRect();

            //Exploding and reporting to the ProjectileManager----
            Explode();
            Game1.projectileManager.ReportExists(this);
        }

        /// <summary>
        /// The nuts and bolts of the explosion. This is where all of the mechanics occur----
        /// </summary>
        private void Explode()
        {
            //Friendly explosions check for collisions with enemies to deal damage and with
            //the player to inflict knockback----
            if (isFriendly)
            {
                //Checking distance between explosion and player----
                Vector2 displacementVector = new Vector2(Game1.playerManager.PlayerObject.CenteredX - CenteredX,
                   Game1.playerManager.PlayerObject.CenteredY - CenteredY);

                if (radius >= displacementVector.Length())
                {
                    //Impart a velocity onto the player----
                    //Normalizing the displacement vector from the explosion to the player----
                    displacementVector.Normalize();

                    //Multiplying the normalized vector by playerKnockback to calculate the velocity vector----
                    displacementVector.X *= playerKnockback;
                    displacementVector.Y *= playerKnockback;
                    Game1.playerManager.AddVelocity(displacementVector);
                }

                //Checking for collisions between the explosion and all enemies----
                foreach(Enemy enemy in Game1.enemyManager.ListOfEnemies)
                {
                    displacementVector = new Vector2(enemy.CenteredX - CenteredX,
                       enemy.CenteredY - CenteredY);

                    if (radius >= displacementVector.Length())
                    {
                        enemy.TakeDamage(damage);
                    }
                }
            }
            //Hostile explosions will be implimented later if there is time----
        }

        /// <summary>
        /// Counts down the explosion's lifetime for the lingering sprite so that the explosion doesn't
        /// disappear after 1 frame----
        /// </summary>
        /// <param name="gameTime"></param>
        public void CountDownLifeTime(GameTime gameTime)
        {
            lifeTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Reporting expiration to the ProjectileManager----
            if (lifeTime <= 0)
            {
                isActive = false;
                Game1.projectileManager.ReportExpired(this);
            }
        }

    }
}
