//Aidan Kamp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecoilGame
{
    public class GameObject
    {
        protected Rectangle objectRect;
        protected Vector2 position;
        protected Texture2D sprite;
        protected bool isActive;

        //Get property for the Rectangle object for collision detection later----
        public Rectangle ObjectRect
        {
            get
            {
                return objectRect;
            }
        }

        //Centered coordinate properties for the rectangle for easier use later----
        public int CenteredX
        {
            get
            {
                return objectRect.X + objectRect.Width / 2;
            }
            set
            {
                objectRect.X = value - objectRect.Width / 2;
            }
        }
        public int CenteredY
        {
            get
            {
                return objectRect.Y + objectRect.Height / 2;
            }
            set
            {
                objectRect.Y = value - objectRect.Height / 2;
            }
        }

        // Having isActive a property for easy drawing purposes
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        /// <summary>
        /// Creates a new basic GameObject----
        /// </summary>
        /// <param name="xPosition">The X coordinate of the object's rectangle (top left corner)----</param>
        /// <param name="yPosition">The Y coordinate of the object's rectangle (top left corner)----</param>
        /// <param name="width">The width of the rectangle----</param>
        /// <param name="height">The heigh of the rectangle----</param>
        /// <param name="texture">The texture to be displayed in the rectangle----</param>
        public GameObject(int xPosition, int yPosition, int width, int height, Texture2D texture , bool active)
        {
            sprite = texture;
            objectRect = new Rectangle(xPosition, yPosition, width, height);
            position = new Vector2(xPosition, yPosition);
            isActive = active;
        }

    }
}
