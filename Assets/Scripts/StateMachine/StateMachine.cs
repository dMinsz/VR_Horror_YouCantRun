using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<TState, TOwner> where TOwner : MonoBehaviour
{
    private TOwner owner;
    private Dictionary<TState, StateBase<TState, TOwner>> states;
    private StateBase<TState, TOwner> curState;

    //ForDebug
    private TState NowState;
    public StateMachine(TOwner owner)
    {
        this.owner = owner;
        this.states = new Dictionary<TState, StateBase<TState, TOwner>>();
    }

    public void AddState(TState state, StateBase<TState, TOwner> stateBase)
    {
        states.Add(state, stateBase);
    }

    public void SetUp(TState startState)
    {
        foreach (StateBase<TState, TOwner> state in states.Values)
        {
            state.Setup();
        }

        curState = states[startState];
        curState.Enter();

        NowState = startState;
    }

    public void Update()
    {
        curState.Update();
        curState.Transition();
    }

    public void ChangeState(TState newState)
    {
        curState.Exit();
        curState = states[newState];
        curState.Enter();

        NowState = newState;
    }

    //forDebug
    public TState GetNowState()
    {
        return NowState;
    }
}