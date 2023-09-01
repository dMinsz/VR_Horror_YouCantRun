using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateStop : MonsterStateBase<Mannequin>
{

    public StateStop(Mannequin owner) : base(owner)
    {
    }

    public override void Enter()
    {
        owner.Agent.speed = 0;
    }

    public override void Exit()
    {
    }

    public override void LateUpdate()
    {
    }

    public override void Setup()
    {
    }

    public override void Transition()
    {
    }

    public override void Update()
    {
    }
}
