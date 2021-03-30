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
        /// Creates a new Projectile and Adds it to ListOfProjectiles
        /// </summary>
        public override void Shoot()
        {
            //Test to see if this will actually create a projectile and how it will work, then we'll add more since we want shotgun to have multiple projectiles
            new Projectile(ObjectRect.Right, this.CenteredY, 20, 20, projectileTexture, true, new Vector2(ObjectRect.Right, this.CenteredY), 20, 5, 10, false, true);

            //throw new NotImplementedException();


        }
    }
}
