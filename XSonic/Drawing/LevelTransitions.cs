using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace XSonic.Drawing
{
    class LevelTransitions : Animation
    {
        private SpriteFont font;
        private string levelTitle;
        private Texture2D textureSheet;
        public XSonicGame Parent { get; set; }

        public LevelTransitions(String levelTitle, SpriteFont font, XSonicGame parent)
        {
            this.font = font;
            this.levelTitle = levelTitle;
            this.framesPerSec = 60;
            this.startFrame = 0;
            this.endFrame = 120;
            this.frameRow = 0;
            this.frameWidth = 0;
            this.frameHeight = 0;
            this.loop = false;
            this.milliElapsed = 0.0f;
            this.currentFrame = 0;
            textureSheet = parent.Content.Load<Texture2D>("LevelTransitions");
            Parent = parent;
        }

        public override void Draw(SpriteBatch sb, Camera c)
        {
            if (this.currentFrame < (endFrame + 1) / 2) Parent.Paused = true;
            else Parent.Paused = false;
            Vector2 stringSize = font.MeasureString(levelTitle);

            int ry1 = -580 + (currentFrame * 20);
            int rx2 = -671 + (currentFrame * 20);
            if (currentFrame > (endFrame - 1) / 2)
            {
                ry1 = -580 + (endFrame - currentFrame) * 20;
                rx2 = -671 + (endFrame - currentFrame) * 20;
            }
            if (ry1 > 0) ry1 = 0;
            if (rx2 > 0) rx2 = 0;
            sb.Draw(textureSheet, new Vector2(250, ry1), new Rectangle(0, 0, 580, 170), Color.White, 1.57f,
                new Vector2(0, 0), 1, SpriteEffects.None, 0);
            sb.Draw(textureSheet, new Vector2(rx2, 410), new Rectangle(0, 176, 671, 101), Color.White, 0,
                new Vector2(0, 0), 1, SpriteEffects.None, .1f);
            sb.DrawString(font, levelTitle, new Vector2(rx2 + 260, 440), Color.White);
        }
    }
}
