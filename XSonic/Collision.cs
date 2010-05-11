using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XSonic
{
    public enum Hit
    {
        None = 0,
        Top = 1,
        Bottom = 2,
        Left = 4,
        Right = 8,
    }
    class Collision
    {
        private static int SIDE_FUDGE_FACTOR = 2;
        // -1 = no intersection
        // 0 = x intersection
        // 1 = y intersection
        public static Hit Intersects(Rectangle a, Rectangle b)
        {
            return Intersects(a, b, false);
        }
        public static Hit Intersects(Rectangle a, Rectangle b, bool isEnemy)
        {
            // check if two Rectangles intersect
            Hit returnVal = Hit.None;
            //if(a.Right > b.Left && a.Left < b.Right && a.Bottom > b.Top && a.Top < b.Bottom)
            if (a.Intersects(b))
            {
                int x1 = Math.Max(a.X, b.X);
                int x2 = Math.Min(a.X + a.Width, b.X + b.Width);

                int y1 = Math.Max(a.Y, b.Y);
                int y2 = Math.Min(a.Y + a.Height, b.Y + b.Height);

                // "fudge" factor
                if (((x2 - x1) < 2) && ((y2 - y1) < 2)) return Hit.None;
                /*if ((x2 - x1) > (y2 - y1)) return 1;
                return 0;*/

                if (isEnemy && (b.Y + b.Height - 15) < a.Y) return Hit.Top;

                int fudge = SIDE_FUDGE_FACTOR;

                if ((x2 - x1) >= (y2 - y1))
                {
                    while (returnVal == Hit.None)
                    {
                        if (a.Y + fudge >= b.Y + b.Height)
                            returnVal |= Hit.Top;
                        if (a.Y + a.Height - fudge <= b.Y)
                            returnVal |= Hit.Bottom;

                        fudge++;
                    }
                }
                else
                {
                    while (returnVal == Hit.None)
                    {
                        if (a.X + a.Width - fudge <= b.X)
                            returnVal |= Hit.Left;
                        if (a.X + fudge >= b.X + b.Width)
                            returnVal |= Hit.Right;

                        fudge++;
                    }
                }
            }
            return returnVal;
        }
    }
}
