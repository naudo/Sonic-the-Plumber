using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XSonic.Drawing;
using XSonic.Audio;

namespace XSonic.World
{
    class QuestionMarkBlockWManyCoins: QuestionMarkBlock
    {
        int coinsLeft;
        double passed;
        bool activated;
        public QuestionMarkBlockWManyCoins(Vector2 location, Vector2 size)
            : base(location, size)
        {
            coinsLeft = new Random().Next(5, 11);
            passed = 0.0f;
            activated = false;
        }

        public override void Update(double ms)
        {
            if (!activated)
                return;

            passed += ms;

            if (coinsLeft > 1 && passed >= 1000.0f)
            {
                coinsLeft -= 1;
                passed = 0.0f;
            }
        }

        public override void TakeHit(XSonic.Characters.Sonic hitter, GameLevel level, out bool blocking)
        {
            passed = 0.0f;
            activated = true;
            blocking = true;
            hitter.Coins++;
            coinsLeft--;
            if(coinsLeft == 0) active = false;
            MovingAnchoredAnimation maa = new MovingAnchoredAnimation(3.0f, new Point[] { new Point((int)this.Location.X, (int)this.Location.Y+48), new Point((int)this.Location.X, (int)this.Location.Y + 144) }, 16, 0, 3, 0, 48, 48, Coin.SpinCoin, true);
            (XSonicGame.CurrentGame.Services.GetService(typeof(AnimationManager)) as AnimationManager).Add(maa);
            (XSonicGame.CurrentGame.Services.GetService(typeof(AudioManager)) as AudioManager).PlaySound("coins");
        }
    }
}
