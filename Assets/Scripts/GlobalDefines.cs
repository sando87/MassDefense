using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemConfig
{
    static public float DragDetectCoefficient = 0.1f; //사용자 입력이 Drag임을 감지하기 위한 최소 이동 거리
    static public float HealthBarShowTimeSec = 3.0f; //유닛 머리위에 체력바가 보여지는 시간(sec)
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