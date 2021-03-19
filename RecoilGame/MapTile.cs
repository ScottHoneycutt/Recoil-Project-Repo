using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//Aidan 
//3/19/2021
//Created base structure with fields and constructor

namespace RecoilGame
{
    class MapTile : GameObject
    {
        //fields

        //MapTile can be set to act as an objective which triggers next level on contact
        private bool isObjective;

        /// <summary>
        /// Param constructor uses base constructor from GameObject class and sets the isObjective field
        /// </summary>
        /// Rectangle location params
        /// <param name="xPosition"></param> 
        /// <param name="yPosition"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="texture"></param> sprite texture
        /// <param name="isActive"></param> whether the tile appears on screen
        /// <param name="isObjective"></param> whether the tile acts an objective
        public MapTile(int xPosition, int yPosition, int width, int height, Texture2D texture, bool isActive, bool isObjective)
            : base(xPosition, yPosition, width, height, texture, isActive)
        {
            this.isObjective = isObjective;
        }
    }
}
