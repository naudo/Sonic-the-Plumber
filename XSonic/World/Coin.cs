using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using XSonic.Drawing;
using XSonic.Characters;
using XSonic.Audio;

namespace XSonic.World
{
    class Coin: Drawable
    {
        private static int NUMFRAMES = 4;
        private static int FRAMEROW = 0;
        private static int FRAMEW = 48;
        private static int FRAMEH = 48;

        public static Texture2D SpinCoin = null;

        private static double mp = -1.0f;
        private static int currentFrame;
        //private Animation animation;
        public Coin(Vector2 location)
        {
            if(mp == -1.0f) mp = 0.0f;
            if (SpinCoin == null)
                SpinCoin = (XSonicGame.CurrentGame.Services.GetService(typeof(AnimationManager)) as AnimationManager).Content.Load<Texture2D>("spincoin");
            this.Size = new Vector2(48, 48);
            this.Location = location;
            this.SpriteSheet = SpinCoin;
            //this.animation = new Animation(new Point((int)location.X, (int)location.Y),
            //    8, 0, 3, 0, 48, 48, SpinCoin, true);

            //(XSonicGame.CurrentGame.Services.GetService(typeof(AnimationManager)) as AnimationManager).Add(this.animation);
        }

        public override bool IsSpecialHit
        {
            get
            {
                return true;
            }
        }

        public override void TakeHit(Sonic player, GameLevel level, out bool blocking)
        {
            blocking = false;
            player.Coins++;
            level.RemoveObjects.Add(this);
            MovingAnchoredAnimation maa = new MovingAnchoredAnimation(3.0f, new Point[] { new Point((int)this.Location.X, (int)this.Location.Y), new Point((int)this.Location.X, (int)this.Location.Y + 96) }, 16, 0, 3, 0, 48, 48, Coin.SpinCoin, true);
            (XSonicGame.CurrentGame.Services.GetService(typeof(AnimationManager)) as AnimationManager).Add(maa);
            (XSonicGame.CurrentGame.Services.GetService(typeof(AudioManager)) as AudioManager).PlaySound("coins");
        }

        public static void UpdateCoins(double ms)
        {
            mp += ms;
        }

        public override void Update(double ms)
        {
            if (mp > 1000.0f / 8.0f)
            { // 8 fps
                mp = 0.0f;
                currentFrame++;
                if (currentFrame >= NUMFRAMES)
                    currentFrame = 0;
            }
        }

        public override Microsoft.Xna.Framework.Rectangle SourceRectangle
        {
            get { return new Microsoft.Xna.Framework.Rectangle(FRAMEW * currentFrame, FRAMEROW * FRAMEH, FRAMEW, FRAMEH); }
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
