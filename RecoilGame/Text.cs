using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//Aidan Kamp
//3/19/2021
//Created base structure with fields and constructor

namespace RecoilGame
{
    class Text
    {
        //fields
        private SpriteFont spriteFont;
        private Vector2 fontPos;
        private string text;

        //Property to change the string displayed----
        public string TextString
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }
        }

        /// <summary>
        /// Param Constuctor initializes fields
        /// </summary>
        /// <param name="spriteFont">the font texture of the text</param> 
        /// <param name="fontPos">text location vector</param> 
        /// <param name="text">The string displayed on screen----</param> 
        public Text(SpriteFont spriteFont, Vector2 fontPos, string text)
        {
            this.spriteFont = spriteFont;
            this.fontPos = fontPos;
            this.text = text;
        }


        /// <summary>
        /// Draws the text to the screen----
        /// </summary>
        /// <param name="sb">Spritebatch used to draw the text----</param>
        public void Draw(SpriteBatch sb)
        {
            sb.DrawString(spriteFont, text, fontPos, Color.White);
        }
    }
}
