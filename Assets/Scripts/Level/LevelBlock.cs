using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBlock : MonoBehaviour
{
    public enum blockTypes
    {
        Normal,
        Immoveable,
        Moveable,
        MoveableStacked,
        Transparent,
        Goal,
        PotionBook
    }
    public blockTypes blockType;
    private bool playerMoveable;
    private bool blocksLight;
    private bool swappable;
}
