using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XSonic.World
{
    class QuestionMarkBlockWLife: QuestionMarkBlock
    {
        public QuestionMarkBlockWLife(Vector2 location, Vector2 size)
            : base(location, size)
        {

        }

        public override void TakeHit(XSonic.Characters.Sonic hitter, GameLevel level, out bool blocking)
        {
            // create life mushroom and deactivate
            blocking = true;
            active = false;

            LifeMushroom temp = new LifeMushroom(new Vector2(this.Location.X, this.Location.Y + 48), new Vector2(48, 48));
            temp.ParentLevel = level;
            level.AddObjects.Add(temp);
        }
    }
}
