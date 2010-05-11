using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XSonic.Characters;
using XSonic.Drawing;
using XSonic.Audio;

namespace XSonic.World
{
    class QuestionMarkBlock : Drawable
    {
        protected bool active;
        public QuestionMarkBlock(Vector2 location, Vector2 size)
        {
            Size = size;
            Location = location;
            active = true;
        }

        public override Microsoft.Xna.Framework.Rectangle SourceRectangle
        {
            get { return new Microsoft.Xna.Framework.Rectangle(active ? 96 : 0, 48, 48, 48); }
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

        public override bool IsSpecialHit
        {
            get
            {
                return active;
            }
        }
        public override bool WasHit(Hit h)
        {
            return ((h & XSonic.Hit.Bottom) == XSonic.Hit.Bottom) && active;
        }
        public override void TakeHit(Sonic hitter, GameLevel level, out bool blocking)
        {
            blocking = true;
            hitter.Coins++;
            active = false;
            MovingAnchoredAnimation maa = new MovingAnchoredAnimation(3.0f, new Point[] { new Point((int)this.Location.X, (int)this.Location.Y+48), new Point((int)this.Location.X, (int)this.Location.Y + 144) }, 16, 0, 3, 0, 48, 48, Coin.SpinCoin, true);
            
            (XSonicGame.CurrentGame.Services.GetService(typeof(AnimationManager)) as AnimationManager).Add(maa);
            (XSonicGame.CurrentGame.Services.GetService(typeof(AudioManager)) as AudioManager).PlaySound("coins");
        }
    }
}
