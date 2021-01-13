using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public float TotalHP = 100;
    public float AttackDamage = 5;
    public float AttackSpeed = 1;
    public float MoveSpeed = 2;
    public float AttackRange = 1;
    public PlayerType PlayerType = PlayerType.Neutral;

    public float CurrentHP { get; private set; }
    public float HPRate { get { return CurrentHP / TotalHP; } }
    public FSM FSMComp { get; private set; }

    private void Start()
    {
        CurrentHP = TotalHP;
        FSMComp = GetComponent<FSM>();
    }

    public bool IsEnemy(Stats opp)
    {
        return opp.PlayerType != PlayerType.Neutral && opp.PlayerType != PlayerType;
    }

    public void GetDamaged(float damage)
    {
        CurrentHP -= damage;
        if (CurrentHP <= 0)
        {
            CurrentHP = 0;
            if (FSMComp != null)
                FSMComp.ChangeState(FSMState.Death);
        }
    }

}
