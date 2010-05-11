using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XSonic.Drawing;

namespace XSonic
{
    public class Camera : Microsoft.Xna.Framework.GameComponent
    {
        public enum TrackMode
        {
            Rigid,
            Lazy,
        }

        public Vector2 CameraSize { get; set; }
        public Vector2 Location { get; set; }
        public GameLevel CurrentLevel { get; set; }
        public TrackMode CameraMode { get; set; }
        public Drawable CameraTarget { get; set; }

        public Camera(Game parent) : base(parent)
        {
            parent.Services.AddService(typeof(Camera), this);
            parent.Components.Add(this);
        }

        public override void Initialize()
        {
            CameraSize = new Vector2(Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height);
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            float offsetx = CameraTarget.Location.X - Location.X;
            offsetx -= CameraSize.X / 2;
            float offsety = 0;
            if (Math.Abs(CameraTarget.Location.Y - Location.Y) < (CameraSize.Y / 4))
            {
                offsety = CameraTarget.Location.Y - Location.Y;
                offsety -= CameraSize.Y / 4;
            }
            else if (Math.Abs(CameraTarget.Location.Y - Location.Y) > (CameraSize.Y * 3 / 4))
            {
                offsety = CameraTarget.Location.Y - Location.Y;
                offsety -= CameraSize.Y * 3 / 4;
            }
            Location = new Vector2(Location.X + offsetx, Location.Y + offsety);
            base.Update(gameTime);
        }

        #region ICamera Members

        public bool IsVisible(Rectangle rec)
        {
            return rec.Intersects(new Rectangle((int)Location.X, (int)Location.Y, (int)CameraSize.X, (int)CameraSize.Y + rec.Height));
        }

        public Vector2 Translate(Vector2 gameLocation)
        {
            Vector2 vec = gameLocation - Location;
            vec.Y = CameraSize.Y - vec.Y;
            return vec;
        }

        #endregion
    }
}
