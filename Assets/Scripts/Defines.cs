public enum EBlockType
{
    NONE,
    NORMAL,
    DOUBLE_SCORE, // 10초간 더블스코어
    TRIPLE_SCORE, // 10초간 트리플스코어
    INVINCIBLE_BUFF // 체력 잠시동안 무한
}

public enum EArrowButtonType
{
    NONE,
    UP,
    LEFT,
    RIGHT,
    DOWN
}

public enum EParticleType
{
    GREEN,
    GOLD,
    DIAMOND
}

public enum EBlockSFXType
{
    ON_NORMAL_BLOCK,
    ON_GOLD_BLOCK,
    ON_DIA_BLOCK,
    ON_INVINCIBLE_BUFF_BLOCK,
    LENGTH
}

public enum ESFXType
{
    UI_CLICK,
    COUNTDOWN,
    LENGTH
}

public enum EArrowDir
{
    FWD,
    RIGHT,
    LEFT
}