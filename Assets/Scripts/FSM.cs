using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FSM : MonoBehaviour
{
    [Serializable]
    public class UnityEventFSM : UnityEvent<FSMCmd, FSMState> { }
    public UnityEventFSM EventFSM = null;

    public FSMInfo Param { get; private set; } = new FSMInfo();
    public FSMState State { get; private set; } = FSMState.Idle;
    public void ChangeState(FSMState state)
    {
        if (State != state)
        {
            EventFSM?.Invoke(FSMCmd.Leave, State);
            State = state;
            EventFSM?.Invoke(FSMCmd.Enter, State);
        }
    }


    private void Start()
    {
        EventFSM?.Invoke(FSMCmd.Enter, State);
    }

    private void Update()
    {
        EventFSM?.Invoke(FSMCmd.Update, State);
    }
}
