using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using XSonic.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using XSonic.Levels;

namespace XSonic
{
    public class GameLevel
    {
        public int LevelHeight { get; set; }
        public int LevelWidth { get; set; }
        public Vector2 SpawnLocation { get; set; }
        public List<Drawable> LevelObjects { get; set; }
        public List<Drawable> RemoveObjects { get; set; }
        public List<Drawable> AddObjects { get; set; }
        public String Title { get; set; }
        public Texture2D BackgroundImage { get; set; }
        public String Level { get; set; }

  
        /// <summary>
        /// Setups a level to be played
        /// </summary>
        /// <param name="level"></param>
        public GameLevel(Level level)
        {
            char[][] data =  level.LevelMatrix;
            Title = level.Name;
            LevelHeight = level.Height; //int.Parse(data[0 + offset]);
            LevelWidth = level.Width;//int.Parse(data[1 + offset]);
            LevelObjects = new List<Drawable>();
            RemoveObjects = new List<Drawable>();
            AddObjects = new List<Drawable>();

            for (int h = 0; h < LevelHeight; h++)
            {
                for (int w = 0; w < LevelWidth; w++)
                {
                    char value = data[h][w];
                    int val = 0;
                    if (Char.IsDigit(value))
                        val = (int)value - (int)'0';
                    else
                        val = (int)value - (int)'A' + 10;
                    GameObjects obj = (GameObjects)val;
                    switch (obj)
                    {
                        case GameObjects.GroundBlock:
                            GroundBlock b = new GroundBlock(new Vector2(w * 48, (LevelHeight - h) * 48), new Vector2(48));
                            LevelObjects.Add(b);
                            break;
                        case GameObjects.SpawnPoint:
                            SpawnLocation = new Vector2(w * 48, (LevelHeight - h) * 48);
                            break;
                        case GameObjects.KillBlock:
                            LevelObjects.Add(new KillBlock(new Vector2(w * 48, (LevelHeight - h) * 48), new Vector2(48)));
                            break;
                        case GameObjects.Goomba:
                            LevelObjects.Add(new Characters.Goomba(new Vector2(w * 48, (LevelHeight - h) * 48), new Vector2(48)));
                            break;
                        case GameObjects.VictoryFlag:
                            LevelObjects.Add(new VictoryFlag(new Vector2(w * 48, (LevelHeight - h) * 48), new Vector2(48)));
                            break;
                        case GameObjects.Coin:
                            LevelObjects.Add(new Coin(new Vector2(w * 48, (LevelHeight - h) * 48)));
                            break;
                        case GameObjects.Block:
                            LevelObjects.Add(new Block(new Vector2(w * 48, (LevelHeight - h) * 48), new Vector2(48)));
                            break;
                        case GameObjects.BrickBlock:
                            LevelObjects.Add(new BrickBlock(new Vector2(w * 48, (LevelHeight - h) * 48), new Vector2(48)));
                            break;
                        case GameObjects.QuestionMarkBlock:
                            LevelObjects.Add(new QuestionMarkBlock(new Vector2(w * 48, (LevelHeight - h) * 48), new Vector2(48)));
                            break;
                        case GameObjects.QuestionMarkBlockWLife:
                            LevelObjects.Add(new QuestionMarkBlockWLife(new Vector2(w * 48, (LevelHeight - h) * 48), new Vector2(48, 48)));
                            break;
                        case GameObjects.FallingGoomba:
                            LevelObjects.Add(new Characters.FGoomba(new Vector2(w * 48, (LevelHeight - h) * 48), new Vector2(48)));
                            break;
                        case GameObjects.QuestionMarkBlockWManyCoins:
                            LevelObjects.Add(new QuestionMarkBlockWManyCoins(new Vector2(w * 48, (LevelHeight - h) * 48), new Vector2(48, 48)));
                            break;
                        case GameObjects.BonusLevelFlag:
                            LevelObjects.Add(new BonusFlag(new Vector2(w * 48, (LevelHeight - h) * 48), new Vector2(48, 48)));
                            break;
                        case GameObjects.Mushroom:
                            LevelObjects.Add(new LifeMushroom(new Vector2(w * 48, (LevelHeight - h) * 48), new Vector2(48, 48)));
                            break;
                    }
                }
            }
        }
        public void LoadContent(ContentManager content)
        {
            foreach (Drawable d in LevelObjects)
            {
                if (d.SpriteSheet == null)
                    d.SpriteSheet = XSonicGame.WorldSS;
                d.ParentLevel = this;
            }

            try
            {
                BackgroundImage = content.Load<Texture2D>(String.Format("{0}_bg", Level));
            }
            catch (Exception e)
            {
                BackgroundImage = content.Load<Texture2D>("Levels\\Default_bg");
            }
        }
    }
}
