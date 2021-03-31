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
        /// <param name="numOfProjectiles">Int For Number Of Shotgun Projectiles</param>
        /// <param name="playerRecoil">Float For Shotgun's Recoil</param>
        public Shotgun(int xPos, int yPos, int width, int height, Texture2D sprite, bool isActive, 
            float cooldown, int numOfProjectiles, float playerRecoil, Texture2D projectileTexture) 
            : base(xPos, yPos, width, height, sprite, isActive, cooldown, numOfProjectiles)
        {
            this.playerRecoil = playerRecoil;
            this.projectileTexture = projectileTexture;
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
        /// Determines the direction of the projectile based on mouse's location dependent on player's location and creates a projectile
        /// </summary>
        public override void Shoot()
        {
            //If cooldown is greater than 0 still, cannot shoot so returns
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

            float bulletSpeed = 8;

            //Creates a new vector2 by multiplying the normalized values by bulletspeed
            Vector2 direction = new Vector2(xNormalized*bulletSpeed, yNormalized*bulletSpeed);

            //Test to see if this will actually create a projectile and how it will work, then we'll add more since we want shotgun to have multiple projectiles
            new Projectile(player.CenteredX, player.CenteredY, 7, 7, projectileTexture, true, direction, 20, 5, 40, false, true);

            //Calls playerManager's shooting capability method
            Game1.playerManager.ShootingCapability();

            //Sets the cooldown
            this.CurrentCooldown = CooldownAmount;
        }

        //Update cooldown method
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
    }
}
