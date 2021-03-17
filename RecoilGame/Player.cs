using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text;

namespace RecoilGame
{
    class Player : GameObject
    {
        //Aidan 3/17/2021

        //Fields

        private List<PlayerWeapon> weaponList;
        private PlayerWeapon currentWeapon;
        private float health;
        private Vector2 velocity;

        /// <summary>
        /// Param Constructor for Player Object
        /// </summary>
        /// <param name="x"></param> x position of player rectangle
        /// <param name="y"></param> y position of player rectangle
        /// <param name="width"></param> width of rectangle
        /// <param name="height"></param> height of rectangle
        /// <param name="texture"></param> player texture
        /// <param name="velocity"></param> player velocity
        /// <param name="health"></param> player health value
        public Player(int x, int y, int width, int height, Texture2D texture, Vector2 velocity, float health)
            : base(x, y, width, height, texture)

        {
            this.health = health;
            this.velocity = velocity;
        }
    }
}
