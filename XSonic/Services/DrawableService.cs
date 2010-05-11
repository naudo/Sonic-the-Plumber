using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XSonic.World;
using Microsoft.Xna.Framework.Graphics;

namespace XSonic.Services
{
    class DrawableService: DrawableGameComponent
    {
        private XSonicGame game;

        public DrawableService(XSonicGame game): base(game)
        {
            game.Services.AddService(typeof(DrawableService), this);
            game.Components.Add(this);
            this.game = game;
            this.DrawOrder = 0;
        }

        public override void Update(GameTime gameTime)
        {
            if (game.Paused) { base.Update(gameTime); return; }
            if (game.FirstRound)
            {
                game.Player.IsAlive = false;
                game.FirstRound = false;
            }
            if (game.Player.IsAlive && !game.Player.HasWon)
            {
              
                double ms = gameTime.ElapsedGameTime.TotalMilliseconds;

                Camera camera = (game.Services.GetService(typeof(Camera)) as Camera);
                game.Player.Update(ms);

                Coin.UpdateCoins(ms);

                foreach (Drawable d in game.CurrentLevel.LevelObjects)
                {
                    if (camera.IsVisible(new Rectangle((int)d.Location.X, (int)d.Location.Y, 48, 48)))
                    {
                        d.Update(ms);
                    }
                }

                foreach (Drawable d in game.CurrentLevel.RemoveObjects)
                {
                    game.CurrentLevel.LevelObjects.Remove(d);
                }

                game.CurrentLevel.RemoveObjects.Clear();

                foreach (Drawable d in game.CurrentLevel.AddObjects)
                {
                    game.CurrentLevel.LevelObjects.Add(d);
                }

                game.CurrentLevel.AddObjects.Clear();

                base.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = new SpriteBatch(this.GraphicsDevice);
            Camera camera = (game.Services.GetService(typeof(Camera)) as Camera);
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.BackToFront, SaveStateMode.SaveState);
            if (game.Player.IsAlive)
            {
                spriteBatch.Draw(game.Player.SpriteSheet, camera.Translate(game.Player.Location), game.Player.SourceRectangle,
                    game.Player.Color, game.Player.Rotation, game.Player.Origin, game.Player.Scale, game.Player.Effects, game.Player.LayerDepth);
            }

            foreach (Drawable d in game.CurrentLevel.LevelObjects)
            {
                if (camera.IsVisible(new Rectangle((int)d.Location.X, (int)d.Location.Y, 48, 48)))
                {
                    spriteBatch.Draw(d.SpriteSheet, camera.Translate(d.Location), d.SourceRectangle,
                        d.Color, d.Rotation, d.Origin, d.Scale, d.Effects, d.LayerDepth);
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
