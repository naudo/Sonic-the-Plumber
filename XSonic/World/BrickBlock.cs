using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XSonic.World
{
    class BrickBlock : Drawable
    {
        public BrickBlock(Vector2 location, Vector2 size)
        {
            Size = size;
            Location = location;
        }

        public override Microsoft.Xna.Framework.Rectangle SourceRectangle
        {
            get { return new Microsoft.Xna.Framework.Rectangle(48, 48, 48, 48); }
        }

        public override Microsoft.Xna.Framework.Graphics.Texture2D SpriteSheet
        {
            get;
            set;
        }
        public override bool IsCollidable
        {
            get
            {
                return true;
            }
        }
    }
}
