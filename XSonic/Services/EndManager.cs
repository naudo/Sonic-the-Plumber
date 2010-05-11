using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XSonic.Characters;
using XSonic.Audio;
using XSonic.Drawing;
using Microsoft.Xna.Framework;
using XSonic.Services;

namespace XSonic
{
    class EndManager: GameComponent, IEndManager
    {
        private bool doing;
        private double doingAt;
        private bool doingBonus;

        private XSonicGame game;

        public EndManager(XSonicGame game) : base(game)
        {
            game.Services.AddService(typeof(IEndManager), this);
            game.Components.Add(this);
            this.game = game;
            doing = false;
            doingAt = -1.0f;
            doingBonus = false;
        }

        public override void Update(GameTime gameTime)
        {
            Do(game, game.Player, (game.Services.GetService(typeof(AudioManager)) as AudioManager), gameTime);

            base.Update(gameTime);
        }

        void IEndManager.Reset()
        {
            doing = false;
            doingAt = -1.0f;
            doingBonus = false;
        }

        public void SetDoingBonus(bool val)
        {
            doingBonus = val;
        }

        public void Do(XSonicGame game, Sonic player, AudioManager audio, GameTime gameTime)
        {
            if (doing)
            {
                if (gameTime.TotalGameTime.TotalMilliseconds - doingAt >= 3000.0f && doingAt != -1.0f)
                {
                    doing = false;
                    doingAt = -1.0f;
                    player.IsAlive = true;

                    game.ResetLevel();
                }
                else
                {
                    //base.Update(gameTime);
                    return;
                }
            }

            if (!player.IsAlive)
            {
                BloodSplatter bs = new BloodSplatter(player.Location, 5000, 30);
                if (player.HasBonus)
                {
                    player.HasBonus = false;
                    doingBonus = false;

                    player.IsAlive = true;


                    game.ResetAfterBonus();
                }
                else if (!doing)
                {
                    doing = true;
                    Point[] deathAnimationPoints = XSonicGame.GetParabolic(player.Location.X, player.Location.Y, 17.25f, 0.8f, 17);//new Point[] { new Point((int)player.Location.X, (int)player.Location.Y), new Point((int)player.Location.X, (int)player.Location.Y + 144), new Point((int)player.Location.X - 96, (int)player.Location.Y + 96), new Point((int)player.Location.X - 96, (int)player.Location.Y - 96) };
                    MovingAnchoredAnimation maa = new MovingAnchoredAnimation(5.0f, deathAnimationPoints, 2, 3, 6, 6, 48, 48, player.SpriteSheet, false);
                    (XSonicGame.CurrentGame.Services.GetService(typeof(AnimationManager)) as AnimationManager).Add(maa);
                    player.Lives--;
                    if (player.Lives >= 0)
                    {
                        audio.PlaySound("1down");
                        doingAt = gameTime.TotalGameTime.TotalMilliseconds;
                    }
                    else
                    {
                        audio.PlaySpecialSong("gameover");
                    }
                }
            }

            if (player.HasWon)
            {
                if(!game.OnLastLevel())
                {
                    doingBonus = false;
                    player.HasBonus = false;
                    game.NextLevel();
                }
            }
            if (player.HasBonus && !doingBonus)
            {
                doingBonus = true;

                game.NextBonusLevel();
            }
        }

        #region IGameComponent Members

        public void Initialize()
        {
        }

        #endregion
    }
}
