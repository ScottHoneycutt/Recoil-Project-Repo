using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text;

namespace RecoilGame
{
    public class Player : GameObject, IDamageable
    {
        //Aidan 
        //3/17/2021
        //Created base structure with fields and constructor
        //
        //3/22/21
        //Removed velocity vector from player as it is now handled in the manager

        //Fields


        private int health;

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
        public Player(int x, int y, int width, int height, Texture2D texture, bool isActive, int health)
            : base(x, y, width, height, texture, isActive)

        {
            this.health = health;
        }

        /// <summary>
        /// Method that allows the player to take damage
        /// </summary>
        /// <param name="damage"></param> damage that the player will take
        public void TakeDamage(int damage)
        {
            //if the damage reduces the health to or below 0, health = 0
            if ((health - damage) <= 0)
            {
                health = 0;
            }
            else
            {
                health -= damage;
            }
        }
    }
}
