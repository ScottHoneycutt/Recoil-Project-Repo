using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RecoilGame
{
    //Aidan 
    //3/19/2021
    //Created base structure with fields and constructor
    class HealthPack : GameObject
    {
        //fields

        private int healing;

        /// <summary>
        /// Param Constructor passes all fields to base constructor and sets healing value;
        /// </summary>
        /// <param name="xPosition"></param> x position of health pack
        /// <param name="yPosition"></param> y position of health pack
        /// <param name="width"></param> width of pack
        /// <param name="height"></param> height of pack
        /// <param name="texture"></param>
        /// <param name="isActive"></param>
        /// <param name="healing"></param> amount of health the pack heals
        public HealthPack(int xPosition, int yPosition, int width, int height, Texture2D texture, bool isActive, int healing)
            : base(xPosition, yPosition, width, height, texture, isActive)
        {
            this.healing = healing;
        }

    }
}
