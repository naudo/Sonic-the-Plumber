using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XSonic.Characters;
using XSonic.Audio;
using Microsoft.Xna.Framework;

namespace XSonic.Services
{
    interface IEndManager
    {
        void Reset();

        void Do(XSonicGame game, Sonic player, AudioManager audio, GameTime gameTime);

        void SetDoingBonus(bool val);
    }
}
