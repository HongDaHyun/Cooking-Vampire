using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "DataCreator/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Title("UI")]
    public string title;
    [TextArea] public string discription;

    [Title("데이터")]
    public StageType[] stage;
    public float speed;
    public int defHP;
    public int gemAmount;
    public int ingredientID; // -1이면 드롭 X
    public AtkType atkType;
    public RuntimeAnimatorController[] animators;
    public ColliderSetting colSetting;

    public EnemyData(EnemyData other)
    {
        title = other.title;
        discription = other.discription;

        stage = other.stage;
        speed = other.speed;
        defHP = other.defHP;
        gemAmount = other.gemAmount;
        ingredientID = other.ingredientID;
        atkType = other.atkType;
        animators = other.animators;
        colSetting = other.colSetting;
    }

    public RuntimeAnimatorController Export_RanAnim()
    {
        return animators[Random.Range(0, animators.Length)];
    }
}

[System.Serializable]
public struct ColliderSetting
{
    public Vector2 colOff, colSize;
    public CapsuleDirection2D direction;
}

public enum AtkType { Normal, Range, Charge, Area, Box, Boss }