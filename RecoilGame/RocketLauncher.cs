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
        private float bulletSpeed;

        //Constructor

        /// <summary>
        /// Constructor for RocketLauncher
        /// </summary>
        /// <param name="xPos"></param>
        /// <param name="yPos"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="sprite"></param>
        /// <param name="isActive"></param>
        /// <param name="projectileTexture"></param>
        public RocketLauncher(int xPos, int yPos, int width, int height, Texture2D sprite, 
            bool isActive, Texture2D projectileTexture) 
            : base(xPos, yPos, width, height, sprite, isActive)
        {
            this.projectileTexture = projectileTexture;
            CooldownAmt = 2;
            damage = 25;
            bulletSpeed = 7f;
        }


        //METHODS

        /// <summary>
        /// Creates a new Projectile
        /// </summary>
        public override void Shoot()
        {
            MouseState mouseState = Mouse.GetState();
            Point mousePoint = new Point(mouseState.X, mouseState.Y);
            Player player = Game1.playerManager.PlayerObject;

            //If cooldown is greater than 0 still, cannot shoot so returns OR if player is clicking within the bounds of the player sprite, returns
            if (CurrentCooldown > 0 || player.ObjectRect.Contains(mousePoint))
            {
                return;
            }

            float angle = (float)((2 * Math.PI) - CurrentAngle);

            new Projectile(objectRect.X, objectRect.Y, 20, 20, projectileTexture, true, bulletSpeed, angle, damage, 8.5f, 10f, true, true, true);

            CurrentCooldown = CooldownAmt;
        }

        /// <summary>
        /// Updates the cooldown based on gameTime
        /// </summary>
        /// <param name="gameTime"></param>
        public override void UpdateCooldown(GameTime gameTime)
        {
            if(CurrentCooldown == 0)
            {
                return;
            }

            CurrentCooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        /// <summary>
        /// Updates the cooldown by setting it to a passed amount
        /// </summary>
        /// <param name="amount"></param>
        public override void UpdateCooldown(int amount)
        {
            CurrentCooldown = amount;
        }
    }
}
