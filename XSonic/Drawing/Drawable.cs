using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using XSonic.Characters;

namespace XSonic
{
    public abstract class Drawable
    {
        public virtual Vector2 Location { get; set; }
        public virtual Vector2 Size { get; set; }

        public abstract Rectangle SourceRectangle { get; }
        public abstract Texture2D SpriteSheet { get; set; }

        public virtual float Rotation { get { return 0f; } }
        public virtual float Scale { get { return 1f; } }
        public virtual Color Color { get { return Color.White; } }
        public virtual Vector2 Origin { get { return new Vector2(0, 0); } }
        public virtual SpriteEffects Effects { get { return SpriteEffects.None; } }
        public virtual float LayerDepth { get { return 0.5f; } }

        public virtual bool IsCollidable { get { return false; } }
        public virtual bool IsHarmful { get { return false; } }
        public virtual bool IsKill { get { return false; } }
        public virtual bool IsVictory { get { return false; } }
        public virtual bool IsBonus { get { return false; } }

        public virtual bool IsSpecialHit { get { return false; } }

        public virtual bool WasHit(Hit h) { return true; }

        public virtual void TakeHit(Sonic player, GameLevel level, out bool blocking) { blocking = true; return; }

        public virtual Rectangle GetCollisionBox { get { return new Rectangle((int)Location.X, (int)Location.Y, (int)Size.X, (int)Size.Y); } }
        public virtual GameLevel ParentLevel { get; set; }

        public virtual void Update(double ms) { }
    }
}
