public enum EBlockType
{
    NONE,
    NORMAL,
    DOUBLE_SCORE, // 10초간 더블스코어
    TRIPLE_SCORE, // 10초간 트리플스코어
    FEVER_BUFF // 밟으면 일정 시간동안 무적, 피버타임
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

public enum EVolumeType
{
    BGM,
    SFX
}

public struct SSettingValue
{
    float bgmVolume;
    float sfxVolume;
    bool isVibrate;
}

public enum ENextBlockType
{
    NONE = -1,
    COLLECT,
    WRONG,
    EMPTY
}