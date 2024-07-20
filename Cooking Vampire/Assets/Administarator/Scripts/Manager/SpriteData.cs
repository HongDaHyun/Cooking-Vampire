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

    public Sprite[] Export_StageSprites(StageType type)
    {
        Sprite[] sprites = Array.Find(stageSprites, data => data.type == type).sprites;

        return sprites;
    }

    public GemSprite Export_GemSprites(int unit)
    {
        return Array.Find(gemSprites, data => data.unit == unit);
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