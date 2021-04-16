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
        private int damage;

        //Constructor
        public RocketLauncher(int xPos, int yPos, int width, int height, Texture2D sprite, 
            bool isActive, Texture2D projectileTexture) 
            : base(xPos, yPos, width, height, sprite, isActive)
        {
            this.projectileTexture = projectileTexture;
            CooldownAmt = 3;
            damage = 30;

            Type = WeaponType.RocketLauncher;
        }

        /// <summary>
        /// Creates a new Projectile and Adds it to ListOfProjectiles
        /// </summary>
        public override void Shoot()
        {
            if(CurrentCooldown > 0)
            {
                return;
            }

            MouseState mouseState = Mouse.GetState();
            Player player = Game1.playerManager.PlayerObject;

            //gets the mouses x and y values and determines the direction dependent on players location
            float mouseX = mouseState.X;
            float mouseY = mouseState.Y;
            float xDirection = (mouseX - player.CenteredX);
            float yDirection = (mouseY - player.CenteredY);
            //Normalizes the x and y values regardless of the distance of the mouse from player
            double magnitude = Math.Sqrt((xDirection * xDirection) + (yDirection * yDirection));
            float xNormalized = xDirection / (float)magnitude;
            float yNormalized = yDirection / (float)magnitude;

            float bulletSpeed = 12;

            //Creates a new vector2 by multiplying the normalized values by bulletspeed
            Vector2 direction = new Vector2(xNormalized * bulletSpeed, yNormalized * bulletSpeed);

            //Test to see if this will actually create a projectile and how it will work, then we'll add more since we want shotgun to have multiple projectiles
            new Projectile((int)player.CenteredX, (int)player.CenteredY, 20, 20, projectileTexture, true, direction, 20, 10, 10, true, true, true);


            CurrentCooldown = CooldownAmt;
        }

        public override void UpdateCooldown(GameTime gameTime)
        {
            if(CurrentCooldown == 0)
            {
                return;
            }

            CurrentCooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void UpdateCooldown(int amount)
        {
            CurrentCooldown = amount;
        }
    }
}
