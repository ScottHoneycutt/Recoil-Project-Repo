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
    /// 3/17/2021----
    /// Projectile class handles simulating the movement and collisions of bullets----
    /// </summary>
    public class Projectile : GameObject
    {
        private Vector2 velocity;
        private int damage;
        private float gravity;
        private float lifetime;
        private bool isExplosive;
        private bool isFriendly;
        private bool affectedByGravity;

        /// <summary>
        /// Projectile class constructor. Takes in a Vector2 for the velocity----
        /// </summary>
        /// <param name="xPosition">X coordinate for the rectangle. Passed to GameObject 
        /// (parent) class----</param>
        /// <param name="yPosition">Y coordinate for the rectangle. Passed to GameObject 
        /// (parent) class----</param>
        /// <param name="width">Width of the rectangle. Passed to GameObject (parent) class----</param>
        /// <param name="height">Height of the rectangle. Passed to GameObject (parent) class----</param>
        /// <param name="texture">Texture for the GameObject (parent) class----</param>
        /// <param name="active">isActive boolean for the GameObject (parent) class----</param>
        /// <param name="velocity">The velocity vector of the projectile----</param>
        /// <param name="damage">The amount of damage the projectile will deal on impact----</param>
        /// <param name="gravity">The gravity constant for the projectile (0 if no gravity, positive 
        /// otherwise)----</param>
        /// <param name="lifetime">The time after which the projectile will become inactive----</param>
        /// <param name="isExplosive">Whether or not the projectile spawns an explosion----</param>
        /// <param name="isFriendly">Determines what the projectile can collide with----</param>
        public Projectile(int xPosition, int yPosition, int width, int height, Texture2D texture, bool active,
            Vector2 velocity, int damage, float gravity, float lifetime, bool isExplosive, bool isFriendly, bool affectedByGravity) 
            : base(xPosition, yPosition, width, height, texture, active)
        {
            this.velocity = velocity;
            this.damage = damage;
            this.gravity = gravity;
            this.lifetime = lifetime;
            this.isExplosive = isExplosive;
            this.isFriendly = isFriendly;
            this.affectedByGravity = affectedByGravity;

            //Projectile's spawn coordinates are centered----
            CenteredX = xPosition;
            CenteredY = yPosition;
            //Updating the projectile's rectangle to match the position----
            ConvertPosToRect();

            //Adding projectiles to ProjectileManager's list of projectiles to simulate----
            Game1.projectileManager.ReportExists(this);
        }

        /// <summary>
        /// Projectile class constructor. Takes in a speed and an angle rather than a Vector2
        /// for velocity----
        /// </summary>
        /// <param name="xPosition">X coordinate for the center of the projectile----</param>
        /// <param name="yPosition">Y coordinate for for center of the projectiles----</param>
        /// <param name="width">Width of the rectangle. Passed to GameObject (parent) class----</param>
        /// <param name="height">Height of the rectangle. Passed to GameObject (parent) class----</param>
        /// <param name="texture">Texture for the GameObject (parent) class----</param>
        /// <param name="active">isActive boolean for the GameObject (parent) class----</param>
        /// <param name="speed">The linear speed at which the projectile moves----</param>
        /// <param name="angle">The angle counterclockwise from the positive X direction of
        /// the projectile's direction----</param>
        /// <param name="damage">The amount of damage the projectile will deal on impact----</param>
        /// <param name="gravity">The gravity constant for the projectile (0 if no gravity----</param>
        /// <param name="lifetime">The time after which the projectile will become inactive----</param>
        /// <param name="isExplosive">Whether or not the projectile spawns an explosion----</param>
        /// <param name="isFriendly">Determines what the projectile can collide with----</param>
        public Projectile(int xPosition, int yPosition, int width, int height, Texture2D texture, bool active,
            float speed, float angle, int damage, float gravity, float lifetime, bool isExplosive, bool isFriendly, bool affectedByGravity)
            : base(xPosition, yPosition, width, height, texture, active)
        {
            //Turning the angle and speed into a velocity vector----
            velocity = new Vector2(speed * MathF.Cos(angle), speed * MathF.Sin(angle));
            //Adjusting the velocity vector to account for the native pixel coordinate system----
            velocity.Y = -velocity.Y;
            this.damage = damage;
            this.gravity = gravity;
            this.lifetime = lifetime;
            this.isExplosive = isExplosive;
            this.isFriendly = isFriendly;
            this.affectedByGravity = affectedByGravity;

            //Projectile's spawn coordinates are centered----
            CenteredX = xPosition;
            CenteredY = yPosition;
            //Updating the projectile's rectangle to match the position----
            ConvertPosToRect();

            //Adding projectiles to ProjectileManager's list of projectiles to simulate----
            Game1.projectileManager.ReportExists(this);
        }

        /// <summary>
        /// Simulates the projectile's movement and calls CheckForCollisions for collision
        /// detection. Is meant to be run every frame----
        /// </summary>
        /// <param name="gameTime">GameTime variable to determine when the object expires----</param>
        public void Simulate(GameTime gameTime)
        {
            //Updating lifetime timer----
            lifetime -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Expiring old projectiles and preventing further simulation----
            if (lifetime <= 0)
            {
                Expire();
                return;
            }

            //Updating position----
            position.X += velocity.X;
            position.Y += velocity.Y;

            objectRect.X = (int)position.X;
            objectRect.Y = (int)position.Y;

            //Updating the projectile's rectangle to match the position----
            ConvertPosToRect();

            //Checking for collisions----
            CheckForCollisions();

            if(affectedByGravity == true)
            {
                //Updating the velocity vector----
                velocity.Y += gravity / 150;
            }
        }

        /// <summary>
        /// Helper method. For collisions between the projectile and other relevant objects----
        /// </summary>
        private void CheckForCollisions()
        {
            if (isFriendly)
            {
                //Check for collisions with all enemy objects here----
                for (int i = 0; i < Game1.enemyManager.ListOfEnemies.Count; i++)
                {
                    if (this.objectRect.Intersects(Game1.enemyManager.ListOfEnemies[i].ObjectRect))
                    {
                        Game1.enemyManager.ListOfEnemies[i].TakeDamage(damage);

                        //Creating an explosion upon collision if the projectile is marked as explosive-----
                        if (isExplosive)
                        {
                            new Explosion((int)CenteredX, (int)CenteredY, 100, 100, Game1.projectileManager.ExplosionTextures,
                                true, 20, 100, 17, 1, true);
                        }

                        Expire();
                        //Saving time by returning early----
                        return;
                    }
                }
            }
            else
            {
                //Check for collisions with the player here----
                if (this.objectRect.Intersects(Game1.playerManager.PlayerObject.ObjectRect))
                {
                    Game1.playerManager.PlayerObject.TakeDamage(damage);

                    //Creating an explosion upon collision if the projectile is marked as explosive-----
                    if (isExplosive)
                    {
                        new Explosion((int)CenteredX, (int)CenteredY, 100, 100, Game1.projectileManager.ExplosionTextures,
                            true, 20, 100, 17, 1, false);
                    }

                    //Expires after collision----
                    Expire();
                    //Saving time by returning early----
                    return;
                }
            }
            //Check for collisions with the terrain here----
            for (int i = 0; i < Game1.levelManager.ListOfMapTiles.Count; i++)
            {
                if (this.objectRect.Intersects(Game1.levelManager.ListOfMapTiles[i].ObjectRect))
                {
                    //Creating an explosion upon collision if the projectile is marked as explosive-----
                    if (isExplosive && isFriendly) 
                    {
                        Explosion expl = new Explosion((int)CenteredX, (int)CenteredY, 100, 100, Game1.projectileManager.ExplosionTextures,
                            true, 20, 100, 17, 1, true);
                    }
                    //Creating an explosion upon collision if the projectile is marked as explosive-----
                    else if (isExplosive && !isFriendly)
                    {
                        new Explosion((int)CenteredX, (int)CenteredY, 100, 100, Game1.projectileManager.ExplosionTextures,
                            true, 20, 100, 17, 1, false);
                    }

                    Expire();
                    //Saving time by returning early----
                    return;
                }
            }
        }

        /// <summary>
        /// Helper method. Helps remove an object from the simulation cycle----
        /// </summary>
        private void Expire()
        {
            isActive = false;
            Game1.projectileManager.ReportExpired(this);
        }
    }
}
