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
        Texture2D spriteFont;
        Vector2 fontPos;

        /// <summary>
        /// Param Constuctor initializes fields
        /// </summary>
        /// <param name="spriteFont"></param> the font texture of the text
        /// <param name="fontPos"></param> text location vector
        public Text(Texture2D spriteFont, Vector2 fontPos)
        {
            this.spriteFont = spriteFont;
            this.fontPos = fontPos;
        }

    }
}
