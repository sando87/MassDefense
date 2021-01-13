using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemConfig
{
    static public float DragDetectCoefficient = 0.1f;
}

public enum PlayerType
{
    None, Human, Computer, Neutral
}

public enum FSMState
{
    None, Idle, Move, MoveToAttack, Chase, Attack, Stun, Appear, Death
}

public enum FSMCmd
{
    None, Enter, Update, Leave
}


public class FSMInfo
{
    public Vector3 DestinationPos;
    public GameObject AttackTarget;
}