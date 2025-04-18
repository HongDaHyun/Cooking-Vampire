using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpriteData : Singleton<SpriteData>
{
    [Title("�����̹�")]
    public StageSprite[] stageSprites;
    public GemSprite[] gemSprites;
    public RuntimeAnimatorController[] effects;
    public Pallate[] pallates;
    public StatSprite_Player[] statSprite_Players;
    public Enemy_Projectile_Sprite[] enemyProjectile_Sprites;
    public Area_Sprite[] area_Sprites;
    public Sprite[] battery_Sprites;

    [Title("��ŷ")]
    public Sprite[] ovenSprites;
    public Sprite[] oak_Sprites;
    public Sprite[] dust_Sprites;

    #region Vampire
    public Sprite[] Export_StageSprites(StageType type)
    {
        Sprite[] sprites = Array.Find(stageSprites, data => data.type == type).sprites;

        return sprites;
    }

    public GemSprite Export_GemSprite(int unit)
    {
        return Array.Find(gemSprites, data => data.unit == unit);
    }

    public Color Export_Pallate(string _name)
    {
        return Array.Find(pallates, color => color.colorName == _name).color;
    }
    public Color Export_TierColor(TierType tier)
    {
        switch(tier)
        {
            case TierType.Common:
                return Export_Pallate("Green");
            case TierType.Rare:
                return Export_Pallate("Blue");
            case TierType.Epic:
                return Export_Pallate("Purple");
            case TierType.Legend:
                return Export_Pallate("Yellow");
            default:
                return Export_Pallate("Gray");
        }
    }
    public Color Export_SignColor(int amount)
    {
        if (amount > 0)
            return Export_Pallate("Green");
        else if (amount < 0)
            return Export_Pallate("Red");
        else
            return Export_Pallate("Brown");
    }
    public string Export_ColorTag(Color color)
    {
        return "#" + ColorUtility.ToHtmlStringRGB(color);
    }

    public Sprite Export_StatSprite_Player(StatID_Player id)
    {
        return Array.Find(statSprite_Players, data => data.id == id).sprite;
    }
    public string Export_StatIcon_Player(StatID_Player id)
    {
        return $"<sprite={(int)id}>";
    }

    public Enemy_Projectile_Sprite Export_Enemy_Projectile_Sprite(string name)
    {
        return Array.Find(enemyProjectile_Sprites, data => data.name == name);
    }

    public Area_Sprite Export_Area_Sprite(string enemyName)
    {
        return Array.Find(area_Sprites, data => data.enemyName == enemyName);
    }
    #endregion
    #region Cooking
    public Sprite Export_OvenSprites(int woodCount, bool isFire)
    {
        if (woodCount >= 1 && woodCount <= 6)
        {
            int index = isFire ? 3 + (woodCount >= 4 ? 1 : 0) : 1 + (woodCount >= 4 ? 1 : 0);
            return ovenSprites[index];
        }
        return ovenSprites[0];
    }
    public Sprite Export_OakSprites(bool isOpen)
    {
        return oak_Sprites[isOpen ? 1 : 0];
    }
    public Sprite Export_CookItemSprites(int ID)
    {
        return DataManager.Instance.Export_CookItemData(ID).sprite;
    }
    public Sprite Export_DustSprite_Ran()
    {
        return dust_Sprites[UnityEngine.Random.Range(0, dust_Sprites.Length)];
    }
    #endregion
}
#region Vampire
public enum EleType { Fire = 0, Ice, Poison, Thunder }

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
    public string colorName;
    public Color color;
}

[Serializable]
public struct StatSprite_Player
{
    public StatID_Player id;
    public Sprite sprite;
}

[Serializable]
public struct Enemy_Projectile_Sprite
{
    public string name;
    public Sprite sprite;
    public RuntimeAnimatorController anim;
}

[Serializable]
public struct Area_Sprite
{
    public string enemyName;
    public RuntimeAnimatorController anim;
}
#endregion

#region Cooking
#endregion