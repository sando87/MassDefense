using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Property : MonoBehaviour
{
    public float TotalHP = 100;
    public float AttackPoint = 5;
    public float AttackSpeed = 1;
    public float MoveSpeed = 2;
    public float AttackRange = 1;
    public PlayerType PlayerType = PlayerType.Neutral;
    public bool IsEnemy(Property opp) { return opp.PlayerType != PlayerType.Neutral && opp.PlayerType != PlayerType; }
}
