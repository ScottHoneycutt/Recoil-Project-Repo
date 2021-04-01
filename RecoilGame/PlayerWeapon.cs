//Trevor Dunn       3/17/21
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecoilGame
{
    abstract class PlayerWeapon : GameObject
    {
        //Fields
        private float cooldown;
        private float currentCooldown;
        private int numOfProjectiles;
        private int damage;


        //CONSTRUCTOR

        /// <summary>
        /// PlayerWeapon Constructor
        /// </summary>
        /// <param name="xPos">X Position of Weapon</param>
        /// <param name="yPos">Y Position of Weapon</param>
        /// <param name="width">Width of Weapon</param>
        /// <param name="height">Height of Weapon</param>
        /// <param name="sprite">Texture2D For Weapon Sprite</param>
        /// <param name="isActive">Bool For If Weapon Is Active</param>
        /// <param name="cooldown">Float For Weapon Cooldown</param>
        /// <param name="numOfProjectiles">Int For Number Of Projectiles</param>
        public PlayerWeapon(int xPos, int yPos, int width, int height, Texture2D sprite, 
            bool isActive, float cooldown, int numOfProjectiles, int damage) : 
            base(xPos, yPos, width, height, sprite, isActive)
        {
            this.cooldown = cooldown;
            this.numOfProjectiles = numOfProjectiles;
            currentCooldown = 0;
        }


        //PROPERTIES

        /// <summary>
        /// Property That Returns Weapon's Cooldown Amount when shot
        /// </summary>
        public float CooldownAmount
        {
            get { return cooldown; }
        }

        /// <summary>
        /// Property For Weapon's Projectiles
        /// </summary>
        public int NumOfProjectiles
        {
            get { return numOfProjectiles; }
        }

        /// <summary>
        /// Property that sets or gets the weapon's current cooldown
        /// </summary>
        public float CurrentCooldown
        {
            get { return currentCooldown; }
            set { currentCooldown = value; }
        }

        /// <summary>
        /// Property that sets or gets the weapon's damage
        /// </summary>
        public int Damage
        {
            get { return damage; }
            set { damage = value; }
        }


        //METHODS

        /// <summary>
        /// Abstract Method For Shoot, Must Be Overriden By Weapons
        /// </summary>
        public abstract void Shoot();

        public abstract void UpdateCooldown(GameTime gameTime);
    }
}
