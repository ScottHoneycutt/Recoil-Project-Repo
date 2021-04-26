//Trevor Dunn       3/17/21
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecoilGame 
{
    class Shotgun : PlayerWeapon
    {
        //Fields
        private float playerRecoil;
        private Texture2D projectileTexture;
        private Random rand;

        //CONSTRUCTOR

        /// <summary>
        /// Constructor For Shotgun
        /// </summary>
        /// <param name="xPos">X Position Of Shotgun</param>
        /// <param name="yPos">Y Position Of Shotgun</param>
        /// <param name="width">Width Of Shotgun</param>
        /// <param name="height">Height Of Shotgun</param>
        /// <param name="sprite">Texture For Shotgun Sprite</param>
        /// <param name="isActive">Bool For If Shotgun Is Active</param>
        /// <param name="cooldown">Float For Cooldown Of Shotgun</param>
        /// <param name="playerRecoil">Float For Shotgun's Recoil</param>
        public Shotgun(int xPos, int yPos, int width, int height, Texture2D sprite, bool isActive, Texture2D projectileTexture) 
        : base(xPos, yPos, width, height, sprite, isActive)
        {
            this.projectileTexture = projectileTexture;

            playerRecoil = 5;
            CooldownAmt = 2.0f;

            rand = new Random();
        }


        //PROPERTIES

        /// <summary>
        /// Property For Shotgun's Recoil
        /// </summary>
        public float PlayerRecoil
        {
            get { return playerRecoil; }
        }


        //METHODS

        /// <summary>
        /// Determines the direction of the projectile based on mouse's location and creates a projectile
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

            //sets the bullet's speed
            float bulletSpeed = 12f;

            float angle = (float)((2*Math.PI) - CurrentAngle);

            //Adding slight randomization to weapon spread----
            float spreadRandom;

            //Firing the primary projectile----
            new Projectile(objectRect.X, (objectRect.Y - (objectRect.Height / 2)), 10, 10, projectileTexture, true, bulletSpeed,
                angle, 15, 0, .5f, false, true, false);

            for (int x = 0; x < 4; x++)
            {
                spreadRandom = (float)(rand.NextDouble() * .2f) + 1;
                new Projectile(objectRect.X, (objectRect.Y - (objectRect.Height / 2)), 10, 10, projectileTexture, true, bulletSpeed,
                    angle - (0.05f*x * spreadRandom), 15, 0, .5f, false, true, false);
                spreadRandom = (float)(rand.NextDouble() * .2f) + 1;
                new Projectile(objectRect.X, (objectRect.Y - (objectRect.Height / 2)), 10, 10, projectileTexture, true, bulletSpeed,
                    angle + (0.05f*x * spreadRandom), 15, 0, .5f, false, true, false);
            }

            //Calls playerManager's shooting capability method
            Game1.playerManager.ShootingCapability();

            //Sets the cooldown
            CurrentCooldown = CooldownAmt;
        }

        /// <summary>
        /// Updates the weapon's cooldown using gameTime
        /// </summary>
        /// <param name="gameTime"></param>
        public override void UpdateCooldown(GameTime gameTime)
        {
            //If cooldown is already 0 returns
            if (CurrentCooldown == 0)
            {
                return;
            }
            
            //Subtracts elapsed time from current cooldown
            CurrentCooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        /// <summary>
        /// Updates the weapon's cooldown by setting it to a passed amount
        /// </summary>
        /// <param name="amount"></param>
        public override void UpdateCooldown(int amount)
        {
            CurrentCooldown = amount;
        }
    }
}
