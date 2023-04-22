using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GamePlayState
{
    intro, mainGamePlay, gameOverScreen, gameWinScreen
}

public enum DamageTypes
{
    laser, piercing
}

public enum DamageMode
{
    damageWhileTouching, damageOnTouchStart, damageOnTouchExit
}

public enum spawnMode
{
    spawnContinously, spawnSetAmount, spawnOnce
}
