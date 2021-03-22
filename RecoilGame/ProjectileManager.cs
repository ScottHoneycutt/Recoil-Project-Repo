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

        public ProjectileManager()
        {
            listOfProjectiles = new List<Projectile>();
        }


        public void Simulate(GameTime gameTime)
        {
            foreach (Projectile proj in listOfProjectiles)
            {
                proj.Simulate(gameTime);
            }
        }

        public void ReportExpired(Projectile expiredProjectile)
        {
            listOfProjectiles.Remove(expiredProjectile);
        }
    }
}
