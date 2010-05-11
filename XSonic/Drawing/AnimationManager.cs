using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace XSonic.Drawing
{
    public class Animation
    {
        public Animation()
        {
        }

        public Animation(Point drawPos, int framesPerSec, int startFrameX, int endFrameX, int frameRowY, int pixelWidth, int pixelHeight, Texture2D ss, bool loop)
        {
            this.drawPos = drawPos;
            this.framesPerSec = framesPerSec;
            this.startFrame = startFrameX;
            this.endFrame = endFrameX;
            this.frameRow = frameRowY;
            this.frameWidth = pixelWidth;
            this.frameHeight = pixelHeight;
            this.spriteSheet = ss;
            this.loop = loop;
            this.milliElapsed = 0.0f;
            this.currentFrame = startFrameX;
        }

        protected Point drawPos;

        public Point DrawPos
        {
            get { return drawPos; }
            set { drawPos = value; }
        }

        public int FrameWidth
        {
            get { return this.frameWidth; }
        }
        public int FrameHeight
        {
            get { return this.frameHeight; }
        }

        protected int framesPerSec;
        protected double milliElapsed;

        protected int startFrame;
        protected int endFrame;
        protected int frameRow;

        protected int frameWidth;
        protected int frameHeight;

        protected Texture2D spriteSheet;

        protected int currentFrame;

        protected bool loop;

        public virtual bool Complete()
        {
            if (!loop && currentFrame == endFrame)
                return true;

            return false;
        }

        public virtual void Next()
        {
            currentFrame++;
            milliElapsed = 0;
            if (currentFrame > endFrame)
            {
                if (loop)
                    currentFrame = startFrame;
            }
        }

        public virtual bool Need(double elapsedMilli)
        {
            milliElapsed += elapsedMilli;
            if (milliElapsed > 1000.0f / (double)framesPerSec)
                return true;
            return false;
        }

        public virtual bool Do(double elapsedMilli)
        {
            if (Need(elapsedMilli))
            {
                Next();
            }

            return !Complete();
        }

        public virtual Rectangle SourceRect
        {
            get
            {
                return new Rectangle(frameWidth * currentFrame, frameHeight * frameRow, frameWidth, frameHeight);
            }
        }

        public Texture2D SpriteSheet
        {
            get { return spriteSheet; }
        }

        public virtual void Draw(SpriteBatch sb, Camera c)
        {
            sb.Draw(this.SpriteSheet, new Vector2(this.DrawPos.X, this.DrawPos.Y), this.SourceRect, Color.White);
        }
    }
    public class AnimationManager: DrawableGameComponent
    {
        protected List<Animation> currentAnimations;
        protected ContentManager content;
        private XSonicGame game;

        public ContentManager Content
        {
            get { return content; }
        }

        public AnimationManager(XSonicGame game, ContentManager Content): base(game)
        {
            this.DrawOrder = 10;
            game.Services.AddService(typeof(AnimationManager), this);
            game.Components.Add(this);
            this.game = game;
            this.content = Content;
            currentAnimations = new List<Animation>();
        }

        public void Add(Animation a)
        {
            currentAnimations.Add(a);
        }

        public void Clear()
        {
            currentAnimations.Clear();
        }

        public void Remove(Animation a)
        {
            currentAnimations.Remove(a);
        }

        public void Do(double milliElapsed, SpriteBatch sb, Camera c)
        {
            List<Animation> toRemove = new List<Animation>();
            foreach (Animation a in currentAnimations)
            {
                if (!a.Do(milliElapsed))
                    toRemove.Add(a);

                a.Draw(sb, c);
                
            }
            foreach (Animation a in toRemove)
                currentAnimations.Remove(a);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sb = new SpriteBatch(this.GraphicsDevice);
            Camera c = (game.Services.GetService(typeof(Camera)) as Camera);
            sb.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.BackToFront, SaveStateMode.SaveState);
            Do(gameTime.ElapsedGameTime.TotalMilliseconds, sb, c);
            sb.End();

            base.Draw(gameTime);
        }
    }
}
