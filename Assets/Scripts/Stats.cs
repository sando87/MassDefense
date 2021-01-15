using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public float TotalHP = 100;
    public float AttackDamage = 5;
    public float AttackSpeed = 1;
    public float MoveSpeed = 2;
    public float DetectRange = 1;
    public float AttackRange = 0.3f;
    public float CharacterHeight = 1;
    public PlayerType PlayerType = PlayerType.Neutral;

    public bool IsEnemy(Stats opp)
    {
        return opp.PlayerType != PlayerType.Neutral && opp.PlayerType != PlayerType;
    }
}
