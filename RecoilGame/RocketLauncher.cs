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
        //Constructor
        public RocketLauncher(int xPos, int yPos, int width, int height, Texture2D sprite, bool isActive, float cooldown, int numOfProjectiles) : base(xPos, yPos, width, height, sprite, isActive, cooldown, numOfProjectiles)
        {

        }

        //Shoot Method
        public override void Shoot()
        {
            throw new NotImplementedException();
        }
    }
}
