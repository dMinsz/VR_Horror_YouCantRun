using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBase<TState, TOwner> where TOwner : MonoBehaviour
{
    protected TOwner owner;
    protected StateMachine<TState, TOwner> stateMachine;

    public StateBase(TOwner owner, StateMachine<TState, TOwner> stateMachine)
    {
        this.owner = owner;
        this.stateMachine = stateMachine;
    }

    public abstract void Setup();
    public abstract void Enter();
    public abstract void Update();
    public abstract void Transition();
    public abstract void Exit();
}
