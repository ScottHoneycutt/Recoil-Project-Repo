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
    /// 3/19/2021----
    /// Manager class for projectiles----
    /// </summary>
    public class ProjectileManager
    {
        private List<Projectile> listOfProjectiles;
        private List<Projectile> expiredProjectiles;
        private List<Explosion> listOfExplosions;
        private List<Explosion> expiredExplosions;

        /// <summary>
        /// Constructor for the projectileManager class----
        /// </summary>
        public ProjectileManager()
        {
            listOfProjectiles = new List<Projectile>();
            expiredProjectiles = new List<Projectile>();
            listOfExplosions = new List<Explosion>();
            expiredExplosions = new List<Explosion>();
        }

        /// <summary>
        /// Tells all projectiles to simulate/update themselves, managing movement and collisions.
        /// Also tells explosions to count down on their lifetime----
        /// </summary>
        /// <param name="gameTime">GameTime passed in to limit how long a projectile lives----</param>
        public void Simulate(GameTime gameTime)
        {
            //Projectiles----
            foreach (Projectile proj in listOfProjectiles)
            {
                proj.Simulate(gameTime);
            }
            //Explosions----
            foreach (Explosion explosion in listOfExplosions)
            {
                explosion.CountDownLifeTime(gameTime);
            }
        }

        /// <summary>
        /// Reports the existence of new projectiles to the ProjectileManager so that it can be added to the
        /// list of active projectiles----
        /// </summary>
        /// /// <param name="newProj">The projectile to add to the list----</param>
        public void ReportExists(Projectile newProj)
        {
            listOfProjectiles.Add(newProj);
        }

        /// <summary>
        /// Reports the existence of new explosions to the ProjectileManager so that it can be added to the
        /// list of active explosions----
        /// </summary>
        /// <param name="newExplosion">The explosion to add to the list----</param>
        public void ReportExists(Explosion newExplosion)
        {
            listOfExplosions.Add(newExplosion);
        }

        /// <summary>
        /// Reports a projectile that has expired so that it can be removed when CollectGarbage is called----
        /// </summary>
        /// <param name="expiredProjectile">The projectile that has expired----</param>
        public void ReportExpired(Projectile expiredProjectile)
        {
            expiredProjectiles.Add(expiredProjectile);
        }

        /// <summary>
        /// Reports a explosion that has expired so that it can be removed when CollectGarbage is called----
        /// </summary>
        /// <param name="expiredExplosion">The explosion that has expired----</param>
        public void ReportExpired(Explosion expiredExplosion)
        {
            expiredExplosions.Add(expiredExplosion);
        }

        /// <summary>
        /// Cleans listOfProjectiles of expired projectiles so that they stop simulating them. 
        /// *****This is to be run AFTER Simulate() in Update() so as to not interefere with it*****-----
        /// </summary>
        public void CollectGarbage()
        {
            //Removing projectiles----
            foreach(Projectile expiredProjectile in expiredProjectiles)
            {
                listOfProjectiles.Remove(expiredProjectile);
            }
            expiredProjectiles.Clear();

            //Removing explosions----
            foreach (Explosion expiredExplosion in expiredExplosions)
            {
                listOfExplosions.Remove(expiredExplosion);
            }
            expiredExplosions.Clear();
        }

        /// <summary>
        /// Tells all projectiles to draw themselves----
        /// </summary>
        /// <param name="sb">The spritebatch to be used for the drawing----</param>
        /// <param name="tint">The color to draw the sprites in----</param>
        public void Draw(SpriteBatch sb, Color tint)
        {
            //Projectiles----
            foreach (Projectile proj in listOfProjectiles)
            {
                proj.Draw(sb, tint);
            }
            //Explosions----
            foreach (Explosion explosion in listOfExplosions)
            {
                explosion.Draw(sb, tint);
            }
        }
    }
}
