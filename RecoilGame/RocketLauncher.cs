//Trevor Dunn       3/19/21
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecoilGame
{
    class RocketLauncher : PlayerWeapon
    {
        //Fields
        private Texture2D projectileTexture;

        //Constructor
        public RocketLauncher(int xPos, int yPos, int width, int height, Texture2D sprite, bool isActive, float cooldown, int numOfProjectiles, Texture2D projectileTexture) : base(xPos, yPos, width, height, sprite, isActive, cooldown, numOfProjectiles)
        {
            this.projectileTexture = projectileTexture;
        }

        //Shoot Method
        public override void Shoot()
        {
            //Test to see if this will actually create a projectile and how it will work, then we'll add more since we want shotgun to have multiple projectiles
            Game1.projectileManager.listOfProjectiles.Add(new Projectile(ObjectRect.Right, this.CenteredY, 20, 20, projectileTexture, true, new Vector2(ObjectRect.Right, this.CenteredY), 20, 5, 10, false, true));

            //throw new NotImplementedException();
        }
    }
}
