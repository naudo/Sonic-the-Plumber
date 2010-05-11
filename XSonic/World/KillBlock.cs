using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XSonic.World
{
    class KillBlock : Drawable
    {
        Texture2D sprite;

        public KillBlock(Vector2 location, Vector2 size)
        {
            Size = size;
            Location = location;
        }

        public override Microsoft.Xna.Framework.Rectangle SourceRectangle
        {
            get { return new Microsoft.Xna.Framework.Rectangle(0, 0, 0, 0); }
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
        public override bool IsHarmful
        {
            get
            {
                return false;
            }
        }
        public override bool IsKill
        {
            get
            {
                return false;
            }
        }
    }
}
