﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
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
        //Dropdown 지점으로 유닛 이동
        FSM fsm = GetComponent<FSM>();
        fsm.Param.DestinationPos = worldPos;
        fsm.ChangeState(FSMState.Move);
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
            Vector3 destPos = GetComponent<FSM>().Param.DestinationPos;
            GetComponent<AroundDetector>().enabled = false;
            LookAt(destPos);
            GetComponent<Animator>().Play("move", -1, 0);
            StartCoroutine("MoveTo", destPos);
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
            GameObject target = GetComponent<FSM>().Param.AttackTarget;
            GetComponent<AroundDetector>().enabled = false;
            StartCoroutine("Attack", target);
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
            if (dir.magnitude > Stats.DetectRange)
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
        FSM enemyFSM = enemy.GetComponent<FSM>();

        while (true)
        {
            if (enemyFSM == null || enemyFSM.State == FSMState.Death)
                break;

            float currentSec = Time.realtimeSinceStartup;
            float delayedSec = currentSec - mLastAttackTime;
            if (delayedSec >= waitSecForNextAttack)
            {
                LookAt(enemy.transform.position);
                PlayAttackAnimAimmingTarget(enemy.transform.position);
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
    private void PlayAttackAnimAimmingTarget(Vector3 pos)
    {
        //y축 기준으로 3등분하여 60도 이하면 up방향 애님, 120도 이하면 mid방향 애님, 그 외 down방향 애님 재생
        Vector2 refDir = new Vector2(0, 1);
        Vector2 dir = pos - transform.position;
        dir.Normalize();
        float dot = Vector2.Dot(refDir, dir);
        float deg = Mathf.Acos(dot) * Mathf.Rad2Deg;
        if (deg < 60)
            GetComponent<Animator>().Play("attack_up", -1, 0);
        else if(deg < 120)
            GetComponent<Animator>().Play("attack_mid", -1, 0);
        else
            GetComponent<Animator>().Play("attack_down", -1, 0);
    }


    //공격모션 애니메이션 중 실제 유효한 모션에 호출되는 함수
    public void OnAnimFired(int sequence)
    {
        float ranOffX = UnityEngine.Random.Range(-0.1f, 0.1f);
        float ranOffY = UnityEngine.Random.Range(-0.1f, 0.1f);
        Skill skillObj = Instantiate<Skill>(BasicSkillPrefab);
        skillObj.Owner = gameObject;
        skillObj.Target = GetComponent<FSM>().Param.AttackTarget;
        skillObj.StartPos = transform.position;
        skillObj.EndPos = skillObj.Target.transform.position + new Vector3(ranOffX, 0.2f + ranOffY, 0);
        skillObj.Damage = GetComponent<Stats>().AttackDamage;
    }
}
