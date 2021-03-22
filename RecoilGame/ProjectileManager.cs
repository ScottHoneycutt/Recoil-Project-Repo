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
        public List<Projectile> listOfProjectiles;
        private List<Projectile> expiredProjectiles;

        public ProjectileManager()
        {
            listOfProjectiles = new List<Projectile>();
            expiredProjectiles = new List<Projectile>();
        }

        /// <summary>
        /// Tells all projectiles to simulate/update themselves, managing movement and collisions----
        /// </summary>
        /// <param name="gameTime">GameTime passed in to limit how long a projectile lives----</param>
        public void Simulate(GameTime gameTime)
        {
            foreach (Projectile proj in listOfProjectiles)
            {
                proj.Simulate(gameTime);
            }
        }

        /// <summary>
        /// Reports the existence of new projectiles to the ProjectileManager so that it can be added to the
        /// list of active projectiles----
        /// </summary>
        public void ReportExists(Projectile newProj)
        {
            listOfProjectiles.Add(newProj);
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
        /// Cleans listOfProjectiles of expired projectiles so that they stop simulating them. 
        /// *****This is to be run AFTER Simulate() in Update() so as to not interefere with it*****-----
        /// </summary>
        public void CollectGarbage()
        {
            foreach(Projectile expiredProjectile in expiredProjectiles)
            {
                listOfProjectiles.Remove(expiredProjectile);
                expiredProjectiles.Clear();
            }
        }

        /// <summary>
        /// Tells all projectiles to draw themselves----
        /// </summary>
        /// <param name="sb">The spritebatch to be used for the drawing----</param>
        /// <param name="tint">The color to draw the sprites in----</param>
        public void Draw(SpriteBatch sb, Color tint)
        {
            foreach (Projectile proj in listOfProjectiles)
            {
                proj.Draw(sb, tint);
            }
        }
    }
}
