using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using XSonic.World;
using XSonic.Drawing;
using XSonic.Audio;

namespace XSonic.Characters
{
    public class Sonic : Drawable
    {
        private int coinCount;
        public int Coins
        {
            get { return coinCount; }
            set
            {
                while (value >= 100)
                {
                    Lives++;
                    value -= 100;
                    (XSonicGame.CurrentGame.Services.GetService(typeof(AudioManager)) as AudioManager).PlaySound("1up");
                }
                coinCount = value;
            }
        }
        public int Lives { get; set; }

        static double MAX_VELOCITY = 10.0;
        public enum PlayerState
        {
            Idle,
            Jogging,
            Running,
            Jumping,
            StopJogging,
            StopRunning,
        }

        public enum Direction
        {
            Left,
            Right,
        }

        public double VelocityX { get; set; }
        public double VelocityY { get; set; }
        int animationIndex;
        double timeCounter;
        Texture2D spriteSheet;
        Direction direction;

        public PlayerState State { get; set; }
        public bool IsAlive { get; set; }
        public bool HasWon { get; set; }
        public bool HasBonus { get; set; }

        public Sonic()
        {
            Lives = 2;
            Coins = 0;
            Reset();
        }

        public void LoadContent(ContentManager content)
        {
            spriteSheet = content.Load<Texture2D>("SonicSS");
        }

        public override void Update(double elapsedms)
        {

            timeCounter += elapsedms;
            if (Location.Y < 0) IsAlive = false;
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            if (Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.Right) || gamepadState.DPad.Left == ButtonState.Pressed || gamepadState.DPad.Right == ButtonState.Pressed )
            {
                if (State == PlayerState.Idle)
                {
                    timeCounter = 0;
                    State = PlayerState.Jogging;
                    animationIndex = 5;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Left) || gamepadState.DPad.Left == ButtonState.Pressed)
                {
                    if (direction != Direction.Left && State != PlayerState.Idle)
                    {
                        if (State == PlayerState.Running)
                            State = PlayerState.StopRunning;
                        else if (State == PlayerState.Jogging)
                            State = PlayerState.StopJogging;
                    }
                    if (State == PlayerState.StopJogging || State == PlayerState.StopRunning)
                    {
                        VelocityX -= 7 * (elapsedms / 1000);
                        if (VelocityX < 0)
                        {
                            State = PlayerState.Jogging;
                            animationIndex = 0;
                            timeCounter = 0;
                            direction = Direction.Left;
                        }
                    }
                    else
                    {
                        direction = Direction.Left;
                    }
                    if (VelocityX > -7.0)
                        if (VelocityX > -1.0 && VelocityX < 0.0)
                            VelocityX = -1.0;
                        else
                            VelocityX -= 3 * (elapsedms / 1000);
                }
                else
                {
                    if (direction != Direction.Right && State != PlayerState.Idle)
                    {
                        if (State == PlayerState.Running)
                            State = PlayerState.StopRunning;
                        else if (State == PlayerState.Jogging)
                            State = PlayerState.StopJogging;
                    }
                    if (State == PlayerState.StopJogging || State == PlayerState.StopRunning)
                    {
                        VelocityX += 7 * (elapsedms / 1000);
                        if (VelocityX > 0)
                        {
                            State = PlayerState.Jogging;
                            animationIndex = 0;
                            timeCounter = 0;
                            direction = Direction.Right;
                        }
                    }
                    else
                    {
                        direction = Direction.Right;
                    }
                    if (VelocityX < 7.0)
                        if (VelocityX < 1.0 && VelocityX > 0.0)
                            VelocityX = 1.0;
                        else
                            VelocityX += 3 * (elapsedms / 1000);
                }
                if (Math.Abs(VelocityX) > MAX_VELOCITY)
                    VelocityX = direction == Direction.Right ? MAX_VELOCITY : 0 - MAX_VELOCITY;
                if (Math.Abs(VelocityX) > 3.5 && State == PlayerState.Jogging)
                {
                    State = PlayerState.Running;
                    animationIndex = 0;
                    timeCounter = 0;
                }
            }
            else
            {
                if (State == PlayerState.Idle && Math.Abs(VelocityX) > 0)
                    State = PlayerState.Running;
                if (State == PlayerState.Jogging)
                    State = PlayerState.StopJogging;
                if (State == PlayerState.Running)
                    State = PlayerState.StopRunning;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space) || Keyboard.GetState().IsKeyDown(Keys.Up) || gamepadState.Buttons.A == ButtonState.Pressed)
            {
                if (State != PlayerState.Jumping && VelocityY > -1)
                {
                    (XSonicGame.CurrentGame.Services.GetService(typeof(AudioManager)) as AudioManager).PlaySound("jump");
                    State = PlayerState.Jumping;
                    VelocityY = 5.5;
                    timeCounter = 0;
                    animationIndex = 0;
                }
            }
            
            if (State == PlayerState.StopJogging || State == PlayerState.StopRunning)
            {
                double multi = State == PlayerState.StopJogging ? 4.0 : 9.0;
                if (VelocityX > 0)
                    VelocityX -= multi * (elapsedms / 1000);
                else
                    VelocityX += multi * (elapsedms / 1000);
                if (Math.Abs(VelocityX) < .25)
                {
                    VelocityX = 0.0;
                    State = PlayerState.Idle;
                }
            }

            VelocityY -= 5.5 * (elapsedms / 1000);

            Vector2 tloc = Location;
            Location = new Vector2(Location.X + (float)VelocityX, Location.Y + (float)VelocityY);
            // collision detection
            foreach (Drawable d in ParentLevel.LevelObjects)
            {
                if (!d.IsCollidable) continue;
                Hit c = Collision.Intersects(GetCollisionBox, d.GetCollisionBox, d.IsHarmful);
                if (c != Hit.None)
                {
                    if (d.IsKill)
                    {
                        IsAlive = false;
                        break;
                    }
                    if (d.IsVictory)
                    {
                        HasWon = true;
                        break;
                    }
                    if (d.IsBonus)
                    {
                        ParentLevel.RemoveObjects.Add(d);
                        HasBonus = true;
                        break;
                    }
                    if (d.IsSpecialHit)
                    {
                        if (d.WasHit(c))
                        {
                            bool blocking;
                            d.TakeHit(this, this.ParentLevel, out blocking);
                            if (!blocking)
                                continue;
                        }
                    }
                }
                if ((c & Hit.Left) == Hit.Left || (c & Hit.Right) == Hit.Right)
                {
                    if (d.IsHarmful && !((c & Hit.Top) == Hit.Top))
                    {
                        IsAlive = false;
                        break;
                    }
                    VelocityX = 0;
                }
                if ((c & Hit.Top) == Hit.Top)
                {
                    if (d.IsHarmful)
                    {
                        (XSonicGame.CurrentGame.Services.GetService(typeof(AudioManager)) as AudioManager).PlaySound("jump");
                       // BloodSplatter bs = new BloodSplatter(d.Location, 500, 55);
                        VelocityY = 5.2;
                        State = PlayerState.Jumping;
                        animationIndex = 0;
                        timeCounter = 0;
                    }
                }
                if ((c & Hit.Top) == Hit.Top || (c & Hit.Bottom) == Hit.Bottom)
                {
                    if (!d.IsHarmful)
                    {
                        if ((c & Hit.Bottom) == Hit.Bottom) VelocityY = -0.0001f;
                        else VelocityY = 0;
                        if (State == PlayerState.Jumping && (c & Hit.Top) == Hit.Top)
                        {
                            State = PlayerState.Idle;
                            animationIndex = 0;
                            timeCounter = 0;
                        }
                    }
                }
            }
            Location = new Vector2(tloc.X + (float)VelocityX, tloc.Y + (float)VelocityY);

            switch (State)
            {
                default:
                    //animationIndex = 0;
                    //timeCounter = 0;
                    //State = PlayerState.Idle;
                    break;
                case PlayerState.Idle:
                    if (animationIndex == 0)
                    {
                        if (timeCounter > 4000)
                        {
                            animationIndex++;
                            timeCounter = 0;
                        }
                    }
                    else
                    {
                        if (timeCounter > 250)
                        {
                            animationIndex++;
                            timeCounter = 0;
                        }
                        if (animationIndex >= 3)
                            animationIndex = 1;
                    }
                    break;
                    
                case PlayerState.Jogging:
                    if (timeCounter > 100)
                    {
                        animationIndex++;
                        timeCounter = 0;
                    }
                    if (animationIndex > 5) animationIndex = 0;
                    break;

                case PlayerState.Running:
                    if (timeCounter > 100)
                    {
                        animationIndex++;
                        timeCounter = 0;
                    }
                    if (animationIndex > 3)
                        animationIndex = 0;
                    break;
                case PlayerState.Jumping:
                    if (timeCounter > 100)
                    {
                        animationIndex++;
                        timeCounter = 0;
                    }
                    if (animationIndex > 3)
                        animationIndex = 0;
                    break;
                case PlayerState.StopJogging:
                case PlayerState.StopRunning:
                    animationIndex = 0;
                    break;
            }
        }

        public override Rectangle SourceRectangle
        {
            get
            {
                switch (State)
                {
                    default:
                    case PlayerState.Idle:
                        return new Rectangle(animationIndex * 50, 0, 48, 48);

                    case PlayerState.Jogging:
                        if (animationIndex == 5)
                            return new Rectangle(0, 50, 48, 48);
                        return new Rectangle(animationIndex * 50 + 200, 0, 48, 48);
                    case PlayerState.Running:
                        if (animationIndex >= 2)
                            return new Rectangle(50 * (animationIndex - 2), 100, 48, 48);
                        return new Rectangle(50 * (animationIndex + 7), 50, 48, 48);
                    case PlayerState.StopRunning:
                    case PlayerState.StopJogging:
                        return new Rectangle(6 * 50, 100, 50, 50);
                    case PlayerState.Jumping:
                        return new Rectangle((animationIndex + 1) * 50, 150, 48, 48);
                }
            }
        }

        public override Texture2D SpriteSheet
        {
            get { return spriteSheet; }
            set { spriteSheet = value; }
        }

        public override SpriteEffects Effects
        {
            get
            {
                return direction == Direction.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            }
        }

        public void Reset()
        {
            VelocityX = 0.0;
            VelocityY = 0.0;
            State = PlayerState.Idle;
            animationIndex = 0;
            timeCounter = 0.0;
            Size = new Vector2(49, 49);
            Location = new Vector2(30, 58);
            direction = Direction.Right;
            IsAlive = true;
            HasWon = false;
        }

        public override Rectangle GetCollisionBox
        {
            get
            {
                return new Rectangle((int)Location.X + 5, (int)Location.Y, (int)Size.X - 10, (int)Size.Y-10);
            }
        }
    }
}
