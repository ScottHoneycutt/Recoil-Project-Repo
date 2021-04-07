﻿using System;
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

        public WeaponManager(Game1 game)
        {
            weapons = new LinkedList<PlayerWeapon>();
            currentWeapon = null;
            weaponTextures = new List<Texture2D>();
            projectileTextures = new List<Texture2D>();

            for(int x = 0; x < 4; x++)
            {
                weaponTextures.Add(game.Content.Load<Texture2D>("square"));
            }

            for(int x = 0; x < 4; x++)
            {
                projectileTextures.Add(game.Content.Load<Texture2D>("square"));
            }
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

        public void AddWeapon(int currentLevel)
        {
            switch(currentLevel)
            {
                case 1:

                    currentWeapon = new Shotgun(0, 0, 50, 20, weaponTextures[0], true, projectileTextures[0]);

                    weapons.AddFirst(currentWeapon);

                    break;

                case 2:

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
            else if(currentWheelValue > prevWheelValue)
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

        public void Draw(SpriteBatch sb, Color tint)
        {
            currentWeapon.Draw(sb, tint);
        }
    }
}
