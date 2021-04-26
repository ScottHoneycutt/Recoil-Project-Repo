using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RecoilGame
{
    public class Objective : GameObject
    {
        public Objective(int xPosition, int yPosition, int width, int height, Texture2D texture, bool active)
        : base(xPosition, yPosition, width, height, texture, active)
        {

        }
    }
}
