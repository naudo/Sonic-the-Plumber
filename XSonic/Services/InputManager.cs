using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace XSonic.Services
{
    class InputManager : GameComponent
    {
        private XSonicGame game;

        public InputManager(XSonicGame game)
            : base(game)
        {
            game.Components.Add(this);
            this.game = game;
        }

        public override void Update(GameTime gameTime)
        {

            GamePadState padState = GamePad.GetState(PlayerIndex.One);
            
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) || (padState.Buttons.Back == ButtonState.Pressed))
                game.Exit();
            // pause the game
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && !game.PausedPressed)
            {
                game.Paused = !game.Paused;
                game.PausedPressed = true;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Enter) || (padState.Buttons.Start == ButtonState.Pressed)) game.PausedPressed = false;
            if (game.Paused) { base.Update(gameTime); return; }

            // restart game
            if (Keyboard.GetState().IsKeyDown(Keys.F2) || (padState.Buttons.LeftShoulder == ButtonState.Pressed))
            {
                IEndManager em = (IEndManager)game.Services.GetService(typeof(IEndManager));
                em.Reset();
                game.StartOver();
                
                return;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.LeftControl) && Keyboard.GetState().IsKeyDown(Keys.LeftShift) && Keyboard.GetState().IsKeyDown(Keys.N)|| (padState.Buttons.RightShoulder == ButtonState.Pressed))
            {
                if (!game.Pressed)
                {
                    game.Pressed = true;
                    if(!game.OnLastLevel())
                    {
                        IEndManager em = (IEndManager)game.Services.GetService(typeof(IEndManager));
                        em.SetDoingBonus(false);
                        game.Player.HasBonus = false;
                        game.NextLevel();
                    }
                }
            }
            else
            {
                game.Pressed = false;
            }
        }
    }
}
