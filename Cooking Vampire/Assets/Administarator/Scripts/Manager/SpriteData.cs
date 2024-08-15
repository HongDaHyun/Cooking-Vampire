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
    public Pallate[] pallates;
    public Sprite[] statSprites;
    public Enemy_Projectile_Sprite[] enemyProjectile_Sprites;

    public Sprite[] Export_StageSprites(StageType type)
    {
        Sprite[] sprites = Array.Find(stageSprites, data => data.type == type).sprites;

        return sprites;
    }

    public GemSprite Export_GemSprites(int unit)
    {
        return Array.Find(gemSprites, data => data.unit == unit);
    }

    public Color Export_Pallate(ColorType type)
    {
        return Array.Find(pallates, color => color.colorType == type).color;
    }
    public Color Export_TierColor(Tier tier)
    {
        switch(tier)
        {
            case Tier.Common:
                return Export_Pallate(ColorType.Green);
            case Tier.Rare:
                return Export_Pallate(ColorType.Blue);
            case Tier.Epic:
                return Export_Pallate(ColorType.Purple);
            case Tier.Legend:
                return Export_Pallate(ColorType.Yellow);
            default:
                return Export_Pallate(ColorType.Gray);
        }
    }

    public Enemy_Projectile_Sprite Export_Enemy_Projectile_Sprite(string name)
    {
        return Array.Find(enemyProjectile_Sprites, data => data.name == name);
    }
}
public enum ColorType { Gray, Mint, Green, Blue, Purple, Red, Yellow }

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
public struct Pallate
{
    public ColorType colorType;
    public Color color;
}

[Serializable]
public struct Enemy_Projectile_Sprite
{
    public string name;
    public Sprite sprite;
    public RuntimeAnimatorController anim;
}