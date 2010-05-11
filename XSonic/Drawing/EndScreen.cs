using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using XSonic.Characters;

namespace XSonic.Drawing
{
    class EndScreen : DrawableGameComponent
    {
        private Texture2D textureSheet;
        private SpriteFont spriteFont;

        public EndScreen(Game game) : base(game)
        {
            this.DrawOrder = int.MaxValue;
            game.Components.Add(this);
        }

        public override void Initialize()
        {
            textureSheet = Game.Content.Load<Texture2D>("LevelTransitions");
            spriteFont = Game.Content.Load<SpriteFont>("TimesNewRoman");
            base.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {
            Sonic player = ((XSonicGame)Game).Player;
            if (player.Lives < 0)
                Draw("Game Over!");
            if (player.HasWon)
                Draw("Congratulations! You beat the game!");
        }

        public void Draw(String text)
        {
            Vector2 stringSize = spriteFont.MeasureString(text);

            SpriteBatch sb = new SpriteBatch(this.GraphicsDevice);
            sb.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.BackToFront, SaveStateMode.SaveState);

            sb.Draw(textureSheet, new Vector2(270, 0), new Rectangle(0, 0, 580, 170), Color.White, 1.57f,
                new Vector2(0, 0), 1, SpriteEffects.None, 0);
            sb.Draw(textureSheet, new Vector2(0, 410), new Rectangle(0, 176, 671, 101), Color.White, 0,
                new Vector2(0, 0), 1, SpriteEffects.None, .1f);
            sb.DrawString(spriteFont, text, new Vector2(280, 440), Color.White);

            sb.End();
        }
    }
}
