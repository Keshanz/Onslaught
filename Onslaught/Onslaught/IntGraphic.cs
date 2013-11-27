using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Onslaught
{
    class IntGraphic
    {
        //point of the top left corner of the graphic
        Vector2 loc;
        Texture2D graphic;

        public IntGraphic(Texture2D graph, Vector2 locus)
        {
            graphic = graph;
            loc = locus;
        }
        
        public void SetLoc(float x, float y)
        {
            loc = new Vector2(x, y);
        }

        public Vector2 GetLoc()
        {
            return loc;
        }

        public Texture2D GetTexture()
        {
            return graphic;
        }

        public Boolean Contains(float x, float y)
        {
            Vector2 testPt = new Vector2(x, y);
            if (x >= loc.X && x <= loc.X + graphic.Width && y >= loc.Y && y <= loc.Y+graphic.Height) return true;
            else return false;
        }

        

    }


}
