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
        private float cooldownAmt;
        private float currentCooldown;
        private float currentAngle;


        public WeaponType Type
        {
            set { weaponType = value; }
            get { return weaponType; }
        }

        /// <summary>
        /// Property for the amount of cooldown caused when shooting
        /// </summary>
        public float CooldownAmt
        {
            get { return cooldownAmt; }
            set { cooldownAmt = value; }
        }

        /// <summary>
        /// Property for the weapons current cooldown value
        /// </summary>
        public float CurrentCooldown
        {
            get { return currentCooldown; }
            set { currentCooldown = value; }
        }

        public float CurrentAngle
        {
            get { return currentAngle; }
            set { currentAngle = value; }
        }

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
            bool isActive) : base(xPos, yPos, width, height, sprite, isActive)
        {
            this.cooldownAmt = 0;
            this.currentCooldown = 0;

            Game1.weaponManager.UpdateRotation();
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

        public abstract void UpdateCooldown(int amount);

        public override void Draw(SpriteBatch sb, Color tint)
        {
            Game1.weaponManager.UpdatePosition();
            System.Diagnostics.Debug.WriteLine(this.Position.Y);
            System.Diagnostics.Debug.WriteLine(this.ObjectRect.Y);

            Rectangle weaponRect = objectRect;

            Point origin = new Point(0, weaponRect.Height/2);

            Game1.weaponManager.UpdateRotation();

            sb.Draw(sprite, weaponRect, null, tint, currentAngle, origin.ToVector2(), SpriteEffects.None, 0.0f);
        }
    }
}
