//Trevor Dunn       4/2/21
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RecoilGame
{
    class MachineGun : PlayerWeapon
    {
        private int numProjectiles;
        private float cooldownAmt;
        private int damage;
        private float currentCooldown;
        private Texture2D projectileTexture;
        
        public MachineGun(int xPos, int yPos, int width, int height, Texture2D sprite, bool isActive, Texture2D projectileTexture) 
            : base(xPos, yPos, width, height, sprite, isActive)
        {
            this.projectileTexture = projectileTexture;

            numProjectiles = 10;
            damage = 1;
            cooldownAmt = 3;
            currentCooldown = 0;

            Type = WeaponType.MachineGun;
        }

        public override void Shoot()
        {
            //If cooldown is greater than 0 still, cannot shoot so returns
            if (currentCooldown > 0)
            {
                return;
            }

            MouseState mouseState = Mouse.GetState();
            Player player = Game1.playerManager.PlayerObject;

            while(mouseState.LeftButton == ButtonState.Pressed && numProjectiles > 0)
            {
                //Normalizes the x and y values regardless of the distance of the mouse from player
                double magnitude = Math.Sqrt((Math.Pow((mouseState.X - player.CenteredX), 2) + Math.Pow((mouseState.Y - player.CenteredY), 2)));
                float xNormalized = (mouseState.X - player.CenteredX) / (float)magnitude;
                float yNormalized = (mouseState.Y - player.CenteredY) / (float)magnitude;

                float bulletSpeed = 8;

                //Creates a new vector2 by multiplying the normalized values by bulletspeed
                Vector2 direction = new Vector2(xNormalized * bulletSpeed, yNormalized * bulletSpeed);

                //Test to see if this will actually create a projectile and how it will work, then we'll add more since we want shotgun to have multiple projectiles
                new Projectile(player.CenteredX, player.CenteredY, 7, 7, projectileTexture, true, direction, damage, 5, 0.75f, false, true);

                //Calls playerManager's shooting capability method
                Game1.playerManager.ShootingCapability();

                numProjectiles--;
            }

            //Sets the cooldown
            currentCooldown = cooldownAmt;
        }

        public override void UpdateCooldown(GameTime gameTime)
        {
            if (currentCooldown == 0)
            {
                numProjectiles = 10;
                return;
            }

            else
            {
                currentCooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }
    }
}
