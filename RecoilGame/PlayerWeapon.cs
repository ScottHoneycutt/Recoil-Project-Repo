//Trevor Dunn       3/17/21
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecoilGame
{
    public enum WeaponType
    {
        Shotgun,
        RocketLauncher,
        MachineGun,
        Sniper
    }

    public abstract class PlayerWeapon : GameObject
    {
        private WeaponType weaponType;

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
            bool isActive) : base(xPos, yPos, width, height, sprite, isActive) { }

        public WeaponType Type
        {
            set { weaponType = value; }
            get { return weaponType; }
        }


        //METHODS

        /// <summary>
        /// Abstract Method For Shoot, Must Be Overriden By Weapons
        /// </summary>
        public abstract void Shoot();

        /// <summary>
        /// Abstract Method For Cooldown Update
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void UpdateCooldown(GameTime gameTime);

        public override void Draw(SpriteBatch sb, Color tint)
        {
            base.Draw(sb, tint);
        }
    }
}
