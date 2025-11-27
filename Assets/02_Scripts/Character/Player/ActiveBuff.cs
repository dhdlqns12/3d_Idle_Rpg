using System;

[Serializable]
public class ActiveBuff
{
    public Enums.BuffType buffType;
    public float value;
    public float remainingTime;

    public ActiveBuff(Enums.BuffType type, float val, float duration)
    {
        buffType = type;
        value = val;
        remainingTime = duration;
    }
}