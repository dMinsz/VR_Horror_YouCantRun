using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAttack : MonsterStateBase<Mannequin>
{

    public StateAttack(Mannequin owner) : base(owner)
    {
    }

    public override void Enter()
    {
        Debug.Log("공격받음");
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
