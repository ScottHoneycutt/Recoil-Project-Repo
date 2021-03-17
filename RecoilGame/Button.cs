using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RecoilGame
{
    // Name: Jack Walsh
    // Date: 3/17/2021
    // Purpose: Create a button for the game UI
    class Button
    {
        //Variables
        private Texture2D normalSprite;
        private Texture2D hoverSprite;
        private Rectangle position;

        /// <summary>
        /// Constructor for button using components of rectangle
        /// </summary>
        /// <param name="normalSprite"> Image for when button is NOT hovered over </param>
        /// <param name="hoverSprite"> Image for when button IS hovered over </param>
        /// <param name="x"> X position of topleft corner of button </param>
        /// <param name="y"> Y position of topleft corner of button </param>
        /// <param name="width"> Width of button </param>
        /// <param name="height"> Height of button </param>
        public Button(Texture2D normalSprite, Texture2D hoverSprite,
            int x, int y, int width, int height) :
            this(normalSprite, hoverSprite, new Rectangle(x, y, width, height))
        {
            //constructor uses rectangle constructor
        }

        /// <summary>
        /// Constructor for button using premade rectangle
        /// </summary>
        /// <param name="normalSprite"> Image for when button is NOT hovered over </param>
        /// <param name="hoverSprite"> Image for when button IS hovered over </param>
        /// <param name="position"> Rectangle containing button </param>
        public Button(Texture2D normalSprite, Texture2D hoverSprite, Rectangle position)
        {
            this.normalSprite = normalSprite;
            this.hoverSprite = hoverSprite;
            this.position = position;
        }

        /// <summary>
        /// Draws button to screen
        /// </summary>
        /// <param name="sb"> SpriteBatch button is in </param>
        public void Draw(SpriteBatch sb)
        {
            MouseState mouseState = Mouse.GetState();

            //if x and y pos of mouse is within position width / height
            if (mouseState.X >= position.X &&
                mouseState.X <= position.X + position.Width &&
                mouseState.Y >= position.Y &&
                mouseState.Y <= position.Y + position.Height)
            {
                sb.Draw(hoverSprite, position, Color.White);
            }
            else
            {
                sb.Draw(normalSprite, position, Color.White);
            }
        }
    }
}
