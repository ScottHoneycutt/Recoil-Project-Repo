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

        private int maxHealth;
        private int health;

        //Second health stat to retain the original if god mode is enabled and then disabled----
        private int originalHealth;

        //Health properties----
        public int Health
        {
            get
            {
                return health;
            }
        }
        public int MaxHealth
        {
            get
            {
                return maxHealth;
            }
        }


        /// <summary>
        /// Param Constructor for Player Object
        /// </summary>
        /// <param name="x"></param> x position of player rectangle
        /// <param name="y"></param> y position of player rectangle
        /// <param name="width"></param> width of rectangle
        /// <param name="height"></param> height of rectangle
        /// <param name="texture"></param> player texture
        /// <param name="maxHealth">Player's maximum health value----</param> 
        public Player(int x, int y, int width, int height, Texture2D texture, bool isActive, int maxHealth)
            : base(x, y, width, height, texture, isActive)

        {
            this.maxHealth = maxHealth;
            health = maxHealth;
            originalHealth = maxHealth;
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
                //If the player is dead (has no remaining health), reset the current level----
            }
            else
            {
                health -= damage;
            }
        }
        /// <summary>
        /// Used for player animation, takes in sprite effect and properly flips the player depending on direction the player is facing
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="tint"></param>
        /// <param name="flip"></param>
        public void DrawSpecial(SpriteBatch sb, Texture2D sprite, SpriteEffects flip)
        {
            if (isActive)
            {
                sb.Draw(
                    sprite, 
                    position, 
                    new Rectangle(0, 0, objectRect.Width, objectRect.Height), 
                    Color.White, 
                    0.0f, 
                    Vector2.Zero, 
                    1.0f, 
                    flip, 
                    0.0f);
            }
        }

        /// <summary>
        /// Toggles god mode on or off. If on, the player gets the maximum integer value for their health----
        /// </summary>
        /// <param name="isEnabled">Whether or not god mode is on----</param>
        public void SetGodMode(bool isEnabled)
        {
            if (isEnabled)
            {
                health = int.MaxValue;
                maxHealth = int.MaxValue;
            }
            else
            {
                health = originalHealth;
                maxHealth = originalHealth;
            }
        }
    }
}
