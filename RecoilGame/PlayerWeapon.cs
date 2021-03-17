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
        private int numOfProjectiles;

        //CONSTRUCTOR

        /// <summary>
        /// Constructor for PlayerWeapon
        /// </summary>
        /// <param name="sprite">Texture For Weapon's Sprite</param>
        /// <param name="rectangle">Rectangle For Weapon's Position</param>
        /// <param name="isActive">Bool For If Weapon Is Active</param>
        /// <param name="cooldown">Float For Weapon's Cooldown</param>
        /// <param name="numOfProjectiles">Int For Number of Projectiles</param>
        public PlayerWeapon (Texture2D sprite, Rectangle rectangle, bool isActive, float cooldown, int numOfProjectiles) : base(sprite, rectangle, isActive)
        {
            this.cooldown = cooldown;
            this.numOfProjectiles = numOfProjectiles;
        }

        //METHODS

        /// <summary>
        /// Abstract Method For Shoot, Must Be Overriden By Weapons
        /// </summary>
        public abstract void Shoot();
    }
}
