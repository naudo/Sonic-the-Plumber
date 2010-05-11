using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XSonic.Drawing
{
    class MovingAnchoredAnimation: AnchoredAnimation
    {
        protected static double FUDGE_DISTANCE = 5.0f;
        protected Point[] drawPoses;
        protected int currentPoint;
        protected float speed;

        public MovingAnchoredAnimation(float speed, Point[] drawPoses, int framesPerSec, int startFrameX, int endFrameX, int frameRowY, int pixelWidth, int pixelHeight, Texture2D ss, bool loop)
            : base(drawPoses[0], framesPerSec, startFrameX, endFrameX, frameRowY, pixelWidth, pixelHeight, ss, loop)
        {
            this.speed = speed;
            this.drawPoses = drawPoses;
            currentPoint = 0;
        }

        public override bool Do(double elapsedMilli)
        {
            if (currentPoint + 1 >= drawPoses.Length)
                return false;

            Vector2 temp = new Vector2(drawPoses[currentPoint + 1].X - drawPoses[currentPoint].X, drawPoses[currentPoint + 1].Y - drawPoses[currentPoint].Y);
            temp.Normalize();

            temp.X *= speed;
            temp.Y *= speed;

            drawPos = new Point((int)Math.Round(drawPos.X + temp.X), (int)Math.Round(drawPos.Y + temp.Y));

            double distance = Math.Sqrt(Math.Pow(drawPoses[currentPoint + 1].X - drawPos.X, 2) + Math.Pow(drawPoses[currentPoint + 1].Y - drawPos.Y, 2));

            if (distance <= speed*speed)
                currentPoint++;
            
            return base.Do(elapsedMilli);
        }
    }
}
