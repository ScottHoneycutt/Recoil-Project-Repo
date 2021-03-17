//Trevor Dunn       3/17/21
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecoilGame
{
    class GameObject
    {
        //Fields
        private Texture2D sprite;
        private Rectangle rectangle;
        private bool isActive;

        //CONSTRUCTOR

        /// <summary>
        /// GameObject's Constructor
        /// </summary>
        /// <param name="sprite">Texture For Object</param>
        /// <param name="rectangle">Rectangle For Object</param>
        /// <param name="isActive">Set Whether Object Is Active</param>
        public GameObject(Texture2D sprite, Rectangle rectangle, bool isActive)
        {
            this.sprite = sprite;
            this.rectangle = rectangle;
            this.isActive = isActive;
        }

        //PROPERTIES

        /// <summary>
        /// Rectangle Property
        /// </summary>
        public Rectangle Rectangle
        {
            get { return rectangle; }
            set { rectangle = value; }
        }

        /// <summary>
        /// IsActive Property
        /// </summary>
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
    }
}
