using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpriteData : Singleton<SpriteData>
{
    [Title("¼­¹ÙÀÌ¹ú")]
    public StageSprite[] stageSprites;
    public GemSprite[] gemSprites;
    public RuntimeAnimatorController[] effects;
    public Color[] levelColor;
    public TierColor[] tierColor;
    public Sprite[] statSprites;

    public Sprite[] Export_StageSprites(StageType type)
    {
        Sprite[] sprites = Array.Find(stageSprites, data => data.type == type).sprites;

        return sprites;
    }

    public GemSprite Export_GemSprites(int unit)
    {
        return Array.Find(gemSprites, data => data.unit == unit);
    }

    public Color Export_TierColor(Tier tier)
    {
        return Array.Find(tierColor, color => color.tier == tier).color;
    }
}

[Serializable]
public struct StageSprite
{
    public StageType type;
    public Sprite[] sprites;
}

[Serializable]
public struct GemSprite
{
    public int unit;
    public Sprite idleSprite;
    public Sprite moveSprite;
}

[Serializable]
public struct TierColor
{
    public Tier tier;
    public Color color;
}