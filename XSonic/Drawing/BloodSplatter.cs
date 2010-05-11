using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XSonic.Drawing
{
    class BloodSplatter
    {
        static private Texture2D bloodSS = null;

        public BloodSplatter(Vector2 centerLocation, int splatterCount, int distance)
        {
            if(bloodSS == null) bloodSS = (XSonicGame.CurrentGame.Services.GetService(typeof(AnimationManager)) as AnimationManager).Content.Load<Texture2D>("Blood");

            Point[] targetPoints = XSonicGame.GetParabolic(centerLocation.X, centerLocation.Y, 800.0, 40.0, splatterCount);

            int minX = int.MaxValue, maxX = int.MinValue;
            for (int x = 0; x < targetPoints.Length; x++)
            {
                if (targetPoints[x].X > maxX)
                    maxX = targetPoints[x].X;
                if (targetPoints[x].X < minX)
                    minX = targetPoints[x].X;
            }
            for (int x = 0; x < targetPoints.Length; x++)
            {
                targetPoints[x].X += ((maxX - minX) / 2);

                double x1 = (double)targetPoints[x].X - centerLocation.X;
                double y1 = (double)targetPoints[x].Y - centerLocation.Y;

                double mag = Math.Sqrt(x1 * x1 + y1 * y1);

                x1 = x1 / mag;
                y1 = y1 / mag;

                x1 *= (double)distance;
                y1 *= (double)distance;

                targetPoints[x].X = (int)Math.Round(x1 + centerLocation.X);
                targetPoints[x].Y = (int)Math.Round(-y1 + centerLocation.Y);
            }
            Random r = new Random();
            for (int x = 0; x < targetPoints.Length; x++)
            {
                MovingAnchoredAnimation maa = new MovingAnchoredAnimation(6.0f + ((float)(r.Next(0, 601) - 300) / 100.0f), new Point[]{ new Point((int)centerLocation.X, (int)centerLocation.Y), targetPoints[x]}, 4, 0, 3, 0, 48, 48, bloodSS, false);
                (XSonicGame.CurrentGame.Services.GetService(typeof(AnimationManager)) as AnimationManager).Add(maa);
            }
        }
    }
}
