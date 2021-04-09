//Trevor Dunn       4/2/21
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RecoilGame
{
    class Sniper : PlayerWeapon
    {
        private Texture2D projectileTexture;
        private float degreesTraveled;
        private float range;
        private int damage;
        private float cooldownAmt;
        private float currentCooldown;
        private bool isTrickShot;

        public Sniper(int xPos, int yPos, int width, int height, Texture2D sprite, bool isActive, Texture2D projectileTexture) 
            : base(xPos, yPos, width, height, sprite, isActive)
        {
            this.projectileTexture = projectileTexture;

            degreesTraveled = 0;
            damage = 25;
            cooldownAmt = 5;
            currentCooldown = 0;
            isTrickShot = false;

            Type = WeaponType.Sniper;
        }

        public void CheckTrickShot(GameTime gameTime)
        {
            float timer = 5;

            MouseState ms = Mouse.GetState();
            MouseState previousState;

            do
            {
                if (ms.RightButton == ButtonState.Released)
                {
                    return;
                }

                timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                previousState = ms;
                ms = Mouse.GetState();

            } while (ms.RightButton == ButtonState.Pressed && timer > 0);

            isTrickShot = true;
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

            //gets the mouses x and y values and determines the direction dependent on players location
            float mouseX = mouseState.X;
            float mouseY = mouseState.Y;
            float xDirection = (mouseX - player.CenteredX);
            float yDirection = (mouseY - player.CenteredY);

            //Normalizes the x and y values regardless of the distance of the mouse from player
            double magnitude = Math.Sqrt((xDirection * xDirection) + (yDirection * yDirection));
            float xNormalized = xDirection / (float)magnitude;
            float yNormalized = yDirection / (float)magnitude;

            float bulletSpeed = 8;

            //Creates a new vector2 by multiplying the normalized values by bulletspeed
            Vector2 direction = new Vector2(xNormalized * bulletSpeed, yNormalized * bulletSpeed);

            //Test to see if this will actually create a projectile and how it will work, then we'll add more since we want shotgun to have multiple projectiles
            new Projectile(player.CenteredX, player.CenteredY, 7, 7, projectileTexture, true, direction, damage, 5, 0.75f, false, true);

            //Calls playerManager's shooting capability method
            Game1.playerManager.ShootingCapability();

            //Sets the cooldown
            currentCooldown = cooldownAmt;
        }

        public override void UpdateCooldown(GameTime gameTime)
        {
            if (currentCooldown == 0)
            {
                return;
            }

            currentCooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Draw(SpriteBatch sb, Color tint)
        {
            base.Draw(sb, tint);
        }

        public override void UpdateCooldown(int amount)
        {
            currentCooldown = amount;
        }
    }
}
