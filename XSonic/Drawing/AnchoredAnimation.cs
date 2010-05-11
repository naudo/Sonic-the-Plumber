using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XSonic.Drawing
{
    class AnchoredAnimation: Animation
    {
        public AnchoredAnimation(Point drawPos, int framesPerSec, int startFrameX, int endFrameX, int frameRowY, int pixelWidth, int pixelHeight, Texture2D ss, bool loop)
            : base(drawPos, framesPerSec, startFrameX, endFrameX, frameRowY, pixelWidth, pixelHeight, ss, loop)
        {
        }

        public override void Draw(SpriteBatch sb, Camera c)
        {
            sb.Draw(this.SpriteSheet, c.Translate(new Vector2(this.DrawPos.X, this.DrawPos.Y)), this.SourceRect, Color.White);
        }
    }
}
