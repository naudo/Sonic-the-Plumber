using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.IO;
using XSonic.Drawing;
using XSonic.Audio;
using XSonic.World;
using XSonic.Levels;
//using System.Timers;
using XSonic.Services;
using XSonic.Characters;

namespace XSonic
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class XSonicGame : Microsoft.Xna.Framework.Game
    {
        public static XSonicGame CurrentGame { get; set; }
        private bool pressed = false;

        private XSonic.Characters.Sonic.PlayerState savedPlayerState;
        private Vector2 savedPlayerLocation;
        private GameLevel savedLevel;
        public bool FirstRound { get; set; }
        public bool Pressed
        {
            get { return pressed; }
            set { pressed = value; }
        }

        public Sonic Player
        {
            get { return player; }
        }

        private static Texture2D worldSS;

        public static Texture2D WorldSS
        {
            get { return worldSS; }
        }

        private Texture2D sonicLife;

        public GameLevel CurrentLevel
        {
            get { return currentLevel; }
        }
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        Characters.Sonic player;
        GameLevel currentLevel;
        int currentLevelIndex;
        int currentBonusLevel;
        List<Levels.Level> levelList;
        public Boolean Paused { get; set; }
        public Boolean PausedPressed { get; set; }
        Random rand = new Random();

        public static double GetY(double x)
        {
            return (-x * x) + 144;
        }

        public static Point[] GetParabolic(double offsetX, double offsetY, double intensityX, double intensityY, int numPoints)
        {
            Point[] result = new Point[numPoints];
            for (int x = 0; x < numPoints; x++)
            {
                result[x] = new Point((int)Math.Round(offsetX - (intensityX * (double)x)), (int)Math.Round((intensityY * GetY((double)(x - ((numPoints - 1) / 2)))) + offsetY));
            }

            return result;
        }

        public XSonicGame()
        {
            if (CurrentGame != null) throw new Exception("lol wut?");
            FirstRound = true;
            CurrentGame = this;
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            Content.RootDirectory = "Content";
            new AudioManager(this);
            new EndManager(this);
            new InputManager(this);
            new AnimationManager(this, Content);
            new DrawableService(this);
            new EndScreen(this);
            new Camera(this);
        }

        /// <summary>
        /// Returns a list of all of the levels that are playable
        /// TODO change this to something that makes a little more sense
        /// </summary>
        /// <returns></returns>
        List<Levels.Level> LoadLevelList()
        {
            List<Level> levels = new List<Level>();
            levels.Add(new One());
            levels.Add(new Two());
            levels.Add(new Three());
            levels.Add(new Four());
            levels.Add(new Five());
            levels.Add(new BonusOne());
            levels.Add(new BonusTwo());

            return levels;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            // TODO: Add your initialization logic hereC:\Users\klogan\Desktop\MTM312 Multimedia, Game and Entertainment Systems\Sonic the Plumber\XSonic\Drawing\MovingAnchoredAnimation.cs
            (Services.GetService(typeof(AnimationManager)) as AnimationManager).Clear();
            levelList = LoadLevelList();
            //bonusList = LoadLevelList();//LoadBonusLevelList();
            currentLevelIndex = -1;
            currentBonusLevel = -1;
            player = new Characters.Sonic();
            NextLevel();
            (Services.GetService(typeof(AudioManager)) as AudioManager).LoadContent(Content);  //load all of the songs and fx


            Coin.SpinCoin = (XSonicGame.CurrentGame.Services.GetService(typeof(AnimationManager)) as AnimationManager).Content.Load<Texture2D>("spincoin");
            Animation coinCountAnimation = new Animation(new Point(10, 10), 8, 0, 3, 0, 48, 48, Coin.SpinCoin, true);
            (Services.GetService(typeof(AnimationManager)) as AnimationManager).Add(coinCountAnimation);

            base.Initialize();
        }

        public void NextLevel()
        {
            Camera camera = (Services.GetService(typeof(Camera)) as Camera);
            currentLevelIndex++;
            player.Reset();
            currentLevel = new GameLevel(levelList[currentLevelIndex]);
            player.Location = currentLevel.SpawnLocation;
            camera.CameraTarget = player;
            camera.CurrentLevel = currentLevel;
            player.ParentLevel = currentLevel;
            LoadContent();
            (Services.GetService(typeof(AnimationManager)) as AnimationManager).Add(new LevelTransitions(currentLevel.Title, spriteFont, this));
        }

        public void NextBonusLevel()
        {
            Camera camera = (Services.GetService(typeof(Camera)) as Camera);
            savedLevel = currentLevel;
            savedPlayerLocation = new Vector2(player.Location.X, player.Location.Y);
            savedPlayerState = player.State;

            player.Reset();
            currentBonusLevel = rand.Next(levelList.Where(l => l.IsBonus).Count());
            currentLevel = new GameLevel(levelList[levelList.Where(l => !l.IsBonus).Count() +  currentBonusLevel]);
            player.Location = currentLevel.SpawnLocation;
            camera.CameraTarget = player;
            camera.CurrentLevel = currentLevel;
            player.ParentLevel = currentLevel;
            LoadContent();
            (Services.GetService(typeof(AnimationManager)) as AnimationManager).Add(new LevelTransitions(currentLevel.Title, spriteFont, this));
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = Content.Load<SpriteFont>("TimesNewRoman");
            sonicLife = Content.Load<Texture2D>("soniclife");

            worldSS = Content.Load<Texture2D>("WorldSS");

            player.LoadContent(Content);

            currentLevel.LoadContent(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here			

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            Camera camera = (Services.GetService(typeof(Camera)) as Camera);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.BackToFront, SaveStateMode.SaveState);

            if (currentLevel.BackgroundImage != null)
            {
                float scale = (currentLevel.LevelWidth * 48) / currentLevel.BackgroundImage.Width;
                spriteBatch.Draw(currentLevel.BackgroundImage, camera.Translate(new Vector2(0, currentLevel.LevelHeight * 48)), new Rectangle(0, 0, currentLevel.BackgroundImage.Width, currentLevel.BackgroundImage.Height),
                    Color.White, 0, new Vector2(0, 0), scale, SpriteEffects.None, 1);
            }


            spriteBatch.DrawString(spriteFont, String.Format(" x {0}", player.Coins), new Vector2(58, 29), Color.Black);
            spriteBatch.Draw(sonicLife, new Rectangle(10, 58, 48, 48), Color.White);
            spriteBatch.DrawString(spriteFont, String.Format("x {0}", player.Lives < 0 ? 0 : player.Lives), new Vector2(58, 79), Color.Black);

            if (Paused)
                spriteBatch.DrawString(spriteFont, "Paused", new Vector2(10, 111), Color.Black);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void ResetLevel()
        {
            currentLevelIndex--;
            player.Reset();
            NextLevel();
        }

        public void ResetAfterBonus()
        {
            Camera camera = (Services.GetService(typeof(Camera)) as Camera);
            currentLevel = savedLevel;
            player.Location = new Vector2(savedPlayerLocation.X, savedPlayerLocation.Y);
            player.State = savedPlayerState;

            camera.CurrentLevel = currentLevel;
            player.ParentLevel = currentLevel;
        }

        public bool OnLastLevel()
        {
            if (currentLevelIndex < (levelList.Where(l => !l.IsBonus).Count() - 1))
            {
                return false;
            }
            return true;
        }

        internal void StartOver()
        {
            Initialize();
            LoadContent();
        }
    }
}
