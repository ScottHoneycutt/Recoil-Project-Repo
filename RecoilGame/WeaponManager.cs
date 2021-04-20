using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RecoilGame
{
    public class WeaponManager
    {
        private LinkedList<PlayerWeapon> weapons;
        private PlayerWeapon currentWeapon;
        private List<Texture2D> weaponTextures;
        private List<Texture2D> projectileTextures;
        private Texture2D crosshairSprite;
        private Texture2D shotgunTexture;
        private Texture2D rpgTexture;

        public WeaponManager(Game1 game)
        {
            weapons = new LinkedList<PlayerWeapon>();
            currentWeapon = null;
            weaponTextures = new List<Texture2D>();
            projectileTextures = new List<Texture2D>();

            weaponTextures.Add(game.Content.Load<Texture2D>("shotgunSprite"));
            weaponTextures.Add(game.Content.Load<Texture2D>("rpgSprite"));

            projectileTextures.Add(game.Content.Load<Texture2D>("bulletTexture"));
            projectileTextures.Add(game.Content.Load<Texture2D>("rocketTexture"));
            crosshairSprite = game.Content.Load<Texture2D>("crosshair");
        }


        public PlayerWeapon CurrentWeapon
        {
            get
            {
                return currentWeapon;
            }
            set
            {
                currentWeapon = value;
            }
        }

        public LinkedList<PlayerWeapon> Weapons
        {
            get
            {
                return weapons;
            }
        }

        /// <summary>
        /// Adds a new weapon based on the current level number
        /// </summary>
        /// <param name="currentLevel"></param>
        public void AddWeapon(int currentLevel)
        {
            switch(currentLevel)
            {
                case 1:

                    currentWeapon = new Shotgun((int)Game1.playerManager.PlayerObject.XPos, (int)Game1.playerManager.PlayerObject.YPos, 50, 20, weaponTextures[0], true, projectileTextures[0]);

                    weapons.AddFirst(currentWeapon);

                    break;

                case 2:
                    System.Diagnostics.Debug.WriteLine(projectileTextures[0]);
                    RocketLauncher rocketLauncher = new RocketLauncher(0, 0, 50, 20, weaponTextures[1], true, projectileTextures[1]);

                    weapons.AddAfter(weapons.Last, rocketLauncher);

                    currentWeapon = rocketLauncher;

                    break;
                /*
                case 3:

                    MachineGun machineGun = new MachineGun(0, 0, 50, 20, weaponTextures[2], true, projectileTextures[2]);

                    weapons.AddAfter(weapons.Last, machineGun);

                    currentWeapon = machineGun;

                    break;

                case 4:

                    Sniper sniper = new Sniper(0, 0, 50, 20, weaponTextures[3], true, projectileTextures[3]);

                    weapons.AddAfter(weapons.Last, sniper);

                    currentWeapon = sniper;

                    break;
                */
                default:
                    break;
            }
        }

        /// <summary>
        /// Switches the weapon based on the mouseWheel values
        /// </summary>
        /// <param name="currentWheelValue"></param>
        /// <param name="prevWheelValue"></param>
        public void SwitchWeapon(int currentWheelValue, int prevWheelValue)
        {
            if(currentWheelValue < prevWheelValue)
            {
                if(weapons.Find(currentWeapon).Previous == null)
                {
                    CurrentWeapon = weapons.Last.Value;
                }
                else
                {
                    CurrentWeapon = weapons.Find(currentWeapon).Previous.Value;
                }
            }
            else
            {
                if(weapons.Find(currentWeapon).Next == null)
                {
                    CurrentWeapon = weapons.First.Value;
                }
                else
                {
                    CurrentWeapon = weapons.Find(currentWeapon).Next.Value;
                }
            }
        }
        /// <summary>
        /// draws the current weapon held
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="tint"></param>
        public void Draw(SpriteBatch sb, Color tint)
        {
            currentWeapon.Draw(sb, tint);
        }

        public void UpdatePosition()
        {
            foreach(PlayerWeapon weapon in weapons)
            {
                int x = (int)Game1.playerManager.PlayerObject.CenteredX;
                
                int y = (int)Game1.playerManager.PlayerObject.CenteredY;

                Vector2 newPos = new Vector2(x, y);

                weapon.Position = newPos;

                weapon.ConvertPosToRect();
            }
        }

        /// <summary>
        /// draws a sprite of a crosshair where the mouse is
        /// </summary>
        /// <param name="sb"></param>
        public void DrawCrosshair(SpriteBatch sb)
        {
            MouseState mouseState = Mouse.GetState();
            int x = mouseState.X;
            int y = mouseState.Y;
            sb.Draw(crosshairSprite,
                new Rectangle(x - 15, y - 15, 30, 30),
                Color.White);
        }

        public void UpdateRotation()
        {
            if(CurrentWeapon != null)
            {
                Game1.weaponManager.UpdatePosition();

                MouseState mouse = Mouse.GetState();

                Vector2 mousePosition = new Vector2(mouse.X, mouse.Y);

                //Converting y coordinates to more conventional coordinates (where up is + and down is -)----
                mousePosition.Y = mousePosition.Y + (500-mousePosition.Y)*2;
                Vector2 adjustedWeaponPos = new Vector2(currentWeapon.Position.X, 
                    currentWeapon.Position.Y + (500 - currentWeapon.Position.Y) * 2); 

                Vector2 distancePosition = adjustedWeaponPos - mousePosition;

                //Calculating angle for the right side of the player----
                float rotation = (360/(2*MathF.PI))*MathF.Asin(distancePosition.Y/distancePosition.Length());

                //Flipping rotation across the y axis if the cursor is on the left side of the player to correct the angle----
                if (distancePosition.X > 0)
                {
                    if (distancePosition.Y > 0)
                    {
                        rotation = rotation + 2*(90 - rotation);
                    }
                    else
                    {
                        rotation = rotation + 2*(90 - rotation);
                    }
                }

                //Converting rotation into radians so that it can be used again----
                rotation = rotation / 360 * (2 * MathF.PI);

                foreach (PlayerWeapon weapon in weapons)
                {
                    weapon.CurrentAngle = rotation;
                }
            }
        }
    }
}
