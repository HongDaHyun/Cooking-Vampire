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

    [Title("������")]
    public StageType[] stage;
    public float speed;
    public float hpScale = 1f;
    public int gemAmount;
    public int ingredientID; // -1�̸� ��� X
    public AtkType atkType;
    public RuntimeAnimatorController[] animators;
    public ColliderSetting colSetting;

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