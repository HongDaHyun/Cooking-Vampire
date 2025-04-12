using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Point : MonoBehaviour
{
    [Title("주변 포인트")]
    public Point L;
    public Point R;
    public Point T;
    public Point D;

    public Point GetNearPoint(Direction dir)
    {
        switch(dir)
        {
            case Direction.L:
                return L;
            case Direction.R:
                return R;
            case Direction.T:
                return T;
            case Direction.D:
                return D;
            default:
                return L;
        }
    }
}

public enum Direction { L, R, T, D };
