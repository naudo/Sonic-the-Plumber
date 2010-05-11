using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XSonic.World
{
    class BonusFlag : Drawable
    {
        public BonusFlag(Vector2 location, Vector2 size)
        {
            Size = size;
            Location = location;
        }

        public override Microsoft.Xna.Framework.Rectangle SourceRectangle
        {
            get { return new Microsoft.Xna.Framework.Rectangle(49 * 4, 0, 48, 48); }
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
        public override bool IsBonus
        {
            get
            {
                return true;
            }
        }
    }
}
