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
            //Create a projectile using projectileTexture

            throw new NotImplementedException();
        }
    }
}
