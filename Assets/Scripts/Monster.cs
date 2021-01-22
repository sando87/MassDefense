using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private float mLastAttackTime = 0;
    public Skill BasicSkillPrefab;

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

    public void OnDetectEvent(Collider2D[] colliders)
    {
        //적으로 감지되는 유닛에게 공격모드로 전환
        foreach (Collider2D col in colliders)
        {
            Stats ppt = col.GetComponent<Stats>();
            if (ppt != null && Stats.IsEnemy(ppt))
            {
                FSM fsm = GetComponent<FSM>();
                fsm.Param.AttackTarget = col.gameObject;
                fsm.Param.DestinationPos = col.transform.position;
                if (fsm.State == FSMState.Idle)
                    fsm.ChangeState(FSMState.Move);
                else if(fsm.State == FSMState.Move)
                    fsm.ChangeState(FSMState.Attack);
                break;
            }
        }
    }

    public void OnHealthBarZero()
    {
        //체력이 0이면 죽음
        GetComponent<FSM>().ChangeState(FSMState.Death);
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
            LookAt(GetComponent<FSM>().Param.DestinationPos);
            GetComponent<Animator>().Play("move", -1, 0);
            StartCoroutine("MoveTo", GetComponent<FSM>().Param.DestinationPos);
        }
        else if (cmd == FSMCmd.Update)
        {
            //매 프래임마다 공격대상이 죽었는지,근접공격에 들어오는지, 도망갔는지 판단
            FSM fsm = GetComponent<FSM>();
            GameObject target = fsm.Param.AttackTarget;
            if(target == null || target.GetComponent<FSM>().State == FSMState.Death)
            {
                fsm.ChangeState(FSMState.Idle);
            }

            Vector2 dir = target.transform.position - transform.position;
            if (dir.magnitude < Stats.AttackRange)
            {
                fsm.ChangeState(FSMState.Attack);
            }
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
            //매 프래임마다 공격대상이 죽거나 도망갔는지 판단
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
                fsm.ChangeState(FSMState.Move);
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
        //dest지점으로 유닛 Smoothly 이동
        float moveSpeed = Stats.MoveSpeed;
        Vector3 dir = dest - transform.position;
        dir.z = 0;
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
        //공격 속도에 따라 반복적으로 공격을 수행하는 동작
        float waitSecForNextAttack = 1 / Stats.AttackSpeed;

        while (true)
        {
            float currentSec = Time.realtimeSinceStartup;
            float delayedSec = currentSec - mLastAttackTime;
            if (delayedSec >= waitSecForNextAttack)
            {
                LookAt(enemy.transform.position);
                GetComponent<Animator>().Play("attack", -1, 0);
                mLastAttackTime = currentSec;
                //Play Attack Sound
                yield return new WaitForSeconds(waitSecForNextAttack);
            }
            else
            {
                yield return new WaitForSeconds(waitSecForNextAttack - delayedSec);
            }
        }
    }

    private void LookAt(Vector3 pos)
    {
        if (pos.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }


    //공격모션 애니메이션 중 실제 유효한 모션에 호출되는 함수
    public void OnAnimFired(int sequence)
    {
        Skill skillObj = Instantiate<Skill>(BasicSkillPrefab);
        skillObj.Owner = gameObject;
        skillObj.Target = GetComponent<FSM>().Param.AttackTarget;
        skillObj.StartPos = transform.position;
        skillObj.EndPos = skillObj.Target.transform.position;
        skillObj.Damage = Stats.AttackDamage;
    }
}
