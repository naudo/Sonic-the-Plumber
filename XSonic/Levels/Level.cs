using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XSonic.Levels
{
    public abstract class Level
    {
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool IsBonus { get; set; }
        public char[][] LevelMatrix { get; set; }
        public Level NextLevel { get; set; }

        public Level()
        {
          
        }
    }
}
