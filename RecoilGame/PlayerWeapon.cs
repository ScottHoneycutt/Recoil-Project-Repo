//Trevor Dunn       3/17/21
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecoilGame
{
    public abstract class PlayerWeapon : GameObject
    {
        private float cooldownAmt;
        private float currentCooldown;
        private float currentAngle;
        private SpriteEffects weaponEffect;

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

        /// <summary>
        /// Property for the weapon's current angle
        /// </summary>
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
            cooldownAmt = 0;
            currentCooldown = 0;

            Game1.weaponManager.UpdateRotation();

            weaponEffect = SpriteEffects.None;
        }


        //METHODS

        /// <summary>
        /// Abstract Method For Shoot, Must Be Overriden By Weapons
        /// </summary>
        public abstract void Shoot();

        /// <summary>
        /// Abstract Method For Cooldown Update, takes in gameTime
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void UpdateCooldown(GameTime gameTime);


        /// <summary>
        /// Abstract method for cooldown update, takes in a fixed amount
        /// </summary>
        /// <param name="amount"></param>
        public abstract void UpdateCooldown(int amount);

        /// <summary>
        /// Overridden draw method
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="tint"></param>
        public override void Draw(SpriteBatch sb, Color tint)
        {
            Game1.weaponManager.UpdatePosition();

            Point origin = new Point(5, 10);

            Game1.weaponManager.UpdateRotation();

            MouseState mouse = Mouse.GetState();

            if(mouse.X < Game1.playerManager.PlayerObject.CenteredX)
            {
                weaponEffect = SpriteEffects.FlipVertically;
            }
            else if(mouse.X >= Game1.playerManager.PlayerObject.CenteredX)
            {
                weaponEffect = SpriteEffects.None;
            }

            sb.Draw(sprite, objectRect, null, tint, currentAngle, origin.ToVector2(), weaponEffect, 0.0f);
        }
    }
}
