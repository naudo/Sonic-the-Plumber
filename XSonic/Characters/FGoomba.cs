using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XSonic.Characters
{
    class FGoomba: Goomba
    {
        protected float velocityY;

        public FGoomba(Vector2 location, Vector2 size)
            : base(location, size)
        {
            velocityY = 0.0f;
        }
        public override Microsoft.Xna.Framework.Rectangle SourceRectangle
        {
            get { return new Rectangle(49, 0, 48, 48); }
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

            velocityY -= (float)(5.5f * (ms / 1000.0f));

            Vector2 tloc = Location;
            Location = new Vector2(Location.X + (float)velocityx, Location.Y + velocityY);
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
                }
                if ((c & Hit.Top) == Hit.Top)
                    velocityY = 0.0f;
            }
            Location = new Vector2(tloc.X + (float)velocityx, tloc.Y + velocityY);
        }
    }
}
