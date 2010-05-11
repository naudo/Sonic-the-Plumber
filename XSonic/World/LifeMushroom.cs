using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XSonic.Audio;

namespace XSonic.World
{
    class LifeMushroom: Drawable
    {
        private double velocityY;
        private double velocityX;
        private double vxBase;

        private Texture2D spriteSheet;

        private static Texture2D wss;

        public LifeMushroom(Vector2 location, Vector2 size)
        {
            spriteSheet = XSonicGame.WorldSS;
            this.Location = location;
            this.Size = size;
            vxBase = 90.0f;
        }

        public override Microsoft.Xna.Framework.Rectangle SourceRectangle
        {
            get { return new Rectangle(144, 0, 48, 48); }
        }

        public override Microsoft.Xna.Framework.Graphics.Texture2D SpriteSheet
        {
            get { return spriteSheet; }
            set { spriteSheet = value; }
        }

        public override bool IsCollidable
        {
            get
            {
                return true;
            }
        }

        public override void Update(double ms)
        {
            velocityY -= 5.5 * (ms / 1000.0f);
            velocityX = vxBase * (ms / 1000.0f);

            Vector2 tloc = Location;
            Location = new Vector2(Location.X + (float)velocityX, Location.Y + (float)velocityY);
            // collision detection
            foreach (Drawable d in ParentLevel.LevelObjects)
            {
                if (!d.IsCollidable) continue;
                if (d == this) continue;
                if (d is LifeMushroom) continue;
                Hit c = Collision.Intersects(GetCollisionBox, d.GetCollisionBox);
                if ((c & Hit.Left) == Hit.Left || (c & Hit.Right) == Hit.Right)
                {
                    vxBase = 0 - vxBase;
                }
                if ((c & Hit.Top) == Hit.Top)
                    velocityY = 0;
            }
            Location = new Vector2(tloc.X + (float)velocityX, tloc.Y + (float)velocityY);
        }

        public override bool IsSpecialHit
        {
            get
            {
                return true;
            }
        }

        public override bool WasHit(Hit h)
        {
            return true;
        }

        public override void TakeHit(XSonic.Characters.Sonic player, GameLevel level, out bool blocking)
        {
            blocking = false;
            player.Lives++;
            (XSonicGame.CurrentGame.Services.GetService(typeof(AudioManager)) as AudioManager).PlaySound("1up");
            level.RemoveObjects.Add(this);
        }
    }
}
