using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateFactory
{
    public IState CreateState(StateType stateType)
    {
        switch(stateType)
        {
            case StateType.Idle: return new IdleState();
            case StateType.Run: return new RunState();
            case StateType.Jump: return new JumpState();
            case StateType.Fall: return new FallState();
            default: return new IdleState();
        }
    }
}
