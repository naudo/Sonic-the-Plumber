using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XSonic.Characters
{
    class Goomba : Drawable
    {
        protected double timeSpan = 0.0;
        protected int animationFrame = 0;
        protected int velocityx = -1;

        public Goomba(Vector2 location, Vector2 size)
        {
            Size = size;
            location.Y += 7;
            Location = location;
        }

        public override void Update(double ms)
        {
            timeSpan += ms;
            if (timeSpan > 200)
            {
                animationFrame++;
                if (animationFrame > 1)
                    animationFrame = 0;
                timeSpan = 0;
            }

            Vector2 tloc = Location;
            Location = new Vector2(Location.X + (float)velocityx, Location.Y);
            // collision detection
            foreach (Drawable d in ParentLevel.LevelObjects)
            {
                if (!d.IsCollidable) continue;
                if (d == this) continue;
                Hit c = Collision.Intersects(GetCollisionBox, d.GetCollisionBox);
                if ((c & Hit.Left) == Hit.Left || (c & Hit.Right) == Hit.Right)
                {
                    if (velocityx < 0) velocityx = 1;
                    else velocityx = -1;
                    break;
                }
            }
            Location = new Vector2(tloc.X + (float)velocityx, tloc.Y);
        }

        public override Microsoft.Xna.Framework.Graphics.SpriteEffects Effects
        {
            get
            {
                return animationFrame == 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            }
        }

        public override Microsoft.Xna.Framework.Rectangle SourceRectangle
        {
            get { return new Rectangle(0, 97, 48, 56); }
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
                return true;
            }
        }

        public override Rectangle GetCollisionBox
        {
            get
            {
                return new Rectangle((int)this.Location.X, (int)this.Location.Y - (this.SourceRectangle.Height - 48), 48, 48);
            }
        }
    }
}
