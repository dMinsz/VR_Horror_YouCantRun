using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterStateBase<T> where T : MonoBehaviour
{
    protected T owner;

    public MonsterStateBase(T owner)
    {
        this.owner = owner;
    }

    public abstract void Setup();
    public abstract void Enter();
    public abstract void Update();
    public abstract void Transition();
    public abstract void LateUpdate();
    public abstract void Exit();
}