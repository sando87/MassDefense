using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private float mLastAttackTime = 0;
    public Stats Stats { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Stats = GetComponent<Stats>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        int ran = UnityEngine.Random.Range(0, 6);
        Debug.Log("OnClick : " + ran);

        if (ran == 0)
            GetComponent<Animator>().Play("idle", -1, 0);
        else if (ran == 1)
            GetComponent<Animator>().Play("move", -1, 0);
        else if (ran == 2)
            GetComponent<Animator>().Play("attack_down", -1, 0);
        else if (ran == 3)
            GetComponent<Animator>().Play("attack_mid", -1, 0);
        else if (ran == 4)
            GetComponent<Animator>().Play("attack_up", -1, 0);
        else if (ran == 5)
            GetComponent<Animator>().Play("death", -1, 0);
    }
    public void OnDragDrop(Vector3 worldPos)
    {
        FSM fsm = GetComponent<FSM>();
        fsm.Param.DestinationPos = worldPos;
        fsm.ChangeState(FSMState.Move);
    }

    public void OnDetectEvent(Collider2D[] colliders)
    {
        foreach (Collider2D col in colliders)
        {
            Stats ppt = col.GetComponent<Stats>();
            if (ppt != null && Stats.IsEnemy(ppt))
            {
                FSM fsm = GetComponent<FSM>();
                fsm.Param.AttackTarget = col.gameObject;
                fsm.ChangeState(FSMState.Attack);
                break;
            }
        }
    }

    public void OnFSM(FSMCmd cmd, FSMState state)
    {
        switch(state)
        {
            case FSMState.Idle: DoFSMIdle(cmd); break;
            case FSMState.Move: DoFSMMove(cmd); break;
            case FSMState.Attack: DoFSMAttack(cmd); break;
            case FSMState.Death: DoFSMDeath(cmd); break;
            case FSMState.Appear: DoFSMAppear(cmd); break;
            default: Debug.Log("Undefined OnFSM State"); break;
        }
    }

    private void DoFSMIdle(FSMCmd cmd)
    {
        if (cmd == FSMCmd.Enter)
        {
            GetComponent<AroundDetector>().enabled = true;
            GetComponent<UserEvent>().enabled = true;
            GetComponent<Animator>().Play("idle", -1, 0);
        }
        else if (cmd == FSMCmd.Update)
        {
        }
        else if (cmd == FSMCmd.Leave)
        {
        }
    }
    private void DoFSMMove(FSMCmd cmd)
    {
        if (cmd == FSMCmd.Enter)
        {
            GetComponent<AroundDetector>().enabled = false;
            GetComponent<Animator>().Play("move", -1, 0);
            StartCoroutine("MoveTo", GetComponent<FSM>().Param.DestinationPos);
        }
        else if (cmd == FSMCmd.Update)
        {
        }
        else if (cmd == FSMCmd.Leave)
        {
            StopCoroutine("MoveTo");
        }
    }
    private void DoFSMAttack(FSMCmd cmd)
    {
        if (cmd == FSMCmd.Enter)
        {
            GetComponent<AroundDetector>().enabled = false;
            StartCoroutine("Attack", GetComponent<FSM>().Param.AttackTarget);
        }
        else if (cmd == FSMCmd.Update)
        {
            FSM fsm = GetComponent<FSM>();
            GameObject target = fsm.Param.AttackTarget;
            FSM targetFSM = target.GetComponent<FSM>();
            if (targetFSM != null && targetFSM.State == FSMState.Death)
            {
                //target이 죽으면 idle로 변환
                fsm.ChangeState(FSMState.Idle);
            }

            Vector2 dir = target.transform.position - transform.position;
            if (dir.magnitude > Stats.AttackRange)
            {
                //target이 공격범위에서 벗어나면 idle로 전환
                fsm.ChangeState(FSMState.Idle);
            }
        }
        else if (cmd == FSMCmd.Leave)
        {
            StopCoroutine("Attack");
        }
    }
    private void DoFSMDeath(FSMCmd cmd)
    {
        if (cmd == FSMCmd.Enter)
        {
            StopAllCoroutines();
            GetComponent<Animator>().Play("death", -1, 0);
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<UserEvent>().enabled = false;
            GetComponent<AroundDetector>().enabled = false;
            //Play Animation Death
            //Play Death Sound
            Destroy(gameObject, 3.0f);
        }
        else if (cmd == FSMCmd.Update)
        {
        }
        else if (cmd == FSMCmd.Leave)
        {
        }
    }
    private void DoFSMAppear(FSMCmd cmd)
    {
        if (cmd == FSMCmd.Enter)
        {
        }
        else if (cmd == FSMCmd.Update)
        {
        }
        else if (cmd == FSMCmd.Leave)
        {
        }
    }

    private IEnumerator MoveTo(Vector3 dest)
    {
        float moveSpeed = Stats.MoveSpeed;
        dest.z = dest.y * 0.1f; //y좌표가 높을수록 해당 객체는 뒤쪽에 그려져야 하므로...
        Vector3 dir = dest - transform.position;
        float distance = dir.magnitude;
        dir.Normalize();
        float duration = distance / moveSpeed;
        float time = 0;
        while (time < duration)
        {
            transform.position += (dir * moveSpeed * Time.deltaTime);
            time += Time.deltaTime;
            yield return null;
        }
        GetComponent<FSM>().ChangeState(FSMState.Idle);
    }
    private IEnumerator Attack(GameObject enemy)
    {
        Stats enemyStats = enemy.GetComponent<Stats>();
        float waitSecForNextAttack = 1 / Stats.AttackSpeed;

        while (true)
        {
            float currentSec = Time.realtimeSinceStartup;
            float delayedSec = currentSec - mLastAttackTime;
            if (delayedSec >= waitSecForNextAttack)
            {
                GetComponent<Animator>().Play("attack_down", -1, 0);
                mLastAttackTime = currentSec;
                //Play Animation Attack
                //Play Attack Sound
                //Create Attack Particle
                enemyStats.GetDamaged(Stats.AttackDamage);
                yield return new WaitForSeconds(waitSecForNextAttack);
            }
            else
            {
                yield return new WaitForSeconds(waitSecForNextAttack - delayedSec);
            }
        }
    }
}
