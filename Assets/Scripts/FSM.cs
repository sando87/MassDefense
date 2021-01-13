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

    public FSMInfo Param { get; }
    public FSMState State { get; private set; } = FSMState.None;
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
