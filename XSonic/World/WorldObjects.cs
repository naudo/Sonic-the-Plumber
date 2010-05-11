using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XSonic.World
{
    public enum GameObjects
    {
        Empty = 0,
        SpawnPoint = 1,
        GroundBlock = 2,
        KillBlock = 3,
        Goomba = 4,
        VictoryFlag = 5,
        Coin = 6,
        Block = 7,
        BrickBlock = 8,
        QuestionMarkBlock = 9,
        QuestionMarkBlockWLife = 10, // A
        FallingGoomba = 11, // B
        QuestionMarkBlockWManyCoins = 12, // C
        BonusLevelFlag = 13, //D
        Mushroom = 14, //E
    }
}
