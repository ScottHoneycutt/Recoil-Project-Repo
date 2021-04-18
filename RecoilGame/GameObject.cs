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
        //position vector coordinate positions
        public float XPos
        {
            get { return position.X; }
            set { position.X = value; }
        }

        public float YPos
        {
            get { return position.Y; }
            set { position.Y = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        //Centered coordinate properties for the rectangle for easier use later----
        public float CenteredX
        {
            get
            {
                return position.X + objectRect.Width / 2;
            }
            set
            {
                position.X = value - objectRect.Width / 2;
            }
        }
        public float CenteredY
        {
            get
            {
                return  position.Y + objectRect.Height / 2;
            }
            set
            {
                position.Y = value - objectRect.Height / 2;
            }
        }

        // Having isActive a property for easy drawing / moving purposes
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        
        //allows to get or change the sprite of the game object
        public Texture2D Sprite
        {
            get { return sprite; }
            set { sprite = value; }
        }
        /// <summary>
        /// Creates a new basic GameObject----
        /// </summary>
        /// <param name="xPosition">The X coordinate of the object's rectangle (top left corner)----</param>
        /// <param name="yPosition">The Y coordinate of the object's rectangle (top left corner)----</param>
        /// <param name="width">The width of the rectangle----</param>
        /// <param name="height">The heigh of the rectangle----</param>
        /// <param name="texture">The texture to be displayed in the rectangle----</param>
        /// <param name="active">Whether or not the object is currently active----</param>
        public GameObject(int xPosition, int yPosition, int width, int height, Texture2D texture , bool active)
        {
            sprite = texture;
            objectRect = new Rectangle(xPosition, yPosition, width, height);
            position = new Vector2(xPosition, yPosition);
            isActive = active;
        }

        /// <summary>
        /// Draws the GameObject if it isActive
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="tint"></param>
        public virtual void Draw(SpriteBatch sb, Color tint)
        {
            if (isActive)
            {
                sb.Draw(sprite, objectRect, tint);
            }
            
        }

        /// <summary>
        /// Aidan Kamp 3/22/21
        /// Converts the float position vector values into the rectangle position
        /// Should increase the accuracy of position
        /// </summary>
        public void ConvertPosToRect()
        {
            objectRect.X = (int)position.X;
            objectRect.Y = (int)position.Y;
        }
    }
}
