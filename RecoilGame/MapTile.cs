using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RecoilGame
{
    public class MapTile : GameObject
    {
        public MapTile(int xPosition, int yPosition, int width, int height, Texture2D texture, bool active)
            : base (xPosition, yPosition, width, height, texture, active)
        {
            
        }
    }
}
