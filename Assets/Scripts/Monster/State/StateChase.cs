using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateChase : MonsterStateBase<Mannequin>
{

    public StateChase(Mannequin owner) : base(owner)
    {
    }

    public override void Enter()
    {
        owner.Agent.speed = owner.MoveSpeed;
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
        owner.Agent.destination = owner.playerPos.position;

        if (Vector3.Distance(owner.playerPos.position, owner.transform.position) > owner.ChaseRange)        // 플레이어가 ChaseRange에서 벗어나면 Return(Patrol로 해도 될듯..)으로 상태 변경
        {
            owner.ChangeState(Mannequin_State.Dormant);
        }
        else if (Vector3.Distance(owner.playerPos.position, owner.transform.position) < owner.AttackRange)  // 플레이어가 공격범위에 들어오면 Attack으로 상태 변경
        {
            owner.ChangeState(Mannequin_State.Attack);
        }
    }
}
